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
using TemplatesDatabase;
using Color = Engine.Color;

namespace Game
{
  public class SubsystemDebugMode : SubsystemBlockBehavior
  {
    private Random m_random = new Random();
    private SubsystemPickables m_subsystemPickables;
    private SubsystemTerrain m_subsystemTerrain;
    public bool enable;
    public int functionValue;

    public override int[] HandledBlocks => new int[2]
    {
      0,
      334
    };

    public override void Load(ValuesDictionary valuesDictionary)
    {
      base.Load(valuesDictionary);
      this.m_subsystemPickables = this.Project.FindSubsystem<SubsystemPickables>(true);
      this.m_subsystemTerrain = this.Project.FindSubsystem<SubsystemTerrain>(true);
      this.enable = false;
      this.functionValue = valuesDictionary.GetValue<int>("FunctionValue");
    }

    public override void Save(ValuesDictionary valuesDictionary)
    {
      base.Save(valuesDictionary);
      valuesDictionary.SetValue<int>("FunctionValue", this.functionValue);
    }

    public override bool OnEditInventoryItem(
      IInventory inventory,
      int slotIndex,
      ComponentPlayer Player)
    {
      Player.ComponentGui.ModalPanelWidget = !(Player.ComponentGui.ModalPanelWidget is InstructionListDialog) ? (Widget) new InstructionListDialog(Player.ComponentMiner, false, 0.0f) : (Widget) null;
      return true;
    }

    public override bool OnUse(Ray3 ray, ComponentMiner componentMiner)
    {
      Color yellow = Color.Yellow;
      Color color = yellow * ((float) byte.MaxValue / (float) MathUtils.Max((int) yellow.R, (int) yellow.G, (int) yellow.B));
      object obj = componentMiner.Raycast(ray, RaycastMode.Digging);
      int activeSlotIndex = componentMiner.Inventory.ActiveSlotIndex;
      int contents1 = Terrain.ExtractContents(componentMiner.Inventory.GetSlotValue(activeSlotIndex));
      Vector3 viewDirection = componentMiner.ComponentPlayer.GameWidget.ActiveCamera.ViewDirection;
      string str1 = string.Format("\nViewAngle: Horizontal:{0} deg, Vertical:{1} deg", (object) Datahandle.GetAngle(viewDirection, "vertical"), (object) Datahandle.GetAngle(viewDirection, "horizontal"));
      switch (obj)
      {
        case TerrainRaycastResult _ when this.enable && contents1 == 0:
        case TerrainRaycastResult _ when contents1 == 334:
          CellFace cellFace = ((TerrainRaycastResult) obj).CellFace;
          int cellValue = this.m_subsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z);
          int contents2 = Terrain.ExtractContents(cellValue);
          string blockID = contents2.ToString();
          int data1 = Terrain.ExtractData(cellValue);
          Terrain.ExtractLight(cellValue);
          int num1 = -1;
          int num2 = Terrain.ReplaceLight(cellValue, 0);
          foreach (Pickable pickable in this.Project.FindSubsystem<SubsystemPickables>().Pickables)
          {
            if ((double) Vector3.Distance(pickable.Position, new Vector3((float) cellFace.X, (float) cellFace.Y, (float) cellFace.Z)) <= 1.5)
              num1 = pickable.Value;
          }
          switch (contents2)
          {
            case 0:
              goto label_19;
            case 227:
              int designIndex = FurnitureBlock.GetDesignIndex(data1);
              ClipboardManager.ClipboardString = designIndex.ToString() ?? "";
              componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("DebugMode-BlockID:" + blockID + "; Special State:" + data1.ToString() + "; FullState:" + num2.ToString() + ";  \nFurnitureIndex:" + designIndex.ToString(), color, false, false);
              return true;
            case 333:
              if (contents1 == 334)
                return false;
              break;
          }
          string str2 = string.Empty;
          if (num1 != -1)
          {
            int contents3 = Terrain.ExtractContents(num1);
            int data2 = Terrain.ExtractData(num1);
            str2 = contents3 != 203 ? ";\nDropID:" + num1.ToString() : ";\nClothingIndex:" + ClothingBlock.GetClothingIndex(data2).ToString();
          }
          componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("DebugMode-BlockID:" + blockID + "; Special State:" + data1.ToString() + "; FullState:" + num2.ToString() + "; \nPosition:(" + cellFace.X.ToString() + "," + cellFace.Y.ToString() + "," + cellFace.Z.ToString() + "); " + str1 + str2, color, false, false);
          ClipboardManager.ClipboardString = cellFace.X.ToString() + " " + cellFace.Y.ToString() + " " + cellFace.Z.ToString();
          return true;
        case BodyRaycastResult _ when this.enable && contents1 == 0:
        case BodyRaycastResult _ when contents1 == 334:
          string name = ((BodyRaycastResult) obj).ComponentBody.Entity.ValuesDictionary.DatabaseObject.Name;
          int entityid = Datahandle.GetEntityid(((BodyRaycastResult) obj).ComponentBody.Entity);
          if (name != string.Empty)
          {
            ClipboardManager.ClipboardString = name + ":" + entityid.ToString();
            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("DebugMode-CreatureName:" + name + "，State:" + entityid.ToString() + "，\nEntity has been copied to clipboard", color, false, false);
            ((BodyRaycastResult) obj).ComponentBody.Entity.FindComponent<ComponentCreature>();
            return true;
          }
          break;
        case MovingBlocksRaycastResult _ when this.enable && contents1 == 0:
        case MovingBlocksRaycastResult _ when contents1 == 334:
          IMovingBlockSet movingBlockSet = ((MovingBlocksRaycastResult) obj).MovingBlockSet;
          string str3 = movingBlockSet.Tag == null ? "MovingTagNameNotFound!" : "MovingTagName:" + movingBlockSet.Tag.ToString();
          string str4 = "(" + movingBlockSet.Position.X.ToString() + "," + movingBlockSet.Position.Y.ToString() + "," + movingBlockSet.Position.Z.ToString() + ")";
          componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("DebugMode-" + str3 + ";\nMovingBlockPosition:" + str4, color, false, false);
          return true;
        default:
          if (contents1 == 334)
          {
            Point3 point3 = Datahandle.Coordbodyhandle(componentMiner.ComponentCreature.ComponentBody.Position);
            ClipboardManager.ClipboardString = point3.X.ToString() + " " + point3.Y.ToString() + " " + point3.Z.ToString();
            componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage("DebugMode-PlayerCoord:(" + point3.X.ToString() + "," + point3.Y.ToString() + "," + point3.Z.ToString() + "); " + str1, color, false, false);
            return true;
          }
          break;
      }
label_19:
      return false;
    }

    public static int GetFunctionSwitch(int functionValue) => functionValue & 1;

    public static int SetFunctionSwitch(int functionValue, int switchvalue) => functionValue & -2 | switchvalue;

    public static int GetFunctionSurvive(int functionValue) => (functionValue & 1) >> 1;

    public static int SetFunctionSurvive(int functionValue, int survivevalue) => functionValue & -3 | survivevalue << 1;
  }
}