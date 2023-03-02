using Engine;
using Game;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class SetDialog : Dialog
    {
        private class PasswordDialog : Dialog
        {
            private readonly ButtonWidget OK;

            private readonly ButtonWidget cancelButton;

            private readonly Game.TextBoxWidget TextBox;

            private readonly ComponentPlayer player;

            public PasswordDialog(ComponentPlayer player)
            {
                this.player = player;
                XElement node = ContentManager.Get<XElement>("Dialog/Manager3");
                LoadChildren(this, node);
                (Children.Find<LabelWidget>("Name")).Text = CreatorMain.Display_Key_Dialog("setdialopass");
                cancelButton = Children.Find<ButtonWidget>("Cancel");
                cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Manager3", "Cancel");
                OK = Children.Find<ButtonWidget>("OK");
                OK.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Manager3", "OK");
                TextBox = Children.Find<Game.TextBoxWidget>("BlockID");
                TextBox.Title = CreatorMain.Display_Key_Dialog("setdialopass1");
                TextBox.Text = "";
                Children.Find<BlockIconWidget>("Block").IsVisible = false;
                Children.Find<ButtonWidget>("SelectBlock").IsVisible = false;
                LoadProperties(this, node);
            }

            public override void Update()
            {
                if (cancelButton.IsClicked)
                {
                    DialogsManager.HideDialog(this);
                }

                if (OK.IsClicked)
                {
                    if (TextBox.Text == CreatorMain.Display_Key_Dialog("setdialopasst"))
                    {
                        CreatorMain.professional = true;
                        player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("Creator") + CreatorMain.version + Environment.OSVersion.Platform.ToString() + CreatorMain.Display_Key_Dialog("setdialopasstp") + string.Format(CreatorMain.Display_Key_Dialog("setdialopasstl"), CreatorAPI.Language), Color.LightYellow, blinking: true, playNotificationSound: false);
                    }
                    else
                    {
                        player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("Creator") + CreatorMain.version + Environment.OSVersion.Platform.ToString() + CreatorMain.Display_Key_Dialog("setdialopasstp1") + string.Format(CreatorMain.Display_Key_Dialog("setdialopasstl"), CreatorAPI.Language), Color.LightYellow, blinking: true, playNotificationSound: false);
                    }

                    DialogsManager.HideAllDialogs();
                    CreatorWidget.Dismiss(result: true);
                }
            }
        }

        private readonly ButtonWidget OK;

        private readonly SubsystemTerrain subsystemTerrain;

        private readonly ComponentPlayer player;

        private readonly ButtonWidget resettingButton;

        private readonly ButtonWidget generatingMod;

        private readonly ButtonWidget generatingSet;

        private readonly ButtonWidget SetPositionMode;

        private readonly ButtonWidget unLimited;

        private readonly ButtonWidget RevokeButton;

        private readonly ButtonWidget AirIdentifyButton;

        private readonly ButtonWidget professionButton;

        private readonly CreatorAPI creatorAPI;

        private readonly ButtonWidget setMainWidgetButton;

        public SetDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            XElement node = ContentManager.Get<XElement>("Dialog/ModSettings");
            LoadChildren(this, node);
            OK = Children.Find<ButtonWidget>("OK");
            OK.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "OK");
            generatingMod = Children.Find<ButtonWidget>("Generati");
            generatingMod.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Generati");
            resettingButton = Children.Find<ButtonWidget>("Reset");
            resettingButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Reset");
            generatingSet = Children.Find<ButtonWidget>("Generation");
            generatingSet.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Generation");
            SetPositionMode = Children.Find<ButtonWidget>("PointMode");
            SetPositionMode.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "PointMode");
            unLimited = Children.Find<ButtonWidget>("HyV Genera");
            unLimited.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "HyV Genera");
            AirIdentifyButton = Children.Find<ButtonWidget>("SelectAir");
            AirIdentifyButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "SelectAir");
            RevokeButton = Children.Find<ButtonWidget>("Undoing");
            RevokeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Undoing");
            professionButton = Children.Find<ButtonWidget>("Pro Mode");
            professionButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Pro Mode");
            setMainWidgetButton = Children.Find<ButtonWidget>("Old UI");
            setMainWidgetButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "ModSettings", "Old UI");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            AirIdentifyButton.Color = ((!creatorAPI.AirIdentify) ? Color.Red : Color.Yellow);
            generatingSet.Color = ((!creatorAPI.launch) ? Color.Red : Color.Yellow);
            unLimited.Color = ((!creatorAPI.UnLimitedOfCreate) ? Color.Red : Color.Yellow);
            RevokeButton.Color = ((!creatorAPI.RevokeSwitch) ? Color.Red : Color.Yellow);
            setMainWidgetButton.Color = ((!creatorAPI.oldMainWidget) ? Color.Red : Color.Yellow);
            if (professionButton.IsClicked)
            {
                DialogsManager.ShowDialog(creatorAPI.componentMiner.ComponentPlayer.ViewWidget.GameWidget, new PasswordDialog(creatorAPI.componentMiner.ComponentPlayer));
            }

            professionButton.IsEnabled = !CreatorMain.professional;
            unLimited.IsEnabled = CreatorMain.professional;
            switch (creatorAPI.amountPoint)
            {
                case CreatorAPI.NumberPoint.One:
                    SetPositionMode.Text = CreatorMain.Display_Key_Dialog("setdialogp1");
                    break;
                case CreatorAPI.NumberPoint.Two:
                    SetPositionMode.Text = CreatorMain.Display_Key_Dialog("setdialogp2");
                    break;
                case CreatorAPI.NumberPoint.Three:
                    SetPositionMode.Text = CreatorMain.Display_Key_Dialog("setdialogp3");
                    break;
                case CreatorAPI.NumberPoint.Four:
                    SetPositionMode.Text = CreatorMain.Display_Key_Dialog("setdialogp4");
                    break;
            }

            if (SetPositionMode.IsClicked)
            {
                DialogsManager.ShowDialog(null, new ListSelectionDialog(CreatorMain.Display_Key_Dialog("setdialospm"), new int[4]
                {
                    1,
                    2,
                    3,
                    4
                }, 56f, (object e) => string.Format("{0}{1}", (int)e, CreatorMain.Display_Key_Dialog("setdialospm1")), delegate (object e)
                {
                    creatorAPI.amountPoint = (CreatorAPI.NumberPoint)((int)e - 1);
                }));
            }

            if (unLimited.IsClicked)
            {
                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("setdialounl"), Color.Red, blinking: true, playNotificationSound: false);
                creatorAPI.UnLimitedOfCreate = !creatorAPI.UnLimitedOfCreate;
            }

            if (AirIdentifyButton.IsClicked)
            {
                creatorAPI.AirIdentify = !creatorAPI.AirIdentify;
            }

            if (RevokeButton.IsClicked)
            {
                creatorAPI.componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("setdialorbu"), Color.Red, blinking: true, playNotificationSound: false);
                creatorAPI.RevokeSwitch = !creatorAPI.RevokeSwitch;
            }

            switch (creatorAPI.CreateBlockType)
            {
                case CreateBlockType.Normal:
                    generatingMod.Text = CreatorMain.Display_Key_Dialog("setdialocbtn");
                    break;
                case CreateBlockType.Fast:
                    generatingMod.Text = CreatorMain.Display_Key_Dialog("setdialocbtq");
                    break;
                case CreateBlockType.Catch:
                    generatingMod.Text = CreatorMain.Display_Key_Dialog("setdialocbtc");
                    break;
            }

            if (resettingButton.IsClicked)
            {
                creatorAPI.launch = true;
                creatorAPI.CreateBlockType = CreateBlockType.Fast;
                generatingSet.Color = Color.Yellow;
                creatorAPI.amountPoint = CreatorAPI.NumberPoint.Two;
            }

            if (generatingMod.IsClicked)
            {
                IList<int> enumValues = EnumUtils.GetEnumValues(typeof(CreateBlockType));
                string[] createZhString = new string[3]
                {
                    CreatorMain.Display_Key_Dialog("setdialocbtn"),
                    CreatorMain.Display_Key_Dialog("setdialocbtq"),
                    CreatorMain.Display_Key_Dialog("setdialocbtc")
                };
                DialogsManager.ShowDialog(null, new ListSelectionDialog(CreatorMain.Display_Key_Dialog("setdialoscbt"), enumValues, 56f, (object e) => createZhString[(int)e], delegate (object e)
                {
                    creatorAPI.CreateBlockType = (CreateBlockType)e;
                }));
            }

            if (generatingSet.IsClicked)
            {
                creatorAPI.launch = !creatorAPI.launch;
            }

            if (OK.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (setMainWidgetButton.IsClicked)
            {
                creatorAPI.oldMainWidget = !creatorAPI.oldMainWidget;
            }
        }
    }
}