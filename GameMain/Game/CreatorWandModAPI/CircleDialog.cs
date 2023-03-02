using Engine;
using Game;
using System.Threading.Tasks;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class CircleDialog : PillarsDialog
    {
        public CircleDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("cirdialogn"));
            Y_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialogx2");
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
                    foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Circle(creatorAPI.Position[0], (int)Height.Value, (int)Radius.Value, createType))
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
                foreach (Point3 item2 in creatorAPI.creatorGenerationAlgorithm.Circle(creatorAPI.Position[0], (int)Height.Value, (int)Radius.Value, createType, Hollow: true))
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

        public override void upDataButton(CreatorMain.CreateType createType, ButtonWidget button)
        {
            if (base.createType != createType)
            {
                base.createType = createType;
                button.Color = Color.Green;
                if (X_Shaft != button)
                {
                    X_Shaft.Color = Color.White;
                }

                if (Y_Shaft != button)
                {
                    Y_Shaft.Color = Color.White;
                }

                if (Z_Shaft != button)
                {
                    Z_Shaft.Color = Color.White;
                }
            }
        }
    }
}