//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class ReplaceDialog : Dialog
    {
        private readonly ButtonWidget cancelButton;

        private readonly ButtonWidget replaceButton;

        private readonly ButtonWidget retainReplaceButton;

        private readonly ButtonWidget roughReplaceButton;

        private readonly Game.TextBoxWidget Blockid;

        private readonly Game.TextBoxWidget Blockid2;

        private readonly ComponentPlayer player;

        private readonly CreatorAPI creatorAPI;

        private readonly SubsystemTerrain subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>();

        public ReplaceDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/替换界面");
            LoadChildren(this, node);
            cancelButton = Children.Find<ButtonWidget>("取消");
            replaceButton = Children.Find<ButtonWidget>("替换");
            retainReplaceButton = Children.Find<ButtonWidget>("保留替换");
            roughReplaceButton = Children.Find<ButtonWidget>("粗糙替换");
            Blockid = Children.Find<Game.TextBoxWidget>("方块ID");
            Blockid2 = Children.Find<Game.TextBoxWidget>("方块ID2");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (roughReplaceButton.IsClicked)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (Blockid2.Text == "")
                {
                    Blockid2.Text = "0";
                }

                Match match = regex.Match(Blockid.Text);
                Match match2 = regex.Match(Blockid2.Text);
                if (match.Success && match2.Success)
                {
                    Point3 Start = creatorAPI.Position[0];
                    Point3 End2 = creatorAPI.Position[1];
                    CreatorMain.Math.StartEnd(ref Start, ref End2);
                    Task.Run(delegate
                    {
                        int num2 = 0;
                        ChunkData chunkData2 = new ChunkData(creatorAPI);
                        creatorAPI.revokeData = new ChunkData(creatorAPI);
                        for (int l = 0; l <= Start.X - End2.X; l++)
                        {
                            for (int m = 0; m <= Start.Y - End2.Y; m++)
                            {
                                for (int n = 0; n <= Start.Z - End2.Z; n++)
                                {
                                    int cellValueFast2 = subsystemTerrain.Terrain.GetCellValueFast(End2.X + l, End2.Y + m, End2.Z + n);
                                    if ((creatorAPI.AirIdentify || Terrain.ExtractContents(cellValueFast2) != 0) && (cellValueFast2 == int.Parse(Blockid.Text) || Terrain.ExtractContents(cellValueFast2) == int.Parse(Blockid.Text)))
                                    {
                                        if (!creatorAPI.launch)
                                        {
                                            return;
                                        }

                                        creatorAPI.CreateBlock(End2.X + l, End2.Y + m, End2.Z + n, int.Parse(Blockid2.Text), chunkData2);
                                        num2++;
                                    }
                                }
                            }
                        }

                        chunkData2.Render();
                        player.ComponentGui.DisplaySmallMessage($"操作成功，共替换{num2}个方块", Color.LightYellow, blinking: true, playNotificationSound: true);
                    });
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage("操作失败", Color.Red, blinking: true, playNotificationSound: true);
                }

                DialogsManager.HideDialog(this);
            }

            if (!replaceButton.IsClicked && !retainReplaceButton.IsClicked)
            {
                return;
            }

            bool type = replaceButton.IsClicked;
            Regex regex2 = new Regex("^[0-9]+$");
            if (Blockid2.Text == "")
            {
                Blockid2.Text = "0";
            }

            Match match3 = regex2.Match(Blockid.Text);
            Match match4 = regex2.Match(Blockid2.Text);
            if (match3.Success && match4.Success)
            {
                Point3 Start2 = creatorAPI.Position[0];
                Point3 End = creatorAPI.Position[1];
                CreatorMain.Math.StartEnd(ref Start2, ref End);
                Task.Run(delegate
                {
                    ChunkData chunkData = new ChunkData(creatorAPI);
                    creatorAPI.revokeData = new ChunkData(creatorAPI);
                    int num = 0;
                    for (int i = 0; i <= Start2.X - End.X; i++)
                    {
                        for (int j = 0; j <= Start2.Y - End.Y; j++)
                        {
                            for (int k = 0; k <= Start2.Z - End.Z; k++)
                            {
                                int cellValueFast = subsystemTerrain.Terrain.GetCellValueFast(End.X + i, End.Y + j, End.Z + k);
                                if (creatorAPI.AirIdentify || Terrain.ExtractContents(cellValueFast) != 0)
                                {
                                    if (type)
                                    {
                                        if (cellValueFast == int.Parse(Blockid.Text))
                                        {
                                            if (!creatorAPI.launch)
                                            {
                                                return;
                                            }

                                            creatorAPI.CreateBlock(End.X + i, End.Y + j, End.Z + k, int.Parse(Blockid2.Text), chunkData);
                                            num++;
                                        }
                                    }
                                    else if (cellValueFast != int.Parse(Blockid.Text))
                                    {
                                        if (!creatorAPI.launch)
                                        {
                                            return;
                                        }

                                        creatorAPI.CreateBlock(End.X + i, End.Y + j, End.Z + k, int.Parse(Blockid2.Text), chunkData);
                                        num++;
                                    }
                                }
                            }
                        }
                    }

                    chunkData.Render();
                    player.ComponentGui.DisplaySmallMessage($"操作成功，共替换{num}个方块", Color.LightYellow, blinking: true, playNotificationSound: true);
                });
            }
            else
            {
                player.ComponentGui.DisplaySmallMessage("操作失败", Color.Red, blinking: true, playNotificationSound: true);
            }

            DialogsManager.HideDialog(this);
        }
    }
}
