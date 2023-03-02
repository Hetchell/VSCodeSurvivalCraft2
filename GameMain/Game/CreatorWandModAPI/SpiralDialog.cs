using Engine;
using Game;
using System.Threading.Tasks;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class SpiralDialog : PillarsDialog
    {
        private readonly SliderWidget Number;

        private readonly LabelWidget numberLabelWidget;

        public SpiralDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("spidialog1"));
            Children.Find<StackPanelWidget>("Data3").IsVisible = true;
            Number = Children.Find<SliderWidget>("Slider3");
            numberLabelWidget = Children.Find<LabelWidget>("Slider data3");
            SoildButton.Text = CreatorMain.Display_Key_Dialog("spidialog2");
            HollowButton.IsVisible = false;
        }

        public override void Update()
        {
            base.Update();
            (radiusDelayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("spidialogr"), (int)Radius.Value));
            (numberLabelWidget).Text = (string.Format(CreatorMain.Display_Key_Dialog("spidialogn"), (int)Number.Value));
        }

        public override void upClickButton(int id)
        {
            if (!SoildButton.IsClicked)
            {
                return;
            }

            Task.Run(delegate
            {
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                int num = 0;
                foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Spiral(creatorAPI.Position[0], (int)Height.Value, (int)Radius.Value, (int)Number.Value, createType, typeBool))
                {
                    if (!creatorAPI.launch)
                    {
                        return;
                    }

                    creatorAPI.CreateBlock(item, id, chunkData);
                    num++;
                }

                chunkData.Render();
                player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
            });
            DialogsManager.HideDialog(this);
        }
    }
}
