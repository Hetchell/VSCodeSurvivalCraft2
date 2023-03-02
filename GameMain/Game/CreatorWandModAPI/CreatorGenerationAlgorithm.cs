using Engine;
using System;
using System.Collections.Generic;

namespace CreatorWandModAPI
{
    public class CreatorGenerationAlgorithm
    {
        public IEnumerable<Point3> TwoPointLineGeneration(Point3 startPoint, Point3 endPoint)
        {
            int lengin = Math.Max(MathUtils.Max(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y)), Math.Abs(startPoint.Z - endPoint.Z));
            int num = 0;
            while (num <= lengin)
            {
                yield return new Point3(startPoint.X + (int)Math.Round((double)num / (double)lengin * (double)(endPoint.X - startPoint.X)), startPoint.Y + (int)Math.Round((double)num / (double)lengin * (double)(endPoint.Y - startPoint.Y)), startPoint.Z + (int)Math.Round((double)num / (double)lengin * (double)(endPoint.Z - startPoint.Z)));
                int num2 = num + 1;
                num = num2;
            }
        }

        public IEnumerable<Point3> TwoPointLineGeneration2(Point3 startPoint, Point3 endPoint)
        {
            int lengin = Math.Max(MathUtils.Max(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y)), Math.Abs(startPoint.Z - endPoint.Z));
            float num = 0f;
            bool sc = false;
            for (; (double)num <= (double)lengin; num += 0.5f)
            {
                double mx = Math.Round((double)num / (double)lengin * (double)(endPoint.X - startPoint.X) + ((endPoint.X - startPoint.X > 0) ? 0.490000009536743 : (-0.490000009536743)));
                if ((int)mx != (int)Math.Round((double)num / (double)lengin * (double)(endPoint.X - startPoint.X)))
                {
                    sc = true;
                }

                double mz = Math.Round((double)num / (double)lengin * (double)(endPoint.Z - startPoint.Z) + ((!sc) ? ((endPoint.Z - startPoint.Z > 0) ? 0.00999999977648258 : (-0.00999999977648258)) : ((endPoint.Z - startPoint.Z < 0) ? 0.00999999977648258 : (-0.00999999977648258))));
                if (!sc && (int)mz == (int)Math.Round((double)num / (double)lengin * (double)(endPoint.X - startPoint.X)))
                {
                    mx += ((mx > 0.0) ? 1.0 : (-1.0));
                }

                yield return new Point3(startPoint.X + (int)mx, startPoint.Y + (int)Math.Round((double)num / (double)lengin * (double)(endPoint.Y - startPoint.Y) + ((endPoint.Y - startPoint.Y > 0) ? 0.00999999977648258 : (-0.00999999977648258))), startPoint.Z + (int)mz);
                Log.Error($"{(float)((double)startPoint.X + (double)num / (double)lengin * (double)(endPoint.X - startPoint.X))},{(float)((double)startPoint.Y + (double)num / (double)lengin * (double)(endPoint.Y - startPoint.Y))},{(float)((double)startPoint.Z + (double)num / (double)lengin * (double)(endPoint.Z - startPoint.Z))}");
                Log.Error($"Round : {mx},{Math.Round((double)num / (double)lengin * (double)(endPoint.Y - startPoint.Y) + ((endPoint.Y - startPoint.Y > 0) ? 0.00999999977648258 : (-0.00999999977648258)))},{mz}");
            }
        }

        public IEnumerable<Point3> Sphere(Vector3 Position, int XRadius, int YRadius, int ZRadius, bool Hollow = false, CreatorMain.CreateType? XYZType = null, bool typeBool = false)
        {
            int MaxRadius = Math.Max(Math.Max(XRadius, YRadius), ZRadius);
            int x = -XRadius;
            while (x <= XRadius)
            {
                int num;
                for (int y = -YRadius; y <= YRadius; y = num)
                {
                    for (int z = -ZRadius; z <= ZRadius; num = z + 1, z = num)
                    {
                        if ((int)Math.Sqrt((double)x * (double)MaxRadius / (double)XRadius * (double)x * (double)MaxRadius / (double)XRadius + (double)y * (double)MaxRadius / (double)YRadius * (double)y * (double)MaxRadius / (double)YRadius + (double)z * (double)MaxRadius / (double)ZRadius * (double)z * (double)MaxRadius / (double)ZRadius) > MaxRadius)
                        {
                            continue;
                        }

                        if (XYZType.HasValue)
                        {
                            if (XYZType == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if ((double)Position.X + (double)x < (double)Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if ((double)Position.X + (double)x > (double)Position.X)
                                {
                                    continue;
                                }
                            }
                            else if (XYZType == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if ((double)Position.Y + (double)y < (double)Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if ((double)Position.Y + (double)y > (double)Position.Y)
                                {
                                    continue;
                                }
                            }
                            else if (XYZType == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if ((double)Position.Z + (double)z < (double)Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if ((double)Position.Z + (double)z > (double)Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if (!Hollow || (int)Math.Sqrt((double)(Math.Abs(x) + 1) * (double)MaxRadius / (double)XRadius * (double)(Math.Abs(x) + 1) * (double)MaxRadius / (double)XRadius + (double)y * (double)MaxRadius / (double)YRadius * (double)y * (double)MaxRadius / (double)YRadius + (double)z * (double)MaxRadius / (double)ZRadius * (double)z * (double)MaxRadius / (double)ZRadius) > MaxRadius || (int)Math.Sqrt((double)x * (double)MaxRadius / (double)XRadius * (double)x * (double)MaxRadius / (double)XRadius + (double)(Math.Abs(y) + 1) * (double)MaxRadius / (double)YRadius * (double)(Math.Abs(y) + 1) * (double)MaxRadius / (double)YRadius + (double)z * (double)MaxRadius / (double)ZRadius * (double)z * (double)MaxRadius / (double)ZRadius) > MaxRadius || (int)Math.Sqrt((double)x * (double)MaxRadius / (double)XRadius * (double)x * (double)MaxRadius / (double)XRadius + (double)y * (double)MaxRadius / (double)YRadius * (double)y * (double)MaxRadius / (double)YRadius + (double)(Math.Abs(z) + 1) * (double)MaxRadius / (double)ZRadius * (double)(Math.Abs(z) + 1) * (double)MaxRadius / (double)ZRadius) > MaxRadius)
                        {
                            yield return new Point3((int)((double)Position.X + (double)x), (int)((double)Position.Y + (double)y), (int)((double)Position.Z + (double)z));
                        }
                    }

                    num = y + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Sphere(Point3 Position, int Radius, bool Hollow = false, CreatorMain.CreateType? XYZType = null, bool typeBool = false)
        {
            int MRadius = Radius * Radius;
            int x = -Radius;
            while (x <= Radius)
            {
                int num;
                for (int y = -Radius; y <= Radius; y = num)
                {
                    for (int z = -Radius; z <= Radius; num = z + 1, z = num)
                    {
                        if (x * x + y * y + z * z > MRadius)
                        {
                            continue;
                        }

                        if (XYZType.HasValue)
                        {
                            if (XYZType == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if (Position.X + x < Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.X + x > Position.X)
                                {
                                    continue;
                                }
                            }
                            else if (XYZType == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if (Position.Y + y < Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Y + y > Position.Y)
                                {
                                    continue;
                                }
                            }
                            else if (XYZType == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if (Position.Z + z < Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Z + z > Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if (!Hollow || (Math.Abs(x) + 1) * (Math.Abs(x) + 1) + y * y + z * z > MRadius || x * x + (Math.Abs(y) + 1) * (Math.Abs(y) + 1) + z * z > MRadius || x * x + y * y + (Math.Abs(z) + 1) * (Math.Abs(z) + 1) > MRadius)
                        {
                            yield return new Point3(Position.X + x, Position.Y + y, Position.Z + z);
                        }
                    }

                    num = y + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Sphere(Point3 Start, Point3 End, bool Hollow = false)
        {
            throw new Exception("no way");
        }

        public IEnumerable<Point3> Prism(Point3 Position, int Radius, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool Hollow = false, CreatorMain.CreateType? XYZtype = null, bool typeBool = false)
        {
            int x = -Radius;
            while (x <= Radius)
            {
                int num;
                for (int y = -Radius; y <= Radius; y = num)
                {
                    for (int z = -Radius; z <= Radius; num = z + 1, z = num)
                    {
                        if (XYZtype.HasValue)
                        {
                            if (XYZtype == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if (Position.X + x < Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.X + x > Position.X)
                                {
                                    continue;
                                }
                            }

                            if (XYZtype == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if (Position.Y + y < Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Y + y > Position.Y)
                                {
                                    continue;
                                }
                            }

                            if (XYZtype == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if (Position.Z + z < Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Z + z > Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if ((createType != CreatorMain.CreateType.Y || (Math.Abs(x) + Math.Abs(y) <= Radius && Math.Abs(z) + Math.Abs(y) <= Radius && (!Hollow || Math.Abs(x) + Math.Abs(y) >= Radius || Math.Abs(z) + Math.Abs(y) >= Radius))) && (createType != 0 || (Math.Abs(x) + Math.Abs(y) <= Radius && Math.Abs(z) + Math.Abs(x) <= Radius && (!Hollow || Math.Abs(x) + Math.Abs(y) >= Radius || Math.Abs(x) + Math.Abs(z) >= Radius))) && (createType != CreatorMain.CreateType.Z || (Math.Abs(z) + Math.Abs(y) <= Radius && Math.Abs(z) + Math.Abs(x) <= Radius && (!Hollow || Math.Abs(z) + Math.Abs(y) >= Radius || Math.Abs(z) + Math.Abs(x) >= Radius))))
                        {
                            yield return new Point3(Position.X + x, Position.Y + y, Position.Z + z);
                        }
                    }

                    num = y + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Pyramid(Point3 Position, int Radius, bool Hollow = false, CreatorMain.CreateType? XYZType = null, bool typeBool = false)
        {
            int x = -Radius;
            while (x <= Radius)
            {
                int num;
                for (int y = -Radius; y <= Radius; y = num)
                {
                    for (int z = -Radius; z <= Radius; num = z + 1, z = num)
                    {
                        if (XYZType.HasValue)
                        {
                            if (XYZType == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if (Position.X + x < Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.X + x > Position.X)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if (Position.Y + y < Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Y + y > Position.Y)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if (Position.Z + z < Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Z + z > Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if (Math.Abs(x) + Math.Abs(y) + Math.Abs(z) <= Radius && (!Hollow || Math.Abs(x) + Math.Abs(y) + Math.Abs(z) >= Radius))
                        {
                            yield return new Point3(Position.X + x, Position.Y + y, Position.Z + z);
                        }
                    }

                    num = y + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Cylindrical(Vector3 Position, int XRadius, int Height, int ZRadius, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool YType = true, bool Hollow = false)
        {
            int MaxRadius = Math.Max(XRadius, ZRadius);
            int x = -XRadius;
            while (x <= XRadius)
            {
                int num2;
                for (int z = -ZRadius; z <= ZRadius; z = num2)
                {
                    if ((int)Math.Sqrt((double)x * (double)MaxRadius / (double)XRadius * (double)x * (double)MaxRadius / (double)XRadius + (double)z * (double)MaxRadius / (double)ZRadius * (double)z * (double)MaxRadius / (double)ZRadius) <= MaxRadius && (!Hollow || (int)Math.Sqrt(((double)Math.Abs(x) + 1.0) * (double)MaxRadius / (double)XRadius * (double)(Math.Abs(x) + 1) * (double)MaxRadius / (double)XRadius + (double)z * (double)MaxRadius / (double)ZRadius * (double)z * (double)MaxRadius / (double)ZRadius) > MaxRadius || (int)Math.Sqrt((double)x * (double)x * (double)MaxRadius / (double)XRadius * (double)MaxRadius / (double)XRadius + (double)(Math.Abs(z) + 1) * (double)MaxRadius / (double)ZRadius * (double)(Math.Abs(z) + 1) * (double)MaxRadius / (double)ZRadius) > MaxRadius))
                    {
                        for (int y = 0; y < Height; y = num2)
                        {
                            int num = (!YType) ? (-y) : y;
                            switch (createType)
                            {
                                case CreatorMain.CreateType.X:
                                    yield return new Point3((int)Position.X + num, (int)Position.Y + x, (int)Position.Z + z);
                                    break;
                                case CreatorMain.CreateType.Y:
                                    yield return new Point3((int)Position.X + x, (int)Position.Y + num, (int)Position.Z + z);
                                    break;
                                default:
                                    yield return new Point3((int)Position.X + x, (int)Position.Y + z, (int)Position.Z + num);
                                    break;
                            }

                            num2 = y + 1;
                        }
                    }

                    num2 = z + 1;
                }

                num2 = x + 1;
                x = num2;
            }
        }

        public IEnumerable<Point3> Cylindrical(Point3 Position, int Radius, int Height, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool YType = true, bool Hollow = false)
        {
            int x = -Radius;
            while (x <= Radius)
            {
                int num2;
                for (int z = -Radius; z <= Radius; z = num2)
                {
                    if (Math.Sqrt(x * x + z * z) <= (double)Radius && (!Hollow || Math.Sqrt((Math.Abs(x) + 1) * (Math.Abs(x) + 1) + z * z) > (double)Radius || Math.Sqrt(x * x + (Math.Abs(z) + 1) * (Math.Abs(z) + 1)) > (double)Radius))
                    {
                        for (int y = 0; y < Height; y = num2)
                        {
                            int num = (!YType) ? (-y) : y;
                            switch (createType)
                            {
                                case CreatorMain.CreateType.X:
                                    yield return new Point3(Position.X + num, Position.Y + x, Position.Z + z);
                                    break;
                                case CreatorMain.CreateType.Y:
                                    yield return new Point3(Position.X + x, Position.Y + num, Position.Z + z);
                                    break;
                                default:
                                    yield return new Point3(Position.X + x, Position.Y + z, Position.Z + num);
                                    break;
                            }

                            num2 = y + 1;
                        }
                    }

                    num2 = z + 1;
                }

                num2 = x + 1;
                x = num2;
            }
        }

        public IEnumerable<Point3> PrismColumn(Point3 Position, int Radius, int Height, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool YType = true, bool Hollow = false, CreatorMain.CreateType? XYZType = null, bool typeBool = false)
        {
            int y = 0;
            while (y < Height)
            {
                int num2;
                for (int x = -Radius; x <= Radius; x = num2)
                {
                    for (int z = -Radius; z <= Radius; num2 = z + 1, z = num2)
                    {
                        if (XYZType.HasValue)
                        {
                            if (XYZType == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if (Position.X + x < Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.X + x > Position.X)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if (Position.Y + y < Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Y + y > Position.Y)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if (Position.Z + z < Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Z + z > Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if (Math.Abs(x) + Math.Abs(z) <= Radius && (!Hollow || Math.Abs(x) + Math.Abs(z) >= Radius))
                        {
                            int num = (!YType) ? (-y) : y;
                            switch (createType)
                            {
                                case CreatorMain.CreateType.X:
                                    yield return new Point3(Position.X + num, Position.Y + x, Position.Z + z);
                                    break;
                                case CreatorMain.CreateType.Y:
                                    yield return new Point3(Position.X + x, Position.Y + num, Position.Z + z);
                                    break;
                                default:
                                    yield return new Point3(Position.X + x, Position.Y + z, Position.Z + num);
                                    break;
                            }
                        }
                    }

                    num2 = x + 1;
                }

                num2 = y + 1;
                y = num2;
            }
        }

        public IEnumerable<Point3> Pillars(Point3 Position, int Radius, int Height, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool YType = true, bool Hollow = false, CreatorMain.CreateType? XYZType = null, bool typeBool = false)
        {
            int y = 0;
            while (y < Height)
            {
                int num2;
                for (int x = -Radius; x <= Radius; x = num2)
                {
                    for (int z = -Radius; z <= Radius; num2 = z + 1, z = num2)
                    {
                        if (XYZType.HasValue)
                        {
                            if (XYZType == CreatorMain.CreateType.X)
                            {
                                if (typeBool)
                                {
                                    if (Position.X + x < Position.X)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.X + x > Position.X)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Y)
                            {
                                if (typeBool)
                                {
                                    if (Position.Y + y < Position.Y)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Y + y > Position.Y)
                                {
                                    continue;
                                }
                            }

                            if (XYZType == CreatorMain.CreateType.Z)
                            {
                                if (typeBool)
                                {
                                    if (Position.Z + z < Position.Z)
                                    {
                                        continue;
                                    }
                                }
                                else if (Position.Z + z > Position.Z)
                                {
                                    continue;
                                }
                            }
                        }

                        if ((Math.Abs(x) <= Radius || Math.Abs(z) <= Radius) && (!Hollow || Math.Abs(x) >= Radius || Math.Abs(z) >= Radius))
                        {
                            int num = (!YType) ? (-y) : y;
                            switch (createType)
                            {
                                case CreatorMain.CreateType.X:
                                    yield return new Point3(Position.X + num, Position.Y + x, Position.Z + z);
                                    break;
                                case CreatorMain.CreateType.Y:
                                    yield return new Point3(Position.X + x, Position.Y + num, Position.Z + z);
                                    break;
                                default:
                                    yield return new Point3(Position.X + x, Position.Y + z, Position.Z + num);
                                    break;
                            }
                        }
                    }

                    num2 = x + 1;
                }

                num2 = y + 1;
                y = num2;
            }
        }

        public IEnumerable<Point3> Rectangular(Point3 Start, Point3 End, bool? type = null)
        {
            CreatorMain.Math.StartEnd(ref Start, ref End);
            int x = 0;
            while (x <= Start.X - End.X)
            {
                int num;
                for (int y = 0; y <= Start.Y - End.Y; y = num)
                {
                    for (int z = 0; z <= Start.Z - End.Z; z = num)
                    {
                        if ((type != true || x <= 0 || x >= Start.X - End.X || y <= 0 || y >= Start.Y - End.Y || z <= 0 || z >= Start.Z - End.Z) && (type != false || ((x < 0 || x > Start.X - End.X || y <= 0 || y >= Start.Y - End.Y || z <= 0 || z >= Start.Z - End.Z) && (y < 0 || y > Start.Y - End.Y || x <= 0 || x >= Start.X - End.X || z <= 0 || z >= Start.Z - End.Z) && (z < 0 || z > Start.Z - End.Z || y <= 0 || y >= Start.Y - End.Y || x <= 0 || x >= Start.X - End.X))))
                        {
                            yield return new Point3(End.X + x, End.Y + y, End.Z + z);
                        }

                        num = z + 1;
                    }

                    num = y + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Circle(Point3 Position, int Height, int Radius, CreatorMain.CreateType type = CreatorMain.CreateType.Y, bool Hollow = false)
        {
            int _Radius = Height - Radius;
            int x = -Radius;
            while (x <= Radius)
            {
                int _radius = _Radius + Radius - x;
                int num;
                for (int z = -Radius; z <= Radius; z = num)
                {
                    if ((int)Math.Sqrt(x * x + z * z) <= Radius && (!Hollow || (int)Math.Sqrt((Math.Abs(x) + 1) * (Math.Abs(x) + 1) + z * z) > Radius || (int)Math.Sqrt(x * x + (Math.Abs(z) + 1) * (Math.Abs(z) + 1)) > Radius))
                    {
                        for (int x_2 = -_radius; x_2 <= _radius; x_2 = num)
                        {
                            for (int z_2 = -_radius; z_2 <= _radius; z_2 = num)
                            {
                                if ((int)Math.Sqrt(x_2 * x_2 + z_2 * z_2) <= _radius && ((int)Math.Sqrt((double)((float)Math.Abs(x_2) + 0.5f) * ((double)Math.Abs(x_2) + 0.5) + (double)((Math.Abs(z_2) + 1) * (Math.Abs(z_2) + 1))) > _radius || (int)Math.Sqrt((double)((Math.Abs(x_2) + 1) * (Math.Abs(x_2) + 1)) + ((double)Math.Abs(z_2) + 0.5) * ((double)Math.Abs(z_2) + 0.5)) > _radius))
                                {
                                    switch (type)
                                    {
                                        case CreatorMain.CreateType.X:
                                            yield return new Point3(Position.X + z, Position.Y + x_2, Position.Z + z_2);
                                            break;
                                        case CreatorMain.CreateType.Y:
                                            yield return new Point3(Position.X + x_2, Position.Y + z, Position.Z + z_2);
                                            break;
                                        default:
                                            yield return new Point3(Position.X + z_2, Position.Y + x_2, Position.Z + z);
                                            break;
                                    }
                                }

                                num = z_2 + 1;
                            }

                            num = x_2 + 1;
                        }
                    }

                    num = z + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Maze(Point3 Start, Point3 End)
        {
            CreatorMain.Math.StartEnd(ref Start, ref End);
            int StartX = Start.X - End.X;
            int StartZ = Start.Z - End.Z;
            bool[,] mazeArray = new Maze(StartX / 2, StartZ / 2).GetBoolArray();
            int x = 0;
            while (x <= ((StartX % 2 != 0) ? (StartX - 1) : StartX))
            {
                int num;
                for (int z = 0; z <= ((StartZ % 2 != 0) ? (StartZ - 1) : StartZ); z = num)
                {
                    if ((x != 1 || z != 0) && (x != ((StartX % 2 != 0) ? (StartX - 1) : StartX) || z != ((StartZ % 2 != 0) ? (StartZ - 1) : StartZ) - 1) && mazeArray[x, z])
                    {
                        for (int y = 0; y <= Start.Y - End.Y; y = num)
                        {
                            yield return new Point3(End.X + x, End.Y + y, End.Z + z);
                            num = y + 1;
                        }
                    }

                    num = z + 1;
                }

                num = x + 1;
                x = num;
            }
        }

        public IEnumerable<Point3> Spiral(Point3 Position, int Height, int Radius, int Number, CreatorMain.CreateType createType = CreatorMain.CreateType.Y, bool YType = true)
        {
            int angle = 0;
            while (angle <= 360 * Number)
            {
                float x = (float)((double)Radius * (double)angle / 360.0) * MathUtils.Cos((float)((double)angle * 3.14159274101257 / 180.0));
                float z = (float)((double)Radius * (double)angle / 360.0) * MathUtils.Sin((float)((double)angle * 3.14159274101257 / 180.0));
                int num2;
                for (int y = 0; y <= Height - 1; y = num2)
                {
                    int num = (!YType) ? (-y) : y;
                    switch (createType)
                    {
                        case CreatorMain.CreateType.X:
                            yield return new Point3(Position.X + num, Position.Y + (int)x, Position.Z + (int)z);
                            break;
                        case CreatorMain.CreateType.Y:
                            yield return new Point3(Position.X + (int)x, Position.Y + num, Position.Z + (int)z);
                            break;
                        default:
                            yield return new Point3(Position.X + (int)x, Position.Y + (int)z, Position.Z + num);
                            break;
                    }

                    num2 = y + 1;
                }

                num2 = angle + 1;
                angle = num2;
            }
        }

        public IEnumerable<Point3> ThreePointPlane(Point3 p1, Point3 p2, Point3 p3)
        {
            List<Point3> listPoint3 = new List<Point3>();
            foreach (Point3 item in TwoPointLineGeneration(p1, p2))
            {
                listPoint3.Add(item);
            }

            foreach (Point3 item2 in listPoint3)
            {
                foreach (Point3 item3 in TwoPointLineGeneration(item2, p3))
                {
                    yield return item3;
                }
            }

            listPoint3.Clear();
            foreach (Point3 item4 in TwoPointLineGeneration(p1, p3))
            {
                listPoint3.Add(item4);
            }

            foreach (Point3 item5 in listPoint3)
            {
                foreach (Point3 item6 in TwoPointLineGeneration(item5, p2))
                {
                    yield return item6;
                }
            }

            listPoint3.Clear();
            foreach (Point3 item7 in TwoPointLineGeneration(p3, p2))
            {
                listPoint3.Add(item7);
            }

            foreach (Point3 item8 in listPoint3)
            {
                foreach (Point3 item9 in TwoPointLineGeneration(item8, p1))
                {
                    yield return item9;
                }
            }
        }

        public IEnumerable<Point3> ThreePointPlane2(Point3 p1, Point3 p2, Point3 p3)
        {
            if (p2.Y > p1.Y)
            {
                Point3 point4 = p1;
                p1 = p2;
                p2 = point4;
            }

            if (p3.Y > p1.Y)
            {
                Point3 point4 = p1;
                p1 = p3;
                p3 = point4;
            }

            if (p3.X > p2.X)
            {
                Point3 point4 = p2;
                p2 = p3;
                p3 = point4;
            }

            List<Point3> listPoint3 = new List<Point3>();
            foreach (Point3 item in TwoPointLineGeneration(p1, p2))
            {
                listPoint3.Add(item);
            }

            int lengin = Math.Max(MathUtils.Max(Math.Abs(p3.X - p1.X), Math.Abs(p3.Y - p1.Y)), Math.Abs(p3.Z - p1.Z));
            foreach (Point3 item2 in TwoPointLineGeneration(p3, p2))
            {
                Point3 point3 = item2;
                int num = 0;
                while (num <= lengin)
                {
                    Point3 point4 = new Point3(point3.X + (int)Math.Round((double)num / (double)lengin * (double)(p1.X - p3.X)), point3.Y + (int)Math.Round((double)num / (double)lengin * (double)(p1.Y - p3.Y)), point3.Z + (int)Math.Round((double)num / (double)lengin * (double)(p1.Z - p3.Z)));
                    if (listPoint3.Contains(point4))
                    {
                        break;
                    }

                    yield return point4;
                    int num2 = num + 1;
                    num = num2;
                }
            }
        }

        public IEnumerable<Point3> FourPointSpace(Point3 p1, Point3 p2, Point3 p3, Point3 p4)
        {
            foreach (Point3 item in ThreePointPlane(p1, p2, p3))
            {
                yield return item;
            }

            foreach (Point3 item2 in ThreePointPlane(p1, p2, p4))
            {
                yield return item2;
            }

            foreach (Point3 item3 in ThreePointPlane(p1, p4, p3))
            {
                yield return item3;
            }

            foreach (Point3 item4 in ThreePointPlane(p4, p2, p3))
            {
                yield return item4;
            }
        }
    }
}