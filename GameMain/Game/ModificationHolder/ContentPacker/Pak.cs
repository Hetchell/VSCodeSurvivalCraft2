

using System.Diagnostics;
using System.IO;
using System.Text;
using GameMain;

namespace ModificationHolder.ContentPacker {
   public class Pak
    {
        private byte[] keys;

        public Pak(String keys)
        {
            this.keys = Encoding.UTF8.GetBytes(keys); ;
        } 

        public void Pack(string PakDirectory)
        {
            if (!Directory.Exists(PakDirectory))
            {
                Debug.Write("Directory pointed to: " + PakDirectory);
                throw new DirectoryNotFoundException("The directory to unpack content.pak into does not exist.");
            }
            List<ContentFileInfo> contentlist = new List<ContentFileInfo>();
            contentlist = ContentFiles(contentlist, PakDirectory);
            List<long> longslist = new List<long>();
            new DirectoryInfo(PakDirectory);
            if (PakDirectory.EndsWith("/") || PakDirectory.EndsWith("\\"))
            {
                PakDirectory = PakDirectory.Substring(0, PakDirectory.Length - 1);
            }
            FileStream fileStream;
            if (File.Exists(PakDirectory + ".pak"))
            {
                if (File.Exists(PakDirectory + ".pak.bak")) File.Delete(PakDirectory + ".pak.bak");
                File.Move(PakDirectory + ".pak", PakDirectory + ".pak.bak");
            }
            fileStream = new FileStream(PakDirectory + ".pak", FileMode.Create);
            PadStream myStream = new PadStream(fileStream,this.keys);
            BinaryWriter binaryWriter = new BinaryWriter(myStream, Encoding.UTF8, true);
            fileStream.WriteByte((byte)'P');
            fileStream.WriteByte((byte)'K');
            fileStream.WriteByte((byte)'2');
            fileStream.WriteByte((byte)'\0');
            binaryWriter.Write((long)0);
            binaryWriter.Write(contentlist.Count);
            WriteToPackage(fileStream, binaryWriter, contentlist, longslist, PakDirectory);
            binaryWriter.Dispose();
            fileStream.Dispose();
        }
        private List<ContentFileInfo> ContentFiles(List<ContentFileInfo> list, string PakDirectory)
        {
            foreach (string dire in Directory.GetDirectories(PakDirectory))
            {
                list = ContentFiles(list, dire);
            }
            foreach (string file in Directory.GetFiles(PakDirectory))
            {
                string extenName = Path.GetExtension(file);
                string typeName;
                switch (extenName)
                {
                    case ".txt":
                        typeName = "System.String";
                        break;
                    case ".xml":
                        typeName = "System.Xml.Linq.XElement";
                        break;
                    case ".png":
                        if (File.Exists(file.Substring(0, file.Length - 4) + ".lst")) continue;
                        typeName = "Engine.Graphics.Texture2D";
                        break;
                    case ".dae":
                        typeName = "Engine.Graphics.Model";
                        break;
                    case ".shader":
                        typeName = "Engine.Graphics.Shader";
                        break;
                    case ".lst":
                        typeName = "Engine.Media.BitmapFont";
                        break;
                    case ".font":
                        typeName = "Engine.Media.BitmapFont";
                        break;
                    case ".wav":
                        typeName = "Engine.Audio.SoundBuffer";
                        break;
                    case ".ogg":
                        typeName = "Engine.Media.StreamingSource";
                        break;
                    default:
                        throw new Exception("Found unrecognized file :" + file);
                }
                ContentFileInfo contentFileInfo;
                if (typeName == "")
                {
                    continue;
                }
                string dir = Path.GetDirectoryName(file);
                string[] Arratitem = dir.Split((char)'\\');
                if (Arratitem.Length > 1)
                {
                    dir = Arratitem[0];
                    for (int i = 1; i < Arratitem.Length; i++) dir += "/" + Arratitem[i];
                }
                contentFileInfo.fileName = dir + "/" + Path.GetFileNameWithoutExtension(file);
                contentFileInfo.typeName = typeName;
                list.Add(contentFileInfo);
            }
            return list;
        }
        public Stream Load(string fileName, string typeName)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                FileStream fileStream;
                switch (typeName)
                {
                    case "System.String":
                        fileStream = File.OpenRead(fileName + ".txt");
                        TextHandler.WriteText(memoryStream, fileStream);
                        break;
                    case "System.Xml.Linq.XElement":
                        fileStream = File.OpenRead(fileName + ".xml");
                        TextHandler.WriteText(memoryStream, fileStream);
                        break;
                    case "Engine.Media.StreamingSource":
                        fileStream = File.OpenRead(fileName + ".ogg");
                        fileStream.CopyTo(memoryStream);
                        break;
                    case "Engine.Graphics.Model":
                        fileStream = File.OpenRead(fileName + ".dae");
                        ModelHandler.WriteModel(memoryStream, fileStream);
                        //fileStream.CopyTo(memoryStream);
                        break;
                    case "Engine.Graphics.Shader":
                        fileStream = File.OpenRead(fileName + ".shader");
                        fileStream.CopyTo(memoryStream);
                        break;
                    case "Engine.Audio.SoundBuffer":
                        fileStream = File.OpenRead(fileName + ".wav");
                        SoundHandler.WriteSound(memoryStream, fileStream);
                        break;
                    case "Engine.Graphics.Texture2D":
                        fileStream = File.OpenRead(fileName + ".png");
                        Texture2DHandler.WriteTexture2D(memoryStream, fileStream);
                        break;
                    case "Engine.Media.BitmapFont":
                        try
                        {
                            fileStream = File.OpenRead(fileName + ".lst");
                        }
                        catch
                        {
                            fileStream = File.OpenRead(fileName + ".font");
                            fileStream.CopyTo(memoryStream);
                            break;
                        }
                        FileStream bitmapStream = File.OpenRead(fileName + ".png");
                        FontHandler.WriteFont(memoryStream, fileStream, bitmapStream);
                        bitmapStream.Dispose();
                        break;
                    default:
                        throw new Exception("Found unrecognized name :" + fileName + "\nfolder type :" + typeName);
                }
                fileStream.Dispose();
                memoryStream.Position = 0L;
                return memoryStream;
            }
            catch (Exception e)
            {
                throw new Exception("Folder write error :" + fileName + "\ttype :" + typeName + "\nmain exception :" + e.Message);
            }
        }

        public void WriteToPackage(FileStream fileStream, BinaryWriter binaryWriter, List<ContentFileInfo> contentslist, List<long> longslist, String pakdir)
        {
            foreach (ContentFileInfo current in contentslist)
            {
                binaryWriter.Write(current.fileName.Substring(pakdir.Length + 1, current.fileName.Length - pakdir.Length - 1));
                binaryWriter.Write(current.typeName);
                longslist.Add(binaryWriter.BaseStream.Position);
                binaryWriter.Write((long)0);
                binaryWriter.Write((long)0);
            }
            long position = binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = 4L;
            binaryWriter.Write(position);
            long num = position;
            int num2 = 0;
            long num3;
            foreach (ContentFileInfo info in contentslist)
            {
                binaryWriter.BaseStream.Position = num;
                binaryWriter.Write((byte)222);
                binaryWriter.Write((byte)173);
                binaryWriter.Write((byte)190);
                binaryWriter.Write((byte)239);
                num3 = binaryWriter.BaseStream.Position;
                PadStream myStream2 = new PadStream(Load(info.fileName, info.typeName), new byte[] { 63 });
                byte[] buffer = new byte[myStream2.Length];
                myStream2.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i++)
                {
                    fileStream.WriteByte(buffer[i]);
                }
                num = binaryWriter.BaseStream.Position;
                binaryWriter.BaseStream.Position = longslist[num2++];
                binaryWriter.Write(num3 - position);
                binaryWriter.Write(myStream2.Length);
            }
        }
    }
}