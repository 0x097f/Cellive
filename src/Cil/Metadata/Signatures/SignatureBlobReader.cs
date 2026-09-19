namespace CilDotNet.Cil.Metadata.Signatures
{
    public sealed class SignatureBlobReader
    {
        private readonly byte[] data;
        private int position;

        public SignatureBlobReader(byte[] data)
        {
            this.data = data ?? Array.Empty<byte>();
            position = 0;
        }

        public int Position => position;
        public int Remaining => data.Length - position;
        public bool HasMore => position < data.Length;

        public byte ReadByte()
        {
            if (position >= data.Length)
                throw new InvalidOperationException("End of signature");
            return data[position++];
        }

        public sbyte ReadSByte() => (sbyte)ReadByte();

        public ushort ReadUInt16()
        {
            if (position + 1 >= data.Length)
                throw new InvalidOperationException("End of signature");
            var value = (ushort)(data[position] | (data[position + 1] << 8));
            position += 2;
            return value;
        }

        public short ReadInt16() => (short)ReadUInt16();

        public uint ReadUInt32()
        {
            if (position + 3 >= data.Length)
                throw new InvalidOperationException("End of signature");
            var value = (uint)(data[position] | (data[position + 1] << 8) |
                               (data[position + 2] << 16) | (data[position + 3] << 24));
            position += 4;
            return value;
        }

        public int ReadInt32() => (int)ReadUInt32();

        public ulong ReadUInt64()
        {
            if (position + 7 >= data.Length)
                throw new InvalidOperationException("End of signature");
            ulong value = 0;
            for (int i = 0; i < 8; i++)
                value |= (ulong)data[position + i] << (i * 8);
            position += 8;
            return value;
        }

        public long ReadInt64() => (long)ReadUInt64();

        public float ReadSingle()
        {
            if (position + 3 >= data.Length)
                throw new InvalidOperationException("End of signature");
            var value = BitConverter.ToSingle(data, position);
            position += 4;
            return value;
        }

        public double ReadDouble()
        {
            if (position + 7 >= data.Length)
                throw new InvalidOperationException("End of signature");
            var value = BitConverter.ToDouble(data, position);
            position += 8;
            return value;
        }

        public uint ReadCompressedUInt32()
        {
            if (position >= data.Length)
                return 0;

            var b1 = data[position++];
            if ((b1 & 0x80) == 0)
                return b1;
            if ((b1 & 0x40) == 0)
            {
                if (position >= data.Length) return 0;
                var b2 = data[position++];
                return (uint)(((b1 & 0x3F) << 8) | b2);
            }
            if (position + 2 >= data.Length) return 0;
            var b3 = data[position++];
            var b4 = data[position++];
            var b5 = data[position++];
            return (uint)(((b1 & 0x1F) << 24) | (b3 << 16) | (b4 << 8) | b5);
        }

        public int ReadCompressedInt32()
        {
            var value = (int)ReadCompressedUInt32();
            if ((value & 1) != 0)
                return -(value >> 1);
            return value >> 1;
        }

        public void Skip(int count)
        {
            position += count;
            if (position > data.Length)
                position = data.Length;
        }

        public byte[] ReadBytes(int count)
        {
            if (position + count > data.Length)
                count = data.Length - position;
            var result = new byte[count];
            Array.Copy(data, position, result, 0, count);
            position += count;
            return result;
        }
        public byte PeekByte()
        {
            if (position >= data.Length)
                throw new InvalidOperationException("End of signature");
            return data[position];
        }

        public bool TryPeekByte(out byte value)
        {
            if (position >= data.Length)
            {
                value = 0;
                return false;
            }
            value = data[position];
            return true;
        }
    }
    }