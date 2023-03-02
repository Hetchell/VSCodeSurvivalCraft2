using System;
using System.Xml.Linq;

namespace Game
{
  public class ManualTopicDialog : CanvasWidget
  {
    public ListPanelWidget m_informationlistWidget;
    public ComponentMiner m_componentMiner;
    public ButtonWidget m_okButton;

    public ManualTopicDialog(ComponentMiner componentMiner, float scroll)
    {
      this.m_componentMiner = componentMiner;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/ManualList"));
      this.m_okButton = this.Children.Find<ButtonWidget>("OkButton");
      this.m_informationlistWidget = this.Children.Find<ListPanelWidget>("Information.List");
      this.m_informationlistWidget.ItemWidgetFactory = (Func<object, Widget>) (item =>
      {
        InformationTopic informationTopic = (InformationTopic) item;
        ContainerWidget containerWidget = (ContainerWidget) Widget.LoadWidget((object) this, ContentManager.Get<XElement>("Dialogs/ManualTopic"), (ContainerWidget) null);
        containerWidget.Children.Find<LabelWidget>("InformationTopic.Title").Text = informationTopic.Title;
        return (Widget) containerWidget;
      });
      this.m_informationlistWidget.ScrollPosition = scroll;
      this.m_informationlistWidget.ItemClicked += (Action<object>) (item => this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ManualDetailsDialog(this.m_componentMiner, (InformationTopic) item, this.m_informationlistWidget.ScrollPosition));
      foreach (object informationtopic in Datahandle.GetInformationtopics())
        this.m_informationlistWidget.AddItem(informationtopic);
    }

    public override void Update()
    {
      if (!this.m_okButton.IsClicked)
        return;
      this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) null;
    }
  }
}
