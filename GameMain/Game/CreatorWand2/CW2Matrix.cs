using Engine;
using Game;

namespace CreatorWand2
{
    public static class CW2Matrix
    {
        /*public static Matrix CW2FromTwoVectors(Vector3 Before, Vector3 After) 
        {
            
            return Matrix.CreateFromAxisAngle(Vector3.Normalize( Vector3.Cross(Before, After)), MathUtils.Acos(Vector3.Dot(Before, After) / Before.Length() / After.Length()));
        }*/
        public static Matrix CW2FromTwoVectors(Vector3 Before, Vector3 After)
        {
            Point2 PBefore = Vector3ToFace(Before);
            Point2 PAfter = Vector3ToFace(After);
            Log.Information(PBefore.ToString() + " -> " + PAfter.ToString());
            switch (PBefore.X)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    switch (PAfter.X)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            // Matrix rotate = Matrix.CreateFromAxisAngle(CellFace.FaceToVector3(PBefore.X), MathUtils.PI / 2f * ( PBefore.Y-PAfter.Y));
                            Matrix face1 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PAfter.X - PBefore.X));
                            return face1;
                        default:
                            switch (PAfter.X)
                            {
                                case 4:
                                    /* * Matrix.Invert(face2) */
                                    Matrix rotate2 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PBefore.Y - PAfter.Y));
                                    Matrix face2 = Matrix.CreateRotationY(MathUtils.PI / 2f * (0 - PBefore.X));
                                    return face2 * Matrix.CreateRotationX(-MathUtils.PI / 2f) * rotate2;
                                default:
                                    Matrix rotate3 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PAfter.Y - PBefore.Y));
                                    Matrix face3 = Matrix.CreateRotationY(MathUtils.PI / 2f * (0 - PBefore.X));
                                    return face3 * Matrix.CreateRotationX(MathUtils.PI / 2f) * rotate3;
                            }
                    }
                default:
                    switch (PBefore.X)
                    {
                        case 4:
                            switch (PAfter.X)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    Matrix rotate4 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PAfter.Y - PBefore.Y));
                                    Matrix face4 = Matrix.CreateRotationY(MathUtils.PI / 2f * (0 - PAfter.X));
                                    return Matrix.Invert(face4 * Matrix.CreateRotationX(-MathUtils.PI / 2f) * rotate4);
                                default:
                                    switch (PAfter.X)
                                    {
                                        case 4:
                                            Matrix rotate5 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PBefore.Y - PAfter.Y));
                                            return rotate5;
                                        default:
                                            Matrix rotate6 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PAfter.Y - PBefore.Y));
                                            return Matrix.CreateRotationX(MathUtils.PI) * rotate6;
                                    }
                            }
                        default:
                            switch (PAfter.X)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    Matrix rotate7 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PBefore.Y - PAfter.Y));
                                    Matrix face7 = Matrix.CreateRotationY(MathUtils.PI / 2f * (0 - PAfter.X));
                                    return Matrix.Invert(face7 * Matrix.CreateRotationX(MathUtils.PI / 2f) * rotate7);
                                default:
                                    switch (PAfter.X)
                                    {
                                        case 4:
                                            Matrix rotate8 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PBefore.Y - PAfter.Y));
                                            return Matrix.CreateRotationX(MathUtils.PI) * rotate8;
                                        default:
                                            Matrix rotate9 = Matrix.CreateRotationY(MathUtils.PI / 2f * (PAfter.Y - PBefore.Y));
                                            return rotate9;
                                    }
                            }
                    }
            }
        }
        public static Point2 Vector3ToFace(Vector3 v, int maxFace = 23)
        {
            Point2 result = new Point2();
            float num = float.NegativeInfinity;
            /*
            int result = 0;
            int maxresult=0;*/
            for (int i = 0; i <= maxFace; i++)
            {
                float num2 = Vector3.Dot(m_rotateToVector3[i], v);
                if (num2 > num)
                {
                    result.X = i / 4;
                    result.Y = i % 4;
                    num = num2;
                }
            }

            return result;
        }
        public static Vector3 FaceToVector3(Point2 face)
        {
            return m_rotateToVector3[face.X * 4 + face.Y];
        }
        public static readonly Vector3[] m_rotateToVector3 = new Vector3[24]
        {
            new Vector3(0f, 0.5f, 1f),
            new Vector3(0.5f, 0, 1f),
            new Vector3(0f, -0.5f, 1f),
            new Vector3(-0.5f, 0, 1f),

            new Vector3(1f, 0.5f, 0f),
            new Vector3(1f, 0f, -0.5f),
            new Vector3(1f, -0.5f, 0f),
            new Vector3(1f, 0f, 0.5f),

            new Vector3(0f, 0.5f, -1f),
            new Vector3(-0.5f, 0f, -1f),
            new Vector3(0f, -0.5f, -1f),
            new Vector3(0.5f, 0f, -1f),

            new Vector3(-1f, 0.5f, 0f),
            new Vector3(-1f, 0f, 0.5f),
            new Vector3(-1f, -0.5f, 0f),
            new Vector3(-1f, 0f, -0.5f),

            new Vector3(0f, 1f, -0.5f),
            new Vector3(0.5f, 1f, 0f),
            new Vector3(0f, 1f, 0.5f),
            new Vector3(-0.5f, 1f, 0f),

            new Vector3(0f, -1f, 0.5f),
            new Vector3(0.5f, -1f, 0f),
            new Vector3(0f, -1f, -0.5f),
            new Vector3(-0.5f, -1f, 0f)
        };
    }
}