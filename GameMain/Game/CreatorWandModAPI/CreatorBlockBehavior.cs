using Engine;
using Engine.Graphics;
using Game;
using System.Collections.Generic;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class CreatorBlockBehavior : SubsystemBlockBehavior, IDrawable
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
                (Children.Find<LabelWidget>("Name")).Text = "请输入密匙";
                cancelButton = Children.Find<ButtonWidget>("Cancel");
                OK = Children.Find<ButtonWidget>("OK");
                TextBox = Children.Find<Game.TextBoxWidget>("BlockID");
                TextBox.Title = "请输入密匙";
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
                    if (TextBox.Text == CreatorMain.password)
                    {
                        CreatorMain.canUse = true;
                        player.ComponentGui.DisplaySmallMessage("创世神" + CreatorMain.version + "功能开启", Color.Yellow, blinking: true, playNotificationSound: false);
                    }
                    else
                    {
                        player.ComponentGui.DisplaySmallMessage("创世神" + CreatorMain.version + "功能开启失败", Color.Red, blinking: true, playNotificationSound: false);
                    }

                    DialogsManager.HideDialog(this);
                }
            }
        }

        public Dictionary<ComponentPlayer, CreatorAPI> dictionaryPlayers = new Dictionary<ComponentPlayer, CreatorAPI>();

        private readonly List<ComponentPlayer> listPlayer = new List<ComponentPlayer>();

        public override int[] HandledBlocks => new int[1]
        {
            195
        };

        public int[] DrawOrders => new int[1]
        {
            800
        };

        public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
        {
            TerrainRaycastResult? terrainRaycastResult = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging);
            if (!terrainRaycastResult.HasValue)
            {
                return false;
            }

            ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
            if (!dictionaryPlayers.TryGetValue(componentPlayer, out CreatorAPI value))
            {
                listPlayer.Add(componentPlayer);
                value = new CreatorAPI(componentMiner);
                dictionaryPlayers.Add(componentPlayer, value);
            }

            value.OnUse(terrainRaycastResult.Value);
            return true;
        }

        public override bool OnEditInventoryItem(IInventory inventory, int slotIndex, ComponentPlayer Player)
        {
            if (Player.ComponentGui.ModalPanelWidget is CreatorWidget)
            {
                Player.ComponentGui.ModalPanelWidget = null;
                DialogsManager.HideAllDialogs();
            }
            else
            {
                if (!dictionaryPlayers.TryGetValue(Player, out CreatorAPI value))
                {
                    listPlayer.Add(Player);
                    value = new CreatorAPI(Player.ComponentMiner);
                    dictionaryPlayers.Add(Player, value);
                }

                CreatorAPI.IsAddedToProject = true;
                if (CreatorMain.canUse)
                {
                    Player.ComponentGui.ModalPanelWidget = new CreatorWidget(value);
                }
                else
                {
                    DialogsManager.ShowDialog(Player.ViewWidget.GameWidget, new PasswordDialog(Player));
                }
            }

            return true;
        }

        public override bool OnHitAsProjectile(CellFace? cellFace, ComponentBody componentBody, WorldItem worldItem)
        {
            ((worldItem as Projectile)?.Owner as ComponentPlayer).ComponentBody.Position = worldItem.Position;
            return true;
        }

        public void Draw(Camera camera, int drawOrder)
        {
            foreach (ComponentPlayer item in listPlayer)
            {
                if ((double)item.ComponentHealth.Health <= 0.0)
                {
                    listPlayer.Remove(item);
                    break;
                }

                Vector3.TransformNormal(0.03f * Vector3.Normalize(Vector3.Cross(camera.ViewDirection, camera.ViewUp)), camera.ViewMatrix);
                Vector3.TransformNormal(-0.03f * Vector3.UnitY, camera.ViewMatrix);
                if (dictionaryPlayers.TryGetValue(item, out CreatorAPI value))
                {
                    if (value.Position[0].Y != -1 && value.Position[1].Y != -1)
                    {
                        Point3 Start = value.Position[0];
                        Point3 End = value.Position[1];
                        CreatorMain.Math.StarttoEnd(ref Start, ref End);
                        BoundingBox boundingBox = new BoundingBox(new Vector3(Start.X, Start.Y, Start.Z), new Vector3((float)End.X + 1f, (float)End.Y + 1f, (float)End.Z + 1f));
                        value.primitivesRenderer3D = new PrimitivesRenderer3D();
                        value.primitivesRenderer3D.FlatBatch(-1, DepthStencilState.None).QueueBoundingBox(boundingBox, Color.Blue);
                        value.primitivesRenderer3D.Flush(camera.ViewProjectionMatrix);
                    }

                    if (value.Position[2].Y != -1 && value.Position[3].Y != -1)
                    {
                        Point3 Start2 = value.Position[2];
                        Point3 End2 = value.Position[3];
                        CreatorMain.Math.StarttoEnd(ref Start2, ref End2);
                        BoundingBox boundingBox2 = new BoundingBox(new Vector3(Start2.X, Start2.Y, Start2.Z), new Vector3((float)End2.X + 1f, (float)End2.Y + 1f, (float)End2.Z + 1f));
                        value.primitivesRenderer3D = new PrimitivesRenderer3D();
                        value.primitivesRenderer3D.FlatBatch(-1, DepthStencilState.None).QueueBoundingBox(boundingBox2, Color.Red);
                        value.primitivesRenderer3D.Flush(camera.ViewProjectionMatrix);
                    }
                }
            }
        }
    }
}