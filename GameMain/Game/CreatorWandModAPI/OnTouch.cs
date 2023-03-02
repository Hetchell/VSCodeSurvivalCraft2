//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public static class OnTouch
    {
        public static object SleepTime
        {
            get;
            private set;
        }

        public static bool Touch(CreatorAPI creatorAPI, Point3 position)
        {
            ComponentPlayer player = creatorAPI.componentMiner.ComponentPlayer;
            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValue(position.X, position.Y, position.Z);
            if (creatorAPI.oneKeyGeneration)
            {
                if (creatorAPI.onekeyType == CreatorAPI.OnekeyType.Build)
                {
                    if (File.Exists(CreatorMain.OneKeyFile))
                    {
                        Task.Run(delegate
                        {
                            OnekeyGeneration.GenerationData(creatorAPI, CreatorMain.OneKeyFile, position);
                        });
                    }
                    else
                    {
                        player.ComponentGui.DisplaySmallMessage("未发现一键生成缓存文件，目录:" + CreatorMain.OneKeyFile + "\n请变更一键生成类型或关闭该功能", Color.Red, blinking: true, playNotificationSound: true);
                    }
                }

                return false;
            }

            if (!creatorAPI.ClearBlock)
            {
                return true;
            }

            Task.Run(delegate
            {
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                int num = 0;
                List<Point3> list = new List<Point3>();
                List<Point3> list2 = new List<Point3>();
                list.Add(position);
                while (list.Count > 0)
                {
                    foreach (Point3 item2 in list)
                    {
                        if (!creatorAPI.launch)
                        {
                            return;
                        }

                        if (creatorAPI.revokeData != null && creatorAPI.revokeData.GetChunk(item2.X, item2.Z) == null)
                        {
                            creatorAPI.revokeData.CreateChunk(item2.X, item2.Z, unLimited: true);
                        }

                        creatorAPI.SetBlock(item2.X, item2.Y, item2.Z, 0);
                        num++;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                for (int k = -1; k <= 1; k++)
                                {
                                    if (item2.Y + j <= 255)
                                    {
                                        int cellContentsFast = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellContentsFast(item2.X + i, item2.Y + j, item2.Z + k);
                                        if ((uint)cellContentsFast > 1u && MathUtils.Abs(i) + MathUtils.Abs(j) + MathUtils.Abs(k) <= 1)
                                        {
                                            Point3 item = new Point3(item2.X + i, item2.Y + j, item2.Z + k);
                                            if (!list.Contains(item) && !list2.Contains(item))
                                            {
                                                list2.Add(item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    list = list2;
                    list2 = new List<Point3>();
                }

                player.ComponentGui.DisplaySmallMessage($"操作成功，共清除{num}个方块", Color.Yellow, blinking: true, playNotificationSound: true);
            });
            return false;
        }
    }
}