using Engine;
using Game;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = Engine.Color;

namespace CreatorWandModAPI
{
    public class NewReplaceDialog : Dialog
    {
        private readonly ButtonWidget cancelButton;

        private readonly ButtonWidget replaceButton;

        private readonly Game.TextBoxWidget Blockid;

        private readonly Game.TextBoxWidget Blockid2;

        private readonly ComponentPlayer player;

        private readonly CreatorAPI creatorAPI;

        public NewReplaceDialog(CreatorAPI creatorAPI)
        {
            this.creatorAPI = creatorAPI;
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement xElement = ContentManager.Get<XElement>("Dialog/Replace");
            LoadChildren(this, new XElement(xElement));
            (Children.Find<LabelWidget>("Replace1")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Replace", "Replace1"));
            (Children.Find<LabelWidget>("Index1:")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Replace", "Index1:"));
            (Children.Find<LabelWidget>("Index2:")).Text = (CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Replace", "Index2:"));
            cancelButton = Children.Find<ButtonWidget>("Cancel");
            cancelButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Replace", "Cancel");
            replaceButton = Children.Find<ButtonWidget>("Replace");
            replaceButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Replace", "Replace");
            Blockid = Children.Find<Game.TextBoxWidget>("BlockID");
            Blockid2 = Children.Find<Game.TextBoxWidget>("BlockID2");
            LoadProperties(this, xElement);
        }

        public override void Update()
        {
            if (cancelButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (!replaceButton.IsClicked)
            {
                return;
            }
            if (Blockid.Text == "")
            {
                Blockid.Text = "0";
            }
            if (Blockid2.Text == "")
            {
                Blockid2.Text = "0";
            }

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9]*$");
            System.Text.RegularExpressions.Match match = regex.Match(Blockid.Text);
            System.Text.RegularExpressions.Match match2 = regex.Match(Blockid2.Text);
            if (match.Success && match2.Success)
            {
                Task.Run(delegate
                {
                    int num = NewReplace.Replace(int.Parse(Blockid.Text), int.Parse(Blockid2.Text), creatorAPI.Position[0], creatorAPI.Position[1]);
                    player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("rpldialogrs"), num), Color.LightYellow, blinking: true, playNotificationSound: true);
                });
            }
            else
            {
                player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("rpldialogrf"), Color.Red, blinking: true, playNotificationSound: true);
            }

            DialogsManager.HideDialog(this);
        }
    }
}
