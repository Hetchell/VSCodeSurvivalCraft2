using Engine;
using Game;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class LevelSetDialog : Dialog
    {
        private readonly SliderWidget Radius;

        private readonly LabelWidget delayLabel;

        private readonly ButtonWidget plusButton;

        private readonly ButtonWidget minusButton;

        private readonly ButtonWidget OK;

        private readonly ComponentPlayer player;

        private readonly ButtonWidget Cancel;

        public LevelSetDialog(CreatorAPI creatorAPI)
        {
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/Level");
            LoadChildren(this, node);
            (Children.Find<LabelWidget>("Level1")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Level", "Level1"));
            Radius = Children.Find<SliderWidget>("Level");
            plusButton = Children.Find<ButtonWidget>("Add button");
            minusButton = Children.Find<ButtonWidget>("Sub button");
            delayLabel = Children.Find<LabelWidget>("Slider");
            OK = Children.Find<ButtonWidget>("OK");
            OK.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Level", "OK");
            Cancel = Children.Find<ButtonWidget>("Cancel");
            Cancel.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Level", "Cancel");
            Radius.Value = player.PlayerData.Level;
            Radius.MinValue = 1f;
            Radius.MaxValue = 21f;
            UpdateControls();
            LoadProperties(this, node);
        }

        public override void Update()
        {
            if (Cancel.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (OK.IsClicked)
            {
                player.PlayerData.Level = Radius.Value;
                player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("editleveldialogs"), (int)Radius.Value), Color.LightYellow, blinking: true, playNotificationSound: true);
                DialogsManager.HideDialog(this);
            }

            if (minusButton.IsClicked)
            {
                Radius.Value = MathUtils.Max(Radius.Value - 1f, (int)Radius.MinValue);
            }

            if (plusButton.IsClicked)
            {
                Radius.Value = MathUtils.Min(Radius.Value + 1f, (int)Radius.MaxValue);
            }

            UpdateControls();
        }

        private void UpdateControls()
        {
            minusButton.IsEnabled = ((double)Radius.Value > (double)Radius.MinValue);
            plusButton.IsEnabled = ((double)Radius.Value < (double)Radius.MaxValue);
            (delayLabel).Text = (string.Format(CreatorMain.Display_Key_Dialog("editleveldialogl"), (int)Radius.Value));
        }
    }
}
