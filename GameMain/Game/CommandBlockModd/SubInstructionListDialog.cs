using Engine;
using System;
using System.Xml.Linq;
using Color = Engine.Color;

namespace Game
{
  public class SubInstructionListDialog : CanvasWidget
  {
    public ButtonWidget m_okButton;
    public ButtonWidget m_backButton;
    public ListPanelWidget m_instructionlistWidget;
    public ComponentMiner m_componentMiner;
    public string instructionName;
    public bool IsCondition;
    public float parentScroll;

    public SubInstructionListDialog(
      ComponentMiner componentMiner,
      string keyname,
      bool condition,
      float scroll)
    {
      this.m_componentMiner = componentMiner;
      this.instructionName = keyname;
      this.IsCondition = condition;
      this.parentScroll = scroll;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/SubInstructionList"));
      this.m_instructionlistWidget = this.Children.Find<ListPanelWidget>("SubInstruction.List");
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
      foreach (object obj in Datahandle.GetSubInstructionlist(this.instructionName))
        this.m_instructionlistWidget.AddItem(obj);
    }

    public override void Update()
    {
      if (this.m_backButton.IsClicked)
        this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionListDialog(this.m_componentMiner, this.IsCondition, this.parentScroll);
      if (!this.m_okButton.IsClicked)
        return;
      this.m_componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) null;
    }
  }
}
