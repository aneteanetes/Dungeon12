using Dungeon.Data;
using LiteDB;
using System;
using System.IO;

namespace Dungeon.Resources
{
    public class Resource : Persist
    {
        public string Path { get; set; }

        public byte[] Data { get; set; }

        public DateTime LastWriteTime { get; set; }

        [BsonIgnore]
        public Stream Stream => new MemoryStream(Data);

        [BsonIgnore]
        public Action Dispose { get; set; }
    }
}
