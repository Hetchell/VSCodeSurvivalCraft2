using Engine;
using Game;
using System;
//using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class CopyPasteDialog : Dialog
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
                Children.Find<BevelledButtonWidget>("SelectBlock").IsVisible = false;
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

                string export_CopyFile_Directory = CreatorMain.Export_CopyFile_Directory;
                string text = (TextBox.Text.Length > 0) ? TextBox.Text : (DateTime.Now.ToString("yyyy-MM-dd") + "_" + DateTime.Now);
                if (!CreatorMain.CopyFile.IsFileInUse())
                {
                    try
                    {
                        System.IO.FileStream fileStream = new System.IO.FileStream(CreatorMain.CopyFile, System.IO.FileMode.Open);
                        if (!System.IO.Directory.Exists(export_CopyFile_Directory))
                        {
                            System.IO.Directory.CreateDirectory(export_CopyFile_Directory);
                        }

                        System.IO.FileStream fileStream2 = new System.IO.FileStream(export_CopyFile_Directory + "/" + text + ".w", System.IO.FileMode.OpenOrCreate);
                        fileStream.CopyTo(fileStream2);
                        fileStream2.Dispose();
                        fileStream.Dispose();
                        switch (CreatorAPI.Language)
                        {
                            case Language.zh_CN:
                                player.ComponentGui.DisplaySmallMessage("导出成功！文件所在位置：\n" + export_CopyFile_Directory + "/" + text + ".w", Color.LightYellow, blinking: true, playNotificationSound: false);
                                break;
                            case Language.en_US:
                                player.ComponentGui.DisplaySmallMessage("Export successful! File location: \n" + export_CopyFile_Directory + "/" + text + ".w", Color.LightYellow, blinking: true, playNotificationSound: false);
                                break;
                            default:
                                player.ComponentGui.DisplaySmallMessage("导出成功！文件所在位置：\n" + export_CopyFile_Directory + "/" + text + ".w", Color.LightYellow, blinking: true, playNotificationSound: false);
                                break;
                        }

                        DialogsManager.HideDialog(this);
                        listView.ClearItems();
                        if (!System.IO.Directory.Exists(CreatorMain.Export_CopyFile_Directory))
                        {
                            System.IO.Directory.CreateDirectory(CreatorMain.Export_CopyFile_Directory);
                        }

                        string[] files = System.IO.Directory.GetFiles(CreatorMain.Export_CopyFile_Directory);
                        foreach (string path in files)
                        {
                            if (System.IO.Path.GetExtension(path) == ".w")
                            {
                                listView.AddItem(System.IO.Path.GetFileName(path));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        switch (CreatorAPI.Language)
                        {
                            case Language.zh_CN:
                                player.ComponentGui.DisplaySmallMessage("发生了一个很严重的错误，\n 错误提示 :" + ex.Message + "\n" + export_CopyFile_Directory, Color.Red, blinking: true, playNotificationSound: false);
                                break;
                            case Language.en_US:
                                player.ComponentGui.DisplaySmallMessage("A very serious error has occurred, \n error message: " + ex.Message + "\n" + export_CopyFile_Directory, Color.Red, blinking: true, playNotificationSound: false);
                                break;
                            default:
                                player.ComponentGui.DisplaySmallMessage("发生了一个很严重的错误，\n 错误提示 :" + ex.Message + "\n" + export_CopyFile_Directory, Color.Red, blinking: true, playNotificationSound: false);
                                break;
                        }

                        DialogsManager.HideDialog(this);
                        DialogsManager.HideDialog(dialog);
                    }
                }
                else
                {
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Operation failed!", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                    }
                }

                DialogsManager.HideDialog(this);
            }
        }

        private readonly ButtonWidget cancelButton;

        private readonly ButtonWidget PasteButton;

        private readonly ButtonWidget CopyButton;

        private readonly ButtonWidget DerivedButton;

        private readonly ButtonWidget ImportButton;

        private readonly ButtonWidget MirrorButton;

        private readonly ComponentPlayer player;

        private readonly ListPanelWidget ListView;

        private readonly CreatorAPI creatorAPI;

        private readonly ButtonWidget DeleteButton;

        public CopyPasteDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/CopyandPaste");
            LoadChildren(this, node);
            (Children.Find<LabelWidget>("Copy and Paste")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Copy and Paste"));
            CopyButton = Children.Find<ButtonWidget>("Copy");
            CopyButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Copy");
            DerivedButton = Children.Find<ButtonWidget>("Export");
            DerivedButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Export");
            ImportButton = Children.Find<ButtonWidget>("Import");
            ImportButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Import");
            DeleteButton = Children.Find<ButtonWidget>("Delete");
            DeleteButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Delete");
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Cancel");
            PasteButton = Children.Find<ButtonWidget>("Paste");
            PasteButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Paste");
            MirrorButton = Children.Find<ButtonWidget>("Mirror");
            MirrorButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "CopyandPaste", "Mirror");
            MirrorBlockBehavior.IsMirror = false;
            ListView = Children.Find<ListPanelWidget>("ListView");
            UpList();
            LoadProperties(this, node);
        }

        private void UpList()
        {
            ListView.ClearItems();
            if (!System.IO.Directory.Exists(CreatorMain.Export_CopyFile_Directory))
            {
                System.IO.Directory.CreateDirectory(CreatorMain.Export_CopyFile_Directory);
            }

            string[] files = System.IO.Directory.GetFiles(CreatorMain.Export_CopyFile_Directory);
            foreach (string path in files)
            {
                if (System.IO.Path.GetExtension(path) == ".w")
                {
                    ListView.AddItem(System.IO.Path.GetFileName(path));
                }
            }
        }

        public override void Update()
        {
            MirrorButton.Color = (MirrorBlockBehavior.IsMirror ? Color.Yellow : Color.Gray);
            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (MirrorButton.IsClicked)
            {
                MirrorBlockBehavior.ChangeIsMirror();
            }

            if (PasteButton.IsClicked)
            {
                if (!CreatorMain.CopyFile.IsFileInUse())
                {
                    Task.Run(delegate
                    {
                        try
                        {


                            CreatorWand2.Cw2CopyAndPaste.PasetData(creatorAPI, CreatorMain.CopyFile, creatorAPI.Position[0], creatorAPI.Position[1]);

                            // CopyAndPaste.PasetData(creatorAPI, CreatorMain.CopyFile, creatorAPI.Position[0], creatorAPI.Position[1]);
                        }
                        catch (Exception ex2)
                        {
                            DialogsManager.ShowDialog(player.GuiWidget, new MessageDialog("Exception", ex2.Message, "OK", null, null));
                        }
                    });
                }
                else
                {
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Operation failed!", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                    }
                }

                DialogsManager.HideDialog(this);
            }

            if (ImportButton.IsClicked)
            {
                if (!System.IO.Directory.Exists(CreatorMain.CacheDirectory))
                {
                    System.IO.Directory.CreateDirectory(CreatorMain.CacheDirectory);
                }

                string copyFile = CreatorMain.CopyFile;
                string text = CreatorMain.Export_CopyFile_Directory + "/" + (string)ListView.SelectedItem;
                if (!text.IsFileInUse() && (!System.IO.File.Exists(copyFile) || !copyFile.IsFileInUse()))
                {
                    System.IO.FileStream fileStream = new System.IO.FileStream(text, System.IO.FileMode.Open);
                    System.IO.FileStream fileStream2 = new System.IO.FileStream(copyFile, System.IO.FileMode.Create);
                    fileStream.CopyTo(fileStream2);
                    fileStream2.Dispose();
                    fileStream.Dispose();
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("导入成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Imported successfully!", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("导入成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                    }
                }
                else
                {
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Operation failed!", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                    }
                }

                DialogsManager.HideDialog(this);
            }

            if (DeleteButton.IsClicked)
            {
                if ((CreatorMain.Export_CopyFile_Directory + "/" + (string)ListView.SelectedItem).Delete())
                {
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("删除成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Deleted successfully!", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("删除成功！", Color.LightYellow, blinking: true, playNotificationSound: false);
                            break;
                    }

                    UpList();
                }
                else
                {
                    switch (CreatorAPI.Language)
                    {
                        case Language.zh_CN:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        case Language.en_US:
                            player.ComponentGui.DisplaySmallMessage("Operation failed!", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                        default:
                            player.ComponentGui.DisplaySmallMessage("操作失败！", Color.Red, blinking: true, playNotificationSound: false);
                            break;
                    }
                }
            }

            DerivedButton.IsEnabled = System.IO.File.Exists(CreatorMain.CopyFile);
            if (DerivedButton.IsClicked)
            {
                DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new DerivedDialog(player, this, ListView));
            }

            if (CopyButton.IsClicked)
            {
                Task.Run(delegate
                {
                    try
                    {
                        if (!System.IO.Directory.Exists(CreatorMain.CacheDirectory))
                        {
                            System.IO.Directory.CreateDirectory(CreatorMain.CacheDirectory);
                        }
                        CreatorWand2.Cw2CopyAndPaste.CreateCopy(creatorAPI, CreatorMain.CacheDirectory, "CacheFile.cd", creatorAPI.Position[0], creatorAPI.Position[1]);
                        //CreatorWand2.Cw2CopyAndPaste.CreateCopy(creatorAPI, CreatorMain.CacheDirectory, "CacheFile.cd", creatorAPI.Position[0], creatorAPI.Position[1]);
                        //CopyAndPaste.CreateCopy(creatorAPI, CreatorMain.CacheDirectory, "CacheFile.cd", creatorAPI.Position[0], creatorAPI.Position[1]);
                    }
                    catch (Exception ex)
                    {
                        DialogsManager.ShowDialog(player.GuiWidget, new MessageDialog("Exception", ex.Message, "OK", null, null));
                    }
                });
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
        }
    }
}