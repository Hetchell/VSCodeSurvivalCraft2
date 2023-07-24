using System.IO;

namespace ModificationHolder.ContentPacker{
    public struct PAKInfo
    {
        public Stream fileStream;
        public string fileName;
        public string typeName;
    }
    public struct ContentFileInfo
    {
        public string fileName;
        public string typeName;
    }
}