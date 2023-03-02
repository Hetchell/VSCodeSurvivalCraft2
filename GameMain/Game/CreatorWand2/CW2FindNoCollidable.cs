using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorWand2
{
    public static class CW2FindNoCollidable
    {
        public static bool FindNoCollidable(Engine.Point3 point3)
        {
            var m_subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(true);
            int foot = m_subsystemTerrain.Terrain.GetCellContentsFast(point3.X, point3.Y, point3.Z);
            int head = m_subsystemTerrain.Terrain.GetCellContentsFast(point3.X, point3.Y + 1, point3.Z);
            Block footblock = BlocksManager.Blocks[foot];
            Block headblock = BlocksManager.Blocks[head];
            if (!(footblock.IsCollidable || headblock.IsCollidable))
            {
                return true;
            }
            return false;
        }
        public static readonly Point3[] BlockToPoint3 = new Point3[]
       {
            new Point3(0, 1, 0),
            new Point3(0, 1, 1),
            new Point3(1, 1, 0),
            new Point3(1, 1, 1),
            new Point3(0, 1, -1),
            new Point3(-1, 1, 0),
            new Point3(-1, 1, 1),
            new Point3(1, 1, -1),
            new Point3(-1, 1, -1),
            new Point3(0, 0, 1),
            new Point3(1, 0, 0),
            new Point3(1, 0, 1),
             new Point3(1, 0, -1),
             new Point3(-1, 0, 1),
            new Point3(-1, 0, -1),
            new Point3(0, 0, -1),
            new Point3(-1, 0, 0),
            new Point3(0, -1, -1),
            new Point3(-1, -1, 0),
            new Point3(0, -1, 0),
             new Point3(-1, -1, -1),
             new Point3(0, -1, 1),
             new Point3(1, -1, 0),
             new Point3(1, -1, 1),
             new Point3(-1, -1, 1),
             new Point3(1, -1, -1),
       };
    }
}
