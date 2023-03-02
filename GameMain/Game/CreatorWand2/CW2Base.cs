//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorWand2
{
    public static class CW2BaseFunction
    {
        public static void GenerateBlock(Point3 point3, int value)
        {
            CW2EntityManager.RemoveEntity(point3);
            //Game.Terrain.MakeBlockValue(value)
            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(point3.X, point3.Y, point3.Z, value);
            CW2EntityManager.AddEntity(point3);

        }
        public static void PointSort(ref Point3 Start, ref Point3 End)
        {
            if (Start.X > End.X)
            {
                int x = Start.X;
                Start.X = End.X;
                End.X = x;
            }

            if (Start.Y > End.Y)
            {
                int y = Start.Y;
                Start.Y = End.Y;
                End.Y = y;
            }

            if (Start.Z > End.Z)
            {
                int z = Start.Z;
                Start.Z = End.Z;
                End.Z = z;
            }
        }
    }
}
