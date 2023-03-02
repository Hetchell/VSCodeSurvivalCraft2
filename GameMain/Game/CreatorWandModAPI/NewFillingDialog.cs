//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class NewFillingDialog : InterfaceDialog
    {
        private readonly ButtonWidget SoildButton;

        public NewFillingDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            base.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            XElement node = ContentManager.Get<XElement>("Dialog/Fill");
            LoadChildren(this, node);
            GeneralSet();
            (Children.Find<LabelWidget>("Fill")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Fill", "Fill"));
            SoildButton = Children.Find<ButtonWidget>("Fill1");
            SoildButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Fill", "Fill1");
            Children.Find<BevelledButtonWidget>("Cancel").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Fill", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            if (CreatorMain.Position != null)
            {
                if (CreatorMain.Position[0].Y > 0 && CreatorMain.Position[1].Y > 0)
                {
                    SoildButton.IsEnabled = true;
                }
                else
                {
                    SoildButton.IsEnabled = false;
                }
            }
            else
            {
                SoildButton.IsEnabled = false;
            }
            if (SoildButton.IsClicked)
            {
                Task.Run(delegate
                {
                    /*ChunkData chunkData = new ChunkData(creatorAPI);
                    creatorAPI.revokeData = new ChunkData(creatorAPI);*/
                    /*int num = 0;

                    foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Rectangular(creatorAPI.Position[0], creatorAPI.Position[1]))
                    {
                        if (!creatorAPI.launch)
                        {
                            return;
                        }

                        if (Terrain.ExtractContents(GameManager.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true).Terrain.GetCellValueFast(item.X, item.Y, item.Z)) == 0)
                        {
                            creatorAPI.CreateBlock(item, blockIconWidget.Value, chunkData);
                            num++;
                        }
                    }

                    chunkData.Render();*/
                    int num = CreatorWand2.CW2Fill.FillObject(creatorAPI.Position[0], creatorAPI.Position[1], blockIconWidget.Value);
                    // chunkData.Render();
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                });
                DialogsManager.HideDialog(this);
            }


        }
    }
}