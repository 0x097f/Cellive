using System;

namespace CilDotNet.EmbeddedResource
{
    public sealed class EmbeddedResourceEntry : IEmbeddedResource
    {
        public string Name { get; set; } = string.Empty;
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public int Size => Data.Length;
        public bool IsLinked { get; set; }
        public string? FileName { get; set; }

        public override string ToString() => $"{Name} ({Size} bytes)";
    }
}