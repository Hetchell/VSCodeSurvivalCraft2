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
