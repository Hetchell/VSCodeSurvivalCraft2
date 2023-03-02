using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorWand2
{
    public static class CW2Cuboid
    {
        public static int GeometryCuboid(Point3 PStart, Point3 PEnd, int value)
        {
            int count = 0;
            try
            {

                CW2BaseFunction.PointSort(ref PStart, ref PEnd);
                var subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
                int i, j, k;

                for (i = PStart.X; i <= PEnd.X; i++)
                {
                    for (j = PStart.Y; j <= PEnd.Y; j++)
                    {
                        for (k = PStart.Z; k <= PEnd.Z; k++)
                        {
                            CW2EntityManager.RemoveEntity(new Point3(i, j, k));
                            subsystemTerrain.ChangeCell(i, j, k, value, true);
                            CW2EntityManager.AddEntity(new Point3(i, j, k));
                            count++;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Err:GeometryCuboid[" + e + "]");
            }
            return count;
        }
        public static int GeometryBox(Point3 PStart, Point3 PEnd, int value)
        {
            int count = 0;
            CW2BaseFunction.PointSort(ref PStart, ref PEnd);
            var subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            for (int i = 0; i <= 1; i++)
            {
                for (int j = PStart.Y; j <= PEnd.Y; j++)
                {
                    for (int k = PStart.Z; k <= PEnd.Z; k++)
                    {
                        CW2EntityManager.RemoveEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j, k));
                        subsystemTerrain.ChangeCell(i == 0 ? PStart.X : PEnd.X, j, k, value, true);
                        CW2EntityManager.AddEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j, k));
                        count++;
                    }
                }
            }
            for (int j = 0; j <= 1; j++)
            {
                for (int i = PStart.X; i <= PEnd.X; i++)
                {

                    for (int k = PStart.Z; k <= PEnd.Z; k++)
                    {
                        CW2EntityManager.RemoveEntity(new Point3(i, j == 0 ? PStart.Y : PEnd.Y, k));
                        subsystemTerrain.ChangeCell(i, j == 0 ? PStart.Y : PEnd.Y, k, value, true);
                        CW2EntityManager.AddEntity(new Point3(i, j == 0 ? PStart.Y : PEnd.Y, k));
                        count++;
                    }
                }
            }
            for (int k = 0; k <= 1; k++)
            {
                for (int i = PStart.X; i <= PEnd.X; i++)
                {
                    for (int j = PStart.Y; j <= PEnd.Y; j++)
                    {
                        CW2EntityManager.RemoveEntity(new Point3(i, j, k == 0 ? PStart.Z : PEnd.Z));
                        subsystemTerrain.ChangeCell(i, j, k == 0 ? PStart.Z : PEnd.Z, value, true);
                        CW2EntityManager.AddEntity(new Point3(i, j, k == 0 ? PStart.Z : PEnd.Z));
                        count++;
                    }
                }
            }
            return count;
        }
        public static int GeometryEdge(Point3 PStart, Point3 PEnd, int value)
        {
            int count = 0;
            CW2BaseFunction.PointSort(ref PStart, ref PEnd);
            var subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    for (int k = PStart.Z; k <= PEnd.Z; k++)
                    {
                        CW2EntityManager.RemoveEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j == 0 ? PStart.Y : PEnd.Y, k));
                        subsystemTerrain.ChangeCell(i == 0 ? PStart.X : PEnd.X, j == 0 ? PStart.Y : PEnd.Y, k, value, true);
                        CW2EntityManager.AddEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j == 0 ? PStart.Y : PEnd.Y, k));
                        count++;
                    }
                }
            }
            for (int j = 0; j <= 1; j++)
            {
                for (int k = 0; k <= 1; k++)
                {
                    for (int i = PStart.X; i <= PEnd.X; i++)
                    {


                        CW2EntityManager.RemoveEntity(new Point3(i, j == 0 ? PStart.Y : PEnd.Y, k == 0 ? PStart.Z : PEnd.Z));
                        subsystemTerrain.ChangeCell(i, j == 0 ? PStart.Y : PEnd.Y, k == 0 ? PStart.Z : PEnd.Z, value, true);
                        CW2EntityManager.AddEntity(new Point3(i, j == 0 ? PStart.Y : PEnd.Y, k == 0 ? PStart.Z : PEnd.Z));
                        count++;
                    }
                }
            }
            for (int i = 0; i <= 1; i++)
            {
                for (int k = 0; k <= 1; k++)
                {

                    for (int j = PStart.Y; j <= PEnd.Y; j++)
                    {

                        CW2EntityManager.RemoveEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j, k == 0 ? PStart.Z : PEnd.Z));
                        subsystemTerrain.ChangeCell(i == 0 ? PStart.X : PEnd.X, j, k == 0 ? PStart.Z : PEnd.Z, value, true);
                        CW2EntityManager.AddEntity(new Point3(i == 0 ? PStart.X : PEnd.X, j, k == 0 ? PStart.Z : PEnd.Z));
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
