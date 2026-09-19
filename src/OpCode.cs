namespace CilDotNet
{
    public sealed class OpCode
    {
        public string Name { get; }
        public OpCodeCategories Code { get; }
        public OperandType OperandType { get; }
        public CilDotNet.Cil.FlowControl FlowControl { get; }
        public int Size { get; }

        public OpCode(string name, OpCodeCategories code, OperandType operandType, CilDotNet.Cil.FlowControl flowControl)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code;
            OperandType = operandType;
            FlowControl = flowControl;
            Size = ((ushort)code) > 0xFF ? 2 : 1;
        }

        public bool IsTwoByte => ((ushort)Code) > 0xFF;

        public override string ToString() => Name;

        public override bool Equals(object? obj) => obj is OpCode other && Code == other.Code;

        public override int GetHashCode() => (int)Code;
    }
}