/*
MIT License

Copyright (c) 2017 Lixue_jiu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
