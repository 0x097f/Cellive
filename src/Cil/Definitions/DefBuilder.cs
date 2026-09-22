using Cellive.Cil.Metadata;
using Cellive.Cil.Metadata.Signatures;
using Cellive.Cil.Metadata.Tables;
using Cellive.Executable;

namespace Cellive.Cil.Definitions
{
    public sealed class DefBuilder
    {
        private readonly Cellive.Cil.Metadata.MetadataReader metadata;
        private readonly TableDecoder decoder;
        private readonly System.IO.BinaryReader reader;
        private readonly PeImage pe;

        private readonly Cellive.Cil.Metadata.Tables.TypeDefTable typeDefTable;
        private readonly TypeRefTable typeRefTable;
        private readonly MethodTable methodTable;
        private readonly FieldTable fieldTable;
        private readonly ParamTable paramTable;
        private readonly PropertyTable propertyTable;
        private readonly EventTable eventTable;
        private readonly MethodSemanticsTable methodSemanticsTable;
        private readonly PropertyMapTable propertyMapTable;
        private readonly EventMapTable eventMapTable;
        private readonly GenericParamTable genericParamTable;
        private readonly GenericParamConstraintTable genericParamConstraintTable;
        private readonly NestedClassTable nestedClassTable;
        private readonly InterfaceImplTable interfaceImplTable;
        private readonly MemberRefTable memberRefTable;
        private readonly AssemblyTable assemblyTable;
        private readonly AssemblyRefTable assemblyRefTable;
        private readonly ModuleTable moduleTable;
        private readonly ConstantTable constantTable;
        private readonly CustomAttributeTable customAttributeTable;

        private readonly Dictionary<int, TypeDef> typeDefs = new();
        private readonly Dictionary<int, MethodDef> methodDefs = new();
        private readonly Dictionary<int, FieldDef> fieldDefs = new();
        private readonly Dictionary<int, ParamDef> paramDefs = new();
        private readonly Dictionary<int, PropertyDef> propertyDefs = new();
        private readonly Dictionary<int, EventDef> eventDefs = new();
        private readonly Dictionary<int, GenericParam> genericParams = new();

        public DefBuilder(PeImage pe, Cellive.Cil.Metadata.MetadataReader metadata, System.IO.BinaryReader reader)
        {
            this.pe = pe;
            this.metadata = metadata;
            this.reader = reader;
            decoder = new TableDecoder(metadata);

            typeDefTable = new TypeDefTable(metadata, decoder, reader);
            typeRefTable = new TypeRefTable(metadata, decoder, reader);
            methodTable = new MethodTable(metadata, decoder, reader);
            fieldTable = new FieldTable(metadata, decoder, reader);
            paramTable = new ParamTable(metadata, decoder, reader);
            propertyTable = new PropertyTable(metadata, decoder, reader);
            eventTable = new EventTable(metadata, decoder, reader);
            methodSemanticsTable = new MethodSemanticsTable(metadata, decoder, reader);
            propertyMapTable = new PropertyMapTable(metadata, decoder, reader);
            eventMapTable = new EventMapTable(metadata, decoder, reader);
            genericParamTable = new GenericParamTable(metadata, decoder, reader);
            genericParamConstraintTable = new GenericParamConstraintTable(metadata, decoder, reader);
            nestedClassTable = new NestedClassTable(metadata, decoder, reader);
            interfaceImplTable = new InterfaceImplTable(metadata, decoder, reader);
            memberRefTable = new MemberRefTable(metadata, decoder, reader);
            assemblyTable = new AssemblyTable(metadata, decoder, reader);
            assemblyRefTable = new AssemblyRefTable(metadata, decoder, reader);
            moduleTable = new ModuleTable(metadata, decoder, reader);
            constantTable = new ConstantTable(metadata, decoder, reader);
            customAttributeTable = new CustomAttributeTable(metadata, decoder, reader);
        }

        public ModuleDef Build()
        {
            var moduleDef = BuildModule();
            BuildTypes(moduleDef);
            BuildNestedTypes(moduleDef);
            BuildInterfaces(moduleDef);
            BuildMembers(moduleDef);
            BuildGenericParameters();
            BuildGenericConstraints();
            BuildPropertiesAndEvents(moduleDef);

            return moduleDef;
        }

