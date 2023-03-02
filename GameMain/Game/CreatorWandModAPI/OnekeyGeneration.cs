using Engine;
using Engine.Serialization;
using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public static class OnekeyGeneration
    {
        public static void CreateOnekey(CreatorAPI creatorAPI, string directory, string path, Point3 Start, Point3 End, Point3 position)
        {
            int num = 0;
            CreatorMain.Math.StartEnd(ref Start, ref End);
            FileStream fileStream = new FileStream(directory + "/" + path, FileMode.Create);
            EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(fileStream, leaveOpen: true);
            engineBinaryWriter.Write(End.X - position.X);
            engineBinaryWriter.Write(End.Y - position.Y);
            engineBinaryWriter.Write(End.Z - position.Z);
            engineBinaryWriter.Write(Start.X - position.X);
            engineBinaryWriter.Write(Start.Y - position.Y);
            engineBinaryWriter.Write(Start.Z - position.Z);
            for (int i = End.X; i <= Start.X; i++)
            {
                for (int j = End.Y; j <= Start.Y; j++)
                {
                    for (int k = End.Z; k <= Start.Z; k++)
                    {
                        engineBinaryWriter.Write(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValueFast(i, j, k));
                        num++;
                    }
                }
            }

            engineBinaryWriter.Dispose();
            fileStream.Dispose();
            creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("okeydialogs"), num), Color.Yellow, blinking: true, playNotificationSound: true);
        }

        public static void GenerationData(CreatorAPI creatorAPI, string path, Point3 position)
        {
            ChunkData chunkData = new ChunkData(creatorAPI);
            creatorAPI.revokeData = new ChunkData(creatorAPI);
            _ = creatorAPI.componentMiner.ComponentPlayer;
            Stream stream = new FileStream(path, FileMode.Open).CreateStream();
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream);
            int num = 0;
            int num2 = engineBinaryReader.ReadInt32();
            int num3 = engineBinaryReader.ReadInt32();
            int num4 = engineBinaryReader.ReadInt32();
            int num5 = engineBinaryReader.ReadInt32();
            int num6 = engineBinaryReader.ReadInt32();
            int num7 = engineBinaryReader.ReadInt32();
            for (int i = num2; i <= num5; i++)
            {
                for (int j = num3; j <= num6; j++)
                {
                    for (int k = num4; k <= num7; k++)
                    {
                        if (!creatorAPI.launch)
                        {
                            return;
                        }

                        int value = engineBinaryReader.ReadInt32();
                        if (creatorAPI.AirIdentify || Terrain.ExtractContents(value) != 0)
                        {
                            creatorAPI.CreateBlock(position.X + i, position.Y + j, position.Z + k, value, chunkData);
                            num++;
                        }
                    }
                }
            }

            engineBinaryReader.Dispose();
            stream.Dispose();
            chunkData.Render();
            creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("okeydialogs2"), num), Color.Yellow, blinking: true, playNotificationSound: true);
        }

        public static void ExportOnekeyoMod2(string path, string exportFile)
        {
            if (path.IsFileInUse())
            {
                throw new FileLoadException("capadialogex");
            }

            Stream stream = new FileStream(exportFile, FileMode.Create);
            Stream stream2 = File.Open(path, FileMode.Open);
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream2);
            int num = engineBinaryReader.ReadInt32();
            int num2 = engineBinaryReader.ReadInt32();
            int num3 = engineBinaryReader.ReadInt32();
            int num4 = engineBinaryReader.ReadInt32();
            int num5 = engineBinaryReader.ReadInt32();
            int num6 = engineBinaryReader.ReadInt32();
            string text = "" + $"{num}\n{num2}\n{num3}\n{num4}\n{num5}\n{num6}";
            for (int i = num; i <= num4; i++)
            {
                for (int j = num2; j <= num5; j++)
                {
                    for (int k = num3; k <= num6; k++)
                    {
                        text += $"\n{engineBinaryReader.ReadInt32()}";
                    }
                }
            }

            stream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetBytes(text).Length);
            engineBinaryReader.Dispose();
            stream2.Dispose();
            stream.Dispose();
        }

        public static void ImportOnekeyoMod2(string path, string importFile)
        {
            new List<string>();
            FileStream fileStream = File.OpenRead(importFile);
            StreamReader streamReader = new StreamReader(fileStream);
            string text = streamReader.ReadToEnd();
            streamReader.Dispose();
            fileStream.Dispose();
            FileStream fileStream2 = new FileStream(path, FileMode.Create);
            EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(fileStream2, leaveOpen: true);
            string text2 = text;
            char[] separator = new char[1]
            {
                '\n'
            };
            string[] array = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in array)
            {
                engineBinaryWriter.Write(int.Parse(s));
            }

            engineBinaryWriter.Dispose();
            fileStream2.Dispose();
        }

        public static void ExportOnekeyoMod(string path, string exportFile)
        {
            if (path.IsFileInUse())
            {
                throw new FileLoadException("capadialogex");
            }

            Stream stream = new FileStream(exportFile, FileMode.Create);
            Stream stream2 = File.Open(path, FileMode.Open);
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream2);
            string text = "";
            int num = engineBinaryReader.ReadInt32();
            int num2 = engineBinaryReader.ReadInt32();
            int num3 = engineBinaryReader.ReadInt32();
            int num4 = engineBinaryReader.ReadInt32();
            int num5 = engineBinaryReader.ReadInt32();
            int num6 = engineBinaryReader.ReadInt32();
            for (int i = num; i <= num4; i++)
            {
                for (int j = num2; j <= num5; j++)
                {
                    for (int k = num3; k <= num6; k++)
                    {
                        int num7 = engineBinaryReader.ReadInt32();
                        if (Terrain.ExtractContents(num7) != 0)
                        {
                            text = ((i != num4 || j != num5 || k != num6) ? (text + $"{i},{j},{k},{num7}\n") : (text + $"{i},{j},{k},{num7}\n"));
                        }
                    }
                }
            }

            stream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetBytes(text).Length);
            engineBinaryReader.Dispose();
            stream2.Dispose();
            stream.Dispose();
        }
    }
}