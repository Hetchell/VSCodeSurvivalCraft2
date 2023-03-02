using Engine;
using Game;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class SetModeDialog : Dialog
    {
        private readonly ComponentPlayer player;

        private readonly ButtonWidget CreativeButton;

        private readonly ButtonWidget ChallengingButton;

        private readonly ButtonWidget cancelButton;

        public ButtonWidget HarmlessButton;

        public ButtonWidget AdventureButton;

        public ButtonWidget CruelButton;

        private readonly SubsystemGameInfo gameInfo;

        public SetModeDialog(CreatorAPI creatorAPI)
        {
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/GameMode");
            LoadChildren(this, node);
            CreativeButton = Children.Find<ButtonWidget>("Creative");
            CreativeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Creative");
            ChallengingButton = Children.Find<ButtonWidget>("Challenging");
            ChallengingButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Challenging");
            HarmlessButton = Children.Find<ButtonWidget>("Harmless");
            HarmlessButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Harmless");
            AdventureButton = Children.Find<ButtonWidget>("Adventure");
            AdventureButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Adventure");
            CruelButton = Children.Find<ButtonWidget>("Cruel");
            CruelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Cruel");
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "GameMode", "Cancel");
            gameInfo = player.Project.FindSubsystem<SubsystemGameInfo>(throwOnError: true);
            switch (gameInfo.WorldSettings.GameMode)
            {
                case GameMode.Creative:
                    CreativeButton.IsEnabled = false;
                    break;
                case GameMode.Challenging:
                    ChallengingButton.IsEnabled = false;
                    break;
                case GameMode.Harmless:
                    HarmlessButton.IsEnabled = false;
                    break;
                case GameMode.Adventure:
                    AdventureButton.IsEnabled = false;
                    break;
                case GameMode.Cruel:
                    CruelButton.IsEnabled = false;
                    break;
            }

            LoadProperties(this, node);
        }

        public override void Update()
        {
            CruelButton.IsEnabled = CreatorMain.professional;
            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (CreativeButton.IsClicked)
            {
                gameInfo.WorldSettings.GameMode = GameMode.Creative;
                player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editmodedialogcv"), Color.LightYellow, blinking: true, playNotificationSound: true);
                UpDataWorld();
            }

            if (ChallengingButton.IsClicked)
            {
                if (gameInfo.WorldSettings.GameMode == GameMode.Creative)
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Challenging;
                    UpDataWorld();
                }
                else
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Challenging;
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editmodedialogc"), Color.LightYellow, blinking: true, playNotificationSound: true);
                    DialogsManager.HideDialog(this);
                }
            }

            if (HarmlessButton.IsClicked)
            {
                if (gameInfo.WorldSettings.GameMode == GameMode.Creative)
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Harmless;
                    UpDataWorld();
                }
                else
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Harmless;
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editmodedialogh"), Color.LightYellow, blinking: true, playNotificationSound: true);
                    DialogsManager.HideDialog(this);
                }
            }

            if (AdventureButton.IsClicked)
            {
                if (gameInfo.WorldSettings.GameMode == GameMode.Creative)
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Adventure;
                    UpDataWorld();
                }
                else
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Adventure;
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editmodedialoga"), Color.LightYellow, blinking: true, playNotificationSound: true);
                    DialogsManager.HideDialog(this);
                }
            }

            if (CruelButton.IsClicked)
            {
                if (gameInfo.WorldSettings.GameMode == GameMode.Creative)
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Cruel;
                    UpDataWorld();
                }
                else
                {
                    gameInfo.WorldSettings.GameMode = GameMode.Cruel;
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editmodedialogcr"), Color.LightYellow, blinking: true, playNotificationSound: true);
                    DialogsManager.HideDialog(this);
                }
            }
        }

        public void UpDataWorld()
        {
            WorldInfo worldInfo = GameManager.WorldInfo;
            GameManager.SaveProject(waitForCompletion: true, showErrorDialog: true);
            GameManager.DisposeProject();
            ScreensManager.SwitchScreen("GameLoading", worldInfo, null);
        }
    }
}
