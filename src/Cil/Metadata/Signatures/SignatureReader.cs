using System;
using System.Collections.Generic;

namespace Cellive.Cil.Metadata.Signatures
{
    public sealed class SignatureReader
    {
        private readonly SignatureBlobReader reader;

        public SignatureReader(byte[] data)
        {
            reader = new SignatureBlobReader(data);
        }

        public SignatureReader(SignatureBlobReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public MethodSignature ReadMethodSignature()
        {
            var sig = new MethodSignature();

            var header = reader.ReadByte();
            sig.HasThis = (header & 0x20) != 0;
            sig.HasExplicitThis = (header & 0x40) != 0;
            sig.IsGeneric = (header & 0x10) != 0;
            sig.IsVarArg = (header & 0x05) == 0x05;

            if (sig.IsGeneric)
                sig.GenericParameterCount = (int)reader.ReadCompressedUInt32();

            sig.ParameterCount = (int)reader.ReadCompressedUInt32();
            sig.ReturnType = ReadType();

            bool inSentinel = false;
            for (int i = 0; i < sig.ParameterCount; i++)
            {
                if (reader.TryPeekByte(out var next) && next == (byte)ElementType.Sentinel)
                {
                    reader.ReadByte();  
                    inSentinel = true;
                    continue;
                }

                var paramType = ReadType();
                if (inSentinel)
                    sig.SentinelParameters.Add(paramType);
                else
                    sig.Parameters.Add(paramType);
            }

            return sig;
        }

        public FieldSignature ReadFieldSignature()
        {
            var sig = new FieldSignature();
            var header = reader.ReadByte();
            sig.IsInstance = (header & 0x20) != 0;
            sig.FieldType = ReadType();
            return sig;
        }

        public PropertySignature ReadPropertySignature()
        {
            var sig = new PropertySignature();
            var header = reader.ReadByte();
            sig.HasThis = (header & 0x20) != 0;
            sig.ParameterCount = (int)reader.ReadCompressedUInt32();
            sig.PropertyType = ReadType();

            for (int i = 0; i < sig.ParameterCount; i++)
            {
                sig.Parameters.Add(ReadType());
            }

            return sig;
        }

        public TypeSignature ReadType()
        {
            var elementType = (ElementType)reader.ReadByte();
            var type = new TypeSignature { ElementType = elementType };

            switch (elementType)
            {
                case ElementType.Void:
                case ElementType.Boolean:
                case ElementType.Char:
                case ElementType.I1:
                case ElementType.U1:
                case ElementType.I2:
                case ElementType.U2:
                case ElementType.I4:
                case ElementType.U4:
                case ElementType.I8:
                case ElementType.U8:
                case ElementType.R4:
                case ElementType.R8:
                case ElementType.String:
                case ElementType.I:
                case ElementType.U:
                case ElementType.Object:
                case ElementType.TypedByRef:
                    type.Name = elementType.ToString();
                    type.Namespace = "System";
                    break;

                case ElementType.Ptr:
                    type.IsPointer = true;
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.ByRef:
                    type.IsByReference = true;
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.ValueType:
                    type.IsValueType = true;
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.Class:
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.Var:
                    type.IsGenericParameter = true;
                    type.IsMethodGenericParameter = false;
                    type.GenericParameterIndex = (int)reader.ReadCompressedUInt32();
                    break;

                case ElementType.MVar:
                    type.IsGenericParameter = true;
                    type.IsMethodGenericParameter = true;
                    type.GenericParameterIndex = (int)reader.ReadCompressedUInt32();
                    break;

                case ElementType.Array:
                    type.IsArray = true;
                    type.ElementTypeSignature = ReadType();
                    type.Rank = (int)reader.ReadCompressedUInt32();
                    var numSizes = (int)reader.ReadCompressedUInt32();
                    for (int i = 0; i < numSizes; i++)
                        type.Sizes.Add((int)reader.ReadCompressedUInt32());
                    var numLoBounds = (int)reader.ReadCompressedUInt32();
                    for (int i = 0; i < numLoBounds; i++)
                        type.LowerBounds.Add(reader.ReadCompressedInt32());
                    break;

                case ElementType.SzArray:
                    type.IsSzArray = true;
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.GenericInst:
                    type.IsGenericInstance = true;
                    var kind = (ElementType)reader.ReadByte();
                    type.IsValueType = kind == ElementType.ValueType;
                    type.ElementTypeSignature = ReadType();
                    var argCount = (int)reader.ReadCompressedUInt32();
                    for (int i = 0; i < argCount; i++)
                        type.GenericArguments.Add(ReadType());
                    break;

                case ElementType.FnPtr:
                    var methodSig = ReadMethodSignature();
                    type.Name = "FnPtr";
                    break;

                case ElementType.Pinned:
                    type.IsPinned = true;
                    type.ElementTypeSignature = ReadType();
                    break;

                case ElementType.CModReqd:
                case ElementType.CModOpt:
                    type.Modifiers.Add(ReadType());
                    type.ElementTypeSignature = ReadType();
                    break;

                default:
                    type.Name = $"Unknown_0x{(byte)elementType:X2}";
                    break;
            }

            return type;
        }

    }
}