//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using CreatorWandModAPI;
using Engine;
using Engine.Serialization;
using Game;
using System;
//using System.Collections.Generic;
using System.IO;
using System.Text;
using Color = Engine.Color;

namespace CreatorWand2
{
    public static class Cw2CopyAndPaste
    {
        public static event Action<Terrain, Point3> CopyBlock;
        public static event Action<SubsystemTerrain, Point3, Matrix> PasteBlock;
        public static Point3 GetPoint3Num(Quaternion quaternion, Point3 PMin, Point3 PMax)
        {
            CW2BaseFunction.PointSort(ref PMin, ref PMax);
            Point3 Right = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetRightVector()).X));
            Point3 Forward = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetForwardVector()).X));
            Point3 Up = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetUpVector()).X));
            return new Point3(MathUtils.Abs((Right * (PMax - PMin)).X + (Right * (PMax - PMin)).Y + (Right * (PMax - PMin)).Z), MathUtils.Abs((Up * (PMax - PMin)).X + (Up * (PMax - PMin)).Y + (Up * (PMax - PMin)).Z), MathUtils.Abs((Forward * (PMax - PMin)).X + (Forward * (PMax - PMin)).Y + (Forward * (PMax - PMin)).Z));

        }
        public static void LoopCopy(Quaternion quaternion, Point3 PMin, Point3 PMax)
        {
            CW2BaseFunction.PointSort(ref PMin, ref PMax);
            //Log.Information(quaternion.GetRightVector());
            Point3 Right = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetRightVector()).X));
            Point3 Forward = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetForwardVector()).X));
            Point3 Up = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(-quaternion.GetUpVector()).X));
            bool RightB = (Right == new Point3(MathUtils.Abs(Right.X), MathUtils.Abs(Right.Y), MathUtils.Abs(Right.Z)));
            bool ForwardB = (Forward == new Point3(MathUtils.Abs(Forward.X), MathUtils.Abs(Forward.Y), MathUtils.Abs(Forward.Z)));
            bool UpB = (Up == new Point3(MathUtils.Abs(Up.X), MathUtils.Abs(Up.Y), MathUtils.Abs(Up.Z)));
            Point3 RightAbs = new Point3(MathUtils.Abs(Right.X), MathUtils.Abs(Right.Y), MathUtils.Abs(Right.Z));
            Point3 ForwardAbs = new Point3(MathUtils.Abs(Forward.X), MathUtils.Abs(Forward.Y), MathUtils.Abs(Forward.Z));
            Point3 UpAbs = new Point3(MathUtils.Abs(Up.X), MathUtils.Abs(Up.Y), MathUtils.Abs(Up.Z));
            //面向0，添负为2，R+分号前，F-分好后，U+分号前
            //Log.Information("U" + Up + " F" + Forward + " R" + Right);

            //交换负号
            Point3 tempi1 = new Point3(!RightB ? (RightAbs.X > 0 ? PMax.X : PMin.X) : PMin.X, !RightB ? (RightAbs.Y > 0 ? PMax.Y : PMin.Y) : PMin.Y, !RightB ? (RightAbs.Z > 0 ? PMax.Z : PMin.Z) : PMin.Z);
            Point3 tempa1 = new Point3(!RightB ? (RightAbs.X > 0 ? PMin.X : PMax.X) : PMax.X, !RightB ? (RightAbs.Y > 0 ? PMin.Y : PMax.Y) : PMax.Y, !RightB ? (RightAbs.Z > 0 ? PMin.Z : PMax.Z) : PMax.Z);

            Point3 tempi2 = new Point3(ForwardB ? (ForwardAbs.X > 0 ? tempa1.X : tempi1.X) : tempi1.X, ForwardB ? (ForwardAbs.Y > 0 ? tempa1.Y : tempi1.Y) : tempi1.Y, ForwardB ? (ForwardAbs.Z > 0 ? tempa1.Z : tempi1.Z) : tempi1.Z);
            Point3 tempa2 = new Point3(ForwardB ? (ForwardAbs.X > 0 ? tempi1.X : tempa1.X) : tempa1.X, ForwardB ? (ForwardAbs.Y > 0 ? tempi1.Y : tempa1.Y) : tempa1.Y, ForwardB ? (ForwardAbs.Z > 0 ? tempi1.Z : tempa1.Z) : tempa1.Z);

            Point3 tempi3 = new Point3(UpB ? (UpAbs.X > 0 ? tempa2.X : tempi2.X) : tempi2.X, UpB ? (UpAbs.Y > 0 ? tempa2.Y : tempi2.Y) : tempi2.Y, UpB ? (UpAbs.Z > 0 ? tempa2.Z : tempi2.Z) : tempi2.Z);
            Point3 tempa3 = new Point3(UpB ? (UpAbs.X > 0 ? tempi2.X : tempa2.X) : tempa2.X, UpB ? (UpAbs.Y > 0 ? tempi2.Y : tempa2.Y) : tempa2.Y, UpB ? (UpAbs.Z > 0 ? tempi2.Z : tempa2.Z) : tempa2.Z);
            if (CW2Matrix.Vector3ToFace(quaternion.GetForwardVector()).X == 4 || CW2Matrix.Vector3ToFace(quaternion.GetForwardVector()).X == 5)
            {
                if (CW2Matrix.Vector3ToFace(quaternion.GetUpVector()).X == 2 || CW2Matrix.Vector3ToFace(quaternion.GetUpVector()).X == 3)
                {
                    Point3 tempi4 = new Point3(RightAbs.X > 0 ? tempa3.X : tempi3.X, RightAbs.Y > 0 ? tempa3.Y : tempi3.Y, RightAbs.Z > 0 ? tempa3.Z : tempi3.Z);
                    Point3 tempa4 = new Point3(RightAbs.X > 0 ? tempi3.X : tempa3.X, RightAbs.Y > 0 ? tempi3.Y : tempa3.Y, RightAbs.Z > 0 ? tempi3.Z : tempa3.Z);
                    tempi3 = tempi4; tempa3 = tempa4;
                    Point3 tempi5 = new Point3(ForwardAbs.X > 0 ? tempa3.X : tempi3.X, ForwardAbs.Y > 0 ? tempa3.Y : tempi3.Y, ForwardAbs.Z > 0 ? tempa3.Z : tempi3.Z);
                    Point3 tempa5 = new Point3(ForwardAbs.X > 0 ? tempi3.X : tempa3.X, ForwardAbs.Y > 0 ? tempi3.Y : tempa3.Y, ForwardAbs.Z > 0 ? tempi3.Z : tempa3.Z);
                    tempi3 = tempi5; tempa3 = tempa5;
                    Point3 tempi6 = new Point3(UpAbs.X > 0 ? tempa3.X : tempi3.X, UpAbs.Y > 0 ? tempa3.Y : tempi3.Y, UpAbs.Z > 0 ? tempa3.Z : tempi3.Z);
                    Point3 tempa6 = new Point3(UpAbs.X > 0 ? tempi3.X : tempa3.X, UpAbs.Y > 0 ? tempi3.Y : tempa3.Y, UpAbs.Z > 0 ? tempi3.Z : tempa3.Z);
                    tempi3 = tempi6; tempa3 = tempa6;
                }
            }
            /*Log.Information("PMin (" + tempi3 + ") +> PMax (" + tempa3 + ")");
            Log.Information("Join (" + (!UpB ? tempi3 : tempa3) + ") U(" + (-Up) + ") +> F(" + (-Forward) + ") +> R(" + (Right) + ")");*/
            for (Point3 i = !UpB ? tempi3 : tempa3; !UpB ? Point3.Max(UpAbs * i, UpAbs * Point3.Max(PMin, PMax)) == UpAbs * Point3.Max(PMin, PMax) : Point3.Min(UpAbs * i, UpAbs * Point3.Min(PMin, PMax)) == UpAbs * Point3.Min(PMin, PMax); i += -Up)
            {
                for (Point3 j = i; !ForwardB ? Point3.Max(ForwardAbs * j, ForwardAbs * Point3.Max(PMin, PMax)) == ForwardAbs * Point3.Max(PMin, PMax) : Point3.Min(ForwardAbs * j, ForwardAbs * Point3.Min(PMin, PMax)) == ForwardAbs * Point3.Min(PMin, PMax); j += -Forward)
                {
                    //for (Point3 k = j; !RightB ? Point3.Max(RightAbs * k, RightAbs * Point3.Max(PMin, PMax)) == RightAbs * Point3.Max(PMin, PMax) : Point3.Min(RightAbs * k, RightAbs * Point3.Min(PMin, PMax)) == RightAbs * Point3.Min(PMin, PMax); k += -Right)//靠右
                    for (Point3 k = j; RightB ? Point3.Max(RightAbs * k, RightAbs * Point3.Max(PMin, PMax)) == RightAbs * Point3.Max(PMin, PMax) : Point3.Min(RightAbs * k, RightAbs * Point3.Min(PMin, PMax)) == RightAbs * Point3.Min(PMin, PMax); k += Right)
                    {
                        CopyBlock?.Invoke(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain, k);

                    }
                }
            }
        }
        public static void LoopPaste(Quaternion quaternion, Point3 PStart, Point3 PNum, Matrix matrix)
        {
            Point3 Right = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(quaternion.GetRightVector()).X));
            Point3 Forward = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(quaternion.GetForwardVector()).X));
            Point3 Up = new Point3(CellFace.FaceToVector3(CW2Matrix.Vector3ToFace(quaternion.GetUpVector()).X));
            for (int i = 0; i <= PNum.Y; i++)
            {
                for (int j = 0; j <= PNum.Z; j++)
                {
                    for (int k = 0; k <= PNum.X; k++)
                    {
                        Point3 a;
                        if (MirrorBlockBehavior.IsMirror)
                        {
                            a = PStart + k * Right + j * Forward + i * Up;
                        }
                        else
                        {
                            a = PStart + k * -Right + j * Forward + i * Up;
                        }

                        //var a = PStart + k * Right + j * Forward + i * Up;//靠右
                        PasteBlock?.Invoke(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true), a, matrix);
                    }
                }

            }
        }
        public static long CreateCopy(CreatorAPI creatorAPI, string directory, string path, Point3 Start, Point3 End)
        {
            long result = 0L;
            int num = 0;


            //Point2 PFov = CW2Matrix.Vector3ToFace(-componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation.GetForwardVector());
            try
            {
                FileStream fileStream = new FileStream(Path.Combine(directory, path), FileMode.Create);
                EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(fileStream, leaveOpen: true);
                ComponentPlayer componentPlayer = creatorAPI.componentMiner.ComponentPlayer;
                Point2 PFov = CW2Matrix.Vector3ToFace(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation.GetForwardVector());

                engineBinaryWriter.Write($"This is the v{CreatorMain.version} version of the creator API");
                engineBinaryWriter.Write(CreatorMain.Numversion);
                engineBinaryWriter.Write('P');
                engineBinaryWriter.Write(PFov);
                engineBinaryWriter.Write(GetPoint3Num(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation, Start, End));
                void p(Terrain terrain, Point3 point3)
                {
                    //写入方块
                    var value = terrain.GetCellValueFast(point3.X, point3.Y, point3.Z);
                    engineBinaryWriter.Write(value);


                    switch (BlocksManager.Blocks[Terrain.ExtractContents(value)])
                    {
                        case FurnaceBlock _:
                        case ChestBlock _:
                        case CraftingTableBlock _:
                        case DispenserBlock _:
                            ComponentBlockEntity blockEntity = GameManager.Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true).GetBlockEntity(point3.X, point3.Y, point3.Z);
                            if (blockEntity != null)
                            {
                                foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
                                {
                                    engineBinaryWriter.Write(item.SlotsCount);
                                    for (int i = 0; i < item.SlotsCount; i++)
                                    {
                                        engineBinaryWriter.Write(item.GetSlotValue(i));
                                        engineBinaryWriter.Write(item.GetSlotCount(i));
                                    }


                                }
                            }
                            break;
                    }

                    switch (Terrain.ExtractContents(terrain.GetCellValueFast(point3.X, point3.Y, point3.Z)))
                    {
                        case 186:
                            {
                                engineBinaryWriter.Write('M');
                                MemoryBankData memoryBankData = GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<MemoryBankData>>(throwOnError: true).GetBlockData(point3) ?? new MemoryBankData();
                                engineBinaryWriter.Write(memoryBankData.SaveString());
                                break;
                            }
                        case 188:
                            {
                                engineBinaryWriter.Write('T');
                                TruthTableData truthTableData = GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<TruthTableData>>(throwOnError: true).GetBlockData(point3) ?? new TruthTableData();
                                engineBinaryWriter.Write(truthTableData.SaveString());
                                break;
                            }
                        case 97:
                        case 98:
                        case 210:
                        case 211:
                            {
                                engineBinaryWriter.Write('S');
                                SignData signData = GameManager.Project.FindSubsystem<SubsystemSignBlockBehavior>(throwOnError: true).GetSignData(point3) ?? new SignData();
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
                                float? num14 = GameManager.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true).ReadPersistentVoltage(point3);
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
                }
                CopyBlock += p;
                LoopCopy(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation, Start, End);
                CopyBlock -= p;
                result = fileStream.Length;
                engineBinaryWriter.Dispose();
                fileStream.Dispose();
                // creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogc1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogc2"), num, result), Color.LightYellow, blinking: true, playNotificationSound: true);

            }
            catch (Exception e)
            {
                Log.Error(e);
            }


            //写入格式
            /*
             *先写入版本名称
             *然后写入版本号（数字）
             *再写入分界符号P
             *写入角色朝向
             *再写入绝对值XYZ
             *写入块的值
             *写入特殊值
             */
            return result;
        }

        public static void PasetData(CreatorAPI creatorAPI, string path, Point3 Start, Point3 End)
        {
            long num = 0L;
            //写入格式
            /*
             *先写入版本名称
             *然后写入版本号（数字）
             *再写入分界符号P
             *写入角色朝向
             *再写入绝对值XYZ
             *写入块的值
             *写入特殊值
             */
            try
            {
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                ComponentPlayer componentPlayer = creatorAPI.componentMiner.ComponentPlayer;
                Stream stream = new FileStream(path, FileMode.Open).CreateStream();
                EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream);
                new MemoryBankData();
                bool flag = false;
                int num2 = 0;
                //($"This is the v{CreatorMain.version} version of the creator API");
                string str = engineBinaryReader.ReadString();
                //版本号控制(CreatorMain.Numversion)
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
                //分界符('P')
                char sddr = engineBinaryReader.ReadChar();
                //之前朝向<Point2>(PFov)
                Point2 PFovB = engineBinaryReader.ReadStruct<Point2>();
                //XYZ
                Point3 num5 = engineBinaryReader.ReadStruct<Point3>();
                //现在朝向
                Point2 PFovN = CW2Matrix.Vector3ToFace(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation.GetForwardVector());
                //Point2 PFovN = CW2Matrix.Vector3ToFace(-componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation.GetForwardVector());
                void q(SubsystemTerrain terrain, Point3 point3, Matrix matrix)
                {

                    int num22 = engineBinaryReader.ReadInt32();
                    if (creatorAPI.AirIdentify || Terrain.ExtractContents(num22) != 0)
                    {
                        Point2 point21 = new Point2(SetFaceAndRotate.GetFace(num22), SetFaceAndRotate.GetRotate(num22));
                        Point2 point22;
                        switch (BlocksManager.Blocks[Terrain.ExtractContents(num22)])
                        {
                            case WireBlock _:
                                int wire = 0;
                                int j = 0;
                                for (int i = 1; i <= 0x3F; i = i << 1)
                                {

                                    if ((point21.X & i) != 0)
                                    {
                                        wire |= (1 << (CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(new Point2(j, 0)), matrix))).X);
                                        //Log.Warning((CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(new Point2(j, 0)), matrix))).X+" is "+ wire);
                                    }
                                    j++;
                                }
                                num22 = SetFaceAndRotate.SetFace(num22, wire);
                                break;
                            case FenceBlock _:
                                int fence = 0;
                                int transf = 0;
                                for (int i = 1; i <= 0x15; i = i << 1)
                                {

                                    if ((point21.X & i) != 0)
                                    {
                                        switch (i)
                                        {
                                            case 1:
                                                transf = 0;
                                                break;
                                            case 2:
                                                transf = 2;
                                                break;
                                            case 4:
                                                transf = 3;
                                                break;
                                            case 8:
                                                transf = 1;
                                                break;
                                        }
                                        switch (CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(new Point2(transf, 0)), matrix)).X)
                                        {
                                            case 0:
                                                fence += 1;
                                                break;
                                            case 1:
                                                fence += 8;
                                                break;
                                            case 2:
                                                fence += 2;
                                                break;
                                            case 3:
                                                fence += 4;
                                                break;
                                        }

                                    }
                                }
                                num22 = SetFaceAndRotate.SetFace(num22, fence);
                                break;
                            default:
                                if (point21.X == -1/* || point21.X > 6*/)
                                {
                                    break;
                                }
                                if (point21.Y == -1)
                                {
                                    point21.Y = 0;
                                    point22 = CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(point21), matrix));
                                    num22 = SetFaceAndRotate.SetFace(num22, point22.X);
                                    break;
                                }
                                point22 = CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(point21), matrix));
                                num22 = SetFaceAndRotate.SetFace(num22, point22.X);
                                num22 = SetFaceAndRotate.SetRotate(num22, point22.Y);
                                break;
                        }
                        /*   switch (Terrain.ExtractContents(terrain.Terrain.GetCellValueFast(point3.X, point3.Y, point3.Z)))
                           {
                               case 133:
                                   Log.Information("Line");
                                   //num22 = SetFaceAndRotate.SetFace(num22, SetFaceAndRotate.GetFaceVector(num22, point21.X, PFovN.X * 10 + PFovN.Y, SetFaceAndRotate.GetFace(num22)));
                                   break;
                               default:
                                   if (point21.X == -1 || point21.X > 6)
                                   {
                                       break;
                                   }
                                   if (point21.Y == -1)
                                   {
                                       point21.Y = 0;
                                       point22 = CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(point21), matrix));
                                       num22 = SetFaceAndRotate.SetFace(num22, point22.X);
                                       break;
                                   }
                                   point22 = CW2Matrix.Vector3ToFace(Vector3.TransformNormal(CW2Matrix.FaceToVector3(point21), matrix));
                                   num22 = SetFaceAndRotate.SetFace(num22, point22.X);
                                   num22 = SetFaceAndRotate.SetRotate(num22, point22.Y);
                                   break;
                           }*/
                        CW2EntityManager.RemoveEntity(point3);
                        creatorAPI.CreateBlock(point3.X, point3.Y, point3.Z, num22, chunkData);

                        CW2EntityManager.AddEntity(point3);
                        //terrain.ChangeCell(point3.X, point3.Y, point3.Z, num22);
                        switch (BlocksManager.Blocks[Terrain.ExtractContents(num22)])
                        {
                            case FurnaceBlock _:
                            case ChestBlock _:
                            case CraftingTableBlock _:
                            case DispenserBlock _:
                                ComponentBlockEntity blockEntity = GameManager.Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true).GetBlockEntity(point3.X, point3.Y, point3.Z);
                                if (blockEntity != null)
                                {
                                    foreach (IInventory item in blockEntity.Entity.FindComponents<IInventory>())
                                    {
                                        int count = engineBinaryReader.ReadInt32();
                                        for (int i = 0; i < count; i++)
                                        {
                                            int SlotValue = engineBinaryReader.ReadInt32();
                                            int SlotCount = engineBinaryReader.ReadInt32();
                                            item.AddSlotItems(i, SlotValue, SlotCount);
                                        }


                                    }
                                }
                                break;
                        }
                        switch (Terrain.ExtractContents(num22))
                        {
                            case 186:
                                {
                                    engineBinaryReader.ReadChar();
                                    MemoryBankData memoryBankData = new MemoryBankData();
                                    memoryBankData.LoadString(engineBinaryReader.ReadString());
                                    GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<MemoryBankData>>(throwOnError: true).SetBlockData(point3, memoryBankData);
                                    break;
                                }
                            case 188:
                                {
                                    engineBinaryReader.ReadChar();
                                    TruthTableData truthTableData = new TruthTableData();
                                    truthTableData.LoadString(engineBinaryReader.ReadString());
                                    GameManager.Project.FindSubsystem<SubsystemEditableItemBehavior<TruthTableData>>(throwOnError: true).SetBlockData(point3, truthTableData);
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
                                    GameManager.Project.FindSubsystem<SubsystemSignBlockBehavior>(throwOnError: true).SetSignData(point3, signData.Lines, signData.Colors, signData.Url);
                                    break;
                                }
                            case 184:
                                if (engineBinaryReader.ReadBoolean())
                                {
                                    GameManager.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true).WritePersistentVoltage(point3, (float)engineBinaryReader.ReadDouble());
                                }

                                break;
                        }

                        num2++;
                    }


                }
                PasteBlock += q;
                switch (PFovB.X)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        PFovB.Y = 0;
                        if (PFovN.X == 4)
                        {
                            PFovB.Y = 2;
                        }
                        break;
                }
                LoopPaste(componentPlayer.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation, Start, num5, CW2Matrix.CW2FromTwoVectors(CW2Matrix.FaceToVector3(PFovB), CW2Matrix.FaceToVector3(PFovN)));
                PasteBlock -= q;

                num = stream.Length;
                engineBinaryReader.Dispose();
                stream.Dispose();
                chunkData.Render();
                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("capadialogp2"), num2, num), Color.LightYellow, blinking: true, playNotificationSound: true);

            }
            catch (Exception e)
            {

                Log.Error(e);
            }
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
            new System.Collections.Generic.List<string>();
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
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
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
