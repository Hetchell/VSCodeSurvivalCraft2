using Engine;
using Game;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class TransferDialog : Dialog
    {
        private readonly ButtonWidget cancelButton;

        private readonly ButtonWidget Transfer;

        private readonly Game.TextBoxWidget X;

        private readonly Game.TextBoxWidget Y;

        private readonly Game.TextBoxWidget Z;

        private BevelledButtonWidget P1;

        private BevelledButtonWidget P2;

        private BevelledButtonWidget P3;

        private BevelledButtonWidget P4;

        private BevelledButtonWidget Spawn;

        private readonly SubsystemTerrain subsystemTerrain;

        private readonly ComponentPlayer player;

        // private Regex Regex;
        //Margin = new Vector2(120fx, 60fy)
        public TransferDialog(CreatorAPI creatorAPI)
        {
            P1 = new BevelledButtonWidget
            {
                Text = "Point1",
                Size = new Vector2(120f, 60f),
                Margin = new Vector2(0*120f, 0*60f)
            };
            P2 = new BevelledButtonWidget
            {
                Text = "Point2",
                Size = new Vector2(120f, 60f),
                Margin = new Vector2(0 * 120f, 1 * 60f)
            };
            P3 = new BevelledButtonWidget
            {
                Text = "Point3",
                Size = new Vector2(120f, 60f),
                Margin = new Vector2(0 * 120f, 2 * 60f)
            };
            P4 = new BevelledButtonWidget
            {
                Text = "Point4",
                Size = new Vector2(120f, 60f),
                Margin = new Vector2(0 * 120f, 3 * 60f)
            };
            Spawn = new BevelledButtonWidget
            {
                Text = "Spawn",
                Size = new Vector2(120f, 60f),
                Margin = new Vector2(0 * 120f, 4 * 60f)
            };
            player = creatorAPI.componentMiner.ComponentPlayer;
            subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            XElement node = ContentManager.Get<XElement>("Dialog/Teleport");
            LoadChildren(this, node);
            (Children.Find<LabelWidget>("Teleport")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "Teleport"));
            (Children.Find<LabelWidget>("X:")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "X:"));
            (Children.Find<LabelWidget>("Y:")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "Y:"));
            (Children.Find<LabelWidget>("Z:")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "Z:"));
            Children.Add(P1, P2, P3, P4, Spawn);
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "Cancel");
            Transfer = Children.Find<ButtonWidget>("OK");
            Transfer.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Teleport", "OK");
            X = Children.Find<Game.TextBoxWidget>("X");
            Y = Children.Find<Game.TextBoxWidget>("Y");
            Z = Children.Find<Game.TextBoxWidget>("Z");
            //Regex = new Regex("^-?[1-9]\\d*$");//"[^0-9]+");^-?[1-9]\d*$
            X.Text = ((int)player.ComponentBody.Position.X).ToString();
            Y.Text = ((int)player.ComponentBody.Position.Y).ToString();
            Z.Text = ((int)player.ComponentBody.Position.Z).ToString();
            P1.IsEnabled = false;
            P2.IsEnabled = false;
            P3.IsEnabled = false;
            P4.IsEnabled = false;
            LoadProperties(this, node);
        }

        public override void Update()
        {
            if (X.IsHitTestVisible || Y.IsHitTestVisible || Z.IsHitTestVisible)
            {
                X.Text = X.Text;// Regex.Replace(X.Text, "");
                Y.Text = Y.Text;// Regex.Replace(Y.Text, "");
                Z.Text = Z.Text;//Regex.Replace(Z.Text, "");
            }

            if (CreatorMain.Position != null)
            {
                Vector3 position = default(Vector3);
                bool flag = false;
                if (CreatorMain.Position[0].Y > 0)
                {
                    P1.IsEnabled = true;
                }

                if (CreatorMain.Position[1].Y > 0)
                {
                    P2.IsEnabled = true;
                }

                if (CreatorMain.Position[2].Y > 0)
                {
                    P3.IsEnabled = true;
                }

                if (CreatorMain.Position[3].Y > 0)
                {
                    P4.IsEnabled = true;
                }

                if (P1.IsClicked)
                {
                    position = new Vector3(CreatorMain.Position[0]);
                    flag = true;
                }

                if (P2.IsClicked)
                {
                    position = new Vector3(CreatorMain.Position[1]);
                    flag = true;
                }

                if (P3.IsClicked)
                {
                    position = new Vector3(CreatorMain.Position[2]);
                    flag = true;
                }

                if (P4.IsClicked)
                {
                    position = new Vector3(CreatorMain.Position[3]);
                    flag = true;
                }
                if (Spawn.IsClicked)
                {
                   
                    //player.ComponentBody.Position
                        position = player.PlayerData.SpawnPosition;
                    flag = true;
                }

                if (flag)
                {
                    if (CreatorWand2.CW2FindNoCollidable.FindNoCollidable(new Point3(position)))
                    {
                        player.ComponentBody.Position = position+new Vector3(0.5f,0,0.5f);

                    }
                    else if (CreatorWand2.CW2FindNoCollidable.FindNoCollidable(new Point3((int)position.X, (int)(position.Y+1), (int)position.Z)))
                    {
                        player.ComponentBody.Position = position + new Vector3(0.5f, 0, 0.5f);
                    }
                    bool finded = false;
                        for (int vectorL = 1; finded != true; vectorL++)
                        {
                            for (int i = 0; i < CreatorWand2.CW2FindNoCollidable.BlockToPoint3.Length; i++)
                            {
                                if (position.Y + CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i].Y * vectorL <= 0 || position.Y + 1 + CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i].Y * vectorL > 255)
                                {
                                    
                                }
                                if (CreatorWand2.CW2FindNoCollidable.FindNoCollidable(new Point3(position) + CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i] * vectorL))
                                {
                                    player.ComponentBody.Position = position + new Vector3(CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i]) * vectorL + new Vector3(0.5f, 0, 0.5f);
                                    finded = true;
                                    break;

                                }
                                else if (CreatorWand2.CW2FindNoCollidable.FindNoCollidable(new Point3((int)position.X, (int)(position.Y + 1), (int)position.Z) + CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i] * vectorL))
                                {
                                    player.ComponentBody.Position = new Vector3(position.X, position.Y + 1, position.Z) + new Vector3(CreatorWand2.CW2FindNoCollidable.BlockToPoint3[i]) * vectorL + new Vector3(0.5f, 0, 0.5f);
                                    finded = true;
                                    break;
                                }
                            }
                        }
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("tradialog2"), (int)position.X, (int)position.Y, (int)position.Z), Color.LightYellow, blinking: true, playNotificationSound: true);
                    DialogsManager.HideDialog(this);

                }
            }

            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (!Transfer.IsClicked)
            {
                return;
            }

            Point3 point = default(Point3);
            if (!int.TryParse(X.Text, out point.X) || !int.TryParse(Y.Text, out point.Y) || !int.TryParse(Z.Text, out point.Z) || (float)int.Parse(Y.Text) < 0f || (float)int.Parse(Y.Text) > 296f)
            {
                if ((float)int.Parse(Y.Text) < 0f || (float)int.Parse(Y.Text) > 296f)
                {
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("tradialog1") + (((float)int.Parse(Y.Text) < 0f || (float)int.Parse(Y.Text) > 296f) ? ": 0<Y<297" : ""), Color.Red, blinking: true, playNotificationSound: true);
                }
                else
                {
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("tradialog1"), Color.Red, blinking: true, playNotificationSound: true);
                }
            }
            else
            {
                Vector3 position2 = new Vector3(point.X, point.Y, point.Z);
                player.ComponentBody.Position = position2;
                player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("tradialog2"), (int)position2.X, (int)position2.Y, (int)position2.Z), Color.LightYellow, blinking: true, playNotificationSound: true);
            }

            DialogsManager.HideDialog(this);
        }
    }
}