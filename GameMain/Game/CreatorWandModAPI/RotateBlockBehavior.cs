using Engine;
using Game;
using System;

namespace CreatorWandModAPI
{
    public enum BlockFace
    {
        ZPositive,
        XPositive,
        ZNegative,
        XNegative,
        YPositive,
        YNegative
    }
    public enum BlockRotate
    {
        Up,
        Right,
        Down,
        Left
    }
    public enum ViewVector
    {
        ZPositive = 0,
        XPositive = 1,
        ZNegative = 2,
        XNegative = 3,
        YPZPositive = 10,
        YPXPositive = 11,
        YPZNegative = 12,
        YPXNegative = 13,
        YNZPositive = 20,
        YNXPositive = 21,
        YNZNegative = 22,
        YNXNegative = 23,
    }
    public static class SetFaceAndRotate
    {
        public static Vector3 Vector3Normalize(Vector3 vector3)
        {
            if (MathUtils.Max(MathUtils.Abs(vector3.X), MathUtils.Abs(vector3.Y), MathUtils.Abs(vector3.Z)) == MathUtils.Abs(vector3.X))
            {
                if (MathUtils.Max(MathUtils.Abs(vector3.Y), MathUtils.Abs(vector3.Z)) == MathUtils.Abs(vector3.Y))
                    return Vector3.Normalize(Vector3.Round(new Vector3(vector3.X, 0f, 0f)));
            }

            if (MathUtils.Max(MathUtils.Abs(vector3.X), MathUtils.Abs(vector3.Y), MathUtils.Abs(vector3.Z)) == MathUtils.Abs(vector3.Y))
            {
                return Vector3.Normalize(Vector3.Round(new Vector3(0f, vector3.Y, 0f)));
            }

            return Vector3.Normalize(Vector3.Round(new Vector3(0f, 0f, vector3.Z)));
        }

