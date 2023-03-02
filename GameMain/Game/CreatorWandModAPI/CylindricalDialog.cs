using Engine;
using Game;
using System.Threading.Tasks;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class CylindricalDialog : PillarsDialog
    {
        private readonly SliderWidget ZRadius;

        private readonly LabelWidget zRadiusLabelWidget;

        public CylindricalDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("cyldialogn"));
            Children.Find<StackPanelWidget>("Data3").IsVisible = true;
            ZRadius = Children.Find<SliderWidget>("Slider3");
            zRadiusLabelWidget = Children.Find<LabelWidget>("Slider data3");
        }

        public override void Update()
        {
            base.Update();
            (radiusDelayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("cyldialog1"), (int)Radius.Value));
            (zRadiusLabelWidget).Text = (string.Format(CreatorMain.Display_Key_Dialog("cyldialog2"), (int)ZRadius.Value));
        }

        public override void upClickButton(int id)
        {
            if (SoildButton.IsClicked)
            {
                Task.Run(delegate
                {
                    int num2 = 0;
                    ChunkData chunkData2 = new ChunkData(creatorAPI);
                    creatorAPI.revokeData = new ChunkData(creatorAPI);
                    foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Cylindrical(new Vector3(creatorAPI.Position[0]), (int)Radius.Value, (int)Height.Value, (int)ZRadius.Value, createType, typeBool))
                    {
                        creatorAPI.CreateBlock(item, id, chunkData2);
                        num2++;
                        if (!creatorAPI.launch)
                        {
                            return;
                        }
                    }

                    chunkData2.Render();
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
                });
                DialogsManager.HideDialog(this);
            }

            if (!HollowButton.IsClicked)
            {
                return;
            }

            Task.Run(delegate
            {
                int num = 0;
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                foreach (Point3 item2 in creatorAPI.creatorGenerationAlgorithm.Cylindrical(new Vector3(creatorAPI.Position[0]), (int)Radius.Value, (int)Height.Value, (int)ZRadius.Value, createType, typeBool, Hollow: true))
                {
                    creatorAPI.CreateBlock(item2, id, chunkData);
                    num++;
                    if (!creatorAPI.launch)
                    {
                        return;
                    }
                }

                chunkData.Render();
                player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
            });
            DialogsManager.HideDialog(this);
        }
    }
}