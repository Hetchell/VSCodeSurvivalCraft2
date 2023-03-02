using Engine;
using Game;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class OnekeyGenerationDialog : Dialog
    {
        private class DerivedDialog : Dialog
        {
            private readonly ButtonWidget OK;

            private readonly ButtonWidget cancelButton;

            private readonly Game.TextBoxWidget TextBox;

            private readonly ComponentPlayer player;

            private readonly Dialog dialog;

            private readonly ListPanelWidget listView;

            public DerivedDialog(ComponentPlayer player, Dialog dialog, ListPanelWidget listView)
            {
                this.player = player;
                this.dialog = dialog;
                this.listView = listView;
                XElement node = ContentManager.Get<XElement>("Dialog/Manager3");
                LoadChildren(this, node);
                (Children.Find<LabelWidget>("Name")).Text = (CreatorMain.Display_Key_Dialog("moddialogname"));
                cancelButton = Children.Find<ButtonWidget>("Cancel");
                cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Manager3", "Cancel");
                OK = Children.Find<ButtonWidget>("OK");
                OK.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Manager3", "OK");
                TextBox = Children.Find<Game.TextBoxWidget>("BlockID");
                TextBox.Text = "";
                Children.Find<BlockIconWidget>("Block").IsVisible = false;
                LoadProperties(this, node);
            }

            public override void Update()
            {
                if (cancelButton.IsClicked)
                {
                    DialogsManager.HideDialog(this);
                }

                if (!OK.IsClicked)
                {
                    return;
                }

                string export_OnekeyFile_Directory = CreatorMain.Export_OnekeyFile_Directory;
                string text = (TextBox.Text.Length > 0) ? TextBox.Text : (DateTime.Now.ToString("yyyy-MM-dd") + "_" + DateTime.Now);
                if (!CreatorMain.OneKeyFile.IsFileInUse())
                {
                    try
                    {
                        FileStream fileStream = new FileStream(CreatorMain.OneKeyFile, FileMode.Open);
                        if (!Directory.Exists(export_OnekeyFile_Directory))
                        {
                            Directory.CreateDirectory(export_OnekeyFile_Directory);
                        }

                        FileStream fileStream2 = new FileStream(export_OnekeyFile_Directory + "/" + text + ".o", FileMode.OpenOrCreate);
                        fileStream.CopyTo(fileStream2);
                        fileStream2.Dispose();
                        fileStream.Dispose();
                        player.ComponentGui.DisplaySmallMessage("导出成功！文件所在位置：\n" + export_OnekeyFile_Directory + "/" + text + ".o", Color.LightGreen, blinking: true, playNotificationSound: false);
                        DialogsManager.HideDialog(this);
                        listView.ClearItems();
                        if (!Directory.Exists(CreatorMain.Export_OnekeyFile_Directory))
                        {
                            Directory.CreateDirectory(CreatorMain.Export_OnekeyFile_Directory);
                        }

                        string[] files = Directory.GetFiles(CreatorMain.Export_OnekeyFile_Directory);
                        foreach (string path in files)
                        {
                            if (Path.GetExtension(path) == ".o")
                            {
                                listView.AddItem(Path.GetFileName(path));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        player.ComponentGui.DisplaySmallMessage("发生了一个很严重的错误，\n 错误提示 :" + ex.Message + "\n" + export_OnekeyFile_Directory, Color.Red, blinking: true, playNotificationSound: false);
                        DialogsManager.HideDialog(this);
                        DialogsManager.HideDialog(dialog);
                    }
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                }

                DialogsManager.HideDialog(this);
            }
        }

        public static readonly string SdcardOnekey = CreatorMain.Sdcard;

        private readonly ButtonWidget OKButton;

        private readonly ButtonWidget TypeButton;

        private readonly ButtonWidget CreateButton;

        private readonly ButtonWidget DerivedButton;

        private readonly ButtonWidget ImportButton;

        private readonly ButtonWidget OnAndOffButton;

        private readonly ComponentPlayer player;

        private readonly ListPanelWidget ListView;

        private readonly CreatorAPI creatorAPI;

        private readonly ButtonWidget DeleteButton;

        public OnekeyGenerationDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/OneClickGeneration");
            LoadChildren(this, node);
            CreateButton = Children.Find<ButtonWidget>("Create");
            CreateButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "Create");
            OnAndOffButton = Children.Find<ButtonWidget>("On");
            OnAndOffButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "On");
            DerivedButton = Children.Find<ButtonWidget>("Export");
            DerivedButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "Export");
            ImportButton = Children.Find<ButtonWidget>("Import");
            ImportButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "Import");
            DeleteButton = Children.Find<ButtonWidget>("Delete");
            DeleteButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "Delete");
            OKButton = Children.Find<ButtonWidget>("OK");
            OKButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "OK");
            ListView = Children.Find<ListPanelWidget>("ListView");
            TypeButton = Children.Find<ButtonWidget>("Type");
            TypeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "OneClickGeneration", "Type");
            UpList();
            LoadProperties(this, node);
        }

        private void UpList()
        {
            ListView.ClearItems();
            if (!Directory.Exists(CreatorMain.Export_OnekeyFile_Directory))
            {
                Directory.CreateDirectory(CreatorMain.Export_OnekeyFile_Directory);
            }

            string[] files = Directory.GetFiles(CreatorMain.Export_OnekeyFile_Directory);
            foreach (string path in files)
            {
                if (Path.GetExtension(path) == ".o")
                {
                    ListView.AddItem(Path.GetFileName(path));
                }
            }
        }

        public override void Update()
        {
            TypeButton.IsEnabled = false;
            if (OKButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (ImportButton.IsClicked)
            {
                if (!Directory.Exists(CreatorMain.CacheDirectory))
                {
                    Directory.CreateDirectory(CreatorMain.CacheDirectory);
                }

                string oneKeyFile = CreatorMain.OneKeyFile;
                string text = CreatorMain.Export_OnekeyFile_Directory + "/" + (string)ListView.SelectedItem;
                if (!text.IsFileInUse() && (!File.Exists(oneKeyFile) || !oneKeyFile.IsFileInUse()))
                {
                    FileStream fileStream = new FileStream(text, FileMode.Open);
                    FileStream fileStream2 = new FileStream(oneKeyFile, FileMode.Create);
                    fileStream.CopyTo(fileStream2);
                    fileStream2.Dispose();
                    fileStream.Dispose();
                    player.ComponentGui.DisplaySmallMessage("导入成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                }

                DialogsManager.HideDialog(this);
            }

            if (DeleteButton.IsClicked)
            {
                if ((CreatorMain.Export_OnekeyFile_Directory + "/" + (string)ListView.SelectedItem).Delete())
                {
                    player.ComponentGui.DisplaySmallMessage("删除成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                    UpList();
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                }
            }

            DerivedButton.IsEnabled = File.Exists(CreatorMain.OneKeyFile);
            if (DerivedButton.IsClicked)
            {
                DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new DerivedDialog(player, this, ListView));
            }

            if (CreateButton.IsClicked)
            {
                if (creatorAPI.Position[2] == new Point3(0, -1, 0))
                {
                    player.ComponentGui.DisplaySmallMessage("请设置点3！", Color.LightRed, blinking: true, playNotificationSound: false);
                }
                else
                {
                    Task.Run(delegate
                    {
                        try
                        {
                            if (!Directory.Exists(CreatorMain.CacheDirectory))
                            {
                                Directory.CreateDirectory(CreatorMain.CacheDirectory);
                            }

                            OnekeyGeneration.CreateOnekey(creatorAPI, CreatorMain.CacheDirectory + "/", "CacheFile.od", creatorAPI.Position[0], creatorAPI.Position[1], creatorAPI.Position[2]);
                        }
                        catch (Exception ex)
                        {
                            player.ComponentGui.DisplaySmallMessage(ex.Message, Color.Red, blinking: true, playNotificationSound: false);
                        }
                    });
                }

                DialogsManager.HideDialog(this);
            }

            if (!ListView.SelectedIndex.HasValue)
            {
                ImportButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
            else
            {
                ImportButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }

            OnAndOffButton.Color = ((!creatorAPI.oneKeyGeneration) ? Color.Red : Color.Yellow);
            if (OnAndOffButton.IsClicked)
            {
                if (creatorAPI.oneKeyGeneration)
                {
                    creatorAPI.oneKeyGeneration = false;
                }
                else
                {
                    creatorAPI.oneKeyGeneration = true;
                }
            }
        }
    }
}
