using Engine;
using Game;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class EditRegionDialog : Dialog
    {
        public SliderWidget TemperatureSlider;

        public SliderWidget HumiditySlider;

        public SliderWidget TopHeightSlider;

        public LabelWidget SliderData;

        public ButtonWidget OKButton;

        public ButtonWidget cancelButton;

        private readonly CreatorAPI creatorAPI;

        private readonly ComponentPlayer player;

        public EditRegionDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/编辑区域");
            LoadChildren(this, node);
            SliderData = Children.Find<LabelWidget>("滑条数据");
            TemperatureSlider = Children.Find<SliderWidget>("滑条1");
            TemperatureSlider.Value = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetSeasonalTemperature(this.creatorAPI.Position[1].X, this.creatorAPI.Position[1].Z);
            HumiditySlider = Children.Find<SliderWidget>("滑条2");
            HumiditySlider.Value = GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetSeasonalHumidity(this.creatorAPI.Position[1].X, this.creatorAPI.Position[1].Z);
            TopHeightSlider = Children.Find<SliderWidget>("滑条3");
            OKButton = Children.Find<ButtonWidget>("确定");
            cancelButton = Children.Find<ButtonWidget>("取消");
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
                    CreatorMain.Math.StartEnd(ref Start, ref End);
                    for (int i = End.X; i <= Start.X; i++)
                    {
                        for (int j = End.Z; j <= Start.Z; j++)
                        {
                            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetChunkAtCell(i, j).SetTemperatureFast(i, j, (int)TemperatureSlider.Value);
                            GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetChunkAtCell(i, j).SetHumidityFast(i, j, (int)HumiditySlider.Value);
                        }
                    }

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