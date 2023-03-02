using System.Xml.Linq;

namespace Game
{
  public class ManualDetailsDialog : CanvasWidget
  {
    public ComponentMiner m_componentMiner;
    public LabelWidget m_titleLabel;
    public LabelWidget m_detailsLabel;
    public ButtonWidget m_backButton;
    public float parentScroll;

    public ManualDetailsDialog(
      ComponentMiner componentMiner,
      InformationTopic informationTopic,
      float scroll)
    {
      this.m_componentMiner = componentMiner;
      this.parentScroll = scroll;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/ManualDetails"));
      this.m_backButton = this.Children.Find<ButtonWidget>("BackButton");
      this.m_titleLabel = this.Children.Find<LabelWidget>("InformationTopic.Title");
      this.m_detailsLabel = this.Children.Find<LabelWidget>("InformationTopic.Details");
      this.m_titleLabel.Text = informationTopic.Title;
      this.m_detailsLabel.Text = informationTopic.Details;
    }

    public override void Update()
    {
      if (!this.m_backButton.IsClicked)
        return;
      this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ManualTopicDialog(this.m_componentMiner, this.parentScroll);
    }
  }
}
