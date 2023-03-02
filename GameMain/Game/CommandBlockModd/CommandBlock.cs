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

namespace Game
{
  public class CommandBlock : CubeBlock, IElectricElementBlock
  {
    public const int Index = 333;

    public override int GetFaceTextureSlot(int face, int value) => face == 4 || face == 5 ? 170 : 121;

    public ElectricElement CreateElectricElement(
      SubsystemElectricity subsystemElectricity,
      int value,
      int x,
      int y,
      int z)
    {
      return (ElectricElement) new CommandElectricElement(subsystemElectricity, new Point3(x, y, z));
    }

    public ElectricConnectorType? GetConnectorType(
      SubsystemTerrain terrain,
      int value,
      int face,
      int connectorFace,
      int x,
      int y,
      int z)
    {
      CommandData commandData = GameManager.Project.FindSubsystem<SubsystemCommandBlockBehavior>(true).GetCommandData(new Point3(x, y, z));
      if (commandData == null || !(commandData.Lines != ""))
        return new ElectricConnectorType?(ElectricConnectorType.Input);
      return commandData.Lines.Substring(0, 1) == "$" ? (connectorFace == 4 ? new ElectricConnectorType?(ElectricConnectorType.Output) : new ElectricConnectorType?(ElectricConnectorType.Input)) : (commandData.Lines.Contains("?") ? new ElectricConnectorType?(ElectricConnectorType.Output) : new ElectricConnectorType?(ElectricConnectorType.Input));
    }

    public int GetConnectionMask(int value) => int.MaxValue;
  }
}
