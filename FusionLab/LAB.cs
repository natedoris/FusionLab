public struct LABHeader
{
    public char[] id;
    public UInt32 version;
    public UInt32 fileCount;
    public UInt32 fileNameListLength;
}

public struct LABFileEntry
{
    public UInt32 nameOffset;
    public UInt32 dataOffset;
    public UInt32 size;
    public char[] fileType;
    public string name;
}