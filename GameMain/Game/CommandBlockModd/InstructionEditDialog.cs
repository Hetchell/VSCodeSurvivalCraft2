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
using System.Xml.Linq;

namespace Game
{
  public class InstructionEditDialog : CanvasWidget
  {
    public SubsystemCommandBlockBehavior m_subsystemCommandBlockBehavior;
    public Point3 m_commandPoint;
    public ContainerWidget m_linesPage;
    public TextBoxWidget m_textBox;
    public ButtonWidget m_okButton;
    public ButtonWidget m_cancelButton;
    public ComponentMiner m_componentMiner;

    public InstructionEditDialog(
      SubsystemCommandBlockBehavior subsystemCommandBlockBehavior,
      Point3 commandPoint,
      ComponentMiner componentMiner)
    {
      this.m_componentMiner = componentMiner;
      this.LoadContents((object) this, ContentManager.Get<XElement>("Dialogs/InstructionEdit"));
      this.m_linesPage = this.Children.Find<ContainerWidget>("EditCommandDialog.LinesPage");
      this.m_textBox = this.Children.Find<TextBoxWidget>("EditCommandDialog.TextBox");
      this.m_okButton = this.Children.Find<ButtonWidget>("EditCommandDialog.OkButton");
      this.m_cancelButton = this.Children.Find<ButtonWidget>("EditCommandDialog.CancelButton");
      this.m_subsystemCommandBlockBehavior = subsystemCommandBlockBehavior;
      this.m_commandPoint = commandPoint;
      CommandData commandData = this.m_subsystemCommandBlockBehavior.GetCommandData(this.m_commandPoint);
      if (commandData != null)
        this.m_textBox.Text = commandData.Lines;
      else
        this.m_textBox.Text = string.Empty;
    }

    public override void Update()
    {
      if (this.m_okButton.IsClicked)
      {
        string text = this.m_textBox.Text;
        this.m_subsystemCommandBlockBehavior.SetCommandData(this.m_commandPoint, text);
        int cellValue = this.m_subsystemCommandBlockBehavior.SubsystemTerrain.Terrain.GetCellValue(this.m_commandPoint.X, this.m_commandPoint.Y, this.m_commandPoint.Z);
        this.m_subsystemCommandBlockBehavior.SubsystemTerrain.ChangeCell(this.m_commandPoint.X, this.m_commandPoint.Y, this.m_commandPoint.Z, !(text != "") || !(text.Substring(0, 1) == "$") ? (text.Contains("?") ? (Terrain.ExtractData(cellValue) == 2 ? Terrain.ReplaceData(cellValue, 3) : Terrain.ReplaceData(cellValue, 2)) : Terrain.ReplaceData(cellValue, 0)) : Terrain.ReplaceData(cellValue, 1));
        this.ParentWidget.Children.Remove((Widget) this);
      }
      if (!this.Input.Cancel && !this.m_cancelButton.IsClicked)
        return;
      this.ParentWidget.Children.Remove((Widget) this);
    }
  }
}
