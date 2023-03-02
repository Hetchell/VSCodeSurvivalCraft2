using Engine;
using Game;
using System.Threading.Tasks;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class PrismColumnDialog : PillarsDialog
    {
        public PrismColumnDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("pcddialog1"));
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
                    foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.PrismColumn(creatorAPI.Position[0], (int)Radius.Value, (int)Height.Value, createType, typeBool))
                    {
                        creatorAPI.CreateBlock(item, id, chunkData2);
                        num2++;
                        if (!creatorAPI.launch)
                        {
                            return;
                        }
                    }

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
                foreach (Point3 item2 in creatorAPI.creatorGenerationAlgorithm.PrismColumn(creatorAPI.Position[0], (int)Radius.Value, (int)Height.Value, createType, typeBool, Hollow: true))
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