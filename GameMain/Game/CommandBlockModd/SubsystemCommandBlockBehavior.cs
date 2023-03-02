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
using System.Collections.Generic;
using System.Globalization;
using TemplatesDatabase;

namespace Game
{
  public class SubsystemCommandBlockBehavior : SubsystemBlockBehavior, IUpdateable
  {
    private List<Vector3> m_lastUpdatePositions = new List<Vector3>();
    private Dictionary<Point3, SubsystemCommandBlockBehavior.TextData> m_textsByPoint = new Dictionary<Point3, SubsystemCommandBlockBehavior.TextData>();
    public SubsystemMovingBlocks m_subsystemMovingBlocks;
    public Dictionary<Vector4, IMovingBlockSet> m_commandMovingBlocks = new Dictionary<Vector4, IMovingBlockSet>();

    public override int[] HandledBlocks => new int[1]{ 333 };

    public override void Load(ValuesDictionary valuesDictionary)
    {
      base.Load(valuesDictionary);
      foreach (ValuesDictionary valuesDictionary1 in valuesDictionary.GetValue<ValuesDictionary>("Texts").Values)
        this.SetCommandData(valuesDictionary1.GetValue<Point3>("Point"), valuesDictionary1.GetValue<string>("Line", string.Empty));
      this.m_subsystemMovingBlocks = this.Project.FindSubsystem<SubsystemMovingBlocks>(true);
    }

    public override void Save(ValuesDictionary valuesDictionary)
    {
      base.Save(valuesDictionary);
      int num = 0;
      ValuesDictionary valuesDictionary1 = new ValuesDictionary();
      valuesDictionary.SetValue<ValuesDictionary>("Texts", valuesDictionary1);
      foreach (SubsystemCommandBlockBehavior.TextData textData in this.m_textsByPoint.Values)
      {
        ValuesDictionary valuesDictionary2 = new ValuesDictionary();
        valuesDictionary2.SetValue<Point3>("Point", textData.Point);
        if (!string.IsNullOrEmpty(textData.Lines))
          valuesDictionary2.SetValue<string>("Line", textData.Lines);
        valuesDictionary1.SetValue<ValuesDictionary>(num++.ToString((IFormatProvider) CultureInfo.InvariantCulture), valuesDictionary2);
      }
    }

    public override bool OnInteract(
      TerrainRaycastResult raycastResult,
      ComponentMiner componentMiner)
    {
      AudioManager.PlaySound("Audio/UI/ButtonClick", 1f, 0.0f, 0.0f);
      Point3 commandPoint = new Point3(raycastResult.CellFace.X, raycastResult.CellFace.Y, raycastResult.CellFace.Z);
      if (componentMiner.ComponentPlayer != null)
        componentMiner.ComponentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionEditDialog(this, commandPoint, componentMiner);
      return true;
    }

    public CommandData GetCommandData(Point3 point)
    {
      SubsystemCommandBlockBehavior.TextData textData;
      if (!this.m_textsByPoint.TryGetValue(point, out textData))
        return (CommandData) null;
      return new CommandData() { Lines = textData.Lines };
    }

    public void SetCommandData(Point3 point, string lines)
    {
      this.m_textsByPoint[point] = new SubsystemCommandBlockBehavior.TextData()
      {
        Point = point,
        Lines = lines
      };
      this.m_lastUpdatePositions.Clear();
    }

    public override void OnBlockRemoved(int value, int newValue, int x, int y, int z)
    {
      this.m_textsByPoint.Remove(new Point3(x, y, z));
      this.m_lastUpdatePositions.Clear();
    }

    public UpdateOrder UpdateOrder => UpdateOrder.Default;

    public void Update(float dt)
    {
      Vector4 key1 = Vector4.Zero;
      bool flag1 = false;
      bool flag2 = false;
      foreach (Vector4 key2 in this.m_commandMovingBlocks.Keys)
      {
        IMovingBlockSet commandMovingBlock = this.m_commandMovingBlocks[key2];
        if (commandMovingBlock != null)
        {
          int type = (int) key2.W & 3;
          Vector3 currentVelocity1;
          if (type != 0)
          {
            currentVelocity1 = commandMovingBlock.CurrentVelocity;
            if ((double) currentVelocity1.Length() != 0.0)
            {
              ReadOnlyList<MovingBlock> blocks = commandMovingBlock.Blocks;
              foreach (MovingBlock movingBlock in blocks)
              {
                Vector3 currentVelocity2 = commandMovingBlock.CurrentVelocity;
                if (type == 1)
                {
                  Point3 point3 = Datahandle.CollideHandle(currentVelocity2, commandMovingBlock.Position + new Vector3(movingBlock.Offset), type);
                  if (Terrain.ExtractContents(this.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetCellValue(point3.X, point3.Y, point3.Z)) != 0)
                  {
                    flag1 = true;
                    flag2 = true;
                    key1 = key2;
                    this.m_subsystemMovingBlocks.RemoveMovingBlockSet(commandMovingBlock);
                    blocks = commandMovingBlock.Blocks;
                    using (ReadOnlyList<MovingBlock>.Enumerator enumerator = blocks.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        MovingBlock current = enumerator.Current;
                        this.Project.FindSubsystem<SubsystemTerrain>(true).ChangeCell((int) MathUtils.Round(commandMovingBlock.Position.X + (float) current.Offset.X), (int) MathUtils.Round(commandMovingBlock.Position.Y) + current.Offset.Y, (int) MathUtils.Round(commandMovingBlock.Position.Z) + current.Offset.Z, current.Value);
                      }
                      break;
                    }
                  }
                }
                if (type == 2)
                {
                  Point3 point3 = Datahandle.CollideHandle(currentVelocity2, commandMovingBlock.Position + new Vector3(movingBlock.Offset), type);
                  if (Terrain.ExtractContents(this.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetCellValue(point3.X, point3.Y, point3.Z)) != 0)
                    this.Project.FindSubsystem<SubsystemTerrain>(true).DestroyCell(0, point3.X, point3.Y, point3.Z, 0, false, false);
                }
              }
            }
          }
          Vector3 vector3 = new Vector3(key2.X, key2.Y, key2.Z);
          currentVelocity1 = commandMovingBlock.CurrentVelocity;
          if ((double) currentVelocity1.Length() == 0.0 && vector3 != commandMovingBlock.Position && !flag2)
          {
            flag1 = true;
            key1 = key2;
            this.m_subsystemMovingBlocks.RemoveMovingBlockSet(commandMovingBlock);
            foreach (MovingBlock block in commandMovingBlock.Blocks)
              this.Project.FindSubsystem<SubsystemTerrain>(true).ChangeCell((int) commandMovingBlock.Position.X + block.Offset.X, (int) commandMovingBlock.Position.Y + block.Offset.Y, (int) commandMovingBlock.Position.Z + block.Offset.Z, block.Value);
          }
        }
      }
      if (!flag1)
        return;
      this.m_commandMovingBlocks.Remove(key1);
    }

    private class TextData
    {
      public Point3 Point;
      public string Lines = string.Empty;
    }
  }
}