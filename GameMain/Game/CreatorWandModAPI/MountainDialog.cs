//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class MountainDialog : Dialog
    {
        private readonly SliderWidget num1;

        private readonly SliderWidget num2;

        private readonly SliderWidget num3;

        private readonly LabelWidget num_1;

        private readonly LabelWidget num_2;

        private readonly LabelWidget num_3;

        private readonly ButtonWidget cancelButton;

        private readonly ButtonWidget OK;

        private readonly ButtonWidget restting;

        private readonly Game.TextBoxWidget TextBox;

        private readonly ComponentPlayer player;

        private readonly CreatorAPI creatorAPI;

        public MountainDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/Mountains");
            LoadChildren(this, node);
            (Children.Find<LabelWidget>("Mountains")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Mountains", "Mountains"));
            OK = Children.Find<ButtonWidget>("OK");
            OK.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Mountains", "OK");
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Mountains", "Cancel");
            num_1 = Children.Find<LabelWidget>("SliderData1");
            num_2 = Children.Find<LabelWidget>("SliderData2");
            num_3 = Children.Find<LabelWidget>("SliderData3");
            TextBox = Children.Find<Game.TextBoxWidget>("BlockID");
            num1 = Children.Find<SliderWidget>("Slider1");
            num2 = Children.Find<SliderWidget>("Slider2");
            num3 = Children.Find<SliderWidget>("Slider3");
            num3.Value = 100f;
            restting = Children.Find<ButtonWidget>("Reset");
            restting.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Mountains", "Reset");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            (num_1).Text = (string.Format(CreatorMain.Display_Key_Dialog("moundialog1"), (int)num1.Value));
            (num_2).Text = (string.Format(CreatorMain.Display_Key_Dialog("moundialog2"), (int)num2.Value));
            (num_3).Text = (string.Format(CreatorMain.Display_Key_Dialog("moundialog3"), (int)num3.Value));
            if (restting.IsClicked)
            {
                num1.Value = 0f;
                num2.Value = 0f;
                num3.Value = 100f;
                TextBox.Text = "3:2:8";
            }

            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (!OK.IsClicked)
            {
                return;
            }

            int? BlockID_2 = null;
            int? BlockID_3 = null;
            string[] array = TextBox.Text.Split(new char[1]
            {
                ':'
            }, StringSplitOptions.RemoveEmptyEntries);
            int BlockID_1;
            if (array.Length == 2)
            {
                if (!int.TryParse(array[0], out BlockID_1))
                {
                    BlockID_1 = 0;
                }

                if (int.TryParse(array[1], out int result))
                {
                    BlockID_2 = result;
                }
            }
            else if (array.Length >= 3)
            {
                if (!int.TryParse(array[0], out BlockID_1))
                {
                    BlockID_1 = 0;
                }

                if (int.TryParse(array[1], out int result2))
                {
                    BlockID_2 = result2;
                }

                if (int.TryParse(array[2], out result2))
                {
                    BlockID_3 = result2;
                }
            }
            else if (!int.TryParse(TextBox.Text, out BlockID_1))
            {
                BlockID_1 = 0;
            }

            Vector3 vector = new Vector3
            {
                X = num1.Value,
                Y = num2.Value,
                Z = num3.Value
            };
            Point3 Start = creatorAPI.Position[0];
            Point3 End = creatorAPI.Position[1];
            CreatorMain.Math.StartEnd(ref Start, ref End);
            float X_Radius = (float)(Start.X - End.X) / 2f;
            float Z_Radius = (float)(Start.Z - End.Z) / 2f;
            float Radius = ((double)X_Radius > (double)Z_Radius) ? X_Radius : Z_Radius;
            float radius = ((double)X_Radius > (double)Z_Radius) ? Z_Radius : X_Radius;
            Radius = Math.Abs(Math.Abs(Radius) - 2f + vector.X);
            if (!BlockID_2.HasValue)
            {
                BlockID_2 = BlockID_1;
            }

            if (!BlockID_3.HasValue)
            {
                BlockID_3 = BlockID_2;
            }

            Task.Run(delegate
            {
                ChunkData chunkData = new ChunkData(creatorAPI);
                creatorAPI.revokeData = new ChunkData(creatorAPI);
                Game.Random random = new Game.Random();
                double num = Math.PI / 2.0;
                int num2 = 0;
                float num3 = (float)(1.25 + (double)vector.Y / 99.0);
                double num4 = 25.0 + (double)vector.Z / 10.0;
                float num5 = random.Float(18f, (float)num4);
                for (int i = (int)(0f - X_Radius); i <= (int)X_Radius; i++)
                {
                    for (int j = (int)(0f - Z_Radius); (double)j <= (double)Z_Radius; j++)
                    {
                        double num6 = Math.Cos(num * (double)i / (double)Radius) * Math.Cos(num * (double)j / (double)radius) * (double)(Start.Y - End.Y);
                        double num7 = Math.Sin(num * (double)i * (double)num3 / (double)radius + 2.0) * Math.Cos(num * (double)j * (double)num3 / (double)Radius + 7.0) * (double)(Start.Y - End.Y) * 0.349999994039536;
                        double num8 = Math.Sin(num * (double)i * (double)num3 * 2.0 / (double)Radius + 2.0 * (double)num5) * Math.Sin(num * (double)j * (double)num3 * 2.0 / (double)radius + 8.0 * (double)num5) * (double)(Start.Y - End.Y) * 0.200000002980232;
                        double num9 = Math.Sin(num * (double)i * (double)num3 * 3.5 / (double)radius + 2.0 * (double)num5 * 1.5) * Math.Sin(num * (double)j * (double)num3 * 3.5 / (double)Radius + 12.0 * (double)num5 * 1.5) * (double)(Start.Y - End.Y) * 0.150000005960464;
                        double num10 = num7;
                        double num11 = num6 - num10 + num8 - num9;
                        if (num11 > 0.0)
                        {
                            for (int k = 0; (double)k <= num11; k++)
                            {
                                Point3 point = new Point3((Start.X + End.X) / 2 + i, End.Y + (int)num11 - k, (Start.Z + End.Z) / 2 + j);
                                if (k > 5)
                                {
                                    creatorAPI.CreateBlock(point, BlockID_1, chunkData);
                                }
                                else if (k > 0)
                                {
                                    creatorAPI.CreateBlock(point, BlockID_2.Value, chunkData);
                                }
                                else if (k == 0)
                                {
                                    creatorAPI.CreateBlock(point, BlockID_3.Value, chunkData);
                                }

                                num2++;
                                if (!creatorAPI.launch)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                chunkData.Render();
                player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num2), Color.LightYellow, blinking: true, playNotificationSound: true);
            });
            DialogsManager.HideDialog(this);
        }
    }
}