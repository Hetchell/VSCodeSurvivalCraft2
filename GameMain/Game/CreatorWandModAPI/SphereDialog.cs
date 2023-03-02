//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class SphereDialog : InterfaceDialog
    {
        private readonly SliderWidget XRadius;

        private readonly SliderWidget YRadius;

        private readonly SliderWidget ZRadius;

        private readonly LabelWidget XdelayLabel;

        private readonly LabelWidget YdelayLabel;

        private bool advanced;

        private readonly LabelWidget ZdelayLabel;

        private readonly ButtonWidget SoildButton;

        private readonly ButtonWidget HollowButton;

        private readonly ButtonWidget AdvancedButton;

        private readonly Widget AdvancedGenerate;

        private readonly ButtonWidget DoublePositionButton;

        private bool DoublePosition;

        public SphereDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            XElement node = ContentManager.Get<XElement>("Dialog/Sphere");
            LoadChildren(this, node);
            GeneralSet();
            (Children.Find<LabelWidget>("Sphere")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "Sphere"));
            XRadius = Children.Find<SliderWidget>("XSlider");
            YRadius = Children.Find<SliderWidget>("YSlider");
            ZRadius = Children.Find<SliderWidget>("ZSlider");
            XdelayLabel = Children.Find<LabelWidget>("XR");
            YdelayLabel = Children.Find<LabelWidget>("YR");
            ZdelayLabel = Children.Find<LabelWidget>("ZR");
            SoildButton = Children.Find<ButtonWidget>("Solid");
            SoildButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "Solid");
            HollowButton = Children.Find<ButtonWidget>("Hollow");
            HollowButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "Hollow");
            AdvancedButton = Children.Find<ButtonWidget>("ADV");
            AdvancedButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "ADV");
            AdvancedGenerate = Children.Find<Widget>("ADVG");
            DoublePositionButton = Children.Find<ButtonWidget>("Limit");
            DoublePositionButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "Limit");
            Children.Find<BevelledButtonWidget>("Cancel").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Sphere", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            AdvancedButton.Color = (advanced ? Color.Yellow : Color.White);
            DoublePositionButton.Color = (DoublePosition ? Color.Yellow : Color.White);
            AdvancedGenerate.IsVisible = advanced;
            DoublePositionButton.IsVisible = advanced;
            (XdelayLabel).Text = (advanced ? string.Format(CreatorMain.Display_Key_Dialog("spdialogx"), (int)XRadius.Value) : string.Format(CreatorMain.Display_Key_Dialog("spdialogr"), (int)XRadius.Value));
            (YdelayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("spdialogy"), (int)YRadius.Value));
            (ZdelayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("spdialogz"), (int)ZRadius.Value));
            int id = blockIconWidget.Value;
            if (DoublePositionButton.IsClicked)
            {
                DoublePosition = !DoublePosition;
            }

            if (AdvancedButton.IsClicked)
            {
                advanced = !advanced;
            }

            if (SoildButton.IsClicked)
            {
                if (advanced)
                {
                    if (!DoublePosition)
                    {
                        Task.Run(delegate
                        {
                            ChunkData chunkData6 = new ChunkData(creatorAPI);
                            creatorAPI.revokeData = new ChunkData(creatorAPI);
                            int num6 = 0;
                            foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3(creatorAPI.Position[0]), (int)XRadius.Value, (int)YRadius.Value, (int)ZRadius.Value))
                            {
                                if (!creatorAPI.launch)
                                {
                                    return;
                                }

                                creatorAPI.CreateBlock(item, id, chunkData6);
                                num6++;
                            }

                            chunkData6.Render();
                            player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num6), Color.LightYellow, blinking: true, playNotificationSound: true);
                        });
                    }
                    else if (creatorAPI.Position[1].Y != -1)
                    {
                        Point3 Start = creatorAPI.Position[0];
                        Point3 End2 = creatorAPI.Position[1];
                        CreatorMain.Math.StartEnd(ref Start, ref End2);
                        float x2 = Math.Abs((float)Start.X - (float)End2.X) / 2f;
                        float y2 = Math.Abs((float)Start.Y - (float)End2.Y) / 2f;
                        float z2 = Math.Abs((float)Start.Z - (float)End2.Z) / 2f;
                        Task.Run(delegate
                        {
                            ChunkData chunkData5 = new ChunkData(creatorAPI);
                            creatorAPI.revokeData = new ChunkData(creatorAPI);
                            int num5 = 0;
                            foreach (Point3 item2 in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3((float)End2.X + x2, (float)End2.Y + y2, (float)End2.Z + z2), (int)x2, (int)y2, (int)z2))
                            {
                                if (!creatorAPI.launch)
                                {
                                    return;
                                }

                                creatorAPI.CreateBlock(item2, id, chunkData5);
                                num5++;
                            }

                            chunkData5.Render();
                        });
                    }
                }
                else
                {
                    Task.Run(delegate
                    {
                        ChunkData chunkData4 = new ChunkData(creatorAPI);
                        creatorAPI.revokeData = new ChunkData(creatorAPI);
                        int num4 = 0;
                        foreach (Point3 item3 in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3(creatorAPI.Position[0]), (int)XRadius.Value, (int)XRadius.Value, (int)XRadius.Value))
                        {
                            if (!creatorAPI.launch)
                            {
                                return;
                            }

                            creatorAPI.CreateBlock(item3, id, chunkData4);
                            num4++;
                        }

                        chunkData4.Render();
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num4), Color.LightYellow, blinking: true, playNotificationSound: true);
                    });
                }

                DialogsManager.HideDialog(this);
            }

            if (!HollowButton.IsClicked)
            {
                return;
            }

            if (advanced)
            {
                if (!DoublePosition)
                {
                    Task.Run(delegate
                    {
                        int num3 = 0;
                        ChunkData chunkData3 = new ChunkData(creatorAPI);
                        creatorAPI.revokeData = new ChunkData(creatorAPI);
                        foreach (Point3 item4 in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3(creatorAPI.Position[0]), (int)XRadius.Value, (int)YRadius.Value, (int)ZRadius.Value, Hollow: true))
                        {
                            if (!creatorAPI.launch)
                            {
                                return;
                            }

                            creatorAPI.CreateBlock(item4, id, chunkData3);
                            num3++;
                        }

                        chunkData3.Render();
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num3), Color.LightYellow, blinking: true, playNotificationSound: true);
                    });
                }
                else if (creatorAPI.Position[1].Y != -1)
                {
                    Point3 Start2 = creatorAPI.Position[0];
                    Point3 End = creatorAPI.Position[1];
                    CreatorMain.Math.StartEnd(ref Start2, ref End);
                    float x = Math.Abs((float)Start2.X - (float)End.X) / 2f;
                    float y = Math.Abs((float)Start2.Y - (float)End.Y) / 2f;
                    float z = Math.Abs((float)Start2.Z - (float)End.Z) / 2f;
                    Task.Run(delegate
                    {
                        ChunkData chunkData2 = new ChunkData(creatorAPI);
                        creatorAPI.revokeData = new ChunkData(creatorAPI);
                        int num2 = 0;
                        foreach (Point3 item5 in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3((float)End.X + x, (float)End.Y + y, (float)End.Z + z), (int)x, (int)y, (int)z, Hollow: true))
                        {
                            if (!creatorAPI.launch)
                            {
                                return;
                            }

                            creatorAPI.CreateBlock(item5, id, chunkData2);
                            num2++;
                        }

                        chunkData2.Render();
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
                    });
                }
            }
            else
            {
                Task.Run(delegate
                {
                    ChunkData chunkData = new ChunkData(creatorAPI);
                    creatorAPI.revokeData = new ChunkData(creatorAPI);
                    int num = 0;
                    foreach (Point3 item6 in creatorAPI.creatorGenerationAlgorithm.Sphere(new Vector3(creatorAPI.Position[0]), (int)XRadius.Value, (int)XRadius.Value, (int)XRadius.Value, Hollow: true))
                    {
                        if (!creatorAPI.launch)
                        {
                            return;
                        }

                        creatorAPI.CreateBlock(item6, id, chunkData);
                        num++;
                    }

                    chunkData.Render();
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                });
            }

            DialogsManager.HideDialog(this);
        }
    }
}