        private ModuleDef BuildModule()
        {
            var moduleRow = moduleTable.First;
            var moduleDef = new ModuleDef
            {
                Token = MetadataToken.FromToken(TableType.Module, 1),
                Name = moduleRow?.Name ?? "Unknown",
                Mvid = moduleRow?.Mvid ?? Guid.Empty,
                EncId = moduleRow?.EncId ?? Guid.Empty,
                EncBaseId = moduleRow?.EncBaseId ?? Guid.Empty,
                PeImage = pe,
            };

            var assemblyRow = assemblyTable.First;
            if (assemblyRow != null)
            {
                var assemblyDef = new AssemblyDef
                {
                    Token = MetadataToken.FromToken(TableType.Assembly, 1),
                    Name = assemblyRow.Name,
                    Version = assemblyRow.Version,
                    Culture = assemblyRow.Culture,
                    PublicKey = assemblyRow.PublicKey,
                    Flags = assemblyRow.Flags,
                    HashAlgId = assemblyRow.HashAlgId,
                };

                assemblyDef.ModulesList.Add(moduleDef);
                moduleDef.Assembly = assemblyDef;
            }

            foreach (var row in assemblyRefTable.Rows)
            {
                moduleDef.AssemblyReferencesList.Add(new AssemblyRef
                {
                    Name = row.Name,
                    Version = row.Version,
                    Culture = row.Culture,
                    PublicKeyOrToken = row.PublicKeyOrToken,
                    Flags = row.Flags,
                });
            }

            return moduleDef;
        }

        private void BuildTypes(ModuleDef moduleDef)
        {
            var rows = typeDefTable.Rows;

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var typeDef = new TypeDef
                {
                    Token = MetadataToken.FromToken(TableType.TypeDef, (uint)(i + 1)),
                    Name = row.Name,
                    Namespace = row.Namespace,
                    Flags = row.Flags,
                    Module = moduleDef,
                };

                typeDefs[i + 1] = typeDef;
                moduleDef.TypesList.Add(typeDef);
            }

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row.ExtendsToken.IsNil)
                    continue;