        public static Matrix GetFaceFunction(Vector3 SrcFace, Vector3 DecFace, bool Mirror)
        {
            switch (CellFace.Vector3ToFace(SrcFace))
            {
                case 0:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationZ(0f);
                        case 1:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 3:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(90f);
                    }
                case 1:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationX(0f);
                        case 2:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 4:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                    }
                case 2:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 1:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationZ(0f);
                        case 3:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                    }
                case 3:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 2:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationX(0f);
                        case 4:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                    }
                case 4:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationY(0f);
                        default:
                            return Matrix.CreateRotationX((float)Math.PI);
                    }
                default:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationX((float)Math.PI);
                        default:
                            return Matrix.CreateRotationY(0f);
                    }
            }
        }

        public static Matrix GetRotateFunction(Vector3 SrcFace, Vector3 DecFace, Vector3 Cellface)
        {
            switch (CellFace.Vector3ToFace(SrcFace))
            {
                case 0:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            switch (CellFace.Vector3ToFace(Cellface))
                            {
                                case 0:
                                    return Matrix.CreateRotationY(0f);
                                case 1:
                                    return Matrix.CreateRotationY(0f);
                                case 2:
                                    return Matrix.CreateRotationY(0f);
                                case 3:
                                    return Matrix.CreateRotationY(0f);
                                case 4:
                                    return Matrix.CreateRotationY(0f);
                                default:
                                    return Matrix.CreateRotationY(0f);
                            }
                        case 1:
                            switch (CellFace.Vector3ToFace(Cellface))
                            {
                                case 0:
                                    return Matrix.CreateRotationY(0f);
                                case 1:
                                    return Matrix.CreateRotationY(0f);
                                case 2:
                                    return Matrix.CreateRotationY(0f);
                                case 3:
                                    return Matrix.CreateRotationY(0f);
                                case 4:
                                    return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                                default:
                                    return Matrix.CreateRotationY((float)Math.PI / 2f);
                            }
                        case 2:
                            switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                            {
                                case 0:
                                    return Matrix.CreateRotationY(0f);
                                case 1:
                                    return Matrix.CreateRotationY(0f);
                                case 2:
                                    return Matrix.CreateRotationY(0f);
                                case 3:
                                    return Matrix.CreateRotationY(0f);
                                case 4:
                                    return Matrix.CreateRotationY(0f);
                                default:
                                    return Matrix.CreateRotationY(0f);
                            }
                        case 3:
                            switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                            {
                                case 0:
                                    return Matrix.CreateRotationY(0f);
                                case 1:
                                    return Matrix.CreateRotationY(0f);
                                case 2:
                                    return Matrix.CreateRotationY(0f);
                                case 3:
                                    return Matrix.CreateRotationY(0f);
                                case 4:
                                    return Matrix.CreateRotationY(0f);
                                default:
                                    return Matrix.CreateRotationY(0f);
                            }
                        case 4:
                            switch (CellFace.Vector3ToFace(DecFace, 3))
                            {
                                case 0:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                case 1:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                case 2:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                default:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                            }
                        default:
                            switch (CellFace.Vector3ToFace(DecFace, 3))
                            {
                                case 0:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                case 1:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                case 2:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                                default:
                                    switch (CellFace.OppositeFace(CellFace.Vector3ToFace(Cellface)))
                                    {
                                        case 0:
                                            return Matrix.CreateRotationY(0f);
                                        case 1:
                                            return Matrix.CreateRotationY(0f);
                                        case 2:
                                            return Matrix.CreateRotationY(0f);
                                        case 3:
                                            return Matrix.CreateRotationY(0f);
                                        case 4:
                                            return Matrix.CreateRotationY(0f);
                                        default:
                                            return Matrix.CreateRotationY(0f);
                                    }
                            }
                    }
                case 1:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationX(0f);
                        case 2:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 4:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                    }
                case 2:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 1:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationZ(0f);
                        case 3:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                    }
                case 3:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case 2:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationX(0f);
                        case 4:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                    }
                case 4:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationY(0f);
                        default:
                            return Matrix.CreateRotationX((float)Math.PI);
                    }
                default:
                    switch (CellFace.Vector3ToFace(DecFace))
                    {
                        case 0:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case 1:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case 2:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case 3:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case 4:
                            return Matrix.CreateRotationX((float)Math.PI);
                        default:
                            return Matrix.CreateRotationY(0f);
                    }
            }
        }

        public static Matrix GetFaceFunction(BlockFace SrcFace, BlockFace DecFace)
        {
            switch (SrcFace)
            {
                case BlockFace.ZPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationZ(0f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(90f);
                    }
                case BlockFace.XPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationX(0f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                    }
                case BlockFace.ZNegative:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationZ(0f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                    }
                case BlockFace.XNegative:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationX(0f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                    }
                case BlockFace.YPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationY(0f);
                        default:
                            return Matrix.CreateRotationX((float)Math.PI);
                    }
                default:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX((float)Math.PI);
                        default:
                            return Matrix.CreateRotationY(0f);
                    }
            }
        }

        public static Matrix GetRotateFunction(BlockFace SrcFace, BlockFace DecFace)
        {
            switch (SrcFace)
            {
                case BlockFace.ZPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY(0f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationY(0f);
                    }
                case BlockFace.XPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationX(0f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                    }
                case BlockFace.ZNegative:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationZ(0f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                    }
                case BlockFace.XNegative:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationY((float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationY((float)Math.PI);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationY(-(float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationX(0f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        default:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                    }
                case BlockFace.YPositive:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationY(0f);
                        default:
                            return Matrix.CreateRotationX((float)Math.PI);
                    }
                default:
                    switch (DecFace)
                    {
                        case BlockFace.ZPositive:
                            return Matrix.CreateRotationX(-(float)Math.PI / 2f);
                        case BlockFace.XPositive:
                            return Matrix.CreateRotationZ((float)Math.PI / 2f);
                        case BlockFace.ZNegative:
                            return Matrix.CreateRotationX((float)Math.PI / 2f);
                        case BlockFace.XNegative:
                            return Matrix.CreateRotationZ(-(float)Math.PI / 2f);
                        case BlockFace.YPositive:
                            return Matrix.CreateRotationX((float)Math.PI);
                        default:
                            return Matrix.CreateRotationY(0f);
                    }
            }
        }

        public static Matrix GetFaceFunction(Vector3 SrcFace, Vector3 DecFace)
        {
            return CreatorWand2.CW2Matrix.CW2FromTwoVectors(SrcFace, DecFace);
        }
        //进行面计算,例如泥土块
        public static int GetFaceVector(int cellValue, Matrix Translate)
        {
            return CellFace.Vector3ToFace(Vector3Normalize(Vector3.TransformNormal(CellFace.FaceToVector3(GetFace(cellValue)), Translate)));
        }
        //进行旋转计算
        public static int GetRotateVector(int cellValue, Matrix Translate)
        {
            return CellFace.Vector3ToFace(Vector3Normalize(Vector3.TransformNormal(CellFace.FaceToVector3(GetFace(cellValue)), Translate)));
        }

        public static int Int_Indexof_Int(int Value, int Count)
        {
            if (Count > 9)
            {
                return -1;
            }

            int num = 0;
            int num2 = -1;
            while (Value != 0)
            {
                if (Value % 10 == Count)
                {
                    num2 = num;
                }

                Value /= 10;
                num++;
            }

            if (num2 != -1)
            {
                return num - num2 - 1;
            }

            return -1;
        }

        public static int GetFaceVector(int cellValue, int Src_Face_2, int Des_Face_2, int Src_Param)
        {
            int num = -1;
            switch (Src_Face_2 / 10)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Src_Face_2 %= 10;
                    break;
                case 4:
                    Src_Face_2 = 10 + Src_Face_2 % 10;
                    break;
                case 5:
                    Src_Face_2 = 20 + Src_Face_2 % 10;
                    break;
            }

            switch (Des_Face_2 / 10)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Des_Face_2 %= 10;
                    break;
                case 4:
                    Des_Face_2 = 10 + Des_Face_2 % 10;
                    break;
                case 5:
                    Des_Face_2 = 20 + Des_Face_2 % 10;
                    break;
            }

            int num2 = 0;
            int[,] array = new int[12, 4]
            {
                {
                    6012,
                    6102,
                    6012,
                    6102
                },
                {
                    6210,
                    6201,
                    6210,
                    6201
                },
                {
                    6210,
                    6201,
                    6210,
                    6201
                },
                {
                    6012377,
                    6123077,
                    6230177,
                    6301277
                },
                {
                    6717320,
                    6727031,
                    6737102,
                    6707213
                },
                {
                    6717302,
                    6727013,
                    6737120,
                    6707231
                },
                {
                    6012347,
                    6123047,
                    6230147,
                    6301247
                },
                {
                    6417320,
                    6427031,
                    6437102,
                    6407213
                },
                {
                    6714302,
                    6724013,
                    6734120,
                    6704231
                },
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
            switch (Terrain.ExtractContents(cellValue))
            {
                case 134:
                case 135:
                case 137:
                case 140:
                case 143:
                case 145:
                case 146:
                case 156:
                case 157:
                case 180:
                case 181:
                case 183:
                case 184:
                case 186:
                case 187:
                case 188:
                case 224:
                    num2 = 9;
                    break;
                case 141:
                case 142:
                case 144:
                    num2 = 9;
                    break;
                case 139:
                case 147:
                case 151:
                case 152:
                case 179:
                case 182:
                case 253:
                case 254:
                    num2 = 9;
                    break;
                case 56:
                case 59:
                case 84:
                case 120:
                case 121:
                case 132:
                case 166:
                case 185:
                case 197:
                case 199:
                case 227:
                    num2 = 3;
                    break;
                case 31:
                    if (Src_Param == 4)
                    {
                        return 4;
                    }

                    num2 = 3;
                    break;
                case 237:
                    num2 = 9;
                    break;
                case 153:
                case 154:
                case 155:
                case 223:
                case 243:
                    num2 = 0;
                    break;
                case 9:
                case 10:
                case 11:
                case 255:
                    switch (Src_Param)
                    {
                        case 0:
                            Src_Param = 2;
                            break;
                        case 1:
                            Src_Param = 0;
                            break;
                        case 2:
                            Src_Param = 1;
                            break;
                    }

                    num = 9;
                    num2 = 0;
                    break;
                case 86:
                    num2 = 9;
                    break;
                case 133:
                    {
                        num2 = 9;
                        int num8 = 7000000 + ((Src_Param & 0x20) >> 5) * 600000 + ((Src_Param & 0x10) >> 4) * 50000 + ((Src_Param & 8) >> 3) * 4000 + ((Src_Param & 4) >> 2) * 300 + ((Src_Param & 2) >> 1) * 20 + (Src_Param & 1);
                        int num9 = 0;
                        int num10 = 0;
                        while (num10 < 6)
                        {
                            if (num10 + 1 == num8 % 10)
                            {
                                int num11 = 0;
                                switch (Int_Indexof_Int(array[num2 + Src_Face_2 / 10, Src_Face_2 % 10], num10))
                                {
                                    case 1:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100000 % 10;
                                        break;
                                    case 2:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10000 % 10;
                                        break;
                                    case 3:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1000 % 10;
                                        break;
                                    case 4:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100 % 10;
                                        break;
                                    case 5:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10 % 10;
                                        break;
                                    case 6:
                                        num11 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1 % 10;
                                        break;
                                }

                                if (MirrorBlockBehavior.IsMirror)
                                {
                                    int num12 = num11;
                                    switch (Des_Face_2 % 10)
                                    {
                                        case 0:
                                        case 2:
                                            switch (num12)
                                            {
                                                case 1:
                                                    num12 = 3;
                                                    break;
                                                case 3:
                                                    num12 = 1;
                                                    break;
                                            }

                                            break;
                                        case 1:
                                        case 3:
                                            switch (num12)
                                            {
                                                case 0:
                                                    num12 = 2;
                                                    break;
                                                case 2:
                                                    num12 = 0;
                                                    break;
                                            }

                                            break;
                                    }

                                    num11 = num12;
                                }

                                switch (num11)
                                {
                                    case 0:
                                        num9 |= 1;
                                        break;
                                    case 1:
                                        num9 |= 2;
                                        break;
                                    case 2:
                                        num9 |= 4;
                                        break;
                                    case 3:
                                        num9 |= 8;
                                        break;
                                    case 4:
                                        num9 |= 0x10;
                                        break;
                                    case 5:
                                        num9 |= 0x20;
                                        break;
                                }
                            }

                            num10++;
                            num8 /= 10;
                        }

                        return num9;
                    }
                case 94:
                case 163:
                case 164:
                case 193:
                case 202:
                    {
                        num2 = 3;
                        int num3 = 70000 + ((Src_Param & 8) >> 3) * 4000 + ((Src_Param & 4) >> 2) * 300 + ((Src_Param & 2) >> 1) * 20 + (Src_Param & 1);
                        int num4 = 0;
                        int num5 = 0;
                        while (num5 < 4)
                        {
                            if (num5 + 1 == num3 % 10)
                            {
                                int count = 0;
                                switch (num5)
                                {
                                    case 0:
                                        count = 1;
                                        break;
                                    case 1:
                                        count = 3;
                                        break;
                                    case 2:
                                        count = 0;
                                        break;
                                    case 3:
                                        count = 2;
                                        break;
                                }

                                int num6 = 0;
                                switch (Int_Indexof_Int(array[num2 + Src_Face_2 / 10, Src_Face_2 % 10], count))
                                {
                                    case 1:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100000 % 10;
                                        break;
                                    case 2:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10000 % 10;
                                        break;
                                    case 3:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1000 % 10;
                                        break;
                                    case 4:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100 % 10;
                                        break;
                                    case 5:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10 % 10;
                                        break;
                                    case 6:
                                        num6 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1 % 10;
                                        break;
                                }

                                if (MirrorBlockBehavior.IsMirror)
                                {
                                    int num7 = num6;
                                    switch (Des_Face_2 % 10)
                                    {
                                        case 0:
                                        case 2:
                                            switch (num7)
                                            {
                                                case 1:
                                                    num7 = 3;
                                                    break;
                                                case 3:
                                                    num7 = 1;
                                                    break;
                                            }

                                            break;
                                        case 1:
                                        case 3:
                                            switch (num7)
                                            {
                                                case 0:
                                                    num7 = 2;
                                                    break;
                                                case 2:
                                                    num7 = 0;
                                                    break;
                                            }

                                            break;
                                    }

                                    num6 = num7;
                                }

                                switch (num6)
                                {
                                    case 0:
                                        num4 |= 4;
                                        break;
                                    case 1:
                                        num4 |= 1;
                                        break;
                                    case 2:
                                        num4 |= 8;
                                        break;
                                    case 3:
                                        num4 |= 2;
                                        break;
                                }
                            }

                            num5++;
                            num3 /= 10;
                        }

                        return num4;
                    }
                case 48:
                case 49:
                case 50:
                case 51:
                case 69:
                case 76:
                case 96:
                case 217:
                    num = 48;
                    num2 = 3;
                    break;
            }

            if (num2 == 0)
            {
                int num13 = -1;
                switch (Int_Indexof_Int(array[num2 + Src_Face_2 / 10, Src_Face_2 % 10], Src_Param))
                {
                    case 1:
                        num13 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100 % 10;
                        break;
                    case 2:
                        num13 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10 % 10;
                        break;
                    case 3:
                        num13 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1 % 10;
                        break;
                }

                if (num == 9)
                {
                    switch (num13)
                    {
                        case 0:
                            num13 = 1;
                            break;
                        case 1:
                            num13 = 2;
                            break;
                        case 2:
                            num13 = 0;
                            break;
                    }
                }

                return num13;
            }

            int num14 = -1;
            switch (Int_Indexof_Int(array[num2 + Src_Face_2 / 10, Src_Face_2 % 10], Src_Param))
            {
                case 1:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100000 % 10;
                    break;
                case 2:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10000 % 10;
                    break;
                case 3:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1000 % 10;
                    break;
                case 4:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 100 % 10;
                    break;
                case 5:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 10 % 10;
                    break;
                case 6:
                    num14 = array[num2 + Des_Face_2 / 10, Des_Face_2 % 10] / 1 % 10;
                    break;
            }

            if (num == 48 && ((Terrain.ExtractData(cellValue) & 0x10) >> 4 == 1 || (Terrain.ExtractData(cellValue) & 8) >> 3 == 1))
            {
                int num15 = num14;
                if ((Src_Face_2 / 10 == 1 && Des_Face_2 / 10 == 2) || (Src_Face_2 / 10 == 2 && Des_Face_2 / 10 == 1))
                {
                    switch (Des_Face_2 % 10)
                    {
                        case 0:
                        case 2:
                            switch (num15)
                            {
                                case 3:
                                    num15 = 0;
                                    break;
                                case 2:
                                    num15 = 3;
                                    break;
                                case 1:
                                    num15 = 2;
                                    break;
                                case 0:
                                    num15 = 1;
                                    break;
                            }

                            break;
                        case 1:
                        case 3:
                            switch (num15)
                            {
                                case 3:
                                    num15 = 0;
                                    break;
                                case 0:
                                    num15 = 1;
                                    break;
                                case 1:
                                    num15 = 2;
                                    break;
                                case 2:
                                    num15 = 3;
                                    break;
                            }

                            break;
                    }
                }

                num14 = num15;
            }

            if (MirrorBlockBehavior.IsMirror)
            {
                int num16 = num14;
                if (num == 48 && ((Terrain.ExtractData(cellValue) & 0x10) >> 4 == 1 || (Terrain.ExtractData(cellValue) & 8) >> 3 == 1))
                {
                    if ((Src_Face_2 / 10 == 1 && Des_Face_2 / 10 == 2) || (Src_Face_2 / 10 == 2 && Des_Face_2 / 10 == 1))
                    {
                        switch (Des_Face_2 % 10)
                        {
                            case 0:
                            case 2:
                                switch (num16)
                                {
                                    case 2:
                                        num16 = 3;
                                        break;
                                    case 1:
                                        num16 = 0;
                                        break;
                                    case 3:
                                        num16 = 2;
                                        break;
                                    case 0:
                                        num16 = 1;
                                        break;
                                }

                                break;
                            case 1:
                            case 3:
                                switch (num16)
                                {
                                    case 3:
                                        num16 = 0;
                                        break;
                                    case 2:
                                        num16 = 1;
                                        break;
                                    case 1:
                                        num16 = 2;
                                        break;
                                    case 0:
                                        num16 = 3;
                                        break;
                                }

                                break;
                        }
                    }
                    else
                    {
                        switch (Des_Face_2 % 10)
                        {
                            case 0:
                            case 2:
                                switch (num16)
                                {
                                    case 3:
                                        num16 = 2;
                                        break;
                                    case 2:
                                        num16 = 3;
                                        break;
                                    case 1:
                                        num16 = 0;
                                        break;
                                    case 0:
                                        num16 = 1;
                                        break;
                                }

                                break;
                            case 1:
                            case 3:
                                switch (num16)
                                {
                                    case 3:
                                        num16 = 0;
                                        break;
                                    case 0:
                                        num16 = 3;
                                        break;
                                    case 1:
                                        num16 = 2;
                                        break;
                                    case 2:
                                        num16 = 1;
                                        break;
                                }

                                break;
                        }

                        Log.Warning("2");
                    }

                    num14 = num16;
                }
                else
                {
                    switch (Des_Face_2 % 10)
                    {
                        case 0:
                        case 2:
                            switch (num16)
                            {
                                case 1:
                                    num16 = 3;
                                    break;
                                case 3:
                                    num16 = 1;
                                    break;
                            }

                            break;
                        case 1:
                        case 3:
                            switch (num16)
                            {
                                case 0:
                                    num16 = 2;
                                    break;
                                case 2:
                                    num16 = 0;
                                    break;
                            }

                            break;
                    }

                    num14 = num16;
                }
            }

            return num14;
        }

        public static int GetRotateVector(int cellValue, int Src_Rotate_2, int Des_Rotate_2, int Src_Param)
        {
            switch (Src_Rotate_2 / 10)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Src_Rotate_2 /= 10;
                    break;
                case 4:
                    Src_Rotate_2 = 10 + Src_Rotate_2 % 10;
                    break;
                case 5:
                    Src_Rotate_2 = 20 + Src_Rotate_2 % 10;
                    break;
            }

            switch (Des_Rotate_2 / 10)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Des_Rotate_2 /= 10;
                    break;
                case 4:
                    Des_Rotate_2 = 10 + Des_Rotate_2 % 10;
                    break;
                case 5:
                    Des_Rotate_2 = 20 + Des_Rotate_2 % 10;
                    break;
            }

            int[,] array = new int[3, 4]
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
            int num = -1;
            switch (Terrain.ExtractContents(cellValue))
            {
                case 134:
                case 135:
                case 137:
                case 140:
                case 143:
                case 145:
                case 146:
                case 156:
                case 157:
                case 180:
                case 181:
                case 183:
                case 184:
                case 186:
                case 187:
                case 188:
                case 224:
                    num = 0;
                    break;
                case 48:
                case 49:
                case 50:
                case 51:
                case 69:
                case 76:
                case 96:
                case 217:
                    if ((Src_Rotate_2 / 10 == 1 && Des_Rotate_2 / 10 == 2) || (Src_Rotate_2 / 10 == 2 && Des_Rotate_2 / 10 == 1))
                    {
                        if (Src_Param != 1)
                        {
                            return 1;
                        }

                        return 0;
                    }

                    return Src_Param;
            }

            bool flag = false;
            switch (GetFace(cellValue))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    flag = true;
                    break;
                case 4:
                case 5:
                    flag = false;
                    break;
            }

            if ((num == 0) & ((((Src_Rotate_2 / 10 == 0) & (Des_Rotate_2 / 10 == 0)) && flag) | ((((Src_Rotate_2 / 10 == 1) & (Des_Rotate_2 / 10 == 1)) | ((Src_Rotate_2 / 10 == 2) & (Des_Rotate_2 / 10 == 2))) && flag)))
            {
                int num2 = Src_Param;
                if (MirrorBlockBehavior.IsMirror)
                {
                    int num3 = num2;
                    switch (Des_Rotate_2 / 10)
                    {
                        case 0:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num3)
                                    {
                                        case 1:
                                            num3 = 3;
                                            break;
                                        case 3:
                                            num3 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num3)
                                    {
                                        case 1:
                                            num3 = 3;
                                            break;
                                        case 3:
                                            num3 = 1;
                                            break;
                                    }

                                    break;
                            }

                            num2 = num3;
                            break;
                        case 1:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num3)
                                    {
                                        case 1:
                                            num3 = 3;
                                            break;
                                        case 3:
                                            num3 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num3)
                                    {
                                        case 2:
                                            num3 = 0;
                                            break;
                                        case 0:
                                            num3 = 2;
                                            break;
                                    }

                                    break;
                            }

                            num2 = num3;
                            break;
                        case 2:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num3)
                                    {
                                        case 1:
                                            num3 = 3;
                                            break;
                                        case 3:
                                            num3 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num3)
                                    {
                                        case 0:
                                            num3 = 2;
                                            break;
                                        case 2:
                                            num3 = 0;
                                            break;
                                    }

                                    break;
                            }

                            num2 = num3;
                            break;
                    }
                }

                return num2;
            }

            if (num == 0 && !flag)
            {
                int num4 = -1;
                switch (Int_Indexof_Int(array[num + Src_Rotate_2 / 10, Src_Rotate_2 % 10], Src_Param))
                {
                    case 1:
                        num4 = array[num + Des_Rotate_2 / 10, Des_Rotate_2 % 10] / 1000 % 10;
                        break;
                    case 2:
                        num4 = array[num + Des_Rotate_2 / 10, Des_Rotate_2 % 10] / 100 % 10;
                        break;
                    case 3:
                        num4 = array[num + Des_Rotate_2 / 10, Des_Rotate_2 % 10] / 10 % 10;
                        break;
                    case 4:
                        num4 = array[num + Des_Rotate_2 / 10, Des_Rotate_2 % 10] / 1 % 10;
                        break;
                }

                if (MirrorBlockBehavior.IsMirror)
                {
                    int num5 = num4;
                    switch (Des_Rotate_2 / 10)
                    {
                        case 0:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num5)
                                    {
                                        case 1:
                                            num5 = 3;
                                            break;
                                        case 3:
                                            num5 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num5)
                                    {
                                        case 1:
                                            num5 = 3;
                                            break;
                                        case 3:
                                            num5 = 1;
                                            break;
                                    }

                                    break;
                            }

                            num4 = num5;
                            break;
                        case 1:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num5)
                                    {
                                        case 1:
                                            num5 = 3;
                                            break;
                                        case 3:
                                            num5 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num5)
                                    {
                                        case 2:
                                            num5 = 0;
                                            break;
                                        case 0:
                                            num5 = 2;
                                            break;
                                    }

                                    break;
                            }

                            num4 = num5;
                            break;
                        case 2:
                            switch (Des_Rotate_2 % 10)
                            {
                                case 0:
                                case 2:
                                    switch (num5)
                                    {
                                        case 1:
                                            num5 = 3;
                                            break;
                                        case 3:
                                            num5 = 1;
                                            break;
                                    }

                                    break;
                                case 1:
                                case 3:
                                    switch (num5)
                                    {
                                        case 0:
                                            num5 = 2;
                                            break;
                                        case 2:
                                            num5 = 0;
                                            break;
                                    }

                                    break;
                            }

                            num4 = num5;
                            break;
                    }
                }

                return num4;
            }

            return 0;
        }
        public static int GetFace(int cellValue)
        {
            switch (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)])
            {
                case BottomSuckerBlock _:
                    return BottomSuckerBlock.GetFace(Terrain.ExtractData(cellValue));
                case SwitchBlock _:
                case PressurePlateBlock _:
                    return (Terrain.ExtractData(cellValue) >> 1) & 7;
                case SpikedPlankBlock _:
                    return SpikedPlankBlock.GetMountingFace(Terrain.ExtractData(cellValue));

                case MotionDetectorBlock _:
                case ButtonBlock _:
                case LightbulbBlock _:
                case FourLedBlock _:
                case LedBlock _:
                case MulticoloredLedBlock _:
                case OneLedBlock _:
                case PhotodiodeBlock _:
                case DetonatorBlock _:
                case TorchBlock _:
                case DispenserBlock _:
                case PostedSignBlock _:
                    return (Terrain.ExtractData(cellValue) & 7);
                case FenceBlock _:
                    return Terrain.ExtractData(cellValue) & 15;
                case SevenSegmentDisplayBlock _:
                case TargetBlock _:
                case LadderBlock _:
                case FenceGateBlock _:
                case FurnitureBlock _:
                case JackOLanternBlock _:
                case IvyBlock _:
                case DoorBlock _:
                case TrapdoorBlock _:
                case ThermometerBlock _:
                case HygrometerBlock _:
                case WireThroughBlock _:
                case WoodBlock _:
                case StairsBlock _:
                case FurnaceBlock _:
                case ChestBlock _:
                case AttachedSignBlock _:
                    return Terrain.ExtractData(cellValue) & 3;
                case PistonBlock _:
                    return (Terrain.ExtractData(cellValue) >> 3) & 7;
                case WireBlock _:
                    return Terrain.ExtractData(cellValue) & 0x3F;
                case GravestoneBlock _:
                    return (Terrain.ExtractData(cellValue) & 8) >> 3;
                case MagnetBlock _:
                    return Terrain.ExtractData(cellValue) & 1;
                case RotateableMountedElectricElementBlock _:
                    return (Terrain.ExtractData(cellValue) >> 2) & 7;
                default:
                    return -1;
            }

        }
        public static int SetFace(int cellValue, int face)
        {
            switch (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)])
            {
                case BottomSuckerBlock _:
                    return Terrain.ReplaceData(cellValue, BottomSuckerBlock.SetFace(Terrain.ExtractData(cellValue), face));
                case SwitchBlock _:
                case PressurePlateBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -15) | ((face & 7) << 1));
                case SpikedPlankBlock _:
                    return Terrain.ReplaceData(cellValue, SpikedPlankBlock.SetMountingFace(Terrain.ExtractData(cellValue), face));
                case MotionDetectorBlock _:
                case ButtonBlock _:
                case LightbulbBlock _:
                case FourLedBlock _:
                case LedBlock _:
                case MulticoloredLedBlock _:
                case OneLedBlock _:
                case PhotodiodeBlock _:
                case DetonatorBlock _:
                case TorchBlock _:
                case DispenserBlock _:
                case PostedSignBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -8) | (face & 7));
                case FenceBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -16) | (face & 15));
                case SevenSegmentDisplayBlock _:
                case TargetBlock _:
                case LadderBlock _:
                case FenceGateBlock _:
                case FurnitureBlock _:
                case JackOLanternBlock _:
                case IvyBlock _:
                case DoorBlock _:
                case TrapdoorBlock _:
                case ThermometerBlock _:
                case HygrometerBlock _:
                case WireThroughBlock _:
                case WoodBlock _:
                case StairsBlock _:
                case FurnaceBlock _:
                case ChestBlock _:
                case AttachedSignBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -4) | (face & 3));
                case PistonBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -57) | ((face & 7) << 3));
                case WireBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -0x40) | (face & 0x3F));
                case GravestoneBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -65) | ((face & 8) << 3));
                case MagnetBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -2) | (face & 1));
                case RotateableMountedElectricElementBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -29) | ((face & 7) << 2));
                default:
                    return cellValue;
            }

        }
        public static int GetRotate(int cellValue)
        {
            switch (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)])
            {
                case RotateableMountedElectricElementBlock _:
                    return Terrain.ExtractData(cellValue) & 3;
                case StairsBlock _:
                    return (Terrain.ExtractData(cellValue) & 4) >> 2;
                case TrapdoorBlock _:
                    return (Terrain.ExtractData(cellValue) & 8) >> 3;
                case SlabBlock _:
                    return Terrain.ExtractData(cellValue) & 1;
                default: return -1;
            }

        }
        public static int SetRotate(int cellValue, int Rotate)
        {
            switch (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)])
            {
                case RotateableMountedElectricElementBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -4) | (Rotate & 3));
                case StairsBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -5) | ((Rotate & 1) << 1));
                case TrapdoorBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -9) | ((Rotate & 8) << 3));
                case SlabBlock _:
                    return Terrain.ReplaceData(cellValue, (Terrain.ExtractData(cellValue) & -2) | (Rotate & 1));
                default: return cellValue;
            }

        }
    }
}