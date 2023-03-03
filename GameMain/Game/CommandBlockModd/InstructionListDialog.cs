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
  public class InstructionListDialog : CanvasWidget
  {
    public ButtonWidget m_okButton;
    public ButtonWidget m_informationButton;
    public ButtonWidget m_funtionButton;
    public ButtonWidget m_searchButton;
    public ButtonWidget m_changeButton;
    public ListPanelWidget m_instructionlistWidget;
    public LabelWidget m_titleLabel;
    public ComponentMiner m_componentMiner;
    public bool IsCondition;

    public InstructionListDialog(ComponentMiner componentMiner, bool condition, float scroll)
    {
      this.m_componentMiner = componentMiner;
      this.IsCondition = condition;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/InstructionList"));
      this.m_instructionlistWidget = this.Children.Find<ListPanelWidget>("Instruction.List");
      this.m_okButton = this.Children.Find<ButtonWidget>("OkButton");
      this.m_informationButton = this.Children.Find<ButtonWidget>("InformationButton");
      this.m_funtionButton = this.Children.Find<ButtonWidget>("FuntionButton");
      this.m_searchButton = this.Children.Find<ButtonWidget>("SearchButton");
      this.m_changeButton = this.Children.Find<ButtonWidget>("ChangeButton");
      this.m_titleLabel = this.Children.Find<LabelWidget>("Title");
      this.m_titleLabel.Text = !condition ? "普通指令列表" : "条件指令列表";
      this.m_instructionlistWidget.ItemWidgetFactory = (Func<object, Widget>) (item =>
      {
        Instruction instruction = (Instruction) item;
        ContainerWidget containerWidget = (ContainerWidget) Widget.LoadWidget((object) this, ContentManager.Get<XElement>("Dialogs/InstructionItem"), (ContainerWidget) null);
        containerWidget.Children.Find<LabelWidget>("InstructionItem.Name").Text = instruction.Name;
        containerWidget.Children.Find<LabelWidget>("InstructionItem.Details").Text = instruction.Details;
        return (Widget) containerWidget;
      });
      this.m_instructionlistWidget.ScrollPosition = scroll;
      this.m_instructionlistWidget.ItemClicked += (Action<object>) (item =>
      {
        Instruction instruction = (Instruction) item;
        if (instruction.ExistSub)
        {
          this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new SubInstructionListDialog(this.m_componentMiner, instruction.Name.Split(' ')[0], (this.IsCondition ? 1 : 0) != 0, this.m_instructionlistWidget.ScrollPosition);
        }
        else
        {
          ClipboardManager.ClipboardString = instruction.Name;
          this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("已复制指令：" + instruction.Name, Color.Yellow, false, false);
        }
      });
      foreach (object obj in Datahandle.InstructionHandler.GetInstructionlist(this.IsCondition))
        this.m_instructionlistWidget.AddItem(obj);
    }

    public override void Update()
    {
      int functionValue = this.m_componentMiner.m_subsystemBlockBehaviors.Project.FindSubsystem<SubsystemDebugMode>().functionValue;
      int functionSwitch = SubsystemDebugMode.GetFunctionSwitch(functionValue);
      bool flag1 = functionSwitch == 1;
      this.m_funtionButton.Color = flag1 ? Color.White : Color.Gray;
      if (this.m_okButton.IsClicked || this.Input.Cancel)
        this.ParentWidget.Children.Remove((Widget) this);
      if (this.m_informationButton.IsClicked)
      {
        this.ParentWidget.Children.Remove((Widget) this);
        this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ManualTopicDialog(this.m_componentMiner, 0.0f);
      }
      if (this.m_funtionButton.IsClicked)
      {
        bool flag2 = !flag1;
        switch (functionSwitch)
        {
          case 0:
            int num1 = SubsystemDebugMode.SetFunctionSwitch(functionValue, 1);
            this.m_componentMiner.m_subsystemBlockBehaviors.Project.FindSubsystem<SubsystemDebugMode>().functionValue = num1;
            this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("已开启指令功能", Color.Yellow, false, false);
            break;
          case 1:
            int num2 = SubsystemDebugMode.SetFunctionSwitch(functionValue, 0);
            this.m_componentMiner.m_subsystemBlockBehaviors.Project.FindSubsystem<SubsystemDebugMode>().functionValue = num2;
            this.m_componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("已关闭指令功能", Color.Yellow, false, false);
            break;
        }
      }
      if (this.m_searchButton.IsClicked)
        this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionSearchDialog(this.m_componentMiner, this.IsCondition, this.m_instructionlistWidget.ScrollPosition);
      if (!this.m_changeButton.IsClicked)
        return;
      this.IsCondition = !this.IsCondition;
      this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionListDialog(this.m_componentMiner, this.IsCondition, 0.0f);
    }
  }
}
