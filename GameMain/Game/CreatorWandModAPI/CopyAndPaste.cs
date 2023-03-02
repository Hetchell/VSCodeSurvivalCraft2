//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

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
    public static class CopyAndPaste
    {
        public static long CreateCopy(CreatorAPI creatorAPI, string directory, string path, Point3 Start, Point3 End)
        {
            long result = 0L;
            int num = 0;
            CreatorMain.Math.StarttoEnd(ref Start, ref End);
            ComponentPlayer componentPlayer = creatorAPI.componentMiner.ComponentPlayer;
            Vector3 forward = Matrix.CreateFromQuaternion(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation).Forward;
            float num2 = float.NegativeInfinity;
            int num3 = 0;
            for (int i = 0; i < 6; i++)
            {
                float num4 = Vector3.Dot(CellFace.FaceToVector3(i), forward);
                if ((double)num4 > (double)num2)
                {
                    num2 = num4;
                    num3 = i;
                }
            }

            float num5 = Vector3.Dot(forward, -Vector3.UnitZ);
            float num6 = Vector3.Dot(forward, -Vector3.UnitX);
            float num7 = Vector3.Dot(forward, Vector3.UnitZ);
            float num8 = Vector3.Dot(forward, Vector3.UnitX);
            int num9 = 0;
            if ((double)num5 == (double)MathUtils.Max(num5, num6, num7, num8))
            {
                num9 = 2;
            }
            else if ((double)num6 == (double)MathUtils.Max(num5, num6, num7, num8))
            {
                num9 = 3;
            }
            else if ((double)num7 == (double)MathUtils.Max(num5, num6, num7, num8))
            {
                num9 = 0;
            }
            else if ((double)num8 == (double)MathUtils.Max(num5, num6, num7, num8))
            {
                num9 = 1;
            }

            Point3 point = default(Point3);
            point.X = Start.Y;
            Point3 point2 = default(Point3);
            point2.X = End.Y;
            point.Y = Start.Z;
            point2.Y = End.Z;
            point.Z = Start.X;
            point2.Z = End.X;
            int num10;
            switch (num3)
            {
                case 0:
                    point.X = Start.Y;
                    point2.X = End.Y;
                    point.Y = Start.Z;
                    point2.Y = End.Z;
                    point.Z = Start.X;
                    point2.Z = End.X;
                    num10 = 2310;
                    break;
                case 1:
                    point.X = Start.Y;
                    point2.X = End.Y;
                    point.Y = Start.X;
                    point2.Y = End.X;
                    point.Z = End.Z;
                    point2.Z = Start.Z;
                    num10 = 2130;
                    break;
                case 2:
                    point.X = Start.Y;
                    point2.X = End.Y;
                    point.Y = End.Z;
                    point2.Y = Start.Z;
                    point.Z = End.X;
                    point2.Z = Start.X;
                    num10 = 2310;
                    break;
                case 3:
                    point.X = Start.Y;
                    point2.X = End.Y;
                    point.Y = End.X;
                    point2.Y = Start.X;
                    point.Z = Start.Z;
                    point2.Z = End.Z;
                    num10 = 2130;
                    break;
                case 4:
                    switch (num9)
                    {
                        case 0:
                            point.X = End.Z;
                            point2.X = Start.Z;
                            point.Y = Start.Y;
                            point2.Y = End.Y;
                            point.Z = Start.X;
                            point2.Z = End.X;
                            num10 = 3214;
                            break;
                        case 1:
                            point.X = End.X;
                            point2.X = Start.X;
                            point.Y = Start.Y;
                            point2.Y = End.Y;
                            point.Z = End.Z;
                            point2.Z = Start.Z;
                            num10 = 1234;
                            break;
                        case 2:
                            point.X = Start.Z;
                            point2.X = End.Z;
                            point.Y = Start.Y;
                            point2.Y = End.Y;
                            point.Z = End.X;
                            point2.Z = Start.X;
                            num10 = 3214;
                            break;
                        case 3:
                            point.X = Start.X;
                            point2.X = End.X;
                            point.Y = Start.Y;
                            point2.Y = End.Y;
                            point.Z = Start.Z;
                            point2.Z = End.Z;
                            num10 = 1234;
                            break;
                        default:
                            point.X = End.Z;
                            point2.X = Start.Z;
                            point.Y = Start.Y;
                            point2.Y = End.Y;
                            point.Z = Start.X;
                            point2.Z = End.X;
                            num10 = 3214;
                            break;
                    }

                    break;
                case 5:
                    switch (num9)
                    {
                        case 0:
                            point.X = Start.Z;
                            point2.X = End.Z;
                            point.Y = End.Y;
                            point2.Y = Start.Y;
                            point.Z = Start.X;
                            point2.Z = End.X;
                            num10 = 3215;
                            break;
                        case 1:
                            point.X = Start.X;
                            point2.X = End.X;
                            point.Y = End.Y;
                            point2.Y = Start.Y;
                            point.Z = End.Z;
                            point2.Z = Start.Z;
                            num10 = 1235;
                            break;
                        case 2:
                            point.X = End.Z;
                            point2.X = Start.Z;
                            point.Y = End.Y;
                            point2.Y = Start.Y;
                            point.Z = End.X;
                            point2.Z = Start.X;
                            num10 = 3215;
                            break;
                        case 3:
                            point.X = End.X;
                            point2.X = Start.X;
                            point.Y = End.Y;
                            point2.Y = Start.Y;
                            point.Z = Start.Z;
                            point2.Z = End.Z;
                            num10 = 1235;
                            break;
                        default:
                            point.X = Start.Z;
                            point2.X = End.Z;
                            point.Y = End.Y;
                            point2.Y = Start.Y;
                            point.Z = Start.X;
                            point2.Z = End.X;
                            num10 = 3215;
                            break;
                    }

                    break;
                default:
                    point.X = Start.Y;
                    point2.X = End.Y;
                    point.Y = Start.Z;
                    point2.Y = End.Z;
                    point.Z = Start.X;
                    point2.Z = End.X;
                    num10 = 2319;
                    break;
            }

            FileStream fileStream = new FileStream(Path.Combine(directory, path), FileMode.Create);
            EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(fileStream, leaveOpen: true);
            engineBinaryWriter.Write($"This is the v{CreatorMain.version} version of the creator API");
            engineBinaryWriter.Write(CreatorMain.Numversion);
            engineBinaryWriter.Write('P');
            engineBinaryWriter.Write(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation);
            engineBinaryWriter.Write(num3 * 10 + num9);
            engineBinaryWriter.Write(Math.Abs(point2.X - point.X));
            engineBinaryWriter.Write(Math.Abs(point2.Y - point.Y));
            engineBinaryWriter.Write(Math.Abs(point2.Z - point.Z));
            int num11 = point.X;
            while ((point.X <= point2.X && num11 <= point2.X) || (point.X > point2.X && num11 >= point2.X))
            {
                int num12 = point.Y;
                while ((point.Y <= point2.Y && num12 <= point2.Y) || (point.Y > point2.Y && num12 >= point2.Y))
                {
                    int num13 = point.Z;
                    while ((point.Z <= point2.Z && num13 <= point2.Z) || (point.Z > point2.Z && num13 >= point2.Z))
                    {
                        int x;
                        int y;
                        int z;
                        switch (num10 / 10)
                        {
                            case 123:
                                x = num11;
                                y = num12;
                                z = num13;
                                break;
                            case 132:
                                x = num11;
                                z = num12;
                                y = num13;
                                break;
                            case 213:
                                y = num11;
                                x = num12;
                                z = num13;
                                break;
                            case 231:
                                y = num11;
                                z = num12;
                                x = num13;
                                break;
                            case 312:
                                z = num11;
                                x = num12;
                                y = num13;
                                break;
                            case 321:
                                z = num11;
                                y = num12;
                                x = num13;
                                break;
                            default:
                                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogc1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                                return result;
                        }

                        engineBinaryWriter.Write(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValueFast(x, y, z));
                        switch (Terrain.ExtractContents(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValueFast(x, y, z)))
                        {
                            case 186:
                                {
                                    engineBinaryWriter.Write('M');
                                    MemoryBankData memoryBankData = GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<MemoryBankData>>(throwOnError: true).GetBlockData(new Point3(x, y, z)) ?? new MemoryBankData();
                                    engineBinaryWriter.Write(memoryBankData.SaveString());
                                    break;
                                }
                            case 188:
                                {
                                    engineBinaryWriter.Write('T');
                                    TruthTableData truthTableData = GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<TruthTableData>>(throwOnError: true).GetBlockData(new Point3(x, y, z)) ?? new TruthTableData();
                                    engineBinaryWriter.Write(truthTableData.SaveString());
                                    break;
                                }
                            case 97:
                            case 98:
                            case 210:
                            case 211:
                                {
                                    engineBinaryWriter.Write('S');
                                    SignData signData = GameManager.Project.FindSubsystem<SubsystemSignBlockBehavior>(throwOnError: true).GetSignData(new Point3(x, y, z)) ?? new SignData();
                                    engineBinaryWriter.Write(signData.Lines[0] ?? "");
                                    engineBinaryWriter.Write(signData.Colors[0]);
                                    engineBinaryWriter.Write(signData.Lines[1] ?? "");
                                    engineBinaryWriter.Write(signData.Colors[1]);
                                    engineBinaryWriter.Write(signData.Lines[2] ?? "");
                                    engineBinaryWriter.Write(signData.Colors[2]);
                                    engineBinaryWriter.Write(signData.Lines[3] ?? "");
                                    engineBinaryWriter.Write(signData.Colors[3]);
                                    engineBinaryWriter.Write(signData.Url ?? "");
                                    break;
                                }
                            case 184:
                                {
                                    float? num14 = GameManager.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true).ReadPersistentVoltage(new Point3(x, y, z));
                                    if (!num14.HasValue)
                                    {
                                        engineBinaryWriter.Write(value: false);
                                        break;
                                    }

                                    engineBinaryWriter.Write(value: true);
                                    engineBinaryWriter.Write((double)num14.Value);
                                    break;
                                }
                        }

                        num++;
                        if (point.Z <= point2.Z)
                        {
                            num13++;
                        }
                        else if (point.Z > point2.Z)
                        {
                            num13--;
                        }
                    }

                    if (point.Y <= point2.Y)
                    {
                        num12++;
                    }
                    else if (point.Y > point2.Y)
                    {
                        num12--;
                    }
                }

                if (point.X <= point2.X)
                {
                    num11++;
                }
                else if (point.X > point2.X)
                {
                    num11--;
                }
            }

            result = fileStream.Length;
            engineBinaryWriter.Dispose();
            fileStream.Dispose();
            creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogc2"), num, result), Color.LightYellow, blinking: true, playNotificationSound: true);
            return result;
        }

        public static void PasetData(CreatorAPI creatorAPI, string path, Point3 Start, Point3 End)
        {
            long num = 0L;
            ChunkData chunkData = new ChunkData(creatorAPI);
            creatorAPI.revokeData = new ChunkData(creatorAPI);
            ComponentPlayer componentPlayer = creatorAPI.componentMiner.ComponentPlayer;
            Stream stream = new FileStream(path, FileMode.Open).CreateStream();
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream);
            new MemoryBankData();
            int[,] array = new int[3, 4]
            {
                {
                    6012345,
                    6123045,
                    6230145,
                    6301245
                },
                {
                    6415320,
                    6425031,
                    6435102,
                    6405213
                },
                {
                    6514302,
                    6524013,
                    6534120,
                    6504231
                }
            };
            int[,] array2 = new int[3, 4]
            {
                {
                    60123,
                    60123,
                    60123,
                    60123
                },
                {
                    62301,
                    63012,
                    60123,
                    61230
                },
                {
                    62301,
                    61230,
                    60123,
                    63012
                }
            };
            bool flag = false;
            int num2 = 0;
            string str = engineBinaryReader.ReadString();
            int num3 = engineBinaryReader.ReadInt32();
            int[] sumnumversion = CreatorMain.Sumnumversion;
            for (int i = 0; i < sumnumversion.Length; i++)
            {
                if (sumnumversion[i] == num3)
                {
                    flag = true;
                    break;
                }
            }

            bool flag2 = false;
            sumnumversion = CreatorMain.Donotnumversion;
            for (int i = 0; i < sumnumversion.Length; i++)
            {
                if (sumnumversion[i] == num3)
                {
                    flag2 = true;
                    break;
                }
            }

            if (!flag)
            {
                if (flag2)
                {
                    DialogsManager.ShowDialog(componentPlayer.GuiWidget, new MessageDialog("Version control", str + "\nNot compatible with current version!", "OK", null, null));
                }
                else
                {
                    DialogsManager.ShowDialog(componentPlayer.GuiWidget, new MessageDialog("Version control", str + "\nThis is a discarded version!", "OK", null, null));
                }

                return;
            }

            engineBinaryReader.ReadChar();
            engineBinaryReader.ReadStruct<Quaternion>();
            int num4 = engineBinaryReader.ReadInt32();
            int num5 = Math.Abs(engineBinaryReader.ReadInt32());
            int num6 = Math.Abs(engineBinaryReader.ReadInt32());
            int num7 = Math.Abs(engineBinaryReader.ReadInt32());
            Vector3 forward = Matrix.CreateFromQuaternion(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation).Forward;
            float num8 = float.NegativeInfinity;
            int num9 = 0;
            for (int j = 0; j < 6; j++)
            {
                float num10 = Vector3.Dot(CellFace.FaceToVector3(j), forward);
                if ((double)num10 > (double)num8)
                {
                    num8 = num10;
                    num9 = j;
                }
            }

            float num11 = Vector3.Dot(forward, -Vector3.UnitZ);
            float num12 = Vector3.Dot(forward, -Vector3.UnitX);
            float num13 = Vector3.Dot(forward, Vector3.UnitZ);
            float num14 = Vector3.Dot(forward, Vector3.UnitX);
            int num15 = 0;
            if ((double)num11 == (double)MathUtils.Max(num11, num12, num13, num14))
            {
                num15 = 2;
            }
            else if ((double)num12 == (double)MathUtils.Max(num11, num12, num13, num14))
            {
                num15 = 3;
            }
            else if ((double)num13 == (double)MathUtils.Max(num11, num12, num13, num14))
            {
                num15 = 0;
            }
            else if ((double)num14 == (double)MathUtils.Max(num11, num12, num13, num14))
            {
                num15 = 1;
            }

            Point3 point = default(Point3);
            point.X = Start.Y;
            point.Y = Start.Z;
            point.Z = Start.X;
            Point3 point2 = default(Point3);
            point2.X = num5;
            point2.Y = num6;
            point2.Z = num7;
            int num16;
            switch (num9)
            {
                case 0:
                    point.X = Start.Y;
                    point2.X = Start.Y + num5;
                    point.Y = Start.Z;
                    point2.Y = Start.Z + num6;
                    point.Z = Start.X;
                    point2.Z = Start.X + num7;
                    if (MirrorBlockBehavior.IsMirror)
                    {
                        point.Z = Start.X;
                        point2.Z = Start.X - num7;
                    }

                    num16 = 2310;
                    break;
                case 1:
                    point.X = Start.Y;
                    point2.X = Start.Y + num5;
                    point.Y = Start.X;
                    point2.Y = Start.X + num6;
                    point.Z = Start.Z;
                    point2.Z = Start.Z - num7;
                    if (MirrorBlockBehavior.IsMirror)
                    {
                        point.Z = Start.Z;
                        point2.Z = Start.Z + num7;
                    }

                    num16 = 2130;
                    break;
                case 2:
                    point.X = Start.Y;
                    point2.X = Start.Y + num5;
                    point.Y = Start.Z;
                    point2.Y = Start.Z - num6;
                    point.Z = Start.X;
                    point2.Z = Start.X - num7;
                    if (MirrorBlockBehavior.IsMirror)
                    {
                        point.Z = Start.X;
                        point2.Z = Start.X + num7;
                    }

                    num16 = 2310;
                    break;
                case 3:
                    point.X = Start.Y;
                    point2.X = Start.Y + num5;
                    point.Y = Start.X;
                    point2.Y = Start.X - num6;
                    point.Z = Start.Z;
                    point2.Z = Start.Z + num7;
                    if (MirrorBlockBehavior.IsMirror)
                    {
                        point.Z = Start.Z;
                        point2.Z = Start.Z - num7;
                    }

                    num16 = 2130;
                    break;
                case 4:
                    switch (num15)
                    {
                        case 0:
                            point.X = Start.Z;
                            point2.X = Start.Z - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.X;
                            point2.Z = Start.X + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X - num7;
                            }

                            num16 = 3214;
                            break;
                        case 1:
                            point.X = Start.X;
                            point2.X = Start.X - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.Z;
                            point2.Z = Start.Z - num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.Z;
                                point2.Z = Start.Z + num7;
                            }

                            num16 = 1234;
                            break;
                        case 2:
                            point.X = Start.Z;
                            point2.X = Start.Z + num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.X;
                            point2.Z = Start.X - num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X + num7;
                            }

                            num16 = 3214;
                            break;
                        case 3:
                            point.X = Start.X;
                            point2.X = Start.X + num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.Z;
                            point2.Z = Start.Z + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.Z;
                                point2.Z = Start.Z - num7;
                            }

                            num16 = 1234;
                            break;
                        default:
                            point.X = Start.Z;
                            point2.X = Start.Z - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.X;
                            point2.Z = Start.X + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X - num7;
                            }

                            num16 = 3214;
                            break;
                    }

                    break;
                case 5:
                    switch (num15)
                    {
                        case 0:
                            point.X = Start.Z;
                            point2.X = Start.Z + num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y - num6;
                            point.Z = Start.X;
                            point2.Z = Start.X + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X - num7;
                            }

                            num16 = 3214;
                            break;
                        case 1:
                            point.X = Start.X;
                            point2.X = Start.X + num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y - num6;
                            point.Z = Start.Z;
                            point2.Z = Start.Z - num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.Z;
                                point2.Z = Start.Z + num7;
                            }

                            num16 = 1234;
                            break;
                        case 2:
                            point.X = Start.Z;
                            point2.X = Start.Z - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y - num6;
                            point.Z = Start.X;
                            point2.Z = Start.X - num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X + num7;
                            }

                            num16 = 3214;
                            break;
                        case 3:
                            point.X = Start.X;
                            point2.X = Start.X - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y - num6;
                            point.Z = Start.Z;
                            point2.Z = Start.Z + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.Z;
                                point2.Z = Start.Z - num7;
                            }

                            num16 = 1234;
                            break;
                        default:
                            point.X = Start.Z;
                            point2.X = Start.Z - num5;
                            point.Y = Start.Y;
                            point2.Y = Start.Y + num6;
                            point.Z = Start.X;
                            point2.Z = Start.X + num7;
                            if (MirrorBlockBehavior.IsMirror)
                            {
                                point.Z = Start.X;
                                point2.Z = Start.X - num7;
                            }

                            num16 = 3214;
                            break;
                    }

                    break;
                default:
                    point.X = Start.Y;
                    point2.X = Start.Y + num5;
                    point.Y = Start.Z;
                    point2.Y = Start.Z + num6;
                    point.Z = Start.X;
                    point2.Z = Start.X + num7;
                    if (MirrorBlockBehavior.IsMirror)
                    {
                        point.Z = Start.X;
                        point2.Z = Start.X - num7;
                    }

                    num16 = 2310;
                    break;
            }

            int num17;
            switch (num9)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    num17 = 0;
                    break;
                case 4:
                    num17 = 1;
                    break;
                case 5:
                    num17 = 2;
                    break;
                default:
                    num17 = 0;
                    break;
            }

            int num18;
            switch (num4 / 10)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    num18 = 0;
                    break;
                case 4:
                    num18 = 1;
                    break;
                case 5:
                    num18 = 2;
                    break;
                default:
                    num18 = 0;
                    break;
            }

            _ = array[num18, num4 % 10];
            _ = array[num17, num15];
            _ = array2[num18, num4 % 10];
            _ = array2[num17, num15];
            int num19 = point.X;
            while ((point.X <= point2.X && num19 <= point2.X) || (point.X > point2.X && num19 >= point2.X))
            {
                int num20 = point.Y;
                while ((point.Y <= point2.Y && num20 <= point2.Y) || (point.Y > point2.Y && num20 >= point2.Y))
                {
                    int num21 = point.Z;
                    while ((point.Z <= point2.Z && num21 <= point2.Z) || (point.Z > point2.Z && num21 >= point2.Z))
                    {
                        int x;
                        int y;
                        int z;
                        switch (num16 / 10)
                        {
                            case 123:
                                x = num19;
                                y = num20;
                                z = num21;
                                break;
                            case 132:
                                x = num19;
                                z = num20;
                                y = num21;
                                break;
                            case 213:
                                y = num19;
                                x = num20;
                                z = num21;
                                break;
                            case 231:
                                y = num19;
                                z = num20;
                                x = num21;
                                break;
                            case 312:
                                z = num19;
                                x = num20;
                                y = num21;
                                break;
                            case 321:
                                z = num19;
                                y = num20;
                                x = num21;
                                break;
                            default:
                                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogp1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
                                return;
                        }

                        if (!creatorAPI.launch)
                        {
                            return;
                        }

                        int num22 = engineBinaryReader.ReadInt32();
                        if (creatorAPI.AirIdentify || Terrain.ExtractContents(num22) != 0)
                        {
                            SetFaceAndRotate.GetFace(num22);
                            SetFaceAndRotate.GetRotate(num22);
                            num22 = SetFaceAndRotate.SetFace(num22, SetFaceAndRotate.GetFaceVector(num22, num4, num9 * 10 + num15, SetFaceAndRotate.GetFace(num22)));
                            num22 = SetFaceAndRotate.SetRotate(num22, SetFaceAndRotate.GetRotateVector(num22, num4, num9 * 10 + num15, SetFaceAndRotate.GetRotate(num22)));
                            SetFaceAndRotate.GetRotate(num22);
                            creatorAPI.CreateBlock(x, y, z, num22, chunkData);
                            switch (Terrain.ExtractContents(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValueFast(x, y, z)))
                            {
                                case 186:
                                    {
                                        engineBinaryReader.ReadChar();
                                        MemoryBankData memoryBankData = new MemoryBankData();
                                        memoryBankData.LoadString(engineBinaryReader.ReadString());
                                        GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<MemoryBankData>>(throwOnError: true).SetBlockData(new Point3(x, y, z), memoryBankData);
                                        break;
                                    }
                                case 188:
                                    {
                                        engineBinaryReader.ReadChar();
                                        TruthTableData truthTableData = new TruthTableData();
                                        truthTableData.LoadString(engineBinaryReader.ReadString());
                                        GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<TruthTableData>>(throwOnError: true).SetBlockData(new Point3(x, y, z), truthTableData);
                                        break;
                                    }
                                case 97:
                                case 98:
                                case 210:
                                case 211:
                                    {
                                        engineBinaryReader.ReadChar();
                                        SignData signData = new SignData();
                                        signData.Lines[0] = engineBinaryReader.ReadString();
                                        signData.Colors[0] = engineBinaryReader.ReadColor();
                                        signData.Lines[1] = engineBinaryReader.ReadString();
                                        signData.Colors[1] = engineBinaryReader.ReadColor();
                                        signData.Lines[2] = engineBinaryReader.ReadString();
                                        signData.Colors[2] = engineBinaryReader.ReadColor();
                                        signData.Lines[3] = engineBinaryReader.ReadString();
                                        signData.Colors[3] = engineBinaryReader.ReadColor();
                                        signData.Url = engineBinaryReader.ReadString();
                                        GameManager.Project.FindSubsystem<SubsystemSignBlockBehavior>(throwOnError: true).SetSignData(new Point3(x, y, z), signData.Lines, signData.Colors, signData.Url);
                                        break;
                                    }
                                case 184:
                                    if (engineBinaryReader.ReadBoolean())
                                    {
                                        GameManager.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true).WritePersistentVoltage(new Point3(x, y, z), (float)engineBinaryReader.ReadDouble());
                                    }

                                    break;
                            }

                            num2++;
                        }

                        if (point.Z <= point2.Z)
                        {
                            num21++;
                        }
                        else if (point.Z > point2.Z)
                        {
                            num21--;
                        }
                    }

                    if (point.Y <= point2.Y)
                    {
                        num20++;
                    }
                    else if (point.Y > point2.Y)
                    {
                        num20--;
                    }
                }

                if (point.X <= point2.X)
                {
                    num19++;
                }
                else if (point.X > point2.X)
                {
                    num19--;
                }
            }

            num = stream.Length;
            engineBinaryReader.Dispose();
            stream.Dispose();
            chunkData.Render();
            creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogp2"), num2, num), Color.LightYellow, blinking: true, playNotificationSound: true);
        }

        public static void ExportCopywMod2(string path, string exportFile)
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
            string text = "" + $"{num}\n{num2}\n{num3}";
            for (int i = 0; i <= num; i++)
            {
                for (int j = 0; j <= num2; j++)
                {
                    for (int k = 0; k <= num3; k++)
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

        public static void ImportCopywMod2(string path, string importFile)
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

        public static void ExportCopywMod(string path, string exportFile)
        {
            if (path.IsFileInUse())
            {
                throw new FileLoadException("capadialogex");
            }

            Stream stream = new FileStream(exportFile, FileMode.Create);
            Stream stream2 = File.Open(path, FileMode.Open);
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream2);
            string str = "";
            int num = engineBinaryReader.ReadInt32();
            int num2 = engineBinaryReader.ReadInt32();
            int num3 = engineBinaryReader.ReadInt32();
            string text = str + $"{num},{num2},{num3}";
            for (int i = 0; i <= num; i++)
            {
                for (int j = 0; j <= num2; j++)
                {
                    for (int k = 0; k <= num3; k++)
                    {
                        int num4 = engineBinaryReader.ReadInt32();
                        if (Terrain.ExtractContents(num4) != 0)
                        {
                            text += $"\n{i},{j},{k},{num4}";
                        }
                    }
                }
            }

            stream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetBytes(text).Length);
            engineBinaryReader.Dispose();
            stream2.Dispose();
            stream.Dispose();
        }

        public static void ImportCopywMod(string path, string importFile)
        {
            List<string> list = new List<string>();
            FileStream fileStream = File.OpenRead(importFile);
            StreamReader streamReader = new StreamReader(fileStream);
            string text = streamReader.ReadToEnd();
            streamReader.Dispose();
            fileStream.Dispose();
            string text2 = text;
            char[] separator = new char[1]
            {
                '\n'
            };
            string[] array = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in array)
            {
                list.Add(item);
            }

            FileStream fileStream2 = new FileStream(path, FileMode.Create);
            EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(fileStream2, leaveOpen: true);
            string[] array2 = list[0].Split(new char[1]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries);
            int num = int.Parse(array2[0]);
            int num2 = int.Parse(array2[1]);
            int num3 = int.Parse(array2[2]);
            list.RemoveAt(0);
            engineBinaryWriter.Write(num);
            engineBinaryWriter.Write(num2);
            engineBinaryWriter.Write(num3);
            for (int j = 0; j <= num; j++)
            {
                for (int k = 0; k <= num2; k++)
                {
                    for (int l = 0; l <= num3; l++)
                    {
                        int value = 0;
                        for (int m = 0; m < list.Count; m++)
                        {
                            string[] array3 = list[m].Split(new char[1]
                            {
                                ','
                            }, StringSplitOptions.RemoveEmptyEntries);
                            if (int.Parse(array3[0]) == j && int.Parse(array3[1]) == k && int.Parse(array3[2]) == l)
                            {
                                value = int.Parse(array3[3]);
                                list.RemoveAt(m);
                            }
                        }

                        engineBinaryWriter.Write(value);
                    }
                }
            }

            engineBinaryWriter.Dispose();
            fileStream2.Dispose();
        }
    }
}