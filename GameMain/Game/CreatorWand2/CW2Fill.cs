using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorWand2
{
    /// <summary>
    /// 填充空白区域
    /// </summary>
    public static class CW2Fill
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PStart"></param>
        /// <param name="PEnd"></param>
        /// <param name="value"></param>
        /// <returns>填充数量</returns>
        public static int FillObject(Point3 PStart, Point3 PEnd, int value)
        {
            CW2BaseFunction.PointSort(ref PStart, ref PEnd);
            var subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            int count = 0;
            for (int i = PStart.X; i <= PEnd.X; i++)
            {
                for (int j = PStart.Y; j <= PEnd.Y; j++)
                {
                    for (int k = PStart.Z; k <= PEnd.Z; k++)
                    {
                        if (Terrain.ExtractContents(subsystemTerrain.Terrain.GetCellValueFast(i, j, k)) == 0)
                        {
                            CW2EntityManager.RemoveEntity(new Point3(i, j, k));
                            //Terrain.MakeBlockValue(value)
                            subsystemTerrain.ChangeCell(i, j, k, value);
                            CW2EntityManager.AddEntity(new Point3(i, j, k));
                            count++;
                        }
                    }
                }
            }
            return count;
        }
    }
}
