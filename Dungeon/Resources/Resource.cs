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
        private MemoryStream stream;

        [BsonIgnore]
        public Stream Stream
        {
            get
            {
                if (stream == default)
                {
                    stream = new MemoryStream(Data);
                }

                return stream;
            }
        }
        
        [BsonIgnore]
        public void Dispose()
        {
            OnDispose?.Invoke();
            stream?.Dispose();
        }

        [BsonIgnore]
        public Action OnDispose { get; set; }
    }
}
