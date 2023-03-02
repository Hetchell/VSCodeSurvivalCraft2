using Engine;
using Game;

namespace CreatorWandModAPI
{
    public class NewReplace
    {
        public static int Replace(int Src_Param, int Des_Param, Point3 Start, Point3 End)
        {
            int num = 0;
            CreatorMain.Math.StarttoEnd(ref Start, ref End);
            bool flag = false;
            bool Outflagcox = false;
            if (Des_Param >= 1024 || Src_Param < 0)
            {
                Outflagcox = true;
            }

            for (int i = Start.Y; i <= End.Y; i++)
            {
                for (int j = Start.Z; j <= End.Z; j++)
                {
                    for (int k = Start.X; k <= End.X; k++)
                    {
                        if (!flag)
                        {
                            if (Src_Param == GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellContentsFast(k, i, j))
                            {
                                if (Outflagcox)
                                {
                                    GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(k, i, j, Terrain.MakeBlockValue(Terrain.ExtractContents(Des_Param), Terrain.ExtractLight(Des_Param), Terrain.ExtractData(Des_Param)));
                                }
                                else
                                {
                                    GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(k, i, j, Terrain.MakeBlockValue(Des_Param, GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellLight(k, i, j), 0));
                                }

                                num++;
                            }
                        }
                        else if (Terrain.ExtractContents(Src_Param) == GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellContentsFast(k, i, j) && Terrain.ExtractData(Src_Param) == Terrain.ExtractData(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValue(k, i, j)))
                        {
                            if (Outflagcox)
                            {
                                GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(k, i, j, Terrain.MakeBlockValue(Terrain.ExtractContents(Des_Param), Terrain.ExtractLight(Des_Param), Terrain.ExtractData(Des_Param)));
                            }
                            else
                            {
                                GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(k, i, j, Terrain.MakeBlockValue(Des_Param, GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellLight(k, i, j), 0));
                            }

                            num++;
                        }
                    }
                }
            }

            return num;
        }
    }
}