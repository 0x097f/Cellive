public class SectionInfo
{
    public string Name { get; set; } = string.Empty; 
    public uint VirtualSize { get; set; } 
    public uint VirtualAddress { get; set; } 
    public uint SizeOfRawData { get; set; }
    public uint PointerToRawData { get; set; }
    public uint Characteristics { get; set; } 
}
