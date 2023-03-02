//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
//using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class RectangularDialog : InterfaceDialog
    {
        private readonly ButtonWidget SoildButton;

        private readonly ButtonWidget HollowButton;

        private readonly ButtonWidget FrameworkButton;

        public RectangularDialog(CreatorAPI creatorAPI)
            : base(creatorAPI)
        {
            base.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            XElement node = ContentManager.Get<XElement>("Dialog/Cuboid");
            LoadChildren(this, node);
            GeneralSet();
            (Children.Find<LabelWidget>("Cuboid")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Cuboid", "Cuboid"));
            SoildButton = Children.Find<ButtonWidget>("Solid");
            SoildButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Cuboid", "Solid");
            HollowButton = Children.Find<ButtonWidget>("Hollow");
            HollowButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Cuboid", "Hollow");
            FrameworkButton = Children.Find<ButtonWidget>("Edges");
            FrameworkButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Cuboid", "Edges");
            Children.Find<ButtonWidget>("Cancel").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Cuboid", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            if (SoildButton.IsClicked)
            {
                try
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        int a = blockIconWidget.Value;
                        int num2 = CreatorWand2.CW2Cuboid.GeometryCuboid(creatorAPI.Position[0], creatorAPI.Position[1], a);
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);

                    });
                }
                catch (System.Exception e)
                {

                    Log.Error("Err:" + e);
                }


                /*Task.Run(() =>
                {
                    int a = blockIconWidget.Value;
                    int num3 = CreatorWand2.CW2Cuboid.GeometryCuboid(creatorAPI.Position[0], creatorAPI.Position[1], a);
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num3), Color.LightYellow, blinking: true, playNotificationSound: true);
                });*/
                DialogsManager.HideAllDialogs();
            }

            if (HollowButton.IsClicked)
            {


                try
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        int a = blockIconWidget.Value;
                        int num2 = CreatorWand2.CW2Cuboid.GeometryBox(creatorAPI.Position[0], creatorAPI.Position[1], a);
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);

                    });
                }
                catch (System.Exception e)
                {

                    Log.Error("Err:" + e);
                }

                /*Task.Run(() =>
                {
                    int a = blockIconWidget.Value;
                    int num2 = CreatorWand2.CW2Cuboid.GeometryBox(creatorAPI.Position[0], creatorAPI.Position[1], a);
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
                });*/
                DialogsManager.HideAllDialogs();
            }

            if (FrameworkButton.IsClicked)
            {
                try
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        int a = blockIconWidget.Value;
                        int num2 = CreatorWand2.CW2Cuboid.GeometryEdge(creatorAPI.Position[0], creatorAPI.Position[1], a);
                        player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
                    });
                }
                catch (System.Exception e)
                {

                    Log.Error("Err:" + e);
                }

                /*Task.Run(() =>
                {
                    int a = blockIconWidget.Value;
                    int num = CreatorWand2.CW2Cuboid.GeometryEdge(creatorAPI.Position[0], creatorAPI.Position[1], a);
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                });*/
                DialogsManager.HideAllDialogs();
                //DialogsManager.HideDialog(this);

            }


        }
    }
}
