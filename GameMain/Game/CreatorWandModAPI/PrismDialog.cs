using Engine;
using Game;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class PrismDialog : InterfaceDialog
    {
        private readonly SliderWidget Radius;

        private readonly LabelWidget delayLabel;

        private readonly ButtonWidget SoildButton;

        private readonly ButtonWidget HollowButton;

        public PrismDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            base.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            XElement node = ContentManager.Get<XElement>("Dialog/Octahedron");
            LoadChildren(this, node);
            GeneralSet();
            setShaftXYZ();
            (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("pddialog1"));
            Radius = Children.Find<SliderWidget>("Slider");
            delayLabel = Children.Find<LabelWidget>("Slider data");
            Children.Find<ButtonWidget>("X-axis").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "X-axis");
            Children.Find<ButtonWidget>("Y-axis").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "Y-axis");
            Children.Find<ButtonWidget>("Z-axis").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "Z-axis");
            SoildButton = Children.Find<ButtonWidget>("Solid");
            SoildButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "Solid");
            HollowButton = Children.Find<ButtonWidget>("Hollow");
            HollowButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "Hollow");
            Children.Find<ButtonWidget>("Cancel").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Octahedron", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            (delayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("pddialog2"), (int)Radius.Value));
            upDataButton();
            if (SoildButton.IsClicked)
            {
                Task.Run(delegate
                {
                    ChunkData chunkData2 = new ChunkData(creatorAPI);
                    creatorAPI.revokeData = new ChunkData(creatorAPI);
                    int num2 = 0;
                    foreach (Point3 item in creatorAPI.creatorGenerationAlgorithm.Prism(creatorAPI.Position[0], (int)Radius.Value, createType))
                    {
                        creatorAPI.CreateBlock(item, blockIconWidget.Value, chunkData2);
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
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                int num = 0;
                foreach (Point3 item2 in creatorAPI.creatorGenerationAlgorithm.Prism(creatorAPI.Position[0], (int)Radius.Value, createType, Hollow: true))
                {
                    creatorAPI.CreateBlock(item2, blockIconWidget.Value, chunkData);
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