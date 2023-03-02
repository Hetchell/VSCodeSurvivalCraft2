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

using Engine;
using System;
using System.Xml.Linq;
using Color = Engine.Color;

namespace Game
{
  public class InstructionSearchDialog : CanvasWidget
  {
    public SubsystemCommandBlockBehavior m_subsystemCommandBlockBehavior;
    public TextBoxWidget m_textBox;
    public ButtonWidget m_quireButton;
    public ButtonWidget m_okButton;
    public ButtonWidget m_backButton;
    public ListPanelWidget m_instructionlistWidget;
    public ComponentMiner m_componentMiner;
    public bool IsCondition;
    public float parentScroll;

    public InstructionSearchDialog(ComponentMiner componentMiner, bool condition, float scroll)
    {
      this.m_componentMiner = componentMiner;
      this.parentScroll = scroll;
      this.IsCondition = condition;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/InstructionSearch"));
      this.m_instructionlistWidget = this.Children.Find<ListPanelWidget>("Instruction.List");
      this.m_textBox = this.Children.Find<TextBoxWidget>("TextBox");
      this.m_quireButton = this.Children.Find<ButtonWidget>("QuireButton");
      this.m_okButton = this.Children.Find<ButtonWidget>("OkButton");
      this.m_backButton = this.Children.Find<ButtonWidget>("BackButton");
      this.m_instructionlistWidget.ItemWidgetFactory = (Func<object, Widget>) (item =>
      {
        Instruction instruction = (Instruction) item;
        ContainerWidget containerWidget = (ContainerWidget) Widget.LoadWidget((object) this, ContentManager.Get<XElement>("Dialogs/InstructionItem"), (ContainerWidget) null);
        containerWidget.Children.Find<LabelWidget>("InstructionItem.Name").Text = instruction.Name;
        containerWidget.Children.Find<LabelWidget>("InstructionItem.Details").Text = instruction.Details;
        return (Widget) containerWidget;
      });
      this.m_instructionlistWidget.ItemClicked += (Action<object>) (item =>
      {
        Instruction instruction = (Instruction) item;
        ClipboardManager.ClipboardString = instruction.Name;
        this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("已复制指令：" + instruction.Name, Color.Yellow, false, false);
      });
    }

    public override void Update()
    {
      if (this.m_quireButton.IsClicked)
      {
        if (this.m_textBox.Text == "" || this.m_textBox.Text == " ")
        {
          this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("请输入关键词", Color.Yellow, false, false);
          return;
        }
        if (this.m_textBox.Text.Contains("value") || this.m_textBox.Text == "keyword")
        {
          this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("‘value’与‘keyword’为无效的关键词", Color.Yellow, false, false);
          return;
        }
        this.m_instructionlistWidget.ClearItems();
        foreach (object obj in Datahandle.GetSearchInstructionlist(this.m_textBox.Text))
          this.m_instructionlistWidget.AddItem(obj);
        this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("查找完成", Color.Yellow, false, false);
      }
      if (this.m_okButton.IsClicked)
        this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) null;
      if (!this.Input.Cancel && !this.m_backButton.IsClicked)
        return;
      this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionListDialog(this.m_componentMiner, this.IsCondition, this.parentScroll);
    }
  }
}
