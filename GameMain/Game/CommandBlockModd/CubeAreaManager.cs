using Engine;
using System;

namespace Game
{
  public static class CubeAreaManager
  {
    public static CubeArea SetArea(int x1, int y1, int z1, int x2, int y2, int z2) => new CubeArea()
    {
      PointMax = new Point3(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2)),
      PointMin = new Point3(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2)),
      LengthX = Math.Abs(x1 - x2) + 1,
      LengthY = Math.Abs(y1 - y2) + 1,
      LengthZ = Math.Abs(z1 - z2) + 1
    };
  }
}