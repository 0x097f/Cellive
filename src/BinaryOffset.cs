public readonly struct BinaryOffset : IEquatable<BinaryOffset>
{
    public uint Value { get; }

    public BinaryOffset(uint value)
    {
        Value = value;
    }

    public static implicit operator uint(BinaryOffset offset) => offset.Value;
    public static implicit operator BinaryOffset(uint value) => new BinaryOffset(value);
    public static implicit operator BinaryOffset(int value) => new BinaryOffset((uint)value);

    public static BinaryOffset operator +(BinaryOffset offset, int delta)
        => new BinaryOffset((uint)(offset.Value + delta));
    public static BinaryOffset operator -(BinaryOffset offset, int delta)
        => new BinaryOffset((uint)(offset.Value - delta));
    public static BinaryOffset operator +(BinaryOffset offset, uint delta)
        => new BinaryOffset(offset.Value + delta);
    public static BinaryOffset operator -(BinaryOffset offset, uint delta)
        => new BinaryOffset(offset.Value - delta);

    public static bool operator ==(BinaryOffset left, BinaryOffset right) => left.Value == right.Value;
    public static bool operator !=(BinaryOffset left, BinaryOffset right) => left.Value != right.Value;

    public bool Equals(BinaryOffset other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is BinaryOffset other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"0x{Value:X8}";

    public bool IsValid => Value != 0;
    public bool IsNull => Value == 0;
}
