// See https://aka.ms/new-console-template for more information

using System.Text;


LABHeader labHeader;

List<LABFileEntry> entries = new List<LABFileEntry>();

const string fileName = @"C:\tn_ftp_home\olpatch1.lab";

var stream = File.OpenRead(fileName);

var reader = new BinaryReader(stream, System.Text.Encoding.ASCII);

labHeader.id = reader.ReadChars(4);
labHeader.version = reader.ReadUInt32();
labHeader.fileCount = reader.ReadUInt32();
labHeader.fileNameListLength = reader.ReadUInt32();

while (stream.Position <= (labHeader.fileCount * 16))
{
    LABFileEntry entry = new LABFileEntry();
    entry.nameOffset = reader.ReadUInt32();
    entry.dataOffset = reader.ReadUInt32();
    entry.size = reader.ReadUInt32();
    entry.fileType = reader.ReadChars(4);

    var sb = new StringBuilder();

    long old_pos = stream.Position;
    stream.Position = (labHeader.fileCount * 16 + 16) + entry.nameOffset;

    while (stream.Position != stream.Length)
    {
        char temp = reader.ReadChar();
        if (temp == '\0') break;
        sb.Append(temp.ToString());
    }

    entry.name = sb.ToString();

    entries.Add(entry);

    stream.Position = old_pos;
}


for (int i = 0; i < entries.Count(); i++)
{
    if (entries[i].name.ToLower().Contains("mill"))
    {
        using (var write_stream = File.OpenWrite(@"C:\tn_ftp_home\" + entries[i].name))
        {
            using (var writer = new BinaryWriter(write_stream, System.Text.Encoding.ASCII))
            {
                int count = 1;
                stream.Position = entries[i].dataOffset;
                while (count <= entries[i].size)
                {
                    byte data = reader.ReadByte();
                    writer.Write(data);
                    count++;
                }
            }
        }
    }
}


foreach (var item in entries)
{
    Console.WriteLine("Filename : " + item.name + "\t\tFilesize :" + item.size);
}

stream.Close();
reader.Close();


