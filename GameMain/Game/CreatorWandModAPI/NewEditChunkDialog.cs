//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class NewEditChunkDialog : Dialog
    {
        public SliderWidget TemperatureSlider;

        public SliderWidget HumiditySlider;

        public LabelWidget SliderData;

        public ButtonWidget OKButton;

        public ButtonWidget cancelButton;

        private readonly CreatorAPI creatorAPI;

        private readonly ComponentPlayer player;

        public NewEditChunkDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/EditChunk");
            LoadChildren(this, node);
            (Children.Find<LabelWidget>("EditingChunk")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditChunk", "EditingChunk"));
            SliderData = Children.Find<LabelWidget>("Slider Data");
            TemperatureSlider = Children.Find<SliderWidget>("Slider 1");
            TemperatureSlider.Value = (GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetTemperature(this.creatorAPI.Position[0].X, this.creatorAPI.Position[0].Z) + GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetTemperature(this.creatorAPI.Position[1].X, this.creatorAPI.Position[1].Z)) / 2;
            HumiditySlider = Children.Find<SliderWidget>("Slider 2");
            HumiditySlider.Value = (GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetHumidity(this.creatorAPI.Position[0].X, this.creatorAPI.Position[0].Z) + GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetHumidity(this.creatorAPI.Position[1].X, this.creatorAPI.Position[1].Z)) / 2;
            OKButton = Children.Find<ButtonWidget>("OK");
            OKButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditChunk", "OK");
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditChunk", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            if (OKButton.IsClicked)
            {
                Point3 Start = creatorAPI.Position[0];
                Point3 End = creatorAPI.Position[1];
                if (Start.Y != -1 && End.Y != -1)
                {
                    CreatorMain.Math.StarttoEnd(ref Start, ref End);
                    Point2 p = new Point2(int.MinValue, int.MinValue);
                    for (int i = Start.X; i <= End.X; i++)
                    {
                        for (int j = Start.Z; j <= End.Z; j++)
                        {
                            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.SetTemperature(i, j, (int)TemperatureSlider.Value);
                            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.SetHumidity(i, j, (int)HumiditySlider.Value);
                            if (!(p == new Point2(i >> 4 << 4, j >> 4 << 4)))
                            {
                                p = new Point2(i >> 4 << 4, j >> 4 << 4);
                                int cellValue = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValue(p.X, GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetTopHeight(i, j), p.Y);
                                GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(p.X, GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetTopHeight(p.X, p.Y), p.Y, Terrain.MakeBlockValue(1));
                                GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).ChangeCell(p.X, GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetTopHeight(p.X, p.Y), p.Y, cellValue);
                            }
                        }
                    }

                    GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Project.Save();
                    GameManager.Project.FindSubsystem<SubsystemUpdate>(throwOnError: true).Update();
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editchunkdialog1"), Color.LightYellow, blinking: true, playNotificationSound: true);
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editchunkdialog2") + $"\nStart:{Start.Y} End:{End.Y}", Color.LightYellow, blinking: true, playNotificationSound: true);
                }

                DialogsManager.HideDialog(this);
            }

            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            (SliderData).Text = (string.Format(CreatorMain.Display_Key_Dialog("editchunkdialog3"), (int)TemperatureSlider.Value, (int)HumiditySlider.Value));
        }
    }
}