                var baseType = ResolveType(row.ExtendsToken);
                if (baseType != null)
                    typeDefs[i + 1].BaseType = baseType;
            }
        }

        private void BuildNestedTypes(ModuleDef moduleDef)
        {
            foreach (var row in nestedClassTable.Rows)
            {
                var nestedIndex = (int)row.NestedClassToken.RowIndex;
                var enclosingIndex = (int)row.EnclosingClassToken.RowIndex;

                if (!typeDefs.TryGetValue(nestedIndex, out var nested))
                    continue;
                if (!typeDefs.TryGetValue(enclosingIndex, out var enclosing))
                    continue;

                nested.DeclaringType = enclosing;
                enclosing.NestedTypesList.Add(nested);
            }
        }

        private void BuildInterfaces(ModuleDef moduleDef)
        {
            foreach (var row in interfaceImplTable.Rows)
            {
                var classIndex = (int)row.ClassToken.RowIndex;
                if (!typeDefs.TryGetValue(classIndex, out var typeDef))
                    continue;

                var iface = ResolveType(row.InterfaceToken);
                if (iface != null)
                    typeDef.InterfacesList.Add(iface);
            }
        }

        private void BuildMembers(ModuleDef moduleDef)
        {
            var typeRows = typeDefTable.Rows;
            var methodRows = methodTable.Rows;
            var fieldRows = fieldTable.Rows;

            for (int i = 0; i < typeRows.Count; i++)
            {
                var row = typeRows[i];
                var typeDef = typeDefs[i + 1];

                int methodStart = (int)row.MethodList - 1;
                int methodEnd = (i + 1 < typeRows.Count)
                    ? (int)typeRows[i + 1].MethodList - 1
                    : methodRows.Count;

                for (int m = methodStart; m < methodEnd && m < methodRows.Count; m++)
                {
                    if (m < 0) continue;
                    var methodDef = BuildMethod(methodRows[m], (uint)(m + 1), typeDef);
                    methodDefs[m + 1] = methodDef;
                    typeDef.MethodsList.Add(methodDef);
                }

                int fieldStart = (int)row.FieldList - 1;
                int fieldEnd = (i + 1 < typeRows.Count)
                    ? (int)typeRows[i + 1].FieldList - 1
                    : fieldRows.Count;

                for (int f = fieldStart; f < fieldEnd && f < fieldRows.Count; f++)
                {
                    if (f < 0) continue;
                    var fieldDef = BuildField(fieldRows[f], (uint)(f + 1), typeDef);
                    fieldDefs[f + 1] = fieldDef;
                    typeDef.FieldsList.Add(fieldDef);
                }
            }
        }

        private MethodDef BuildMethod(MethodRow row, uint index, TypeDef declaringType)
        {
            var methodDef = new MethodDef
            {
                Token = MetadataToken.FromToken(TableType.Method, index),
                Name = row.Name,
                RVA = row.RVA,
                Flags = row.Flags,
                ImplFlags = row.ImplFlags,
                Signature = row.Signature,
                DeclaringType = declaringType,
            };

            if (row.Signature.Length > 0)
            {
                try
                {
                    var sigReader = new SignatureReader(row.Signature);
                    var methodSig = sigReader.ReadMethodSignature();
                    methodDef.ReturnType = ResolveTypeSignature(methodSig.ReturnType);

                    int paramStart = (int)row.ParamList - 1;

                    for (int p = 0; p < methodSig.Parameters.Count; p++)
                    {
                        var paramDef = new ParamDef
                        {
                            Token = MetadataToken.FromToken(TableType.Param, (uint)(paramStart + p + 1)),
                            Index = p,
                            Sequence = (ushort)(p + 1),
                            Method = methodDef,
                            ParameterType = ResolveTypeSignature(methodSig.Parameters[p]),
                        };

                        int paramRowIndex = paramStart + p;
                        if (paramRowIndex >= 0 && paramRowIndex < paramTable.Rows.Count)
                        {
                            var paramRow = paramTable.Rows[paramRowIndex];
                            paramDef.Name = paramRow.Name;
                            paramDef.Flags = paramRow.Flags;
                        }

                        paramDefs[paramStart + p + 1] = paramDef;
                        methodDef.ParametersList.Add(paramDef);
                    }
                }
                catch
                {
                }
            }

            return methodDef;
        }

        private FieldDef BuildField(FieldRow row, uint index, TypeDef declaringType)
        {
            var fieldDef = new FieldDef
            {
                Token = MetadataToken.FromToken(TableType.Field, index),
                Name = row.Name,
                Flags = row.Flags,
                Signature = row.Signature,
                DeclaringType = declaringType,
            };

            if (row.Signature.Length > 0)
            {
                try
                {
                    var sigReader = new SignatureReader(row.Signature);
                    var fieldSig = sigReader.ReadFieldSignature();
                    fieldDef.FieldType = ResolveTypeSignature(fieldSig.FieldType);
                }
                catch
                {
                }
            }

            var constant = constantTable.FindByParent(fieldDef.Token);
            if (constant != null)
            {
                fieldDef.ConstantValue = constant.Value;
            }

            return fieldDef;
        }

        private void BuildGenericParameters()
        {
            foreach (var row in genericParamTable.Rows)
            {
                var gp = new GenericParam
                {
                    Token = MetadataToken.FromToken(TableType.GenericParam, (uint)(genericParams.Count + 1)),
                    Name = row.Name,
                    Position = row.Number,
                    Flags = row.Flags,
                };

                var owner = row.OwnerToken;
                if (owner.Table == TableType.TypeDef)
                {
                    var index = (int)owner.RowIndex;
                    if (typeDefs.TryGetValue(index, out var typeDef))
                    {
                        gp.OwnerType = typeDef;
                        typeDef.GenericParametersList.Add(gp);
                    }
                }
                else if (owner.Table == TableType.Method)
                {
                    var index = (int)owner.RowIndex;
                    if (methodDefs.TryGetValue(index, out var methodDef))
                    {
                        gp.OwnerMethod = methodDef;
                        methodDef.GenericParametersList.Add(gp);
                    }
                }

                genericParams[genericParams.Count + 1] = gp;
            }
        }

        private void BuildGenericConstraints()
        {
            foreach (var row in genericParamConstraintTable.Rows)
            {
                var gpIndex = (int)row.OwnerToken.RowIndex;
                if (!genericParams.TryGetValue(gpIndex, out var gp))
                    continue;

                var constraint = ResolveType(row.ConstraintToken);
                if (constraint != null)
                    gp.ConstraintsList.Add(constraint);
            }
        }

        private void BuildPropertiesAndEvents(ModuleDef moduleDef)
        {
            var propertyRows = propertyTable.Rows;
            var eventRows = eventTable.Rows;

            foreach (var map in propertyMapTable.Rows)
            {
                var typeIndex = (int)map.ParentToken.RowIndex;
                if (!typeDefs.TryGetValue(typeIndex, out var typeDef))
                    continue;

                int start = (int)map.PropertyList - 1;
                int end = propertyRows.Count;

                foreach (var next in propertyMapTable.Rows)
                {
                    if ((int)next.ParentToken.RowIndex == typeIndex &&
                        (int)next.PropertyList - 1 > start)
                    {
                        end = (int)next.PropertyList - 1;
                        break;
                    }
                }

                for (int p = start; p < end && p < propertyRows.Count; p++)
                {
                    if (p < 0) continue;
                    var propDef = new PropertyDef
                    {
                        Token = MetadataToken.FromToken(TableType.Property, (uint)(p + 1)),
                        Name = propertyRows[p].Name,
                        Flags = propertyRows[p].Flags,
                        Signature = propertyRows[p].Type,
                        DeclaringType = typeDef,
                    };

                    if (propDef.Signature.Length > 0)
                    {
                        try
                        {
                            var sigReader = new SignatureReader(propDef.Signature);
                            var propSig = sigReader.ReadPropertySignature();
                            propDef.PropertyType = ResolveTypeSignature(propSig.PropertyType);
                        }
                        catch
                        {
                        }
                    }

                    foreach (var sem in methodSemanticsTable.FindByAssociation(propDef.Token))
                    {
                        var methodIndex = (int)sem.MethodToken.RowIndex;
                        if (!methodDefs.TryGetValue(methodIndex, out var methodDef))
                            continue;

                        if (sem.IsGetter)
                            propDef.Getter = methodDef;
                        else if (sem.IsSetter)
                            propDef.Setter = methodDef;
                        else if (sem.IsOther)
                            propDef.OtherMethod = methodDef;
                    }

                    propertyDefs[p + 1] = propDef;
                    typeDef.PropertiesList.Add(propDef);
                }
            }

            foreach (var map in eventMapTable.Rows)
            {
                var typeIndex = (int)map.ParentToken.RowIndex;
                if (!typeDefs.TryGetValue(typeIndex, out var typeDef))
                    continue;

                int start = (int)map.EventList - 1;
                int end = eventRows.Count;

                foreach (var next in eventMapTable.Rows)
                {
                    if ((int)next.ParentToken.RowIndex == typeIndex &&
                        (int)next.EventList - 1 > start)
                    {
                        end = (int)next.EventList - 1;
                        break;
                    }
                }

                for (int e = start; e < end && e < eventRows.Count; e++)
                {
                    if (e < 0) continue;
                    var eventDef = new EventDef
                    {
                        Token = MetadataToken.FromToken(TableType.Event, (uint)(e + 1)),
                        Name = eventRows[e].Name,
                        Flags = eventRows[e].Flags,
                        DeclaringType = typeDef,
                    };

                    eventDef.EventType = ResolveType(eventRows[e].EventTypeToken);

                    foreach (var sem in methodSemanticsTable.FindByAssociation(eventDef.Token))
                    {
                        var methodIndex = (int)sem.MethodToken.RowIndex;
                        if (!methodDefs.TryGetValue(methodIndex, out var methodDef))
                            continue;

                        if (sem.IsAddOn)
                            eventDef.AddMethod = methodDef;
                        else if (sem.IsRemoveOn)
                            eventDef.RemoveMethod = methodDef;
                        else if (sem.IsFire)
                            eventDef.RaiseMethod = methodDef;
                    }

                    eventDefs[e + 1] = eventDef;
                    typeDef.EventsList.Add(eventDef);
                }
            }
        }

        private ITypeDef? ResolveType(MetadataToken token)
        {
            switch (token.Table)
            {
                case TableType.TypeDef:
                    return typeDefs.TryGetValue((int)token.RowIndex, out var td) ? td : null;
                case TableType.TypeRef:
                    return ResolveTypeRef((int)token.RowIndex);
                case TableType.TypeSpec:
                    return null;
                default:
                    return null;
            }
        }

        private ITypeDef? ResolveTypeRef(int index)
        {
            if (index < 1 || index > typeRefTable.Rows.Count)
                return null;

            var row = typeRefTable.Rows[index - 1];
            var typeRef = new TypeRef
            {
                Token = MetadataToken.FromToken(TableType.TypeRef, (uint)index),
                Name = row.Name,
                Namespace = row.Namespace,
            };

            return typeRef;
        }

        private ITypeDef? ResolveTypeSignature(TypeSignature? signature)
        {
            if (signature == null)
                return null;

            if (!string.IsNullOrEmpty(signature.Name))
            {
                return new TypeRef
                {
                    Name = signature.Name,
                    Namespace = signature.Namespace ?? string.Empty,
                };
            }

            if (signature.ElementTypeSignature != null)
                return ResolveTypeSignature(signature.ElementTypeSignature);

            return null;
        }
    }
}
