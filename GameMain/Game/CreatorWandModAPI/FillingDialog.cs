//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class FillingDialog : InterfaceDialog
    {
        public bool typeBool = true;

        private readonly ButtonWidget pillingButton;

        private readonly ButtonWidget pilling2Button;

        public FillingDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            XElement node = ContentManager.Get<XElement>("Dialog/Fill");
            LoadChildren(this, node);
            GeneralSet();
            setShaftXYZ();
            switch (CreatorAPI.Language)
            {
                case Language.zh_CN:
                    Y_Shaft.Text = "正Y轴";
                    break;
                case Language.en_US:
                    Y_Shaft.Text = "Y-axis";
                    break;
                default:
                    Y_Shaft.Text = "正Y轴";
                    break;
            }

            pillingButton = Children.Find<ButtonWidget>("填充");
            pilling2Button = Children.Find<ButtonWidget>("填充2");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            if (pillingButton.IsClicked)
            {
                FillingBlock();
                DialogsManager.HideDialog(this);
            }

            if (pilling2Button.IsClicked)
            {
                FillingBlock(limit: true);
                DialogsManager.HideDialog(this);
            }

            upDataButton();
        }

        public void FillingBlock(bool limit = false)
        {
            Point3 Start = creatorAPI.Position[0];
            Point3 End = creatorAPI.Position[1];
            CreatorMain.Math.StartEnd(ref Start, ref End);
            if (createType == CreatorMain.CreateType.X)
            {
                int x = Start.X;
                Start.X = Start.Y;
                Start.Y = x;
                int x2 = End.X;
                End.X = End.Y;
                End.Y = x2;
            }
            else if (createType == CreatorMain.CreateType.Z)
            {
                int z = Start.Z;
                Start.Z = Start.Y;
                Start.Y = z;
                int z2 = End.Z;
                End.Z = End.Y;
                End.Y = z2;
            }

            Task.Run(delegate
            {
                int num = 0;
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                for (int i = End.X; i <= Start.X; i++)
                {
                    for (int j = End.Z; j <= Start.Z; j++)
                    {
                        bool flag = false;
                        bool flag2 = false;
                        for (int k = End.Y; k <= Start.Y; k++)
                        {
                            if (!creatorAPI.launch)
                            {
                                return;
                            }

                            int num2 = (createType != CreatorMain.CreateType.Y) ? (typeBool ? k : (Start.Y + End.Y - k)) : (typeBool ? (Start.Y + End.Y - k) : k);
                            int num3 = Terrain.ExtractContents((createType == CreatorMain.CreateType.X) ? subsystemTerrain.Terrain.GetCellValueFast(num2, i, j) : ((createType != CreatorMain.CreateType.Y) ? subsystemTerrain.Terrain.GetCellValueFast(i, j, num2) : subsystemTerrain.Terrain.GetCellValueFast(i, num2, j)));
                            if (flag2 && limit && num3 != 0)
                            {
                                break;
                            }

                            if (!flag && num3 != 0)
                            {
                                flag = true;
                            }
                            else if (flag && num3 == 0)
                            {
                                flag2 = true;
                                if (createType == CreatorMain.CreateType.X)
                                {
                                    creatorAPI.CreateBlock(num2, i, j, blockIconWidget.Value, chunkData);
                                    num++;
                                }
                                else if (createType == CreatorMain.CreateType.Y)
                                {
                                    creatorAPI.CreateBlock(i, num2, j, blockIconWidget.Value, chunkData);
                                    num++;
                                }
                                else
                                {
                                    creatorAPI.CreateBlock(i, j, num2, blockIconWidget.Value, chunkData);
                                    num++;
                                }
                            }
                        }
                    }
                }

                chunkData.Render();
                switch (CreatorAPI.Language)
                {
                    case Language.zh_CN:
                        player.ComponentGui.DisplaySmallMessage($"操作成功，共生成{num}个方块", Color.LightYellow, blinking: true, playNotificationSound: true);
                        break;
                    case Language.en_US:
                        player.ComponentGui.DisplaySmallMessage($"The operation was successful, generating a total of {num} blocks", Color.LightYellow, blinking: true, playNotificationSound: true);
                        break;
                    default:
                        player.ComponentGui.DisplaySmallMessage($"操作成功，共生成{num}个方块", Color.LightYellow, blinking: true, playNotificationSound: true);
                        break;
                }
            });
        }

        public override void upDataButton(CreatorMain.CreateType createType, ButtonWidget button)
        {
            if (base.createType == createType)
            {
                if (typeBool)
                {
                    typeBool = false;
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            button.Text = "负" + getTypeName(createType) + "轴";
                            break;
                        case Language.en_US:
                            button.Text = "- " + getTypeName(createType) + "-axis";
                            break;
                        default:
                            button.Text = "负" + getTypeName(createType) + "轴";
                            break;
                    }

                    button.Color = Color.Red;
                }
                else
                {
                    typeBool = true;
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            button.Text = "正" + getTypeName(createType) + "轴";
                            break;
                        case Language.en_US:
                            button.Text = "+ " + getTypeName(createType) + "-axis";
                            break;
                        default:
                            button.Text = "正" + getTypeName(createType) + "轴";
                            break;
                    }

                    button.Color = Color.Green;
                }

                return;
            }

            typeBool = true;
            base.createType = createType;
            switch (CreatorAPI.Language)
            {
                case Language.zh_CN:
                    button.Text = "正" + getTypeName(createType) + "轴";
                    break;
                case Language.en_US:
                    button.Text = "+ " + getTypeName(createType) + "-axis";
                    break;
                default:
                    button.Text = "正" + getTypeName(createType) + "轴";
                    break;
            }

            button.Color = Color.Green;
            if (X_Shaft != button)
            {
                switch (CreatorAPI.Language)
                {
                    case Language.zh_CN:
                        X_Shaft.Text = "X轴";
                        break;
                    case Language.en_US:
                        X_Shaft.Text = "X-axis";
                        break;
                    default:
                        X_Shaft.Text = "X轴";
                        break;
                }

                X_Shaft.Color = Color.White;
            }

            if (Y_Shaft != button)
            {
                switch (CreatorAPI.Language)
                {
                    case Language.zh_CN:
                        X_Shaft.Text = "Y轴";
                        break;
                    case Language.en_US:
                        X_Shaft.Text = "Y-axis";
                        break;
                    default:
                        X_Shaft.Text = "Y轴";
                        break;
                }

                Y_Shaft.Color = Color.White;
            }

            if (Z_Shaft != button)
            {
                switch (CreatorAPI.Language)
                {
                    case Language.zh_CN:
                        X_Shaft.Text = "Z轴";
                        break;
                    case Language.en_US:
                        X_Shaft.Text = "Z-axis";
                        break;
                    default:
                        X_Shaft.Text = "Z轴";
                        break;
                }

                Z_Shaft.Color = Color.White;
            }
        }
    }
}
