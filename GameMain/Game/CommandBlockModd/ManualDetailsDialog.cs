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
