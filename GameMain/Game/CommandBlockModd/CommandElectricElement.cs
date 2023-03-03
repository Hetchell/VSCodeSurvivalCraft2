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
using Engine.Audio;
using Engine.Content;
using Engine.Graphics;
using Engine.Media;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TemplatesDatabase;
using Color = Engine.Color;
using Image = Engine.Media.Image;

namespace Game
{
  public class CommandElectricElement : ElectricElement
  {
    private LogoPoint[] m_logoPoint = new LogoPoint[4];
    private SubsystemLogo m_subsystemLogo;
    private int? m_lastMessageValue;
    private double? m_lastMessageTime;
    public Color displayColor = Color.Green;
    private Random m_random = new Random();
    public ListPanelWidget m_worldsListWidget;
    public int coordMode;
    public bool clockAllowed = true;
    public float m_voltage;
    public SubsystemCommandBlockBehavior m_subsystemCommandBlockBehavior;

    public CommandElectricElement(SubsystemElectricity subsystemElectricity, Point3 point)
      : base(subsystemElectricity, (IEnumerable<CellFace>) new List<CellFace>()
      {
        new CellFace(point.X, point.Y, point.Z, 0),
        new CellFace(point.X, point.Y, point.Z, 1),
        new CellFace(point.X, point.Y, point.Z, 2),
        new CellFace(point.X, point.Y, point.Z, 3),
        new CellFace(point.X, point.Y, point.Z, 4),
        new CellFace(point.X, point.Y, point.Z, 5)
      })
    {
      this.m_subsystemLogo = subsystemElectricity.Project.FindSubsystem<SubsystemLogo>(true);
      this.m_subsystemCommandBlockBehavior = subsystemElectricity.Project.FindSubsystem<SubsystemCommandBlockBehavior>(true);
    }

    public override void OnAdded()
    {
      CellFace cellFace = this.CellFaces[0];
      Point3 commandpoint3 = new Point3(cellFace.Point.X, cellFace.Point.Y, cellFace.Point.Z);
      Vector3 vector3_1 = new Vector3((float) cellFace.X + 0.5f, (float) cellFace.Y + 0.5f, (float) cellFace.Z + 0.5f);
      for (int face = 0; face < this.m_logoPoint.Length; ++face)
      {
        LogoPoint logoPoint = new LogoPoint();
        Vector3 vector3_2 = CellFace.FaceToVector3(face);
        Vector3 unitY = Vector3.UnitY;
        Vector3 vector3_3 = Vector3.Cross(vector3_2, unitY);
        logoPoint.Position = vector3_1 + 0.5075f * CellFace.FaceToVector3(face);
        logoPoint.Forward = vector3_2;
        logoPoint.Up = unitY;
        logoPoint.Right = vector3_3;
        logoPoint.Color = this.displayColor;
        logoPoint.Size = 0.52f;
        logoPoint.FarSize = 0.52f;
        logoPoint.FarDistance = 1f;
        logoPoint.Type = (LogoPointType) face;
        this.m_logoPoint[face] = logoPoint;
      }
      this.m_subsystemLogo.AddLogoPoint(commandpoint3, this.m_logoPoint);
    }

    public override void OnRemoved()
    {
      CellFace cellFace = this.CellFaces[0];
      this.m_subsystemLogo.RemoveLogoPoint(new Point3(cellFace.Point.X, cellFace.Point.Y, cellFace.Point.Z));
    }

    public override float GetOutputVoltage(int face) => this.m_voltage;

    public override void OnConnectionsChanged()
    {
    }

    public override void OnNeighborBlockChanged(
      CellFace cellFace,
      int neighborX,
      int neighborY,
      int neighborZ)
    {
    }

    public override void OnCollide(CellFace cellFace, float velocity, ComponentBody componentBody)
    {
    }

    public override void OnHitByProjectile(CellFace cellFace, WorldItem worldItem)
    {
    }

    public override bool OnInteract(
      TerrainRaycastResult raycastResult,
      ComponentMiner componentMiner)
    {
      return false;
    }

    public bool Instruction(string[] words, ComponentPlayer componentPlayer, Point3 position)
    {
      bool coordRelative = this.coordMode != 0;
      if (componentPlayer.m_subsystemGameInfo == null)
        return false;
      bool modeCreative = componentPlayer.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative;
      bool condition = false;
      try
      {
        if (words[0] == "key")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words[1] == "906691197")
          {
            int num = SubsystemDebugMode.SetFunctionSurvive(this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().functionValue, 1);
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().functionValue = num;
            componentPlayer.ComponentGui.DisplaySmallMessage("已解锁生存模式的所有指令功能，输入‘0000’重新锁定", Color.Yellow, false, false);
          }
          else if (words[1] == "0000")
          {
            int num = SubsystemDebugMode.SetFunctionSurvive(this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().functionValue, 0);
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().functionValue = num;
            componentPlayer.ComponentGui.DisplaySmallMessage("已锁定生存模式的作弊指令功能", Color.Yellow, false, false);
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误：输入的密码有误！", Color.Yellow, false, false);
        }
        else if (words[0] == "help")
        {
          if (words.Length != 1)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (this.m_lastMessageTime.HasValue)
          {
            if (this.SubsystemElectricity.SubsystemTime.GameTime - this.m_lastMessageTime.Value <= 2.0)
              goto label_1185;
          }
          componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new InstructionListDialog(componentPlayer.ComponentMiner, false, 0.0f);
          this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
        }
        else if (words[0] == "book")
        {
          if (words.Length != 1)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (this.m_lastMessageTime.HasValue)
          {
            if (this.SubsystemElectricity.SubsystemTime.GameTime - this.m_lastMessageTime.Value <= 2.0)
              goto label_1185;
          }
          componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ManualTopicDialog(componentPlayer.ComponentMiner, 0.0f);
          this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
        }
        else if (words[0] == "debugmode")
        {
          if (words.Length != 1 && (words.Length != 2 || !(words[1] == "false") && !(words[1] == "true")))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words.Length == 1)
          {
            if (this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().enable)
            {
              this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().enable = false;
              componentPlayer.ComponentGui.DisplaySmallMessage("已关闭调试模式", Color.Yellow, false, false);
            }
            else
            {
              this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().enable = true;
              componentPlayer.ComponentGui.DisplaySmallMessage("已开启调试模式", Color.Yellow, false, false);
            }
          }
          else if (words[1] == "true")
          {
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().enable = true;
            componentPlayer.ComponentGui.DisplaySmallMessage("已开启调试模式", Color.Yellow, false, false);
          }
          else
          {
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().enable = false;
            componentPlayer.ComponentGui.DisplaySmallMessage("已关闭调试模式", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "message")
        {
          if (words.Length < 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          Color white = Color.White;
          white *= (float) byte.MaxValue / (float) MathUtils.Max((int) white.R, (int) white.G, (int) white.B);
          componentPlayer.ComponentGui.DisplaySmallMessage(words[1], white, true, true);
        }
        else if (words[0] == "largemessage")
        {
          if (words.Length != 3)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          componentPlayer.ComponentGui.DisplayLargeMessage(words[1], words[2], 3f, 0.0f);
        }
        else if (words[0] == "place")
        {
          if (words.Length != 5 && (words.Length != 9 || !(words[4] == "to")))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words.Length == 5)
          {
            int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
            int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
            int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
            int num = int.Parse(words[4]);
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x, y, z, num);
          }
          else
          {
            int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
            int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
            int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
            int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
            int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
            int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
            int num = int.Parse(words[8]);
            CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
            for (int index1 = 0; index1 < cubeArea.LengthX; ++index1)
            {
              for (int index2 = 0; index2 < cubeArea.LengthY; ++index2)
              {
                for (int index3 = 0; index3 < cubeArea.LengthZ; ++index3)
                  this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea.PointMin.X + index1, cubeArea.PointMin.Y + index2, cubeArea.PointMin.Z + index3, num);
              }
            }
          }
        }
        else if (words[0] == "places")
        {
          if (words.Length <= 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string[] strArray = new string[words.Length - 1];
          for (int index = 0; index < words.Length - 1; ++index)
            strArray[index] = words[index + 1];
          words = strArray;
          this.BlocksPlace(words, componentPlayer, position, condition, coordRelative);
        }
        else if (words[0] == "replace")
        {
          if (words.Length != 10 || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int num1 = int.Parse(words[8]);
          int num2 = int.Parse(words[9]);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          for (int index4 = 0; index4 < cubeArea.LengthX; ++index4)
          {
            for (int index5 = 0; index5 < cubeArea.LengthY; ++index5)
            {
              for (int index6 = 0; index6 < cubeArea.LengthZ; ++index6)
              {
                if (Terrain.ReplaceLight(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + index4, cubeArea.PointMin.Y + index5, cubeArea.PointMin.Z + index6), 0) == num1)
                  this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea.PointMin.X + index4, cubeArea.PointMin.Y + index5, cubeArea.PointMin.Z + index6, num2);
              }
            }
          }
        }
        else if (words[0] == "dig")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().DestroyCell(1, x, y, z, 0, false, false);
        }
        else if (words[0] == "digs")
        {
          if (words.Length != 9 || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int num3 = int.Parse(words[8]);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          for (int index7 = 0; index7 < cubeArea.LengthX; ++index7)
          {
            for (int index8 = 0; index8 < cubeArea.LengthY; ++index8)
            {
              for (int index9 = 0; index9 < cubeArea.LengthZ; ++index9)
              {
                int num4 = Terrain.ReplaceLight(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + index7, cubeArea.PointMin.Y + index8, cubeArea.PointMin.Z + index9), 0);
                if (num3 == num4)
                  this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().DestroyCell(1, cubeArea.PointMin.X + index7, cubeArea.PointMin.Y + index8, cubeArea.PointMin.Z + index9, 0, false, false);
              }
            }
          }
        }
        else if (words[0] == "addentity")
        {
          if (words.Length != 5)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          string str1 = words[4].Substring(0, 1).ToUpper() + words[4].Substring(1);
          string str2 = (string) null;
          string entityTemplateName;
          if (str1.Contains(":"))
          {
            entityTemplateName = str1.Split(':')[0];
            str2 = str1.Split(':')[1];
          }
          else
          {
            entityTemplateName = str1;
            str2 = str1;
          }
          Entity entity = DatabaseManager.CreateEntity(this.SubsystemElectricity.Project, entityTemplateName, true);
          entity.FindComponent<ComponentFrame>(true).Position = new Vector3((float) x, (float) y, (float) z);
          entity.FindComponent<ComponentFrame>(true).Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.m_random.Float(0.0f, 6.283185f));
          entity.FindComponent<ComponentSpawn>(true).SpawnDuration = 0.0f;
          this.SubsystemElectricity.Project.AddEntity(entity);
        }
        else if (words[0] == "removenpc")
        {
          if (words.Length != 1 && words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
          if (words.Length == 1)
          {
            foreach (ComponentBody componentBody in result)
            {
              ComponentPlayer component1 = componentBody.Entity.FindComponent<ComponentPlayer>();
              ComponentCreature component2 = componentBody.Entity.FindComponent<ComponentCreature>();
              if (component2 != null && component1 == null)
                this.SubsystemElectricity.Project.RemoveEntity(component2.ComponentBody.Entity, true);
            }
          }
          else
          {
            foreach (Component componentBody in this.GetComponentBodies(words[1], position))
              this.SubsystemElectricity.Project.RemoveEntity(componentBody.Entity, true);
          }
          if (modeCreative)
          {
            if (words.Length == 1)
              componentPlayer.ComponentGui.DisplaySmallMessage("已清理所有动物", Color.Yellow, false, false);
            else
              componentPlayer.ComponentGui.DisplaySmallMessage("已清理指定动物", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "kill")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          if (word.ToLower() == "pl")
          {
            DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
            foreach (Component component3 in result)
            {
              ComponentPlayer component4 = component3.Entity.FindComponent<ComponentPlayer>();
              component4?.ComponentMiner.ComponentCreature.ComponentHealth.Injure(1f, component4.ComponentMiner.ComponentCreature, true, "不知道谁输的指令");
            }
          }
          else
          {
            foreach (Component componentBody in this.GetComponentBodies(word, position))
              componentBody.Entity.FindComponent<ComponentCreature>()?.ComponentHealth.Injure(1f, (ComponentCreature) null, true, "magic");
            if (modeCreative)
              componentPlayer.ComponentGui.DisplaySmallMessage("已杀死指定的'" + word + "'", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "heal")
        {
          if (words.Length != 3 || int.Parse(words[2]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num = int.Parse(words[2]);
          string word = words[1];
          if (word.ToLower() == "pl")
          {
            DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
            foreach (Component component5 in result)
            {
              ComponentPlayer component6 = component5.Entity.FindComponent<ComponentPlayer>();
              if (component6 != null)
              {
                float amount = (float) num / component6.ComponentMiner.ComponentCreature.ComponentHealth.AttackResilience;
                component6.ComponentMiner.ComponentCreature.ComponentHealth.Heal(amount);
              }
            }
          }
          else
          {
            foreach (Component componentBody in this.GetComponentBodies(word, position))
            {
              ComponentCreature component = componentBody.Entity.FindComponent<ComponentCreature>();
              if ((double) component.ComponentHealth.Health > 0.0)
              {
                float amount = (float) num / component.ComponentHealth.AttackResilience;
                component.ComponentHealth.Heal(amount);
              }
            }
          }
        }
        else if (words[0] == "injure")
        {
          if (words.Length != 3 || int.Parse(words[2]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num = int.Parse(words[2]);
          string word = words[1];
          if (word.ToLower() == "pl")
          {
            DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
            foreach (Component component7 in result)
            {
              ComponentPlayer component8 = component7.Entity.FindComponent<ComponentPlayer>();
              if (component8 != null)
              {
                float amount = (float) num / component8.ComponentMiner.ComponentCreature.ComponentHealth.AttackResilience;
                component8.ComponentMiner.ComponentCreature.ComponentHealth.Injure(amount, component8.ComponentMiner.ComponentCreature, true, "不知道谁输的指令");
              }
            }
          }
          else
          {
            foreach (Component componentBody in this.GetComponentBodies(word, position))
            {
              ComponentCreature component = componentBody.Entity.FindComponent<ComponentCreature>();
              if (component != null)
              {
                float amount = (float) num / component.ComponentHealth.AttackResilience;
                component.ComponentHealth.Injure(amount, (ComponentCreature) null, true, "magic");
              }
            }
          }
        }
        else if (words[0] == "catchfire")
        {
          if ((words.Length != 3 || int.Parse(words[2]) < 0) && (words.Length != 5 || int.Parse(words[4]) < 0))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words.Length == 5)
          {
            int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
            int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
            int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
            float fireExpandability = (float) int.Parse(words[4]) / 10f;
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemFireBlockBehavior>().SetCellOnFire(x, y, z, fireExpandability);
          }
          else
          {
            float num = (float) int.Parse(words[2]) / 10f;
            string word = words[1];
            if (word.ToLower() == "pl")
            {
              DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
              this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
              foreach (Component component in result)
              {
                if (component.Entity.FindComponent<ComponentPlayer>() != null)
                  componentPlayer.ComponentHealth.m_componentOnFire.m_fireDuration = num;
              }
            }
            else
            {
              foreach (ComponentBody componentBody in this.GetComponentBodies(word, position))
              {
                ComponentCreature component9 = componentBody.Entity.FindComponent<ComponentCreature>();
                ComponentBoat component10 = componentBody.Entity.FindComponent<ComponentBoat>();
                if (component9 != null)
                  component9.ComponentHealth.m_componentOnFire.m_fireDuration = num;
                if (component10 != null)
                {
                  ComponentOnFire component11 = componentBody.Entity.FindComponent<ComponentOnFire>();
                  if (component11 != null)
                    component11.m_fireDuration = num;
                }
              }
            }
          }
        }
        else if (words[0] == "shapeshifter")
        {
          if (words.Length != 2 || !(words[1] == "true") && !(words[1] == "false"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          bool flag = words[1] == "true";
          DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
          foreach (ComponentBody componentBody in result)
          {
            componentBody.Entity.FindComponent<ComponentCreature>();
            string name = componentBody.Entity.ValuesDictionary.DatabaseObject.Name;
            if (flag)
            {
              if (name == "Wolf_Gray")
              {
                ComponentShapeshifter component = componentBody.Entity.FindComponent<ComponentShapeshifter>();
                component.IsEnabled = true;
                component.ShapeshiftTo("Werewolf");
              }
            }
            else if (name == "Werewolf")
            {
              ComponentShapeshifter component = componentBody.Entity.FindComponent<ComponentShapeshifter>();
              component.IsEnabled = true;
              component.ShapeshiftTo("Wolf_Gray");
            }
          }
          foreach (ComponentBody componentBody in result)
          {
            componentBody.Entity.FindComponent<ComponentCreature>();
            string name = componentBody.Entity.ValuesDictionary.DatabaseObject.Name;
            if (flag)
            {
              if (name == "Werewolf")
                componentBody.Entity.FindComponent<ComponentShapeshifter>().IsEnabled = false;
            }
            else if (name == "Wolf_Gray")
              componentBody.Entity.FindComponent<ComponentShapeshifter>().IsEnabled = false;
          }
        }
        else if (words[0] == "invulnerable")
        {
          if (words.Length != 2 || !(words[1] == "true") && !(words[1] == "false"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words[1] == "true")
            componentPlayer.ComponentHealth.IsInvulnerable = true;
          else
            componentPlayer.ComponentHealth.IsInvulnerable = false;
        }
        else if (words[0] == "adddrops")
        {
          if (words.Length != 6 || int.Parse(words[5]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num5 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num6 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int num7 = int.Parse(words[4]);
          int num8 = int.Parse(words[5]);
          for (int index = 0; index < num8; ++index)
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemPickables>().AddPickable(num7, 1, new Vector3((float) num5 + this.m_random.Float(0.48f, 0.52f), (float) y, (float) num6 + this.m_random.Float(0.48f, 0.52f)), new Vector3?(), new Matrix?());
        }
        else if (words[0] == "removedrops")
        {
          if (words.Length != 1 && words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          bool flag = words.Length == 1;
          int num = 0;
          foreach (Pickable pickable in this.SubsystemElectricity.Project.FindSubsystem<SubsystemPickables>().Pickables)
          {
            if (flag)
            {
              pickable.ToRemove = true;
            }
            else
            {
              num = Terrain.ExtractContents(int.Parse(words[1]));
              int contents = Terrain.ExtractContents(pickable.Value);
              if (num == contents)
                pickable.ToRemove = true;
            }
          }
          if (modeCreative)
          {
            if (flag)
              componentPlayer.ComponentGui.DisplaySmallMessage("已清除所有掉落物", Color.Yellow, false, false);
            else
              componentPlayer.ComponentGui.DisplaySmallMessage("已清除所有id为" + num.ToString() + "的掉落物", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "launchdrops")
        {
          if (words.Length != 6)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num9 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int num10 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num11 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int num12 = int.Parse(words[4]);
          string[] strArray = words[5].Split(',');
          if (strArray.Length == 3)
          {
            Vector3 vector3 = new Vector3((float) int.Parse(strArray[0]), (float) int.Parse(strArray[1]), (float) int.Parse(strArray[2]));
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemPickables>().AddPickable(num12, 1, new Vector3((float) num9 + 0.5f, (float) num10 + 0.5f, (float) num11 + 0.5f), new Vector3?(vector3), new Matrix?());
          }
          else
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("输入的方向值有误", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "tp")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num13 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num14 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Position = new Vector3((float) num13 + 0.5f, (float) y, (float) num14 + 0.5f);
          if (modeCreative)
            componentPlayer.ComponentGui.DisplaySmallMessage(string.Format("已将玩家传送至标准坐标({0},{1},{2})", (object) num13, (object) y, (object) num14), Color.Yellow, false, false);
        }
        else if (words[0] == "spawn")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num15 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num16 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          componentPlayer.PlayerData.SpawnPosition = new Vector3((float) num15 + 0.5f, (float) y, (float) num16 + 0.5f);
          if (modeCreative)
            componentPlayer.ComponentGui.DisplaySmallMessage(string.Format("已将重生点设置为标准坐标({0},{1},{2})", (object) num15, (object) y, (object) num16), Color.Yellow, false, false);
        }
        else if (words[0] == "boxstage")
        {
          if (words.Length != 2 || int.Parse(words[1]) <= 0 || int.Parse(words[1]) >= 6)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          switch (int.Parse(words[1]))
          {
            case 1:
              componentPlayer.ComponentBody.BoxSize = new Vector3(0.5f, 1.77f, 0.5f);
              break;
            case 2:
              componentPlayer.ComponentBody.BoxSize = new Vector3(0.7f, 1f, 0.7f);
              break;
            case 3:
              componentPlayer.ComponentBody.BoxSize = new Vector3(1f, 2f, 1f);
              break;
            case 4:
              componentPlayer.ComponentBody.BoxSize = new Vector3(2f, 2f, 2f);
              break;
            case 5:
              componentPlayer.ComponentBody.BoxSize = new Vector3(2f, 3f, 2f);
              break;
          }
        }
        else if (words[0] == "playerstats")
        {
          if (words.Length != 3 || int.Parse(words[2]) < 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string lower = words[1].ToLower();
          int num17 = int.Parse(words[2]);
          switch (lower)
          {
            case "attack":
              componentPlayer.ComponentMiner.AttackPower = (float) num17;
              break;
            case "defense":
              componentPlayer.ComponentHealth.AttackResilience = (float) num17;
              componentPlayer.ComponentHealth.FallResilience = (float) num17;
              componentPlayer.ComponentHealth.FireResilience = (float) (2 * num17);
              break;
            case "endurance":
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：endurance指令在非创造模式下提交才有效", Color.Yellow, false, false);
                return false;
              }
              float num18 = (float) num17 / 10f;
              if ((double) num18 > 1.0)
                num18 = 1f;
              componentPlayer.ComponentVitalStats.Stamina = num18;
              break;
            case "fatigue":
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：fatigue指令在非创造模式下提交才有效", Color.Yellow, false, false);
                return false;
              }
              float num19 = (float) num17 / 10f;
              if ((double) num19 > 1.0)
                num19 = 1f;
              componentPlayer.ComponentVitalStats.Sleep = num19;
              break;
            case "health":
              componentPlayer.ComponentHealth.Health = (float) num17 / 10f;
              break;
            case "hunger":
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：hunger指令在非创造模式下提交才有效", Color.Yellow, false, false);
                return false;
              }
              float num20 = (float) num17 / 10f;
              if ((double) num20 > 1.0)
                num20 = 1f;
              componentPlayer.ComponentVitalStats.Food = num20;
              break;
            case "level":
              componentPlayer.PlayerData.Level = (float) num17;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将等级设置为" + int.Parse(words[1]).ToString() + "级", Color.Yellow, false, false);
                break;
              }
              break;
            case "speed":
              float x = (float) num17 / 10f;
              componentPlayer.ComponentLocomotion.WalkSpeed = 2f * x;
              componentPlayer.ComponentLocomotion.JumpSpeed = 3f * MathUtils.Sqrt(x);
              componentPlayer.ComponentLocomotion.LadderSpeed = 1.5f * x;
              componentPlayer.ComponentLocomotion.SwimSpeed = 1.5f * x;
              break;
            case "temperature":
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：temperature指令在非创造模式下提交才有效", Color.Yellow, false, false);
                return false;
              }
              componentPlayer.ComponentVitalStats.Temperature = (float) num17;
              break;
            case "wetness":
              float num21 = (float) num17 / 10f;
              componentPlayer.ComponentVitalStats.Wetness = num21;
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:playerstats指令不存在属性关键字：" + lower, Color.Yellow, false, false);
              break;
          }
        }
        else if (words[0] == "playeraction")
        {
          if (words.Length != 3)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string lower = words[1].ToLower();
          switch (lower)
          {
            case "cough":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[2] == "true")
              {
                componentPlayer.ComponentFlu.Cough();
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentFlu.m_coughDuration = 0.0f;
                break;
              }
              break;
            case "dig":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[1] == "true")
              {
                for (int index = 0; index < BlocksManager.Blocks.Length; ++index)
                  BlocksManager.Blocks[index].DigResilience = 0.0f;
                break;
              }
              if (words[2] == "false")
              {
                for (int index = 0; index < BlocksManager.Blocks.Length; ++index)
                  BlocksManager.Blocks[index].DigResilience = float.PositiveInfinity;
                break;
              }
              break;
            case "fly":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[2] == "true")
              {
                componentPlayer.ComponentLocomotion.IsCreativeFlyEnabled = true;
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentLocomotion.IsCreativeFlyEnabled = false;
                break;
              }
              break;
            case "hasflu":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[2] == "true")
              {
                componentPlayer.ComponentFlu.StartFlu();
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentFlu.m_fluDuration = 0.0f;
                break;
              }
              break;
            case "jump":
              componentPlayer.ComponentBody.m_velocity = new Vector3(componentPlayer.ComponentBody.m_velocity.X, (float) int.Parse(words[2]), componentPlayer.ComponentBody.m_velocity.Z);
              break;
            case "lock":
              if (words[2] == "true")
              {
                componentPlayer.ComponentLocomotion.LookSpeed = 8E-08f;
                componentPlayer.ComponentLocomotion.TurnSpeed = 8E-08f;
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentLocomotion.LookSpeed = 8f;
                componentPlayer.ComponentLocomotion.TurnSpeed = 8f;
                break;
              }
              break;
            case "look":
              if (words[2].Split(',').Length == 2)
              {
                Vector2 vector2 = new Vector2((float) ((double) int.Parse(words[2].Split(',')[0]) / 10.0), (float) ((double) int.Parse(words[2].Split(',')[1]) / 10.0));
                componentPlayer.ComponentLocomotion.VrLookOrder = new Vector2?(vector2);
                break;
              }
              break;
            case "move":
              if (words[2].Split(',').Length == 2)
              {
                Vector2 vector2 = new Vector2((float) ((double) int.Parse(words[2].Split(',')[0]) / 10.0), (float) ((double) int.Parse(words[2].Split(',')[1]) / 10.0));
                componentPlayer.ComponentBody.m_velocity = new Vector3(vector2.X, 0.0f, vector2.Y);
                break;
              }
              break;
            case "rider":
              if (words[2] == "true")
              {
                if (componentPlayer.ComponentRider.Mount == null)
                {
                  ComponentMount nearestMount = componentPlayer.ComponentRider.FindNearestMount();
                  if (nearestMount != null)
                  {
                    componentPlayer.ComponentRider.StartMounting(nearestMount);
                    break;
                  }
                  break;
                }
                break;
              }
              if (words[2] == "false")
              {
                if (componentPlayer.ComponentRider.Mount != null)
                {
                  componentPlayer.ComponentRider.StartDismounting();
                  break;
                }
                break;
              }
              break;
            case "sick":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[2] == "true")
              {
                componentPlayer.ComponentSickness.StartSickness();
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentSickness.m_sicknessDuration = 0.0f;
                break;
              }
              break;
            case "sleep":
              if (words[2] == "true")
              {
                componentPlayer.ComponentSleep.Sleep(false);
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentSleep.WakeUp();
                break;
              }
              break;
            case "sneak":
              if (words[2] == "true")
              {
                componentPlayer.ComponentBody.IsSneaking = true;
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentBody.IsSneaking = false;
                break;
              }
              break;
            case "sneeze":
              if (!modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("非创造模式不支持该指令", Color.Yellow, false, false);
                return false;
              }
              if (words[2] == "true")
              {
                componentPlayer.ComponentFlu.Sneeze();
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentFlu.m_sneezeDuration = 0.0f;
                break;
              }
              break;
            case "stiff":
              if (words[2] == "true")
              {
                componentPlayer.ComponentBody.AirDrag = new Vector2(1000f, 1000f);
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentBody.AirDrag = new Vector2(0.25f, 0.25f);
                break;
              }
              break;
            case "waterbreathe":
              if (words[2] == "true")
              {
                componentPlayer.ComponentHealth.AirCapacity = -1f;
                break;
              }
              if (words[2] == "false")
              {
                componentPlayer.ComponentHealth.AirCapacity = 10f;
                break;
              }
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:playeraction指令不存在属性关键字：" + lower, Color.Yellow, false, false);
              break;
          }
        }
        else if (words[0] == "playerinput")
        {
          if (words.Length != 3 || int.Parse(words[2]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          Vector3 viewDirection = componentPlayer.GameWidget.ActiveCamera.ViewDirection;
          Vector3 vector3 = new Vector3(viewDirection.X, 0.0f, viewDirection.Z);
          int num = int.Parse(words[2]);
          string lower = words[1].ToLower();
          switch (lower)
          {
            case "lookdown":
              componentPlayer.ComponentLocomotion.VrLookOrder = new Vector2?(new Vector2(componentPlayer.ComponentLocomotion.LookAngles.X, componentPlayer.ComponentLocomotion.LookAngles.Y - (float) num / 20f));
              break;
            case "lookleft":
              componentPlayer.ComponentLocomotion.VrLookOrder = new Vector2?(new Vector2((float) num / 20f, componentPlayer.ComponentLocomotion.LookAngles.Y));
              break;
            case "lookright":
              componentPlayer.ComponentLocomotion.VrLookOrder = new Vector2?(new Vector2((float) -num / 20f, componentPlayer.ComponentLocomotion.LookAngles.Y));
              break;
            case "lookup":
              componentPlayer.ComponentLocomotion.VrLookOrder = new Vector2?(new Vector2(componentPlayer.ComponentLocomotion.LookAngles.X, componentPlayer.ComponentLocomotion.LookAngles.Y + (float) num / 20f));
              break;
            case "movedown":
              componentPlayer.ComponentBody.m_velocity = -1f * vector3 / vector3.Length() * (float) num;
              break;
            case "moveleft":
              vector3 = new Vector3(-viewDirection.Z, 0.0f, viewDirection.X);
              componentPlayer.ComponentBody.m_velocity = -1f * vector3 / vector3.Length() * (float) num;
              break;
            case "moveright":
              vector3 = new Vector3(-viewDirection.Z, 0.0f, viewDirection.X);
              componentPlayer.ComponentBody.m_velocity = vector3 / vector3.Length() * (float) num;
              break;
            case "moveup":
              componentPlayer.ComponentBody.m_velocity = vector3 / vector3.Length() * (float) num;
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:playerinput指令不存在属性关键字：" + lower, Color.Yellow, false, false);
              break;
          }
        }
        else if (words[0] == "additems")
        {
          if (words.Length != 4 || int.Parse(words[1]) < 0 || int.Parse(words[3]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (modeCreative)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：additems指令在非创造模式下提交才有效", Color.Yellow, false, false);
            return false;
          }
          ComponentInventoryBase component12 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentInventoryBase>();
          ComponentCraftingTable component13 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentCraftingTable>();
          if (component12 == null || component13 == null)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：背包数据不存在", Color.Yellow, false, false);
            return false;
          }
          int index10 = int.Parse(words[1]);
          int num22 = int.Parse(words[2]);
          int num23 = int.Parse(words[3]);
          if (index10 <= 25)
          {
            if (component12.m_slots[index10].Value == num22)
            {
              component12.m_slots[index10].Count += num23;
            }
            else
            {
              component12.m_slots[index10].Value = num22;
              component12.m_slots[index10].Count = num23;
            }
          }
          else if (index10 > 25 && index10 <= 31)
          {
            int index11 = index10 - 26;
            if (component13.m_slots[index11].Value == num22)
            {
              component13.m_slots[index11].Count += num23;
            }
            else
            {
              component13.m_slots[index11].Value = num22;
              component13.m_slots[index11].Count = num23;
            }
          }
          else if (index10 > 31)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：格子序号超出,序号s的取值范围为0-31", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "removeitems")
        {
          if (words.Length != 1 && (words.Length != 2 && (words.Length != 3 || int.Parse(words[2]) <= 0) || int.Parse(words[1]) < 0))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (modeCreative)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：removeitems指令在非创造模式下提交才有效", Color.Yellow, false, false);
            return false;
          }
          ComponentInventoryBase component14 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentInventoryBase>();
          ComponentCraftingTable component15 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentCraftingTable>();
          if (component14 == null || component15 == null)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：背包数据不存在", Color.Yellow, false, false);
            return false;
          }
          if (words.Length == 1)
          {
            for (int index = 0; index < component14.m_slots.Count; ++index)
            {
              component14.m_slots[index].Value = 0;
              component14.m_slots[index].Count = 0;
            }
            for (int index = 0; index < component15.m_slots.Count; ++index)
            {
              component15.m_slots[index].Value = 0;
              component15.m_slots[index].Count = 0;
            }
            return true;
          }
          int index12 = int.Parse(words[1]);
          if (index12 <= 25)
          {
            if (words.Length == 3)
            {
              int num = int.Parse(words[2]);
              component14.m_slots[index12].Count = Math.Max(0, component14.m_slots[index12].Count - num);
            }
            if (words.Length == 2)
            {
              component14.m_slots[index12].Value = 0;
              component14.m_slots[index12].Count = 0;
            }
          }
          else if (index12 > 25 && index12 <= 31)
          {
            int index13 = index12 - 26;
            if (words.Length == 3)
            {
              int num = int.Parse(words[2]);
              component15.m_slots[index13].Count = Math.Max(0, component15.m_slots[index13].Count - num);
            }
            if (words.Length == 2)
            {
              component15.m_slots[index13].Value = 0;
              component15.m_slots[index13].Count = 0;
            }
          }
          else if (index12 > 31)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：格子序号超出,序号s的取值范围为0-31", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "addchestitems")
        {
          if (words.Length != 7 || int.Parse(words[4]) < 0 || int.Parse(words[6]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int index = int.Parse(words[4]);
          int num24 = int.Parse(words[5]);
          int num25 = int.Parse(words[6]);
          ComponentChest component = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z).Entity.FindComponent<ComponentChest>(true);
          if (component == null)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误：位置(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")找不到方块实体", Color.Yellow, false, false);
            return false;
          }
          if (index < 16)
          {
            if (component.m_slots[index].Value == num24)
            {
              component.m_slots[index].Count += num25;
            }
            else
            {
              component.m_slots[index].Value = num24;
              component.m_slots[index].Count = num25;
            }
          }
          else
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：箱子格子序号超出,序号s的取值范围为0-15", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "removechestitems")
        {
          if (words.Length != 4 && (words.Length != 5 && (words.Length != 6 || int.Parse(words[5]) <= 0) || int.Parse(words[4]) < 0))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          ComponentChest component = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z).Entity.FindComponent<ComponentChest>(true);
          if (component == null)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误：位置(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")找不到方块实体", Color.Yellow, false, false);
            return false;
          }
          if (words.Length == 4)
          {
            for (int index = 0; index < component.m_slots.Count; ++index)
            {
              component.m_slots[index].Value = 0;
              component.m_slots[index].Count = 0;
            }
            return true;
          }
          int index14 = int.Parse(words[4]);
          if (index14 < 16)
          {
            if (words.Length == 6)
            {
              int num = int.Parse(words[5]);
              component.m_slots[index14].Count = Math.Max(0, component.m_slots[index14].Count - num);
            }
            if (words.Length == 5)
            {
              component.m_slots[index14].Value = 0;
              component.m_slots[index14].Count = 0;
            }
          }
          else
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：箱子格子序号超出,序号s的取值范围为0-15", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "openchest")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          ComponentBlockEntity blockEntity = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z);
          if (blockEntity != null)
          {
            if (componentPlayer != null)
            {
              ComponentChest component = blockEntity.Entity.FindComponent<ComponentChest>(true);
              componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ChestWidget(componentPlayer.ComponentMiner.Inventory, component);
            }
          }
        }
        else if (words[0] == "openfurnace")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          ComponentBlockEntity blockEntity = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z);
          if (blockEntity != null)
          {
            if (componentPlayer != null)
            {
              ComponentFurnace component = blockEntity.Entity.FindComponent<ComponentFurnace>(true);
              componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new FurnaceWidget(componentPlayer.ComponentMiner.Inventory, component);
            }
          }
        }
        else if (words[0] == "opendispenser")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          ComponentBlockEntity blockEntity = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z);
          if (blockEntity != null)
          {
            if (componentPlayer != null)
            {
              ComponentDispenser component = blockEntity.Entity.FindComponent<ComponentDispenser>(true);
              componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new DispenserWidget(componentPlayer.ComponentMiner.Inventory, component);
            }
          }
        }
        else if (words[0] == "opentable")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          ComponentBlockEntity blockEntity = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z);
          if (blockEntity != null)
          {
            if (componentPlayer != null)
            {
              ComponentCraftingTable component = blockEntity.Entity.FindComponent<ComponentCraftingTable>(true);
              componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new CraftingTableWidget(componentPlayer.ComponentMiner.Inventory, component);
            }
          }
        }
        else if (words[0] == "widget")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          if (!(word == "clothing"))
          {
            if (!(word == "stats"))
            {
              if (!(word == "inventory"))
              {
                if (word == "none")
                {
                  if (componentPlayer.ComponentGui.ModalPanelWidget != null)
                    componentPlayer.ComponentGui.ModalPanelWidget = (Widget) null;
                }
                else
                  this.ErrorTips(componentPlayer, condition, word, "widget");
              }
              else if (modeCreative)
              {
                if (!(componentPlayer.ComponentGui.ModalPanelWidget is CreativeInventoryWidget))
                  componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new CreativeInventoryWidget(componentPlayer.Entity);
              }
              else if (!(componentPlayer.ComponentGui.ModalPanelWidget is FullInventoryWidget))
                componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new FullInventoryWidget(componentPlayer.ComponentMiner.Inventory, componentPlayer.Entity.FindComponent<ComponentCraftingTable>(true));
            }
            else if (!(componentPlayer.ComponentGui.ModalPanelWidget is VitalStatsWidget))
              componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new VitalStatsWidget(componentPlayer);
          }
          else if (!(componentPlayer.ComponentGui.ModalPanelWidget is ClothingWidget))
            componentPlayer.ComponentGui.ModalPanelWidget = (Widget) new ClothingWidget(componentPlayer);
        }
        else if (words[0] == "clickbutton")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int face = ((MountedElectricElementBlock) BlocksManager.Blocks[142]).GetFace(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(x, y, z));
          ElectricElement electricElement = this.SubsystemElectricity.GetElectricElement(x, y, z, face);
          if (electricElement != null)
            (electricElement as ButtonElectricElement).Press();
        }
        else if (words[0] == "explosion")
        {
          if (words.Length != 5 || int.Parse(words[4]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int pressure = int.Parse(words[4]);
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemExplosions>().AddExplosion(x, y, z, (float) pressure, false, false);
        }
        else if (words[0] == "lightning")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          Vector3 targetPosition = new Vector3((float) (int.Parse(words[1]) + (coordRelative ? position.X : 0)), (float) (int.Parse(words[2]) + (coordRelative ? position.Y : 0)), (float) (int.Parse(words[3]) + (coordRelative ? position.Z : 0)));
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemSky>().MakeLightningStrike(targetPosition);
        }
        else if (words[0] == "rain")
        {
          if (words.Length != 2 || !(words[1] == "true") && !(words[1] == "false"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (words[1] == "true")
          {
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemWeather>().GlobalPrecipitationIntensity = 1f;
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemWeather>().m_precipitationStartTime = 0.0;
          }
          else
          {
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemWeather>().GlobalPrecipitationIntensity = 0.0f;
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemWeather>().m_precipitationEndTime = 0.0;
          }
        }
        else if (words[0] == "temperature")
        {
          if (words.Length != 9 || !(words[4] == "to") || int.Parse(words[8]) < 0 || int.Parse(words[8]) > 15)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int temperature = int.Parse(words[8]);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          if (cubeArea.PointMin.Y != -1)
          {
            for (int index15 = 0; index15 < cubeArea.LengthX; ++index15)
            {
              for (int index16 = 0; index16 < cubeArea.LengthZ; ++index16)
                this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.SetTemperature(cubeArea.PointMin.X + index15, cubeArea.PointMin.Z + index16, temperature);
            }
          }
        }
        else if (words[0] == "humidity")
        {
          if (words.Length != 9 || !(words[4] == "to") || int.Parse(words[8]) < 0 || int.Parse(words[8]) > 15)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int humidity = int.Parse(words[8]);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          if (cubeArea.PointMin.Y != -1)
          {
            for (int index17 = 0; index17 < cubeArea.LengthX; ++index17)
            {
              for (int index18 = 0; index18 < cubeArea.LengthZ; ++index18)
                this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.SetHumidity(cubeArea.PointMin.X + index17, cubeArea.PointMin.Z + index18, humidity);
            }
          }
        }
        else if (words[0] == "copy")
        {
          if (words.Length != 16 && (words.Length != 17 || !(words[16] == "/airapply") && !(words[16] == "/shear")) || !(words[4] == "to") || !(words[8] == "/paste") || !(words[12] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1_1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1_1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1_1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2_1 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2_1 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2_1 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int x1_2 = int.Parse(words[9]) + (coordRelative ? position.X : 0);
          int y1_2 = int.Parse(words[10]) + (coordRelative ? position.Y : 0);
          int z1_2 = int.Parse(words[11]) + (coordRelative ? position.Z : 0);
          int x2_2 = int.Parse(words[13]) + (coordRelative ? position.X : 0);
          int y2_2 = int.Parse(words[14]) + (coordRelative ? position.Y : 0);
          int z2_2 = int.Parse(words[15]) + (coordRelative ? position.Z : 0);
          bool flag1 = words.Length == 17 && words[16] == "/airapply";
          bool flag2 = words.Length == 17 && words[16] == "/shear";
          CubeArea cubeArea1 = CubeAreaManager.SetArea(x1_1, y1_1, z1_1, x2_1, y2_1, z2_1);
          CubeArea cubeArea2 = CubeAreaManager.SetArea(x1_2, y1_2, z1_2, x2_2, y2_2, z2_2);
          if (cubeArea1.LengthX == cubeArea2.LengthX && cubeArea1.LengthY == cubeArea2.LengthY && cubeArea1.LengthZ == cubeArea2.LengthZ)
          {
            if (flag2)
            {
              int[] numArray = new int[cubeArea1.LengthX * cubeArea1.LengthY * cubeArea1.LengthZ];
              int index19 = 0;
              for (int index20 = 0; index20 < cubeArea1.LengthX; ++index20)
              {
                for (int index21 = 0; index21 < cubeArea1.LengthY; ++index21)
                {
                  for (int index22 = 0; index22 < cubeArea1.LengthZ; ++index22)
                  {
                    numArray[index19] = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea1.PointMin.X + index20, cubeArea1.PointMin.Y + index21, cubeArea1.PointMin.Z + index22);
                    this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea1.PointMin.X + index20, cubeArea1.PointMin.Y + index21, cubeArea1.PointMin.Z + index22, 0);
                    ++index19;
                  }
                }
              }
              int index23 = 0;
              for (int index24 = 0; index24 < cubeArea1.LengthX; ++index24)
              {
                for (int index25 = 0; index25 < cubeArea1.LengthY; ++index25)
                {
                  for (int index26 = 0; index26 < cubeArea1.LengthZ; ++index26)
                  {
                    this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea2.PointMin.X + index24, cubeArea2.PointMin.Y + index25, cubeArea2.PointMin.Z + index26, numArray[index23]);
                    ++index23;
                  }
                }
              }
            }
            else
            {
              for (int index27 = 0; index27 < cubeArea1.LengthX; ++index27)
              {
                for (int index28 = 0; index28 < cubeArea1.LengthY; ++index28)
                {
                  for (int index29 = 0; index29 < cubeArea1.LengthZ; ++index29)
                  {
                    int cellValue = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea1.PointMin.X + index27, cubeArea1.PointMin.Y + index28, cubeArea1.PointMin.Z + index29);
                    if (flag1)
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea2.PointMin.X + index27, cubeArea2.PointMin.Y + index28, cubeArea2.PointMin.Z + index29, cellValue);
                    else if (Terrain.ExtractContents(cellValue) != 0)
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea2.PointMin.X + index27, cubeArea2.PointMin.Y + index28, cubeArea2.PointMin.Z + index29, cellValue);
                  }
                }
              }
            }
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:请选取两个长宽高相等的立方体区域", Color.Yellow, false, false);
        }
        else if (words[0] == "moveblock")
        {
          if (!((words.Length == 9 && words[4] == "to") | (words.Length == 10 && words[4] == "to" && int.Parse(words[9]) >= 0) | (words.Length == 11 && words[4] == "to" && int.Parse(words[9]) >= 0 && words[10] == "/dig") | (words.Length == 11 && words[4] == "to" && int.Parse(words[9]) >= 0 && words[10] == "/limit")))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          string[] strArray = words[8].Split(',');
          if (strArray.Length != 3)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:输入的向量格式不正确", Color.Yellow, false, false);
            return false;
          }
          int num26 = int.Parse(strArray[0]);
          int num27 = int.Parse(strArray[1]);
          int num28 = int.Parse(strArray[2]);
          int num29 = 0;
          float speed = words.Length != 9 ? (float) int.Parse(words[9]) / 10f : 5f;
          if (words.Length == 11 && words[10] == "/limit")
            num29 = 1;
          if (words.Length == 11 && words[10] == "/dig")
            num29 = 2;
          SubsystemMovingBlocks subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemMovingBlocks>(true);
          List<MovingBlock> blocks = new List<MovingBlock>();
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          Vector3 position1 = new Vector3((float) cubeArea.PointMin.X, (float) cubeArea.PointMin.Y, (float) cubeArea.PointMin.Z);
          Vector3 targetPosition = new Vector3((float) (cubeArea.PointMin.X + num26), (float) (cubeArea.PointMin.Y + num27), (float) (cubeArea.PointMin.Z + num28));
          for (int x = 0; x < cubeArea.LengthX; ++x)
          {
            for (int y = 0; y < cubeArea.LengthY; ++y)
            {
              for (int z = 0; z < cubeArea.LengthZ; ++z)
              {
                int cellValue = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + x, cubeArea.PointMin.Y + y, cubeArea.PointMin.Z + z);
                blocks.Add(new MovingBlock()
                {
                  Value = cellValue,
                  Offset = new Point3(x, y, z)
                });
              }
            }
          }
          IMovingBlockSet movingBlockSet = subsystem.AddMovingBlockSet(position1, targetPosition, speed, 0.0f, 0.0f, new Vector2(1f, 1f), (IEnumerable<MovingBlock>) blocks, "Piston", (object) null, true);
          if (movingBlockSet != null)
          {
            try
            {
              foreach (MovingBlock movingBlock in blocks)
                this.SubsystemElectricity.SubsystemTerrain.ChangeCell(cubeArea.PointMin.X + movingBlock.Offset.X, cubeArea.PointMin.Y + movingBlock.Offset.Y, cubeArea.PointMin.Z + movingBlock.Offset.Z, 0);
            }
            catch
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的运动方块", Color.Yellow, false, false);
              return false;
            }
            int w = this.m_subsystemCommandBlockBehavior.m_commandMovingBlocks.Count << 8 | num29;
            this.m_subsystemCommandBlockBehavior.m_commandMovingBlocks.Add(new Vector4(position1.X, position1.Y, position1.Z, (float) w), movingBlockSet);
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("提示:运动方块添加失败", Color.Yellow, false, false);
        }
        else if (words[0] == "moveset")
        {
          bool flag3 = words.Length == 10 && words[1] == "add" && words[6] == "to";
          bool flag4 = words.Length == 5 && words[1] == "move" && int.Parse(words[4]) > 0;
          bool flag5 = words.Length == 3 && words[1] == "stop";
          bool flag6 = words.Length == 3 && words[1] == "remove";
          if (!(flag3 | flag4 | flag5 | flag6))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (flag3)
          {
            int x1 = int.Parse(words[3]) + (coordRelative ? position.X : 0);
            int y1 = int.Parse(words[4]) + (coordRelative ? position.Y : 0);
            int z1 = int.Parse(words[5]) + (coordRelative ? position.Z : 0);
            int x2 = int.Parse(words[7]) + (coordRelative ? position.X : 0);
            int y2 = int.Parse(words[8]) + (coordRelative ? position.Y : 0);
            int z2 = int.Parse(words[9]) + (coordRelative ? position.Z : 0);
            string word = words[2];
            SubsystemMovingBlocks subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemMovingBlocks>(true);
            if (subsystem.FindMovingBlocks("Piston", (object) word) == null)
            {
              List<MovingBlock> blocks = new List<MovingBlock>();
              CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
              Vector3 vector3 = new Vector3((float) cubeArea.PointMin.X, (float) cubeArea.PointMin.Y, (float) cubeArea.PointMin.Z);
              for (int x = 0; x < cubeArea.LengthX; ++x)
              {
                for (int y = 0; y < cubeArea.LengthY; ++y)
                {
                  for (int z = 0; z < cubeArea.LengthZ; ++z)
                  {
                    int cellValue = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + x, cubeArea.PointMin.Y + y, cubeArea.PointMin.Z + z);
                    blocks.Add(new MovingBlock()
                    {
                      Value = cellValue,
                      Offset = new Point3(x, y, z)
                    });
                  }
                }
              }
              if (subsystem.AddMovingBlockSet(vector3, vector3, 0.0f, 0.0f, 0.0f, new Vector2(1f, 1f), (IEnumerable<MovingBlock>) blocks, "Piston", (object) word, true) != null)
              {
                try
                {
                  foreach (MovingBlock movingBlock in blocks)
                    this.SubsystemElectricity.SubsystemTerrain.ChangeCell(cubeArea.PointMin.X + movingBlock.Offset.X, cubeArea.PointMin.Y + movingBlock.Offset.Y, cubeArea.PointMin.Z + movingBlock.Offset.Z, 0);
                }
                catch
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的运动方块", Color.Yellow, false, false);
                  return false;
                }
              }
              else
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:添加运动方块设计失败", Color.Yellow, false, false);
                return false;
              }
            }
            else
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("提示:名为" + word + "的运动方块设计已存在", Color.Yellow, false, false);
              return false;
            }
          }
          if (flag4)
          {
            string word = words[2];
            SubsystemMovingBlocks subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemMovingBlocks>(true);
            IMovingBlockSet movingBlocks = subsystem.FindMovingBlocks("Piston", (object) word);
            if (movingBlocks != null)
            {
              try
              {
                float speed = (float) int.Parse(words[4]) / 10f;
                string[] strArray = words[3].Split(',');
                if (strArray.Length != 3)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:输入的向量格式不正确", Color.Yellow, false, false);
                  return false;
                }
                int num30 = int.Parse(strArray[0]);
                int num31 = int.Parse(strArray[1]);
                int num32 = int.Parse(strArray[2]);
                movingBlocks.Stop();
                subsystem.RemoveMovingBlockSet(movingBlocks);
                Vector3 position2 = movingBlocks.Position;
                Vector3 targetPosition = new Vector3(position2.X + (float) num30, position2.Y + (float) num31, position2.Z + (float) num32);
                subsystem.AddMovingBlockSet(position2, targetPosition, speed, 0.0f, 0.0f, new Vector2(1f, 1f), (IEnumerable<MovingBlock>) movingBlocks.Blocks, "Piston", (object) word, true);
              }
              catch
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的运动方块", Color.Yellow, false, false);
                return false;
              }
            }
            else
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("提示:名为" + word + "的运动方块设计不存在", Color.Yellow, false, false);
              return false;
            }
          }
          if (flag5)
          {
            string word = words[2];
            SubsystemMovingBlocks subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemMovingBlocks>(true);
            IMovingBlockSet movingBlocks = subsystem.FindMovingBlocks("Piston", (object) word);
            if (movingBlocks != null)
            {
              try
              {
                movingBlocks.Stop();
                subsystem.RemoveMovingBlockSet(movingBlocks);
                Vector3 position3 = movingBlocks.Position;
                subsystem.AddMovingBlockSet(position3, position3, 0.0f, 0.0f, 0.0f, new Vector2(1f, 1f), (IEnumerable<MovingBlock>) movingBlocks.Blocks, "Piston", (object) word, true);
              }
              catch
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的运动方块", Color.Yellow, false, false);
                return false;
              }
            }
            else
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("提示:名为" + word + "的运动方块设计不存在", Color.Yellow, false, false);
              return false;
            }
          }
          if (flag6)
          {
            string word = words[2];
            SubsystemMovingBlocks subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemMovingBlocks>(true);
            IMovingBlockSet movingBlocks = subsystem.FindMovingBlocks("Piston", (object) word);
            if (movingBlocks != null)
            {
              try
              {
                subsystem.RemoveMovingBlockSet(movingBlocks);
                foreach (MovingBlock block in movingBlocks.Blocks)
                  this.SubsystemElectricity.SubsystemTerrain.ChangeCell((int) movingBlocks.Position.X + block.Offset.X, (int) movingBlocks.Position.Y + block.Offset.Y, (int) movingBlocks.Position.Z + block.Offset.Z, block.Value);
              }
              catch
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的运动方块", Color.Yellow, false, false);
                return false;
              }
            }
            else
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("提示:名为" + word + "的运动方块设计不存在", Color.Yellow, false, false);
              return false;
            }
          }
        }
        else if (words[0] == "hammer")
        {
          if (words.Length != 4 && (words.Length != 5 && (words.Length != 6 || !(words[5] == "/edit")) || int.Parse(words[4]) < 1 || int.Parse(words[4]) > 4))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int face = 0;
          bool flag7 = words.Length == 4;
          bool flag8 = words.Length == 6;
          if (!flag7)
            face = int.Parse(words[4]);
          SubsystemFurnitureBlockBehavior m_subsystemFurnitureBlockBehavior = this.SubsystemElectricity.Project.FindSubsystem<SubsystemFurnitureBlockBehavior>(true);
          CellFace start = new CellFace(x, y, z, face);
          Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
          try
          {
            FurnitureDesign design = (FurnitureDesign) null;
            FurnitureDesign sourceDesign = (FurnitureDesign) null;
            Dictionary<Point3, int> valuesDictionary = new Dictionary<Point3, int>();
            Point3 point1 = start.Point;
            Point3 point2 = start.Point;
            int startValue = this.SubsystemElectricity.SubsystemTerrain.Terrain.GetCellValue(start.Point.X, start.Point.Y, start.Point.Z);
            if (BlocksManager.Blocks[Terrain.ExtractContents(startValue)] is FurnitureBlock)
            {
              int designIndex = FurnitureBlock.GetDesignIndex(Terrain.ExtractData(startValue));
              sourceDesign = m_subsystemFurnitureBlockBehavior.GetDesign(designIndex);
              if (sourceDesign == null)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：没有找到合适的方块", Color.Yellow, false, false);
                return false;
              }
              design = sourceDesign.Clone();
              design.LinkedDesign = (FurnitureDesign) null;
              design.InteractionMode = FurnitureInteractionMode.None;
              valuesDictionary.Add(start.Point, startValue);
            }
            else
            {
              Stack<Point3> point3Stack = new Stack<Point3>();
              point3Stack.Push(start.Point);
              while (point3Stack.Count > 0)
              {
                Point3 key = point3Stack.Pop();
                if (!valuesDictionary.ContainsKey(key))
                {
                  int cellValue = this.SubsystemElectricity.SubsystemTerrain.Terrain.GetCellValue(key.X, key.Y, key.Z);
                  if (SubsystemFurnitureBlockBehavior.IsValueDisallowed(cellValue))
                  {
                    componentPlayer.ComponentGui.DisplaySmallMessage("提示：没有找到合适的方块", Color.Yellow, false, false);
                    return false;
                  }
                  if (Terrain.ExtractContents(cellValue) != 0)
                  {
                    if (key.X < point1.X)
                      point1.X = key.X;
                    if (key.Y < point1.Y)
                      point1.Y = key.Y;
                    if (key.Z < point1.Z)
                      point1.Z = key.Z;
                    if (key.X > point2.X)
                      point2.X = key.X;
                    if (key.Y > point2.Y)
                      point2.Y = key.Y;
                    if (key.Z > point2.Z)
                      point2.Z = key.Z;
                    valuesDictionary[key] = cellValue;
                    point3Stack.Push(new Point3(key.X - 1, key.Y, key.Z));
                    point3Stack.Push(new Point3(key.X + 1, key.Y, key.Z));
                    point3Stack.Push(new Point3(key.X, key.Y - 1, key.Z));
                    point3Stack.Push(new Point3(key.X, key.Y + 1, key.Z));
                    point3Stack.Push(new Point3(key.X, key.Y, key.Z - 1));
                    point3Stack.Push(new Point3(key.X, key.Y, key.Z + 1));
                  }
                }
              }
              if (valuesDictionary.Count == 0)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：没有找到合适的方块", Color.Yellow, false, false);
                return false;
              }
              design = new FurnitureDesign(this.SubsystemElectricity.SubsystemTerrain);
              Point3 point3_1 = point2 - point1;
              int resolution = MathUtils.Max(MathUtils.Max(point3_1.X, point3_1.Y, point3_1.Z) + 1, 2);
              int[] values = new int[resolution * resolution * resolution];
              foreach (KeyValuePair<Point3, int> keyValuePair in valuesDictionary)
              {
                Point3 point3_2 = keyValuePair.Key - point1;
                values[point3_2.X + point3_2.Y * resolution + point3_2.Z * resolution * resolution] = keyValuePair.Value;
              }
              design.SetValues(resolution, values);
              int steps = start.Face > 3 ? CellFace.Vector3ToFace(v, 3) : CellFace.OppositeFace(start.Face);
              design.Rotate(1, steps);
              Point3 location1 = design.Box.Location;
              Point3 point3_3 = new Point3(design.Resolution);
              Box box = design.Box;
              Point3 location2 = box.Location;
              box = design.Box;
              Point3 size = box.Size;
              Point3 point3_4 = location2 + size;
              Point3 point3_5 = point3_3 - point3_4;
              Point3 delta = new Point3((point3_5.X - location1.X) / 2, -location1.Y, (point3_5.Z - location1.Z) / 2);
              design.Shift(delta);
            }
            if (flag8)
            {
              BuildFurnitureDialog buildFurnitureDialog = new BuildFurnitureDialog(design, sourceDesign, (Action<bool>) (result =>
              {
                if (!result)
                  return;
                design = m_subsystemFurnitureBlockBehavior.TryAddDesign(design);
                if (design == null)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("提示：没有找到合适的方块", Color.Yellow, false, false);
                }
                else
                {
                  if (m_subsystemFurnitureBlockBehavior.m_subsystemGameInfo.WorldSettings.GameMode != GameMode.Creative)
                  {
                    foreach (KeyValuePair<Point3, int> keyValuePair in valuesDictionary)
                      this.SubsystemElectricity.SubsystemTerrain.DestroyCell(0, keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z, 0, true, true);
                  }
                  int num = Terrain.MakeBlockValue(227, 0, FurnitureBlock.SetDesignIndex(0, design.Index, design.ShadowStrengthFactor, design.IsLightEmitter));
                  int count = MathUtils.Clamp(design.Resolution, 4, 8);
                  Matrix matrix = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Matrix;
                  Vector3 position4 = matrix.Translation + 1f * matrix.Forward + 1f * Vector3.UnitY;
                  m_subsystemFurnitureBlockBehavior.m_subsystemPickables.AddPickable(num, count, position4, new Vector3?(), new Matrix?());
                  componentPlayer.ComponentMiner.DamageActiveTool(1);
                  componentPlayer.ComponentMiner.Poke(false);
                  for (int index = 0; index < 3; ++index)
                    Time.QueueTimeDelayedExecution(Time.FrameStartTime + (double) index * 0.25, (Action) (() => m_subsystemFurnitureBlockBehavior.m_subsystemSoundMaterials.PlayImpactSound(startValue, new Vector3(start.Point), 1f)));
                  if (componentPlayer.ComponentMiner.ComponentCreature.PlayerStats == null)
                    return;
                  componentPlayer.ComponentMiner.ComponentCreature.PlayerStats.FurnitureItemsMade += (long) count;
                }
              }));
              if (componentPlayer != null)
                DialogsManager.ShowDialog(componentPlayer.GuiWidget, (Dialog) buildFurnitureDialog);
            }
            else
            {
              design = m_subsystemFurnitureBlockBehavior.TryAddDesign(design);
              if (design == null)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示：没有找到合适的方块", Color.Yellow, false, false);
              }
              else
              {
                if (m_subsystemFurnitureBlockBehavior.m_subsystemGameInfo.WorldSettings.GameMode != GameMode.Creative)
                {
                  foreach (KeyValuePair<Point3, int> keyValuePair in valuesDictionary)
                    this.SubsystemElectricity.SubsystemTerrain.DestroyCell(0, keyValuePair.Key.X, keyValuePair.Key.Y, keyValuePair.Key.Z, 0, true, true);
                }
                int num = Terrain.MakeBlockValue(227, 0, FurnitureBlock.SetDesignIndex(0, design.Index, design.ShadowStrengthFactor, design.IsLightEmitter));
                int count = MathUtils.Clamp(design.Resolution, 4, 8);
                Matrix matrix = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Matrix;
                Vector3 position5 = matrix.Translation + 1f * matrix.Forward + 1f * Vector3.UnitY;
                m_subsystemFurnitureBlockBehavior.m_subsystemPickables.AddPickable(num, count, position5, new Vector3?(), new Matrix?());
                componentPlayer.ComponentMiner.DamageActiveTool(1);
                componentPlayer.ComponentMiner.Poke(false);
                for (int index = 0; index < 3; ++index)
                  Time.QueueTimeDelayedExecution(Time.FrameStartTime + (double) index * 0.25, (Action) (() => m_subsystemFurnitureBlockBehavior.m_subsystemSoundMaterials.PlayImpactSound(startValue, new Vector3(start.Point), 1f)));
                if (componentPlayer.ComponentMiner.ComponentCreature.PlayerStats != null)
                  componentPlayer.ComponentMiner.ComponentCreature.PlayerStats.FurnitureItemsMade += (long) count;
              }
            }
          }
          catch (Exception ex)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:家具建造出错", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "unhammer")
        {
          if (words.Length != 5 || int.Parse(words[4]) < 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num33 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int num34 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num35 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int index30 = int.Parse(words[4]);
          FurnitureDesign design = this.SubsystemElectricity.Project.FindSubsystem<SubsystemFurnitureBlockBehavior>(true).GetDesign(index30);
          if (design != null)
          {
            int[] values = design.m_values;
            int resolution = design.m_resolution;
            int num36 = 0;
            for (int index31 = 0; index31 < resolution; ++index31)
            {
              for (int index32 = 0; index32 < resolution; ++index32)
              {
                for (int index33 = 0; index33 < resolution; ++index33)
                  this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num33 - index31, num34 + index32, num35 + index33, values[num36++]);
              }
            }
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:找不到序号为" + index30.ToString() + "的家具", Color.Yellow, false, false);
        }
        else if (words[0] == "replacedesign")
        {
          if (words.Length != 10 || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          int num37 = int.Parse(words[8]);
          int num38 = int.Parse(words[9]);
          SubsystemFurnitureBlockBehavior subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemFurnitureBlockBehavior>(true);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          for (int index34 = 0; index34 < cubeArea.LengthX; ++index34)
          {
            for (int index35 = 0; index35 < cubeArea.LengthY; ++index35)
            {
              for (int index36 = 0; index36 < cubeArea.LengthZ; ++index36)
              {
                int cellValue = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + index34, cubeArea.PointMin.Y + index35, cubeArea.PointMin.Z + index36);
                int contents = Terrain.ExtractContents(cellValue);
                int data1 = Terrain.ExtractData(cellValue);
                if (contents == 227)
                {
                  int designIndex = FurnitureBlock.GetDesignIndex(data1);
                  FurnitureDesign design = subsystem.GetDesign(designIndex);
                  if (design != null)
                  {
                    List<FurnitureDesign> furnitureDesignList = design.CloneChain();
                    foreach (FurnitureDesign furnitureDesign1 in furnitureDesignList)
                    {
                      int[] values = new int[design.m_values.Length];
                      for (int index37 = 0; index37 < design.m_values.Length; ++index37)
                        values[index37] = Terrain.ReplaceLight(design.m_values[index37], 0) != num37 ? design.m_values[index37] : num38;
                      furnitureDesign1.SetValues(design.m_resolution, values);
                      FurnitureDesign furnitureDesign2 = subsystem.TryAddDesignChain(furnitureDesignList[0], true);
                      if (furnitureDesign2 != null)
                      {
                        int data2 = FurnitureBlock.SetDesignIndex(data1, furnitureDesign2.Index, furnitureDesign2.ShadowStrengthFactor, furnitureDesign2.IsLightEmitter);
                        int num39 = Terrain.ReplaceData(cellValue, data2);
                        this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea.PointMin.X + index34, cubeArea.PointMin.Y + index35, cubeArea.PointMin.Z + index36, num39);
                      }
                    }
                  }
                  else
                    componentPlayer.ComponentGui.DisplaySmallMessage("错误:找不到序号为" + designIndex.ToString() + "的家具", Color.Yellow, false, false);
                }
              }
            }
          }
        }
        else if (words[0] == "linkdesign")
        {
          if (words.Length != 2 && (words.Length != 3 || !(words[2] == "/wire")))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string[] strArray = words[1].Split(',');
          bool flag = words.Length == 2;
          if (strArray.Length < 2)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：家具数量少于2，无法链接", Color.Yellow, false, false);
            return false;
          }
          SubsystemFurnitureBlockBehavior subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemFurnitureBlockBehavior>(true);
          FurnitureDesign design1 = subsystem.GetDesign(int.Parse(strArray[0]));
          List<FurnitureDesign> furnitureDesignList = new List<FurnitureDesign>();
          if (design1.LinkedDesign == null)
          {
            for (int index = 0; index < strArray.Length; ++index)
            {
              FurnitureDesign design2 = subsystem.GetDesign(int.Parse(strArray[index]));
              if (design2 == null)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误：家具序号出错，不存在序号：" + int.Parse(strArray[index]).ToString(), Color.Yellow, false, false);
                return false;
              }
              FurnitureDesign furnitureDesign = design2.Clone();
              furnitureDesignList.Add(furnitureDesign);
            }
          }
          else
          {
            for (int index = 0; index < strArray.Length; ++index)
            {
              FurnitureDesign design3 = subsystem.GetDesign(int.Parse(strArray[index]));
              if (design3 == null)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误：家具序号出错，不存在序号：" + int.Parse(strArray[index]).ToString(), Color.Yellow, false, false);
                return false;
              }
              foreach (FurnitureDesign furnitureDesign3 in design3.ListChain())
              {
                FurnitureDesign furnitureDesign4 = furnitureDesign3.Clone();
                furnitureDesignList.Add(furnitureDesign4);
              }
            }
            componentPlayer.ComponentGui.DisplaySmallMessage("提示：检测到首个家具为多态家具，以追加扩展的方式链接", Color.Yellow, false, false);
          }
          for (int index = 0; index < furnitureDesignList.Count; ++index)
          {
            furnitureDesignList[index].InteractionMode = flag ? FurnitureInteractionMode.Multistate : FurnitureInteractionMode.ConnectedMultistate;
            furnitureDesignList[index].LinkedDesign = furnitureDesignList[(index + 1) % furnitureDesignList.Count];
          }
          FurnitureDesign furnitureDesign5 = subsystem.TryAddDesignChain(furnitureDesignList[0], true);
          if (furnitureDesign5 != null)
          {
            int num = Terrain.MakeBlockValue(227, 0, FurnitureBlock.SetDesignIndex(0, furnitureDesign5.Index, furnitureDesign5.ShadowStrengthFactor, furnitureDesign5.IsLightEmitter));
            Vector3 vector3 = new Vector3((float) this.CellFaces[0].Point.X, (float) this.CellFaces[0].Point.Y, (float) this.CellFaces[0].Point.Z);
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemPickables>().AddPickable(num, 1, new Vector3(vector3.X + 0.5f, vector3.Y + 1f, vector3.Z + 0.5f), new Vector3?(), new Matrix?());
          }
        }
        else if (words[0] == "write")
        {
          if (words.Length < 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string path = (!(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2")) + "/Command";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          FileStream fileStream = new FileStream(path + "/command.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
          StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
          string str = words[1];
          if (words.Length > 2)
          {
            for (int index = 2; index < words.Length; ++index)
              str = str + " " + words[index];
          }
          streamWriter.WriteLine(str);
          streamWriter.Flush();
          streamWriter.Close();
          fileStream.Close();
          componentPlayer.ComponentGui.DisplaySmallMessage("写入成功，路径：\n" + path, Color.Yellow, false, false);
        }
        else if (words[0] == "writeappend")
        {
          if (words.Length < 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string path = (!(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2")) + "/Command";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          StreamWriter streamWriter = new StreamWriter(path + "/command.txt", true);
          string str = words[1];
          if (words.Length > 2)
          {
            for (int index = 2; index < words.Length; ++index)
              str = str + " " + words[index];
          }
          streamWriter.WriteLine(str);
          streamWriter.Flush();
          streamWriter.Close();
          componentPlayer.ComponentGui.DisplaySmallMessage("追加写入成功，路径：\n" + path, Color.Yellow, false, false);
        }
        else if (words[0] == "read")
        {
          if (words.Length != 2 || int.Parse(words[1]) < 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          if (int.Parse(words[1]) == 0)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("", Color.Yellow, false, false);
            return true;
          }
          string path = (!(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2")) + "/Command";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          if (File.Exists(path + "/command.txt"))
          {
            FileStream fileStream = new FileStream(path + "/command.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader((Stream) fileStream);
            int num40 = int.Parse(words[1]);
            int num41 = 1;
            string text;
            while ((text = streamReader.ReadLine()) != null)
            {
              if (num40 == num41)
                componentPlayer.ComponentGui.DisplaySmallMessage(text, Color.Yellow, false, false);
              ++num41;
            }
            streamReader.Close();
            fileStream.Close();
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:待读取的command.txt文件不存在", Color.Yellow, false, false);
        }
        else if (words[0] == "copyfile")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          string str = !(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2");
          if (!Directory.Exists(str + "/Command"))
            Directory.CreateDirectory(str + "/Command");
          if (File.Exists(str + "/Command/" + word))
          {
            string path1 = str + "/Command/" + word;
            string path2 = Engine.Storage.GetSystemPath(GameManager.m_worldInfo.DirectoryName) + "/" + word;
            FileStream fileStream = new FileStream(path1, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            FileStream destination = new FileStream(path2, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fileStream.CopyTo((Stream) destination);
            fileStream.Dispose();
            destination.Dispose();
            componentPlayer.ComponentGui.DisplaySmallMessage("已将文件‘" + word + "’复制到存档目录下", Color.Yellow, false, false);
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:Command目录不存在‘" + word + "’文件", Color.Yellow, false, false);
        }
        else if (words[0] == "getcell")
        {
          if (words.Length != 8 && (words.Length != 9 || !(words[8] == "/airapply")) || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
          int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
          bool flag = false;
          if (words.Length == 9)
            flag = true;
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          int[] numArray = new int[cubeArea.LengthX * cubeArea.LengthY * cubeArea.LengthZ];
          int index38 = 0;
          string path = (!(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2")) + "/Command";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          FileStream fileStream = new FileStream(path + "/cell.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
          StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
          for (int index39 = 0; index39 < cubeArea.LengthX; ++index39)
          {
            for (int index40 = 0; index40 < cubeArea.LengthY; ++index40)
            {
              for (int index41 = 0; index41 < cubeArea.LengthZ; ++index41)
              {
                numArray[index38] = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + index39, cubeArea.PointMin.Y + index40, cubeArea.PointMin.Z + index41);
                string str = index39.ToString() + "," + index40.ToString() + "," + index41.ToString() + "," + numArray[index38].ToString();
                if (flag)
                {
                  streamWriter.WriteLine(str);
                  ++index38;
                }
                else if (Terrain.ExtractContents(numArray[index38]) != 0)
                {
                  streamWriter.WriteLine(str);
                  ++index38;
                }
              }
            }
          }
          streamWriter.Flush();
          streamWriter.Close();
          fileStream.Close();
          if (modeCreative)
            componentPlayer.ComponentGui.DisplaySmallMessage("方块文件已生成，路径：\n" + path, Color.Yellow, false, false);
        }
        else if (words[0] == "getterrain")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          SubsystemTerrain subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>();
          int cellLight = subsystem.Terrain.GetCellLight(x, y, z);
          int num = subsystem.Terrain.GetSeasonalTemperature(x, z) + SubsystemWeather.GetTemperatureAdjustmentAtHeight(y);
          if (num < 0)
            num = 0;
          if (num > 15)
            num = 15;
          int humidity = subsystem.Terrain.GetHumidity(x, z);
          float oceanShoreDistance = subsystem.TerrainContentsGenerator.CalculateOceanShoreDistance((float) x, (float) z);
          float skyLightIntensity = componentPlayer.PlayerData.m_subsystemSky.SkyLightIntensity;
          componentPlayer.ComponentGui.DisplaySmallMessage(string.Format("方块位置亮度：{0}，温度：{1}，湿度：{2}，\n离海岸的距离：{3}，当前天空亮度：{4}", (object) cellLight, (object) num, (object) humidity, (object) oceanShoreDistance, (object) skyLightIntensity), Color.Yellow, false, false);
        }
        else if (words[0] == "getid")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num42 = int.Parse(words[1]);
          int num43 = num42 & 1023;
          int num44 = (num42 & -16384) >> 14;
          int num45 = (num42 & 15360) >> 10;
          componentPlayer.ComponentGui.DisplaySmallMessage(string.Format("方块完整值:{0}，ID:{1}，特殊值:{2}，光亮特殊值:{3}", (object) num42, (object) num43, (object) num44, (object) num45), Color.Yellow, false, false);
        }
        else if (words[0] == "getplayerstats")
        {
          if (words.Length != 1)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num46 = (int) ((double) componentPlayer.ComponentHealth.Health * 10.0);
          int num47 = (int) ((double) componentPlayer.ComponentVitalStats.Food * 10.0);
          int num48 = (int) ((double) componentPlayer.ComponentVitalStats.Stamina * 10.0);
          int num49 = (int) ((double) componentPlayer.ComponentVitalStats.Sleep * 10.0);
          int attackPower = (int) componentPlayer.ComponentMiner.AttackPower;
          int attackResilience = (int) componentPlayer.ComponentHealth.AttackResilience;
          int num50 = (int) ((double) componentPlayer.ComponentLocomotion.WalkSpeed * 10.0);
          int temperature = (int) componentPlayer.ComponentVitalStats.Temperature;
          int num51 = (int) ((double) componentPlayer.ComponentVitalStats.Wetness * 10.0);
          componentPlayer.ComponentGui.DisplaySmallMessage(string.Format("生命值:{0}，饥饿度:{1}，耐力值:{2}，疲劳度:{3}，\n攻击力:{4}，防御值:{5}，行走速度:{6}，\n体温:{7}，体湿:{8}", (object) num46, (object) num47, (object) num48, (object) num49, (object) attackPower, (object) attackResilience, (object) num50, (object) temperature, (object) num51), Color.Yellow, false, false);
        }
        else if (words[0] == "world")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          bool flag = false;
          List<WorldInfo> worldInfoList = new List<WorldInfo>((IEnumerable<WorldInfo>) WorldsManager.WorldInfos);
          worldInfoList.Sort((Comparison<WorldInfo>) ((w1, w2) => DateTime.Compare(w2.LastSaveTime, w1.LastSaveTime)));
          foreach (WorldInfo worldInfo in worldInfoList)
          {
            if (worldInfo.WorldSettings.Name == word)
            {
              GameManager.SaveProject(true, true);
              GameManager.DisposeProject();
              ScreensManager.SwitchScreen("GameLoading", (object) worldInfo, null);
              flag = true;
              break;
            }
          }
          if (!flag)
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:找不到名为“" + word + "”的世界", Color.Yellow, false, false);
        }
        else if (words[0] == "worldtexture")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          WorldInfo worldInfo = GameManager.m_worldInfo;
          BlocksTexturesManager.UpdateBlocksTexturesList();
          foreach (string blockTextureName in BlocksTexturesManager.m_blockTextureNames)
          {
            if (word == "Survivalcraft" || blockTextureName == word)
            {
              worldInfo.WorldSettings.BlocksTextureName = word + ".scbtex";
              GameManager.SaveProject(true, true);
              GameManager.DisposeProject();
              if (worldInfo.WorldSettings.GameMode != GameMode.Creative && worldInfo.WorldSettings.GameMode != GameMode.Adventure)
                worldInfo.WorldSettings.ResetOptionsForNonCreativeMode();
              WorldsManager.ChangeWorld(worldInfo.DirectoryName, worldInfo.WorldSettings);
              ScreensManager.SwitchScreen("GameLoading", (object) worldInfo, null);
              return true;
            }
          }
          string pathname = Datahandle.GetPathname(word);
          if (File.Exists(pathname))
          {
            string path3 = pathname;
            string path4 = !(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:/TexturePacks") + "/" + word + ".scbtex" : Engine.Storage.GetSystemPath(BlocksTexturesManager.BlockTexturesDirectoryName) + "/" + word + ".scbtex";
            if (!File.Exists(path4))
            {
              FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
              FileStream destination = new FileStream(path4, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
              Image image = Image.Load((Stream) fileStream);
              if (!MathUtils.IsPowerOf2((long) image.Width) || !MathUtils.IsPowerOf2((long) image.Height))
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的材质", Color.Yellow, false, false);
                return false;
              }
              fileStream.Position = 0L;
              fileStream.CopyTo((Stream) destination);
              fileStream.Dispose();
              destination.Dispose();
            }
            BlocksTexturesManager.UpdateBlocksTexturesList();
            worldInfo.WorldSettings.BlocksTextureName = word + ".scbtex";
            GameManager.SaveProject(true, true);
            GameManager.DisposeProject();
            if (worldInfo.WorldSettings.GameMode != GameMode.Creative && worldInfo.WorldSettings.GameMode != GameMode.Adventure)
              worldInfo.WorldSettings.ResetOptionsForNonCreativeMode();
            WorldsManager.ChangeWorld(worldInfo.DirectoryName, worldInfo.WorldSettings);
            ScreensManager.SwitchScreen("GameLoading", (object) worldInfo, null);
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:待装载的材质文件不存在", Color.Yellow, false, false);
        }
        else if (words[0] == "audio")
        {
          if (int.Parse(words[2]) < 0 || ((words.Length != 4 ? 0 : (int.Parse(words[3]) >= 0 ? 1 : 0)) | (words.Length == 3 ? 1 : 0)) == 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          Vector3 vector3 = new Vector3(position);
          string word = words[1];
          float volume1 = (float) int.Parse(words[2]) / 15f;
          float num52 = 1f;
          bool flag9 = true;
          bool flag10 = false;
          bool flag11 = words.Length == 3;
          if ((double) volume1 > 1.0)
            volume1 = 1f;
          int n = 0;
          if (!flag11)
            n = int.Parse(words[3]);
          float x = 0.0f;
          float num53 = 2093f / 16f * MathUtils.Pow(1.059463f, (float) n);
          int num54 = 0;
          for (int index = 4; index <= 6; ++index)
          {
            float num55 = num53 / (523.25f * MathUtils.Pow(2f, (float) (index - 5)));
            if (num54 == 0 || (double) num55 >= 0.5 && (double) num55 < (double) x)
            {
              num54 = index;
              x = num55;
            }
          }
          if ((double) x != 0.0)
            num52 = MathUtils.Clamp(MathUtils.Log(x) / MathUtils.Log(2f), -1f, 1f);
          string pathname = Datahandle.GetPathname(word);
          if (File.Exists(pathname))
          {
            try
            {
              Stream stream = (Stream) new FileStream(pathname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
              if (!Wav.IsWavStream(stream) && !Ogg.IsOggStream(stream))
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("提示:仅支持wav和ogg格式的音频", Color.Yellow, false, false);
                return false;
              }
              SoundBuffer soundBuffer = SoundBuffer.Load(stream);
              if ((double) volume1 > 0.0)
              {
                float pitch = MathUtils.Pow(2f, num52);
                if (word.Contains("ogg") || word.Contains("OGG"))
                {
                  if (flag11)
                  {
                    new Sound(soundBuffer, volume1, disposeOnStop: true).Play();
                  }
                  else
                  {
                    if (n > 11)
                      pitch = 1f;
                    new Sound(soundBuffer, volume1, pitch, disposeOnStop: true).Play();
                  }
                }
                else if (n != 15)
                {
                  float minDistance = (float) (0.5 + 5.0 * (double) volume1);
                  float volume2 = this.SubsystemElectricity.SubsystemAudio.CalculateVolume(this.SubsystemElectricity.SubsystemAudio.CalculateListenerDistance(vector3), minDistance);
                  if (flag11)
                  {
                    new Sound(soundBuffer, volume1, disposeOnStop: true).Play();
                  }
                  else
                  {
                    if (n > 11)
                      pitch = 1f;
                    new Sound(soundBuffer, volume1 * volume2, pitch, disposeOnStop: true).Play();
                  }
                }
              }
              stream.Close();
              return true;
            }
            catch
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + word + "’为无效的音频文件", Color.Yellow, false, false);
              return false;
            }
          }
          else
          {
            if (!word.Contains("."))
            {
              try
              {
                flag10 = true;
                ContentManager.Get("Audio/" + word);
              }
              catch
              {
                flag9 = false;
              }
              if (flag9)
              {
                if (n != 15 && (double) volume1 > 0.0)
                {
                  float minDistance = (float) (0.5 + 5.0 * (double) volume1);
                  if (flag11)
                  {
                    this.SubsystemElectricity.SubsystemAudio.PlaySound("Audio/" + word, volume1, 1f, 0.0f, 0.0f);
                  }
                  else
                  {
                    if (n > 11)
                      num52 = 1f;
                    this.SubsystemElectricity.SubsystemAudio.PlaySound("Audio/" + word, volume1, num52, vector3, minDistance, true);
                  }
                }
                return true;
              }
            }
            if (flag10)
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:在Content/Audio中找不到名为‘" + word + "’的音频文件", Color.Yellow, false, false);
            else
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:在存档目录或Command目录中找不到名为‘" + word + "’的音频文件", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "pattern")
        {
          if ((words.Length != 7 || int.Parse(words[5]) <= 0 || int.Parse(words[5]) >= 7 || int.Parse(words[6]) < 0) && words.Length != 5)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          string word = words[4];
          if (!(word.Contains("png") | word.Contains("PNG")))
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示:图片需转换成png格式，名称包括后缀", Color.Yellow, false, false);
            return false;
          }
          float num56 = 3f;
          Point3 point3 = new Point3(x, y, z);
          Vector3 vector3_1 = new Vector3(0.0f, 0.0f, -1f);
          Vector3 vector3_2 = new Vector3(0.0f, -1f, 0.0f);
          Vector3 vector3_3 = new Vector3(-1f, 0.0f, 0.0f);
          Vector3 vector3_4 = new Vector3(point3) + new Vector3(0.5f, 0.5f, 0.0f);
          if (words.Length == 7)
          {
            int num57 = int.Parse(words[5]);
            num56 = (float) int.Parse(words[6]) / 10f;
            if ((double) num56 == 0.0)
            {
              if (this.m_subsystemLogo.m_patternPoints.ContainsKey(point3))
              {
                this.m_subsystemLogo.RemovePatternPoint(point3);
                this.m_subsystemLogo.RemovePatternTexture(point3);
              }
              return true;
            }
            switch (num57)
            {
              case 2:
                vector3_1 = new Vector3(-1f, 0.0f, 0.0f);
                vector3_3 = new Vector3(0.0f, 0.0f, -1f);
                vector3_4 = new Vector3(point3) + new Vector3(1f, 0.5f, 0.5f);
                break;
              case 3:
                vector3_1 = new Vector3(0.0f, -1f, 0.0f);
                vector3_2 = new Vector3(0.0f, 0.0f, -1f);
                vector3_4 = new Vector3(point3) + new Vector3(0.5f, 1f, 0.5f);
                break;
              case 4:
                vector3_3 = new Vector3(1f, 0.0f, 0.0f);
                vector3_4 = new Vector3(point3) + new Vector3(0.5f, 0.5f, 1f);
                break;
              case 5:
                vector3_1 = new Vector3(-1f, 0.0f, 0.0f);
                vector3_3 = new Vector3(0.0f, 0.0f, 1f);
                vector3_2 = new Vector3(0.0f, -1f, 0.0f);
                vector3_4 = new Vector3(point3) + new Vector3(0.0f, 0.5f, 0.5f);
                break;
              case 6:
                vector3_1 = new Vector3(0.0f, -1f, 0.0f);
                vector3_2 = new Vector3(0.0f, 0.0f, -1f);
                vector3_3 = new Vector3(1f, 0.0f, 0.0f);
                vector3_4 = new Vector3(point3) + new Vector3(0.5f, 0.0f, 0.5f);
                break;
            }
          }
          string pathname = Datahandle.GetPathname(word);
          if (File.Exists(pathname))
          {
            try
            {
              Texture2D patterntexture = Texture2D.Load((Stream) new FileStream(pathname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite), false, 1);
              PatternPoint patternPoint = new PatternPoint();
              patternPoint.Position = vector3_4;
              patternPoint.Forward = vector3_1;
              patternPoint.Up = vector3_2;
              patternPoint.Right = vector3_3;
              patternPoint.Color = Color.White;
              patternPoint.Size = num56 / 2.05f;
              patternPoint.FarSize = num56 / 2.05f;
              patternPoint.FarDistance = 1f;
              if (!this.m_subsystemLogo.m_patternPoints.ContainsKey(point3))
              {
                this.m_subsystemLogo.AddPatternPoint(point3, patternPoint);
                this.m_subsystemLogo.AddPatternTexture(point3, patterntexture);
              }
              else
              {
                this.m_subsystemLogo.m_patternPoints[point3] = patternPoint;
                this.m_subsystemLogo.m_patternTextures[point3] = patterntexture;
              }
            }
            catch (Exception ex)
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("提示：发生未知错误，图片生成失败", Color.Yellow, false, false);
            }
          }
        }
        else if (words[0] == "removepattern")
        {
          if (words.Length != 8 || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          CubeArea cubeArea = CubeAreaManager.SetArea(int.Parse(words[1]) + (coordRelative ? position.X : 0), int.Parse(words[2]) + (coordRelative ? position.Y : 0), int.Parse(words[3]) + (coordRelative ? position.Z : 0), int.Parse(words[5]) + (coordRelative ? position.X : 0), int.Parse(words[6]) + (coordRelative ? position.Y : 0), int.Parse(words[7]) + (coordRelative ? position.Z : 0));
          bool flag = false;
          for (int index42 = 0; index42 < cubeArea.LengthX; ++index42)
          {
            for (int index43 = 0; index43 < cubeArea.LengthY; ++index43)
            {
              for (int index44 = 0; index44 < cubeArea.LengthZ; ++index44)
              {
                Point3 point3 = new Point3(cubeArea.PointMin.X + index42, cubeArea.PointMin.Y + index43, cubeArea.PointMin.Z + index44);
                if (this.m_subsystemLogo.m_patternPoints.ContainsKey(point3))
                {
                  this.m_subsystemLogo.RemovePatternPoint(point3);
                  this.m_subsystemLogo.RemovePatternTexture(point3);
                  flag = true;
                }
              }
            }
          }
          if (modeCreative)
          {
            if (flag)
              componentPlayer.ComponentGui.DisplaySmallMessage("移除光点贴图成功", Color.Yellow, false, false);
            else
              componentPlayer.ComponentGui.DisplaySmallMessage("提示：指定区域不存在光点贴图", Color.Yellow, false, false);
          }
        }
        else if (words[0] == "image")
        {
          if (!(words.Length == 5 | words.Length == 6))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int z = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          string word = words[4];
          int num58 = 1;
          if (words.Length == 6)
          {
            if (words[5] == "/tile")
              num58 = 2;
            else if (words[5] == "/rotate")
            {
              num58 = 3;
            }
            else
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误：无效的扩展指令", Color.Yellow, false, false);
              return false;
            }
          }
          if (!(word.Contains("png") | word.Contains("PNG")))
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示:图片需转换成png格式，名称包括后缀", Color.Yellow, false, false);
            return false;
          }
          string pathname = Datahandle.GetPathname(word);
          if (File.Exists(pathname))
          {
            Stream stream = (Stream) new FileStream(pathname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Image image = Image.Load(stream);
            int width = image.Width;
            int height = image.Height;
            for (int y2 = 0; y2 < height; ++y2)
            {
              for (int x2 = 0; x2 < width; ++x2)
              {
                Color pixel = image.GetPixel(x2, y2);
                byte a = pixel.A;
                byte r = pixel.R;
                byte g = pixel.G;
                byte b = pixel.B;
                if (a < (byte) 20)
                {
                  switch (num58)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1 - x2, y1 - y2, z, 0);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1 - x2, y1, z - y2, 0);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1, y1 - y2, z - x2, 0);
                      continue;
                    default:
                      continue;
                  }
                }
                else
                {
                  int num59 = Datahandle.Colorhandle(new Color(r, g, b, a));
                  switch (num58)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1 - x2, y1 - y2, z, 72 + (num59 * 2 + 1) * 16384);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1 - x2, y1, z - y2, 72 + (num59 * 2 + 1) * 16384);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1, y1 - y2, z - x2, 72 + (num59 * 2 + 1) * 16384);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
            stream.Close();
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:在存档目录或Command目录中找不到名为“" + word + "”的图片", Color.Yellow, false, false);
        }
        else if (words[0] == "build")
        {
          if (words.Length != 5)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num60 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
          int num61 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
          int num62 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
          string word = words[4];
          string pathname = Datahandle.GetPathname(word);
          if (File.Exists(pathname))
          {
            FileStream fileStream = new FileStream(pathname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader((Stream) fileStream);
            int num63 = 1;
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string[] strArray = str.Split(',');
              if (strArray.Length == 4)
              {
                try
                {
                  this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num60 + int.Parse(strArray[0]), num61 + int.Parse(strArray[1]), num62 + int.Parse(strArray[2]), int.Parse(strArray[3]));
                }
                catch
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage(word + "为无效的方块文件，在第" + num63.ToString() + "行发生错误", Color.Yellow, false, false);
                  return false;
                }
              }
              ++num63;
            }
            streamReader.Close();
            fileStream.Close();
          }
          else
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("在存档目录或Command目录中找不到名为‘" + word + "’的方块文件", Color.Yellow, false, false);
            return false;
          }
        }
        else if (words[0] == "model")
        {
          if (words.Length != 3)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string lower1 = words[1].ToLower();
          string word = words[2];
          string str3 = "";
          string name = "";
          Model model = new Model();
          bool flag12 = true;
          if (word.Contains("/"))
          {
            flag12 = false;
            for (int index = 0; index < word.Length; ++index)
            {
              if (word.Substring(index, 1) == "/")
              {
                str3 = word.Substring(0, index);
                name = word.Substring(index + 1);
                break;
              }
            }
          }
          if (flag12)
          {
            try
            {
              model = ContentManager.Get<Model>("Models/" + word);
            }
            catch
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:Content/Models中不存在名为‘" + word + "’的模型", Color.Yellow, false, false);
              return false;
            }
          }
          string pathname = Datahandle.GetPathname(str3 + ".pak");
          if (File.Exists(pathname))
          {
            if (!flag12)
            {
              try
              {
                Stream stream = (Stream) new FileStream(pathname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                ContentCache.AddPackage((Func<Stream>) (() => stream), Encoding.UTF8.GetBytes(ContentManager.Pad()), new byte[1]
                {
                  (byte) 63
                });
                try
                {
                  model = ContentManager.Get<Model>(name);
                }
                catch
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:在" + str3 + ".pak中找不到名为‘" + name + "’的模型", Color.Yellow, false, false);
                  return false;
                }
                stream.Close();
              }
              catch
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + str3 + ".pak’为无效的pak文件", Color.Yellow, false, false);
                return false;
              }
            }
          }
          if (!File.Exists(pathname) && !flag12)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:存档目录或Command目录不存在‘" + str3 + ".pak’文件", Color.Yellow, false, false);
            return false;
          }
          bool flag13;
          string str4;
          string str5;
          if (lower1.Contains(":"))
          {
            flag13 = true;
            str4 = lower1.Split(':')[0];
            str5 = lower1.Split(':')[1];
          }
          else
          {
            flag13 = false;
            str4 = lower1;
            str5 = lower1;
          }
          DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
          if (str4 == "pl")
          {
            foreach (Component component16 in result)
            {
              ComponentPlayer component17 = component16.Entity.FindComponent<ComponentPlayer>();
              if (component17 != null)
              {
                try
                {
                  component17.ComponentCreatureModel.Model = model;
                }
                catch (Exception ex)
                {
                  if (modeCreative)
                    componentPlayer.ComponentGui.DisplaySmallMessage("提示:该模型与指定的实体模型不匹配,\n指定实体模型属于" + component17.ComponentCreatureModel.GetType().ToString(), Color.Yellow, false, false);
                  return false;
                }
              }
            }
            return true;
          }
          foreach (ComponentBody componentBody in result)
          {
            ComponentCreature component18 = componentBody.Entity.FindComponent<ComponentCreature>();
            ComponentBoat component19 = componentBody.Entity.FindComponent<ComponentBoat>();
            string lower2 = componentBody.Entity.ValuesDictionary.DatabaseObject.Name.ToLower();
            if (component18 != null || component19 != null)
            {
              if (lower2 == str4)
              {
                try
                {
                  if (flag13)
                  {
                    if (!(Datahandle.GetEntityid(componentBody.Entity).ToString() == str5))
                      continue;
                  }
                  if (lower2 == "boat")
                    componentBody.Entity.FindComponent<ComponentModel>().Model = model;
                  else
                    component18.ComponentCreatureModel.Model = model;
                }
                catch (Exception ex)
                {
                  if (modeCreative)
                    componentPlayer.ComponentGui.DisplaySmallMessage("提示:该模型与指定的实体模型不匹配,\n指定实体模型属于" + component18.GetType().ToString(), Color.Yellow, false, false);
                }
              }
            }
          }
        }
        else if (words[0] == "modeltexture")
        {
          if (words.Length != 3)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string lower3 = words[1].ToLower();
          string name = words[2];
          bool flag14 = false;
          Texture2D texture2D = (Texture2D) null;
          string pathname = Datahandle.GetPathname(name);
          if (!name.Contains("png") && !name.Contains("PNG"))
          {
            flag14 = true;
            if (name.Substring(0, 1) == "!")
              name = name.Substring(1);
            try
            {
              texture2D = ContentManager.Get<Texture2D>("Textures/Creatures/" + name);
            }
            catch
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:待装载的模型材质文件不存在", Color.Yellow, false, false);
              return false;
            }
          }
          if (File.Exists(pathname))
          {
            if (!flag14)
            {
              try
              {
                Stream stream = (Stream) new FileStream(pathname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                texture2D = Texture2D.Load(stream, false, 1);
                stream.Close();
              }
              catch
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的模型材质文件", Color.Yellow, false, false);
                return false;
              }
            }
          }
          if (!File.Exists(pathname) && !flag14)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:待装载的模型材质文件不存在", Color.Yellow, false, false);
            return false;
          }
          bool flag15;
          string str6;
          string str7;
          if (lower3.Contains(":"))
          {
            flag15 = true;
            str6 = lower3.Split(':')[0];
            str7 = lower3.Split(':')[1];
          }
          else
          {
            flag15 = false;
            str6 = lower3;
            str7 = lower3;
          }
          DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
          if (str6 == "pl")
          {
            foreach (Component component20 in result)
            {
              ComponentPlayer component21 = component20.Entity.FindComponent<ComponentPlayer>();
              if (component21 != null)
              {
                try
                {
                  component21.ComponentCreatureModel.TextureOverride = texture2D;
                }
                catch (Exception ex)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的模型材质文件", Color.Yellow, false, false);
                  return false;
                }
              }
            }
            return true;
          }
          foreach (ComponentBody componentBody in result)
          {
            ComponentCreature component22 = componentBody.Entity.FindComponent<ComponentCreature>();
            ComponentBoat component23 = componentBody.Entity.FindComponent<ComponentBoat>();
            string lower4 = componentBody.Entity.ValuesDictionary.DatabaseObject.Name.ToLower();
            if (component22 != null || component23 != null)
            {
              if (lower4 == str6)
              {
                try
                {
                  if (flag15)
                  {
                    if (!(Datahandle.GetEntityid(componentBody.Entity).ToString() == str7))
                      continue;
                  }
                  if (lower4 == "boat")
                    componentBody.Entity.FindComponent<ComponentModel>().TextureOverride = texture2D;
                  else
                    component22.ComponentCreatureModel.TextureOverride = texture2D;
                }
                catch (Exception ex)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:无效的模型材质文件", Color.Yellow, false, false);
                }
              }
            }
          }
        }
        else if (words[0] == "modelsize")
        {
          if (words.Length != 3 || int.Parse(words[2]) <= 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string lower5 = words[1].ToLower();
          float num64 = (float) int.Parse(words[2]) / 10f;
          bool flag;
          string str8;
          string str9;
          if (lower5.Contains(":"))
          {
            flag = true;
            str8 = lower5.Split(':')[0];
            str9 = lower5.Split(':')[1];
          }
          else
          {
            flag = false;
            str8 = lower5;
            str9 = lower5;
          }
          if (str8 != "boat")
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("提示:暂时仅支持船模型修改大小", Color.Yellow, false, false);
            return false;
          }
          DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), result);
          foreach (ComponentBody componentBody in result)
          {
            ComponentBoat component24 = componentBody.Entity.FindComponent<ComponentBoat>();
            componentBody.Entity.FindComponent<ComponentCreature>();
            string lower6 = componentBody.Entity.ValuesDictionary.DatabaseObject.Name.ToLower();
            if (component24 != null && lower6 == str8 && (!flag || Datahandle.GetEntityid(componentBody.Entity).ToString() == str9))
            {
              ComponentNewSimpleModel component25 = componentBody.Entity.FindComponent<ComponentNewSimpleModel>();
              component25.scale = num64;
              component25.Animate();
              Vector3 vector3 = new Vector3(1.45f, 0.5f, 1.45f);
              componentBody.BoxSize = vector3;
              float num65 = num64;
              if ((double) num64 < 10.0)
              {
                float num66 = num64 * 0.6f;
                float x = num66 * componentBody.BoxSize.X;
                float y = componentBody.BoxSize.Y;
                float z = num66 * componentBody.BoxSize.Z;
                componentBody.BoxSize = new Vector3(x, y, z);
              }
              else
              {
                float x = num64 * componentBody.BoxSize.X;
                float y = num64 * componentBody.BoxSize.Y;
                float z = num64 * componentBody.BoxSize.Z;
                componentBody.BoxSize = new Vector3(x, y, z);
              }
              num64 = num65;
            }
          }
        }
        else if (words[0] == "gamemode")
        {
          if (words.Length != 2 || int.Parse(words[1]) < 0 || int.Parse(words[1]) > 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num = int.Parse(words[1]);
          SubsystemGameInfo subsystemGameInfo = componentPlayer.m_subsystemGameInfo;
          switch (num)
          {
            case 0:
              subsystemGameInfo.WorldSettings.GameMode = GameMode.Creative;
              break;
            case 1:
              subsystemGameInfo.WorldSettings.GameMode = GameMode.Harmless;
              break;
            case 2:
              subsystemGameInfo.WorldSettings.GameMode = GameMode.Challenging;
              break;
            case 3:
              subsystemGameInfo.WorldSettings.GameMode = GameMode.Cruel;
              break;
            case 4:
              subsystemGameInfo.WorldSettings.GameMode = GameMode.Adventure;
              break;
          }
          WorldInfo worldInfo = GameManager.WorldInfo;
          GameManager.SaveProject(true, true);
          GameManager.DisposeProject();
          ScreensManager.SwitchScreen("GameLoading", (object) worldInfo, null);
        }
        else if (words[0] == "changetime")
        {
          if (words.Length != 2 || int.Parse(words[1]) <= 0 || int.Parse(words[1]) >= 5)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          SubsystemTimeOfDay subsystem = this.SubsystemElectricity.Project.FindSubsystem<SubsystemTimeOfDay>();
          int num67 = int.Parse(words[1]);
          float num68 = MathUtils.Remainder(MathUtils.Remainder(0.25f * (float) num67, 1f) - subsystem.TimeOfDay, 1f);
          subsystem.TimeOfDayOffset += (double) num68;
          if (modeCreative)
          {
            switch (num67)
            {
              case 1:
                componentPlayer.ComponentGui.DisplaySmallMessage("已将时间切换为日出", Color.Yellow, false, false);
                break;
              case 2:
                componentPlayer.ComponentGui.DisplaySmallMessage("已将时间切换为中午", Color.Yellow, false, false);
                break;
              case 3:
                componentPlayer.ComponentGui.DisplaySmallMessage("已将时间切换为日落", Color.Yellow, false, false);
                break;
              case 4:
                componentPlayer.ComponentGui.DisplaySmallMessage("已将时间切换为午夜", Color.Yellow, false, false);
                break;
            }
          }
        }
        else if (words[0] == "capturephoto")
          ScreenCaptureManager.CapturePhoto((Action) (() =>
          {
            if (!modeCreative)
              return;
            componentPlayer.ComponentGui.DisplaySmallMessage("图片已储存到图库", Color.Yellow, false, false);
          }), (Action<Exception>) (_param1 => componentPlayer.ComponentGui.DisplaySmallMessage("提示：图片截取失败", Color.Yellow, false, false)));
        else if (words[0] == "web")
        {
          if (words.Length != 2)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          WebBrowserManager.LaunchBrowser(words[1]);
        }
        else if (words[0] == "settings")
        {
          if (words.Length != 3)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          string word = words[1];
          switch (word)
          {
            case "brightness":
              float num69 = (float) int.Parse(words[2]) / 10f;
              if ((double) num69 > 1.0)
                num69 = 1f;
              SettingsManager.Brightness = num69;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将黑暗区域亮度设置为:" + int.Parse(words[1]).ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            case "looksensitivity":
              int num70 = int.Parse(words[2]);
              if (num70 > 10)
                num70 = 10;
              SettingsManager.LookSensitivity = (float) num70 / 10f;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将屏幕观看灵敏度设置为:" + num70.ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            case "movesensitivity":
              int num71 = int.Parse(words[2]);
              if (num71 > 10)
                num71 = 10;
              SettingsManager.MoveSensitivity = (float) num71 / 10f;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将移动灵敏度设置为:" + num71.ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            case "musicvolume":
              int num72 = int.Parse(words[2]);
              if (num72 > 10)
                num72 = 10;
              SettingsManager.MusicVolume = (float) num72 / 10f;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将背景音乐音量设置为:" + num72.ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            case "skyrendering":
              int num73 = int.Parse(words[2]);
              switch (num73)
              {
                case 1:
                  SettingsManager.SkyRenderingMode = SkyRenderingMode.Disabled;
                  break;
                case 2:
                  SettingsManager.SkyRenderingMode = SkyRenderingMode.Full;
                  break;
                case 3:
                  SettingsManager.SkyRenderingMode = SkyRenderingMode.NoClouds;
                  break;
              }
              if (modeCreative)
              {
                switch (num73)
                {
                  case 1:
                    componentPlayer.ComponentGui.DisplaySmallMessage("已将天空渲染改为禁用", Color.Yellow, false, false);
                    break;
                  case 2:
                    componentPlayer.ComponentGui.DisplaySmallMessage("已将天空渲染改为全部", Color.Yellow, false, false);
                    break;
                  case 3:
                    componentPlayer.ComponentGui.DisplaySmallMessage("已将天空渲染改为无云", Color.Yellow, false, false);
                    break;
                }
              }
              else
                break;
              break;
            case "soundsvolume":
              int num74 = int.Parse(words[2]);
              if (num74 > 10)
                num74 = 10;
              SettingsManager.SoundsVolume = (float) num74 / 10f;
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将游戏音量设置为:" + num74.ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            case "visibility":
              SettingsManager.VisibilityRange = int.Parse(words[2]);
              if (modeCreative)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("已将游戏视距设置为:" + int.Parse(words[1]).ToString(), Color.Yellow, false, false);
                break;
              }
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:settings指令不存在属性关键字：" + word, Color.Yellow, false, false);
              break;
          }
        }
        else if (words[0] == "blockdata")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          this.ChangeBlockdata(words, componentPlayer);
        }
        else if (words[0] == "clothesdata")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          this.ChangeClothesdata(words, componentPlayer);
        }
        else if (words[0] == "creaturedata")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          DynamicArray<ComponentBody> dynamicArray = new DynamicArray<ComponentBody>();
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) position.X, (float) position.Z) - new Vector2(64f), new Vector2((float) position.X, (float) position.Z) + new Vector2(64f), dynamicArray);
          this.ChangeCreaturedata(words, componentPlayer, dynamicArray);
        }
        else if (words[0] == "exit")
        {
          if (words.Length != 1)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          GameManager.SaveProject(true, true);
          GameManager.DisposeProject();
          Environment.Exit(0);
        }
        else if (!(words[0] == "test"))
          return this.ErrorTips(componentPlayer, condition, words[0], "empty");
      }
      catch
      {
        return this.ErrorTips(componentPlayer, condition, words[0], "warn");
      }
label_1185:
      return true;
    }

    public bool Conditionjudge(string[] words, ComponentPlayer componentPlayer, Point3 position)
    {
      bool flag1 = this.coordMode != 0;
      if (componentPlayer.m_subsystemGameInfo == null)
        return false;
      bool flag2 = componentPlayer.m_subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative;
      bool condition = true;
      try
      {
        if (words[0] == "place?")
        {
          if (words.Length != 5)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (flag1 ? position.X : 0);
          int y = int.Parse(words[2]) + (flag1 ? position.Y : 0);
          int z = int.Parse(words[3]) + (flag1 ? position.Z : 0);
          if (int.Parse(words[4]) == Terrain.ReplaceLight(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(x, y, z), 0))
            return true;
        }
        else if (words[0] == "places?")
        {
          if (words.Length != 9 || !(words[4] == "to"))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (flag1 ? position.X : 0);
          int y1 = int.Parse(words[2]) + (flag1 ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (flag1 ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (flag1 ? position.X : 0);
          int y2 = int.Parse(words[6]) + (flag1 ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (flag1 ? position.Z : 0);
          int num1 = int.Parse(words[8]);
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          for (int index1 = 0; index1 < cubeArea.LengthX; ++index1)
          {
            for (int index2 = 0; index2 < cubeArea.LengthY; ++index2)
            {
              for (int index3 = 0; index3 < cubeArea.LengthZ; ++index3)
              {
                int num2 = Terrain.ReplaceLight(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(cubeArea.PointMin.X + index1, cubeArea.PointMin.Y + index2, cubeArea.PointMin.Z + index3), 0);
                if (num1 == num2)
                  return true;
              }
            }
          }
        }
        else if (words[0] == "dig?")
        {
          if (words.Length != 2 || int.Parse(words[1]) < 0)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int num = int.Parse(words[1]);
          PlayerInput playerInput = componentPlayer.ComponentInput.PlayerInput;
          if (playerInput.Dig.HasValue)
          {
            ComponentMiner componentMiner = componentPlayer.ComponentMiner;
            playerInput = componentPlayer.ComponentInput.PlayerInput;
            Ray3 ray = playerInput.Dig.Value;
            TerrainRaycastResult? nullable = componentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Digging);
            if (nullable.HasValue)
            {
              CellFace cellFace = nullable.Value.CellFace;
              if (Terrain.ReplaceLight(this.SubsystemElectricity.SubsystemTerrain.Terrain.GetCellValue(cellFace.X, cellFace.Y, cellFace.Z), 0) == num)
                return true;
            }
          }
        }
        else if (words[0] == "blockchange?")
        {
          if (words.Length != 4)
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x = int.Parse(words[1]) + (flag1 ? position.X : 0);
          int y = int.Parse(words[2]) + (flag1 ? position.Y : 0);
          int z = int.Parse(words[3]) + (flag1 ? position.Z : 0);
          int num3 = Terrain.ReplaceLight(this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().Terrain.GetCellValue(x, y, z), 0);
          if (!this.m_lastMessageValue.HasValue)
            this.m_lastMessageValue = new int?(num3);
          int num4 = num3;
          int? lastMessageValue = this.m_lastMessageValue;
          int valueOrDefault = lastMessageValue.GetValueOrDefault();
          if (!(num4 == valueOrDefault & lastMessageValue.HasValue))
          {
            this.m_lastMessageValue = new int?(num3);
            return true;
          }
        }
        else if (words[0] == "dropsexist?")
        {
          if (words.Length != 5 && (words.Length != 9 || !(words[4] == "to")))
            return this.ErrorTips(componentPlayer, condition, words[0], "limit");
          int x1 = int.Parse(words[1]) + (flag1 ? position.X : 0);
          int y1 = int.Parse(words[2]) + (flag1 ? position.Y : 0);
          int z1 = int.Parse(words[3]) + (flag1 ? position.Z : 0);
          int x2 = int.Parse(words[5]) + (flag1 ? position.X : 0);
          int y2 = int.Parse(words[6]) + (flag1 ? position.Y : 0);
          int z2 = int.Parse(words[7]) + (flag1 ? position.Z : 0);
          int num = int.Parse(words[8]);
          List<Vector3> vector3List = new List<Vector3>();
          CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
          foreach (Pickable pickable in this.SubsystemElectricity.Project.FindSubsystem<SubsystemPickables>().Pickables)
          {
            if (pickable.Value == num)
            {
              Vector3 position1 = pickable.Position;
              if ((double) position1.X >= (double) cubeArea.PointMin.X && (double) position1.Y >= (double) cubeArea.PointMin.Y && (double) position1.Z >= (double) cubeArea.PointMin.Z && (double) position1.X <= (double) cubeArea.PointMax.X && (double) position1.Y <= (double) cubeArea.PointMax.Y && (double) position1.Z <= (double) cubeArea.PointMax.Z)
                return true;
            }
          }
        }
        else
        {
          if (words[0] == "entityexist?")
          {
            if (words.Length != 9 || !(words[4] == "to"))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            CubeArea cubeArea = CubeAreaManager.SetArea(int.Parse(words[1]) + (flag1 ? position.X : 0), int.Parse(words[2]) + (flag1 ? position.Y : 0), int.Parse(words[3]) + (flag1 ? position.Z : 0), int.Parse(words[5]) + (flag1 ? position.X : 0), int.Parse(words[6]) + (flag1 ? position.Y : 0), int.Parse(words[7]) + (flag1 ? position.Z : 0));
            if (words[8].ToLower() == "pl")
            {
              Point3 point3 = Datahandle.Coordbodyhandle(componentPlayer.ComponentBody.Position);
              return point3.X >= cubeArea.PointMin.X && point3.Y >= cubeArea.PointMin.Y && point3.Z >= cubeArea.PointMin.Z && point3.X <= cubeArea.PointMax.X && point3.Y <= cubeArea.PointMax.Y && point3.Z <= cubeArea.PointMax.Z;
            }
            foreach (ComponentFrame componentFrame in new List<ComponentBody>())
            {
              Point3 point3 = Datahandle.Coordbodyhandle(componentFrame.Position);
              if (point3.X >= cubeArea.PointMin.X && point3.Y >= cubeArea.PointMin.Y && point3.Z >= cubeArea.PointMin.Z && point3.X <= cubeArea.PointMax.X && point3.Y <= cubeArea.PointMax.Y && point3.Z <= cubeArea.PointMax.Z)
                return true;
            }
            return false;
          }
          if (words[0] == "itemsexist?")
          {
            if (words.Length != 2 && (words.Length != 4 || int.Parse(words[3]) <= 0 || int.Parse(words[1]) < 0))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            ComponentCraftingTable component1 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentCraftingTable>();
            ComponentInventoryBase component2 = componentPlayer.ComponentMiner.ComponentCreature.ComponentBody.Entity.FindComponent<ComponentInventoryBase>();
            if (words.Length == 4)
            {
              int index4 = int.Parse(words[1]);
              int num5 = int.Parse(words[2]);
              int num6 = int.Parse(words[3]);
              if (index4 < 26)
              {
                if (component2.m_slots[index4].Value == num5 && component2.m_slots[index4].Count == num6)
                  return true;
              }
              else if (index4 < 32)
              {
                int index5 = index4 - 26;
                if (component1.m_slots[index5].Value == num5 && component1.m_slots[index5].Count == num6)
                  return true;
              }
            }
            else
            {
              int num = int.Parse(words[1]);
              for (int index = 0; index < component2.m_slots.Count; ++index)
              {
                if (component2.m_slots[index].Value == num && component2.m_slots[index].Count >= 1)
                  return true;
              }
              for (int index = 0; index < component1.m_slots.Count; ++index)
              {
                if (component1.m_slots[index].Value == num && component1.m_slots[index].Count >= 1)
                  return true;
              }
            }
          }
          else if (words[0] == "chestitemsexist?")
          {
            if (words.Length != 5 && (words.Length != 7 || int.Parse(words[4]) < 0 || int.Parse(words[6]) <= 0))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            int x = int.Parse(words[1]) + (flag1 ? position.X : 0);
            int y = int.Parse(words[2]) + (flag1 ? position.Y : 0);
            int z = int.Parse(words[3]) + (flag1 ? position.Z : 0);
            bool flag3 = words.Length == 7;
            ComponentChest component = this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity(x, y, z).Entity.FindComponent<ComponentChest>(true);
            if (component == null)
              return false;
            if (flag3)
            {
              int index = int.Parse(words[4]);
              int num7 = int.Parse(words[5]);
              int num8 = int.Parse(words[6]);
              if (index < 16 && component.m_slots[index].Value == num7 && component.m_slots[index].Count == num8)
                return true;
            }
            else
            {
              int num = int.Parse(words[4]);
              for (int index = 0; index < component.m_slots.Count; ++index)
              {
                if (component.m_slots[index].Value == num && component.m_slots[index].Count >= 1)
                  return true;
              }
            }
          }
          else if (words[0] == "handitemsexist?")
          {
            if (words.Length != 2 && (words.Length != 3 || int.Parse(words[2]) <= 0))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            int slotValue = componentPlayer.ComponentMiner.Inventory.GetSlotValue(componentPlayer.ComponentMiner.Inventory.ActiveSlotIndex);
            int slotCount = componentPlayer.ComponentMiner.Inventory.GetSlotCount(componentPlayer.ComponentMiner.Inventory.ActiveSlotIndex);
            if (words.Length == 3)
            {
              int num9 = int.Parse(words[1]);
              int num10 = int.Parse(words[2]);
              if (slotValue == num9 && slotCount == num10)
                return true;
            }
            else
            {
              int num = int.Parse(words[1]);
              if (slotValue == num && slotCount >= 1)
                return true;
            }
          }
          else if (words[0] == "eyesangle?")
          {
            if (words.Length != 8 || !(words[2] == "to") || !(words[4] == "and") || !(words[6] == "to"))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            int num11 = int.Parse(words[1]);
            int num12 = int.Parse(words[3]);
            int num13 = int.Parse(words[5]);
            int num14 = int.Parse(words[7]);
            if (num11 > num12)
            {
              int num15 = num12;
              num12 = num11;
              num11 = num15;
            }
            if (num13 > num14)
            {
              int num16 = num14;
              num14 = num13;
              num13 = num16;
            }
            Vector3 viewDirection = componentPlayer.GameWidget.ActiveCamera.ViewDirection;
            int angle1 = Datahandle.GetAngle(viewDirection, "vertical");
            int angle2 = Datahandle.GetAngle(viewDirection, "horizontal");
            if (angle1 >= num11 && angle1 <= num12 && angle2 >= num13 && angle2 <= num14)
              return true;
          }
          else if (words[0] == "playerheight?")
          {
            if (words.Length != 4 || !(words[2] == "to"))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            int num17 = int.Parse(words[1]);
            int num18 = int.Parse(words[3]);
            if (num17 > num18)
            {
              int num19 = num18;
              num18 = num17;
              num17 = num19;
            }
            Point3 point3 = Datahandle.Coordbodyhandle(componentPlayer.ComponentBody.Position);
            if (point3.Y >= num17 && point3.Y <= num18)
              return true;
          }
          else if (words[0] == "playerstats?")
          {
            if (words.Length != 5 || !(words[3] == "to"))
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            string word = words[1];
            int num20 = int.Parse(words[2]);
            int num21 = int.Parse(words[4]);
            if (num20 > num21)
            {
              int num22 = num21;
              num21 = num20;
              num20 = num22;
            }
            switch (word)
            {
              case "attack":
                if ((int) componentPlayer.ComponentMiner.AttackPower >= num20 && (int) componentPlayer.ComponentMiner.AttackPower <= num21)
                  return true;
                break;
              case "defense":
                if ((int) componentPlayer.ComponentHealth.AttackResilience >= num20 && (int) componentPlayer.ComponentHealth.AttackResilience <= num21)
                  return true;
                break;
              case "endurance":
                if ((int) ((double) componentPlayer.ComponentVitalStats.Stamina * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentVitalStats.Stamina * 10.0) <= num21)
                  return true;
                break;
              case "fatigue":
                if ((int) ((double) componentPlayer.ComponentVitalStats.Sleep * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentVitalStats.Sleep * 10.0) <= num21)
                  return true;
                break;
              case "health":
                if ((int) ((double) componentPlayer.ComponentHealth.Health * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentHealth.Health * 10.0) <= num21)
                  return true;
                break;
              case "hunger":
                if ((int) ((double) componentPlayer.ComponentVitalStats.Food * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentVitalStats.Food * 10.0) <= num21)
                  return true;
                break;
              case "level":
                if ((int) componentPlayer.PlayerData.Level >= num20 && (int) componentPlayer.PlayerData.Level <= num21)
                  return true;
                break;
              case "speed":
                if ((int) ((double) componentPlayer.ComponentLocomotion.WalkSpeed * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentLocomotion.WalkSpeed * 10.0) <= num21)
                  return true;
                break;
              case "temperature":
                if ((int) componentPlayer.ComponentVitalStats.Temperature >= num20 && (int) componentPlayer.ComponentVitalStats.Temperature <= num21)
                  return true;
                break;
              case "wetness":
                if ((int) ((double) componentPlayer.ComponentVitalStats.Wetness * 10.0) >= num20 && (int) ((double) componentPlayer.ComponentVitalStats.Wetness * 10.0) <= num21)
                  return true;
                break;
            }
          }
          else if (words[0] == "playeraction?")
          {
            if (words.Length != 2)
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            switch (words[1])
            {
              case "aim":
                if (componentPlayer.ComponentInput.PlayerInput.Aim.HasValue)
                  return true;
                break;
              case "climb":
                if (componentPlayer.ComponentLocomotion.m_climbing)
                  return true;
                break;
              case "fall":
                if (componentPlayer.ComponentLocomotion.m_falling)
                  return true;
                break;
              case "fly":
                if (componentPlayer.ComponentLocomotion.m_flying)
                  return true;
                break;
              case "hasflu":
                if (componentPlayer.ComponentFlu.HasFlu)
                  return true;
                break;
              case "hit":
                if (componentPlayer.ComponentInput.PlayerInput.Hit.HasValue)
                  return true;
                break;
              case "interact":
                if (componentPlayer.ComponentInput.PlayerInput.Interact.HasValue)
                  return true;
                break;
              case "jump":
                if (componentPlayer.ComponentInput.PlayerInput.Jump)
                  return true;
                break;
              case "move":
                if ((double) componentPlayer.ComponentBody.m_velocity.LengthSquared() > 1.0 / 16.0)
                  return true;
                break;
              case "rider":
                if (componentPlayer.ComponentRider.Mount != null)
                  return true;
                break;
              case "sick":
                if (componentPlayer.ComponentSickness.IsSick)
                  return true;
                break;
              case "sleep":
                if (componentPlayer.ComponentSleep.IsSleeping)
                  return true;
                break;
              case "sneak":
                if (componentPlayer.ComponentBody.IsSneaking)
                  return true;
                break;
              case "swim":
                if (componentPlayer.ComponentLocomotion.m_swimming)
                  return true;
                break;
              case "walk":
                if ((double) componentPlayer.ComponentBody.m_velocity.LengthSquared() > 1.0 / 16.0 && componentPlayer.ComponentLocomotion.m_walking)
                  return true;
                break;
            }
          }
          else if (words[0] == "playerinput?")
          {
            if (words.Length != 2)
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            switch (words[1])
            {
              case "lookdown":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Look.Y < 0.0)
                {
                  float num = MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.X);
                  if ((int) ((double) MathUtils.Atan(MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.Y) / num) / 3.14 * 180.0) > 30)
                    return true;
                  break;
                }
                break;
              case "lookleft":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Look.X < 0.0)
                {
                  float num = MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.X);
                  if ((int) ((double) MathUtils.Atan(MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.Y) / num) / 3.14 * 180.0) < 60)
                    return true;
                  break;
                }
                break;
              case "lookright":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Look.X > 0.0)
                {
                  float num = MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.X);
                  if ((int) ((double) MathUtils.Atan(MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.Y) / num) / 3.14 * 180.0) < 60)
                    return true;
                  break;
                }
                break;
              case "lookup":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Look.Y > 0.0)
                {
                  float num = MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.X);
                  if ((int) ((double) MathUtils.Atan(MathUtils.Abs(componentPlayer.ComponentInput.PlayerInput.Look.Y) / num) / 3.14 * 180.0) > 30)
                    return true;
                  break;
                }
                break;
              case "movedown":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Move.Z < 0.0)
                  return true;
                break;
              case "moveleft":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Move.X < 0.0)
                  return true;
                break;
              case "moveright":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Move.X > 0.0)
                  return true;
                break;
              case "moveup":
                if ((double) componentPlayer.ComponentInput.PlayerInput.Move.Z > 0.0)
                  return true;
                break;
            }
          }
          else if (words[0] == "widget?")
          {
            if (words.Length != 2)
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            switch (words[1].ToLower())
            {
              case "chest":
                if (componentPlayer.ComponentGui.ModalPanelWidget is ChestWidget)
                  return true;
                break;
              case "clothing":
                if (componentPlayer.ComponentGui.ModalPanelWidget is ClothingWidget)
                  return true;
                break;
              case "dispenser":
                if (componentPlayer.ComponentGui.ModalPanelWidget is DispenserWidget)
                  return true;
                break;
              case "furnace":
                if (componentPlayer.ComponentGui.ModalPanelWidget is FurnaceWidget)
                  return true;
                break;
              case "inventory":
                if (componentPlayer.ComponentGui.ModalPanelWidget is CreativeInventoryWidget || componentPlayer.ComponentGui.ModalPanelWidget is FullInventoryWidget)
                  return true;
                break;
              case "none":
                if (componentPlayer.ComponentGui.ModalPanelWidget == null)
                  return true;
                break;
              case "stats":
                if (componentPlayer.ComponentGui.ModalPanelWidget is VitalStatsWidget)
                  return true;
                break;
              case "table":
                if (componentPlayer.ComponentGui.ModalPanelWidget is CraftingTableWidget)
                  return true;
                break;
            }
          }
          else
          {
            if (!(words[0] == "gamemode?"))
              return this.ErrorTips(componentPlayer, condition, words[0], "empty");
            if (words.Length != 2 || int.Parse(words[1]) < 0 || int.Parse(words[1]) > 4)
              return this.ErrorTips(componentPlayer, condition, words[0], "limit");
            int num = int.Parse(words[1]);
            if (componentPlayer.m_subsystemGameInfo.WorldSettings.GameMode == (GameMode) num)
              return true;
          }
        }
        return false;
      }
      catch
      {
        return this.ErrorTips(componentPlayer, condition, words[0], "warn");
      }
    }

    public override bool Simulate()
    {
      Point3 point3_1;
      CellFace cellFace = this.CellFaces[0];
      int x = cellFace.Point.X;
      cellFace = this.CellFaces[0];
      int y = cellFace.Point.Y;
      cellFace = this.CellFaces[0];
      int z = cellFace.Point.Z;
      Point3 local = new Point3(x, y, z);
      point3_1 = local;
      CommandData commandData = this.SubsystemElectricity.Project.FindSubsystem<SubsystemCommandBlockBehavior>(true).GetCommandData(point3_1);
      int functionValue = this.SubsystemElectricity.Project.FindSubsystem<SubsystemDebugMode>().functionValue;
      GameMode gameMode = this.SubsystemElectricity.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings.GameMode;
      ComponentPlayer componentPlayer1 = new ComponentPlayer();
      List<Point3> positionlist = new List<Point3>();
      foreach (ComponentPlayer componentPlayer2 in this.SubsystemElectricity.Project.FindSubsystem<SubsystemPlayers>(true).ComponentPlayers)
      {
        if (componentPlayer2 != null)
        {
          componentPlayer1 = componentPlayer2;
          break;
        }
      }
      if (componentPlayer1 == null || commandData == null || commandData.Lines == "")
        return false;
      char[] variable = new char[4]{ 'X', 'Y', 'V', 'W' };
      int[] signals = this.Getsignals();
      int num1 = signals[4];
      bool flag1 = false;
      bool flag2 = false;
      string[] words = commandData.Lines.Split(' ');
      foreach (string str in words)
      {
        if (str == "")
          return false;
      }
      words[0] = words[0].ToLower();
      bool flag3 = words[0].Contains("?");
      if (words[0].Substring(0, 1) == "$")
      {
        flag1 = true;
        words[0] = words[0].Substring(1);
      }
      bool flag4 = SubsystemDebugMode.GetFunctionSwitch(functionValue) == 1;
      if (words[0] != "help" && !flag4)
        return false;
      if (gameMode == GameMode.Challenging || gameMode == GameMode.Cruel)
      {
        string[] strArray = new string[29]
        {
          "key",
          "help",
          "about",
          "debugmode",
          "message",
          "largemessage",
          "tp",
          "rain",
          "stoprain",
          "clickbutton",
          "spawn",
          "world",
          "audio",
          "getterrain",
          "world",
          "getid",
          "getcell",
          "write",
          "read",
          "worldtexture",
          "modeltexture",
          "model",
          "web",
          "systemvolume",
          "sensitivity",
          "visibility",
          "brightness",
          "skyrendering",
          "exit"
        };
        foreach (string str in strArray)
        {
          if (str == words[0])
          {
            flag2 = true;
            break;
          }
        }
        if (!(SubsystemDebugMode.GetFunctionSurvive(functionValue) == 1 | flag2 | flag3))
        {
          this.ErrorTips(componentPlayer1, true, words[0], "lock");
          return false;
        }
      }
      this.coordMode = 0;
      if (words.Length > 1 && words[words.Length - 1].Substring(0, 1) == "@")
      {
        string str = words[words.Length - 1];
        if (str.ToLower() == "@c")
          this.coordMode = 1;
        else if (str.ToLower() == "@pl")
        {
          this.coordMode = 2;
          point3_1 = Datahandle.Coordbodyhandle(componentPlayer1.ComponentBody.Position);
        }
        else
        {
          this.coordMode = 3;
          foreach (ComponentBody componentBody in this.GetComponentBodies(str.Substring(1), point3_1))
          {
            if (componentBody.Entity.FindComponent<ComponentCreature>() != null)
            {
              Point3 point3_2 = Datahandle.Coordbodyhandle(componentBody.Position);
              positionlist.Add(point3_2);
            }
          }
        }
        string[] strArray = new string[words.Length - 1];
        for (int index = 0; index < strArray.Length; ++index)
          strArray[index] = words[index];
        words = strArray;
      }
      if (words[0] == "blockdata" || words[0] == "creaturedata" || words[0] == "clothesdata")
        words[1] = words[1].ToLower();
      float num2 = 0.3f;
      if (words[0] == "playerinput?")
        num2 = 0.01f;
      if (words[0] == "playeraction?" && (words[1] == "jump" || words[1] == "hit" || words[1] == "interact"))
        num2 = 1E-05f;
      if (flag3)
      {
        this.SubsystemElectricity.QueueElectricElementForSimulation((ElectricElement) this, this.SubsystemElectricity.CircuitStep + 1);
        if (!this.m_lastMessageTime.HasValue || this.SubsystemElectricity.SubsystemTime.GameTime - this.m_lastMessageTime.Value > (double) num2)
        {
          if (flag1)
          {
            if (this.Getclocktype())
            {
              if (num1 >= 8 && this.clockAllowed)
              {
                this.clockAllowed = false;
                this.m_voltage = !this.Command(Datahandle.GetReplacewords(words, variable, signals), componentPlayer1, point3_1, positionlist, true) ? 0.0f : 1f;
                this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
                return true;
              }
              if (num1 < 8)
                this.clockAllowed = true;
            }
            else
            {
              this.m_voltage = !this.Command(Datahandle.GetReplacewords(words, variable, signals), componentPlayer1, point3_1, positionlist, true) ? 0.0f : 1f;
              this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
              return true;
            }
          }
          else
          {
            this.m_voltage = !this.Command(words, componentPlayer1, point3_1, positionlist, true) ? 0.0f : 1f;
            this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
            return true;
          }
        }
        return false;
      }
      if (flag1)
      {
        if (this.Getclocktype())
        {
          if (num1 >= 8 && this.clockAllowed)
          {
            this.clockAllowed = false;
            this.Command(Datahandle.GetReplacewords(words, variable, signals), componentPlayer1, point3_1, positionlist, false);
          }
          if (num1 < 8)
            this.clockAllowed = true;
        }
        else if (signals[0] > 0 || signals[1] > 0 || signals[2] > 0 || signals[3] > 0)
          this.Command(Datahandle.GetReplacewords(words, variable, signals), componentPlayer1, point3_1, positionlist, false);
      }
      else if (this.CalculateHighInputsCount() > 0)
        this.Command(words, componentPlayer1, point3_1, positionlist, false);
      return false;
    }

    public bool Command(
      string[] words,
      ComponentPlayer componentPlayer,
      Point3 position,
      List<Point3> positionlist,
      bool condition)
    {
      bool flag = false;
      if (condition)
      {
        switch (this.coordMode)
        {
          case 0:
            flag = this.Conditionjudge(words, componentPlayer, position);
            break;
          case 1:
            flag = this.Conditionjudge(words, componentPlayer, position);
            break;
          case 2:
            flag = this.Conditionjudge(words, componentPlayer, position);
            break;
          case 3:
            if (positionlist.Count > 0)
            {
              using (List<Point3>.Enumerator enumerator = positionlist.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Point3 current = enumerator.Current;
                  flag = this.Conditionjudge(words, componentPlayer, current);
                }
                break;
              }
            }
            else
              break;
        }
      }
      else
      {
        switch (this.coordMode)
        {
          case 0:
            this.Instruction(words, componentPlayer, position);
            break;
          case 1:
            this.Instruction(words, componentPlayer, position);
            break;
          case 2:
            this.Instruction(words, componentPlayer, position);
            break;
          case 3:
            if (positionlist.Count > 0)
            {
              using (List<Point3>.Enumerator enumerator = positionlist.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Point3 current = enumerator.Current;
                  this.Instruction(words, componentPlayer, current);
                }
                break;
              }
            }
            else
              break;
        }
      }
      return flag;
    }

    public int[] Getsignals()
    {
      int[] numArray = new int[6];
      foreach (ElectricConnection connection in this.Connections)
      {
        if (connection.ConnectorType != ElectricConnectorType.Output && connection.NeighborConnectorType != ElectricConnectorType.Input)
        {
          switch (connection.NeighborConnectorFace)
          {
            case 0:
              numArray[0] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(0) * 15f);
              continue;
            case 1:
              numArray[1] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(1) * 15f);
              continue;
            case 2:
              numArray[2] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(2) * 15f);
              continue;
            case 3:
              numArray[3] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(3) * 15f);
              continue;
            case 4:
              numArray[4] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(4) * 15f);
              continue;
            case 5:
              numArray[5] = (int) MathUtils.Round(connection.NeighborElectricElement.GetOutputVoltage(5) * 15f);
              continue;
            default:
              continue;
          }
        }
      }
      return numArray;
    }

    public bool Getclocktype()
    {
      bool flag = false;
      foreach (ElectricConnection connection in this.Connections)
      {
        if (connection.ConnectorType != ElectricConnectorType.Output && connection.NeighborConnectorType != ElectricConnectorType.Input)
        {
          ElectricConnectorDirection? connectorDirection1 = SubsystemElectricity.GetConnectorDirection(this.CellFaces[0].Face, 0, connection.ConnectorFace);
          if (connectorDirection1.HasValue)
          {
            ElectricConnectorDirection? nullable = connectorDirection1;
            ElectricConnectorDirection connectorDirection2 = ElectricConnectorDirection.Bottom;
            if (nullable.GetValueOrDefault() == connectorDirection2 & nullable.HasValue)
              flag = true;
          }
        }
      }
      return flag;
    }

    public List<ComponentBody> GetComponentBodies(
      string entityName,
      Point3 entityPosition)
    {
      string lower1 = entityName.ToLower();
      Point3 point3 = entityPosition;
      bool flag;
      string str1;
      string str2;
      if (lower1.Contains(":"))
      {
        flag = true;
        str1 = lower1.Split(':')[0];
        str2 = lower1.Split(':')[1];
      }
      else
      {
        flag = false;
        str1 = lower1;
        str2 = lower1;
      }
      List<ComponentBody> componentBodies = new List<ComponentBody>();
      DynamicArray<ComponentBody> result = new DynamicArray<ComponentBody>();
      this.SubsystemElectricity.Project.FindSubsystem<SubsystemBodies>().FindBodiesInArea(new Vector2((float) point3.X, (float) point3.Z) - new Vector2(64f), new Vector2((float) point3.X, (float) point3.Z) + new Vector2(64f), result);
      foreach (ComponentBody componentBody in result)
      {
        ComponentCreature component1 = componentBody.Entity.FindComponent<ComponentCreature>();
        ComponentBoat component2 = componentBody.Entity.FindComponent<ComponentBoat>();
        string lower2 = componentBody.Entity.ValuesDictionary.DatabaseObject.Name.ToLower();
        int entityid;
        if (component1 != null && lower2 == str1)
        {
          if (flag)
          {
            entityid = Datahandle.GetEntityid(componentBody.Entity);
            if (!(entityid.ToString() == str2))
              goto label_9;
          }
          componentBodies.Add(componentBody);
        }
label_9:
        if (component2 != null && lower2 == str1)
        {
          if (flag)
          {
            entityid = Datahandle.GetEntityid(componentBody.Entity);
            if (!(entityid.ToString() == str2))
              continue;
          }
          componentBodies.Add(componentBody);
        }
      }
      return componentBodies;
    }

    public bool ErrorTips(
      ComponentPlayer componentPlayer,
      bool condition,
      string str,
      string type)
    {
      if (condition)
      {
        if (this.m_lastMessageTime.HasValue && this.SubsystemElectricity.SubsystemTime.GameTime - this.m_lastMessageTime.Value < 2.0)
          return false;
        this.m_lastMessageTime = new double?(this.SubsystemElectricity.SubsystemTime.GameTime);
      }
      Point3 point3 = new Point3(this.CellFaces[0].Point.X, this.CellFaces[0].Point.Y, this.CellFaces[0].Point.Z);
      string str1 = string.Format(";\nCommandError At:({0},{1},{2})", (object) point3.X, (object) point3.Y, (object) point3.Z);
      if (!(type == "warn"))
      {
        if (!(type == "empty"))
        {
          if (!(type == "limit"))
          {
            if (type == "lock")
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:挑战或残酷模式不支持" + str + "指令" + str1, Color.Yellow, false, false);
          }
          else
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:输入的" + str + "指令格式有误，请核对指令列表" + str1, Color.Yellow, false, false);
        }
        else
          componentPlayer.ComponentGui.DisplaySmallMessage("错误:输入的" + str + "指令不存在，请核对指令列表" + str1, Color.Yellow, false, false);
      }
      else
        componentPlayer.ComponentGui.DisplaySmallMessage("错误:输入的" + str + "指令带有非法字符，请修整输入的值" + str1, Color.Yellow, false, false);
      return false;
    }

    public bool BlocksPlace(
      string[] words,
      ComponentPlayer componentPlayer,
      Point3 position,
      bool condition,
      bool coordRelative)
    {
      if (words[0] == "cube")
      {
        if (words.Length != 9 || !(words[4] == "to"))
          return this.ErrorTips(componentPlayer, condition, "places" + words[0], "limit");
        int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
        int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
        int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
        int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
        int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
        int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
        int num = int.Parse(words[8]);
        CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
        for (int index1 = 0; index1 < cubeArea.LengthX; ++index1)
        {
          for (int index2 = 0; index2 < cubeArea.LengthY; ++index2)
          {
            for (int index3 = 0; index3 < cubeArea.LengthZ; ++index3)
              this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(cubeArea.PointMin.X + index1, cubeArea.PointMin.Y + index2, cubeArea.PointMin.Z + index3, num);
          }
        }
      }
      else if (words[0] == "line")
      {
        if (words.Length != 9 || !(words[4] == "to"))
          return this.ErrorTips(componentPlayer, condition, "places" + words[0], "limit");
        int x1 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
        int y1 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
        int z1 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
        int x2 = int.Parse(words[5]) + (coordRelative ? position.X : 0);
        int y2 = int.Parse(words[6]) + (coordRelative ? position.Y : 0);
        int z2 = int.Parse(words[7]) + (coordRelative ? position.Z : 0);
        int num1 = int.Parse(words[8]);
        CubeArea cubeArea = CubeAreaManager.SetArea(x1, y1, z1, x2, y2, z2);
        int num2 = Math.Max(Math.Max(cubeArea.LengthX, cubeArea.LengthY), cubeArea.LengthZ);
        for (int index = 0; index <= num2; ++index)
          this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(x1 + (int) Math.Round((double) index / (double) num2 * (double) (x2 - x1)), y1 + (int) Math.Round((double) index / (double) num2 * (double) (y2 - y1)), z1 + (int) Math.Round((double) index / (double) num2 * (double) (z2 - z1)), num1);
      }
      else if (words[0] == "sphere")
      {
        if (words.Length != 6 || int.Parse(words[4]) <= 0)
          return this.ErrorTips(componentPlayer, condition, "places" + words[0], "limit");
        int num3 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
        int num4 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
        int num5 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
        int num6 = int.Parse(words[4]);
        int num7 = int.Parse(words[5]);
        for (int index4 = -num6 + 1; index4 < num6; ++index4)
        {
          for (int index5 = -num6 + 1; index5 < num6; ++index5)
          {
            for (int index6 = -num6 + 1; index6 < num6; ++index6)
            {
              if (index4 * index4 + index5 * index5 + index6 * index6 <= num6 * num6)
                this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num3 + index4, num4 + index5, num5 + index6, num7);
            }
          }
        }
      }
      else if (words[0] == "column")
      {
        if (words.Length != 8 || int.Parse(words[6]) <= 0 || int.Parse(words[6]) >= 4 || int.Parse(words[5]) <= 0)
          return this.ErrorTips(componentPlayer, condition, "places" + words[0], "limit");
        int num8 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
        int num9 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
        int num10 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
        int num11 = int.Parse(words[4]);
        int num12 = int.Parse(words[5]);
        int num13 = int.Parse(words[6]);
        int num14 = int.Parse(words[7]);
        if (num11 > 0)
        {
          for (int index7 = 0; index7 <= num11; ++index7)
          {
            for (int index8 = -num12 + 1; index8 < num12; ++index8)
            {
              for (int index9 = -num12 + 1; index9 < num12; ++index9)
              {
                if (index8 * index8 + index9 * index9 <= num12 * num12)
                {
                  switch (num13)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index7, num9 + index8, num10 + index9, num14);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index8, num9 + index7, num10 + index9, num14);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index8, num9 + index9, num10 + index7, num14);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
        if (num11 < 0)
        {
          for (int index10 = 0; index10 >= num11; --index10)
          {
            for (int index11 = -num12 + 1; index11 < num12; ++index11)
            {
              for (int index12 = -num12 + 1; index12 < num12; ++index12)
              {
                if (index11 * index11 + index12 * index12 <= num12 * num12)
                {
                  switch (num13)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index10, num9 + index11, num10 + index12, num14);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index11, num9 + index10, num10 + index12, num14);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num8 + index11, num9 + index12, num10 + index10, num14);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
      }
      else if (words[0] == "cone")
      {
        if (words.Length != 8 || int.Parse(words[6]) <= 0 || int.Parse(words[6]) >= 4 || int.Parse(words[5]) <= 0)
          return this.ErrorTips(componentPlayer, condition, "places" + words[0], "limit");
        int num15 = int.Parse(words[1]) + (coordRelative ? position.X : 0);
        int num16 = int.Parse(words[2]) + (coordRelative ? position.Y : 0);
        int num17 = int.Parse(words[3]) + (coordRelative ? position.Z : 0);
        int num18 = int.Parse(words[4]);
        int num19 = int.Parse(words[5]);
        int num20 = int.Parse(words[6]);
        int num21 = int.Parse(words[7]);
        if (num18 > 0)
        {
          for (int index13 = 0; index13 <= num18; ++index13)
          {
            for (int index14 = -num19 + 1; index14 < num19; ++index14)
            {
              for (int index15 = -num19 + 1; index15 < num19; ++index15)
              {
                if (index14 * index14 + index15 * index15 <= num19 * num19 * (num18 - index13) * (num18 - index13) / num18 / num18)
                {
                  switch (num20)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index13, num16 + index14, num17 + index15, num21);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index14, num16 + index13, num17 + index15, num21);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index14, num16 + index15, num17 + index13, num21);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
        if (num18 < 0)
        {
          for (int index16 = 0; index16 >= num18; --index16)
          {
            for (int index17 = -num19 + 1; index17 < num19; ++index17)
            {
              for (int index18 = -num19 + 1; index18 < num19; ++index18)
              {
                if (index17 * index17 + index18 * index18 <= num19 * num19 * (num18 - index16) * (num18 - index16) / num18 / num18)
                {
                  switch (num20)
                  {
                    case 1:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index16, num16 + index17, num17 + index18, num21);
                      continue;
                    case 2:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index17, num16 + index16, num17 + index18, num21);
                      continue;
                    case 3:
                      this.SubsystemElectricity.Project.FindSubsystem<SubsystemTerrain>().ChangeCell(num15 + index17, num16 + index18, num17 + index16, num21);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
      }
      return true;
    }

    public void ChangeBlockdata(string[] words, ComponentPlayer componentPlayer)
    {
      string word = words[1];
      int i = int.Parse(words[2]) & 1023;
      string s = words[3];
      int num1 = 0;
      float num2 = 0.0f;
      bool flag1 = false;
      try
      {
        if (word == "behaviors")
        {
          try
          {
            string[] strArray1 = new string[30]
            {
              "Throwable",
              "Wood",
              "Ivy",
              "Rot",
              "BottomSucker",
              "WaterPlant",
              "Fence",
              "ImpactExplosives",
              "Explosives",
              "Carpet",
              "Campfile",
              "Furniture",
              "Egg",
              "Hammer",
              "Bow",
              "Crossbow",
              "Musket",
              "Arrow",
              "Bomb",
              "Fireworks",
              "Piston",
              "Grave",
              "Soil",
              "Saddle",
              "Match",
              "Bucket",
              "Cactus",
              "Bullet",
              "Campfire",
              "Fertilizer"
            };
            if (s.Contains(","))
            {
              string[] strArray2 = s.Split(',');
              for (int index = 0; index < strArray2.Length; ++index)
              {
                if (!strArray2[index].Contains("BlockBehavior"))
                  strArray2[index] = strArray2[index] + "BlockBehavior";
                foreach (string str in strArray1)
                {
                  if (str + "BlockBehavior" == strArray2[index])
                  {
                    flag1 = true;
                    break;
                  }
                }
                if (!flag1)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:behaviors属性中不存在‘" + s + "’行为", Color.Yellow, false, false);
                  return;
                }
                flag1 = false;
              }
              s = string.Join(",", strArray2);
            }
            else
            {
              if (!s.Contains("BlockBehavior"))
                s += "BlockBehavior";
              foreach (string str in strArray1)
              {
                if (str + "BlockBehavior" == s)
                  flag1 = true;
              }
              if (!flag1)
              {
                componentPlayer.ComponentGui.DisplaySmallMessage("错误:behaviors属性中不存在‘" + s + "’行为", Color.Yellow, false, false);
                return;
              }
            }
            ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).Behaviors = s;
            ValuesDictionary valuesDictionary = (ValuesDictionary) null;
            this.SubsystemElectricity.Project.FindSubsystem<SubsystemBlockBehaviors>(true).Load(valuesDictionary);
          }
          catch
          {
            Log.Warning("The illegal behaviors");
          }
        }
        else if (word == "defaulticonblockoffset" || word == "defaulticonviewoffset" || word == "firstpersonoffset" || word == "firstpersonrotation" || word == "inhandoffset" || word == "inhandrotation")
        {
          if (s.Split(',').Length != 3)
          {
            componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + s + "’为无效的值", Color.Yellow, false, false);
          }
          else
          {
            Vector3 vector3 = new Vector3((float) (int.Parse(s.Split(',')[0]) / 10), (float) (int.Parse(s.Split(',')[1]) / 10), (float) (int.Parse(s.Split(',')[2]) / 10));
            if (!(word == "defaulticonblockoffset"))
            {
              if (!(word == "defaulticonviewoffset"))
              {
                if (!(word == "firstpersonoffset"))
                {
                  if (!(word == "firstpersonrotation"))
                  {
                    if (!(word == "inhandoffset"))
                    {
                      if (word == "inhandrotation")
                        ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).InHandRotation = vector3;
                      else
                        componentPlayer.ComponentGui.DisplaySmallMessage("错误:blockdata指令不存在属性关键字：" + word, Color.Yellow, false, false);
                    }
                    else
                      ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).InHandOffset = vector3;
                  }
                  else
                    ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FirstPersonRotation = vector3;
                }
                else
                  ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FirstPersonOffset = vector3;
              }
              else
                ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultIconViewOffset = vector3;
            }
            else
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultIconBlockOffset = vector3;
          }
        }
        else if (word == "foodtype")
        {
          if (!(s == "Bread"))
          {
            if (!(s == "Fish"))
            {
              if (!(s == "Fruit"))
              {
                if (!(s == "Grass"))
                {
                  if (!(s == "Meat"))
                  {
                    if (s == "None")
                      ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.None;
                    else
                      componentPlayer.ComponentGui.DisplaySmallMessage("错误:不存在‘" + s + "’类型的食物", Color.Yellow, false, false);
                  }
                  else
                    ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.Meat;
                }
                else
                  ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.Grass;
              }
              else
                ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.Fruit;
            }
            else
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.Fish;
          }
          else
            ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FoodType = FoodType.Bread;
        }
        else
        {
          bool flag2 = true;
          if (s.Contains("false") || s.Contains("true"))
            flag2 = false;
          if (flag2)
          {
            try
            {
              num1 = int.Parse(s);
              num2 = (float) num1 / 10f;
            }
            catch
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + s + "’为无效的值", Color.Yellow, false, false);
              return;
            }
          }
          switch (word)
          {
            case "aligntovelocity":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).AlignToVelocity = !(s == "false");
              break;
            case "defaultdropcontent":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultDropContent = num1;
              break;
            case "defaultdropcount":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultDropCount = (float) num1;
              break;
            case "defaultemittedlightamount":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultEmittedLightAmount = num1;
              break;
            case "defaultexperiencecount":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultExperienceCount = num2;
              break;
            case "defaultexplosionincendiary":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultExplosionIncendiary = !(s == "false");
              break;
            case "defaultexplosionpressure":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultExplosionPressure = (float) num1;
              break;
            case "defaultheat":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultHeat = num2;
              break;
            case "defaulticonviewscale":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultIconViewScale = num2;
              break;
            case "defaultisinteractive":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultIsInteractive = !(s == "false");
              break;
            case "defaultmeleehitprobability":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultMeleeHitProbability = num2;
              break;
            case "defaultmeleepower":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultMeleePower = num2;
              break;
            case "defaultprojectilepower":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultProjectilePower = num2;
              break;
            case "defaultrotperiod":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultRotPeriod = num1;
              break;
            case "defaultshadowstrength":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultShadowStrength = num1;
              break;
            case "defaultsicknessprobability":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultSicknessProbability = num2;
              break;
            case "defaulttextureslot":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DefaultTextureSlot = num1;
              break;
            case "density":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).Density = num2;
              break;
            case "digresilience":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DigResilience = num2;
              break;
            case "disintegratesonhit":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).DisintegratesOnHit = !(s == "false");
              break;
            case "durability":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).Durability = num1;
              break;
            case "explosionresilience":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ExplosionResilience = (float) num1;
              break;
            case "fireduration":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FireDuration = (float) num1;
              break;
            case "firstpersonscale":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FirstPersonScale = num2;
              break;
            case "frictionfactor":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FrictionFactor = num2;
              break;
            case "fuelfireduration":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FuelFireDuration = (float) num1;
              break;
            case "fuelheatlevel":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).FuelHeatLevel = (float) num1;
              break;
            case "hackpower":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).HackPower = num2;
              break;
            case "hascollisionbehavior":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).HasCollisionBehavior = !(s == "false");
              break;
            case "inhandscale":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).InHandScale = num2;
              break;
            case "isaimable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsAimable = !(s == "false");
              break;
            case "iscollidable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsCollidable = !(s == "false");
              break;
            case "isdiggingtransparent":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsDiggingTransparent = !(s == "false");
              break;
            case "iseditable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsEditable = !(s == "false");
              break;
            case "isexplosiontransparent":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsExplosionTransparent = !(s == "false");
              break;
            case "isfluidblocker":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsFluidBlocker = !(s == "false");
              break;
            case "isgatherable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsGatherable = !(s == "false");
              break;
            case "isnonduplicable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsNonDuplicable = !(s == "false");
              break;
            case "isplaceable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsPlaceable = !(s == "false");
              break;
            case "isplacementtransparent":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsPlacementTransparent = !(s == "false");
              break;
            case "isstickable":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsStickable = !(s == "false");
              break;
            case "istransparent":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).IsTransparent = !(s == "false");
              break;
            case "killswhenstuck":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).KillsWhenStuck = !(s == "false");
              break;
            case "lightattenuation":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).LightAttenuation = num1;
              break;
            case "maxstacking":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).MaxStacking = num1;
              break;
            case "noautojump":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).NoAutoJump = !(s == "false");
              break;
            case "nosmoothrise":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).NoSmoothRise = !(s == "false");
              break;
            case "objectshadowstrength":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ObjectShadowStrength = num2;
              break;
            case "playerlevelrequired":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).PlayerLevelRequired = num1;
              break;
            case "projectiledamping":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ProjectileDamping = num2;
              break;
            case "projectileresilience":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ProjectileResilience = num2;
              break;
            case "projectilespeed":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ProjectileSpeed = (float) num1;
              break;
            case "projectilestickprobability":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ProjectileStickProbability = num2;
              break;
            case "projectiletipoffset":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ProjectileTipOffset = num2;
              break;
            case "quarrypower":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).QuarryPower = num2;
              break;
            case "requiredtoollevel":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).RequiredToolLevel = num1;
              break;
            case "shovelpower":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ShovelPower = num2;
              break;
            case "sleepsuitability":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).SleepSuitability = num2;
              break;
            case "toollevel":
              ((IEnumerable<Block>) BlocksManager.Blocks).FirstOrDefault<Block>((Func<Block, bool>) (b => b.BlockIndex == i)).ToolLevel = num1;
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:blockdata指令不存在属性关键字：" + word, Color.Yellow, false, false);
              break;
          }
        }
      }
      catch
      {
        componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + s + "’为无效的值", Color.Yellow, false, false);
      }
    }

    public void ChangeCreaturedata(
      string[] words,
      ComponentPlayer componentPlayer,
      DynamicArray<ComponentBody> dynamicArray)
    {
      string word1 = words[1];
      string lower1 = words[2].ToLower();
      string word2 = words[3];
      int num1 = 0;
      float num2 = 0.0f;
      bool flag1;
      string str1;
      string str2;
      if (lower1.Contains(":"))
      {
        flag1 = true;
        str1 = lower1.Split(':')[0];
        str2 = lower1.Split(':')[1];
      }
      else
      {
        flag1 = false;
        str1 = lower1;
        str2 = lower1;
      }
      foreach (ComponentBody dynamic in dynamicArray)
      {
        ComponentCreature component1 = dynamic.Entity.FindComponent<ComponentCreature>();
        string lower2 = dynamic.Entity.ValuesDictionary.DatabaseObject.Name.ToLower();
        if (component1 != null && lower2 == str1 && (!flag1 || Datahandle.GetEntityid(dynamic.Entity).ToString() == str2))
        {
          if ((double) component1.ComponentHealth.Health > 0.0)
          {
            try
            {
              if (word1 == "behaviors")
              {
                string[] strArray = word2.Split(',');
                if (strArray.Length < 2)
                  strArray[0] = word2;
                if (strArray[0] == "remove")
                {
                  switch (strArray[1])
                  {
                    case "all":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Clear();
                      break;
                    case "avoidfire":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentAvoidFireBehavior>());
                      break;
                    case "avoidplayer":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentAvoidPlayerBehavior>());
                      break;
                    case "cattledrive":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentCattleDriveBehavior>());
                      break;
                    case "cetaceanbreathe":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentCetaceanBreatheBehavior>());
                      break;
                    case "chase":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentChaseBehavior>());
                      break;
                    case "diginmud":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentDigInMudBehavior>());
                      break;
                    case "dumprider":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentDumpRiderBehavior>());
                      break;
                    case "eatpickable":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentEatPickableBehavior>());
                      break;
                    case "findplayer":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentFindPlayerBehavior>());
                      break;
                    case "fishoutofwater":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentFishOutOfWaterBehavior>());
                      break;
                    case "flyaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentFlyAroundBehavior>());
                      break;
                    case "flyaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentFlyAwayBehavior>());
                      break;
                    case "herd":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentHerdBehavior>());
                      break;
                    case "howl":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentHowlBehavior>());
                      break;
                    case "layegg":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentLayEggBehavior>());
                      break;
                    case "lookaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentLookAroundBehavior>());
                      break;
                    case "moveaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentMoveAwayBehavior>());
                      break;
                    case "randomfeed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentRandomFeedBehavior>());
                      break;
                    case "randompeck":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentRandomPeckBehavior>());
                      break;
                    case "runaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentRunAwayBehavior>());
                      break;
                    case "stare":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentStareBehavior>());
                      break;
                    case "steed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentSteedBehavior>());
                      break;
                    case "stubbornsteed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentStubbornSteedBehavior>());
                      break;
                    case "summon":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentSummonBehavior>());
                      break;
                    case "swimaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentSwimAroundBehavior>());
                      break;
                    case "swimaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentSwimAwayBehavior>());
                      break;
                    case "walkaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentWalkAroundBehavior>());
                      break;
                    default:
                      componentPlayer.ComponentGui.DisplaySmallMessage("错误:不存在‘" + word2[1].ToString() + "’行为", Color.Yellow, false, false);
                      return;
                  }
                }
                if (strArray[0] == "add")
                {
                  switch (strArray[1])
                  {
                    case "avoidfire":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentAvoidFireBehavior>());
                      continue;
                    case "avoidplayer":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentAvoidPlayerBehavior>());
                      continue;
                    case "cattledrive":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentCattleDriveBehavior>());
                      continue;
                    case "cetaceanbreathe":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentCetaceanBreatheBehavior>());
                      continue;
                    case "chase":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentChaseBehavior>());
                      continue;
                    case "diginmud":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentDigInMudBehavior>());
                      continue;
                    case "dumprider":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentDumpRiderBehavior>());
                      continue;
                    case "eatpickable":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentEatPickableBehavior>());
                      continue;
                    case "findplayer":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentFindPlayerBehavior>());
                      continue;
                    case "fishoutofwater":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentFishOutOfWaterBehavior>());
                      continue;
                    case "flyaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentFlyAroundBehavior>());
                      continue;
                    case "flyaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentFlyAwayBehavior>());
                      continue;
                    case "herd":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentHerdBehavior>());
                      continue;
                    case "howl":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentHowlBehavior>());
                      continue;
                    case "layegg":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentLayEggBehavior>());
                      continue;
                    case "lookaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentLookAroundBehavior>());
                      continue;
                    case "moveaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentMoveAwayBehavior>());
                      continue;
                    case "randomfeed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentRandomFeedBehavior>());
                      continue;
                    case "randompeck":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentRandomPeckBehavior>());
                      continue;
                    case "runaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentRunAwayBehavior>());
                      continue;
                    case "stare":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentStareBehavior>());
                      continue;
                    case "steed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentSteedBehavior>());
                      continue;
                    case "stubbornsteed":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentStubbornSteedBehavior>());
                      continue;
                    case "summon":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Remove((ComponentBehavior) component1.Entity.FindComponent<ComponentSummonBehavior>());
                      continue;
                    case "swimaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentSwimAroundBehavior>());
                      continue;
                    case "swimaway":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentSwimAwayBehavior>());
                      continue;
                    case "walkaround":
                      component1.Entity.FindComponent<ComponentBehaviorSelector>().m_behaviors.Add((ComponentBehavior) component1.Entity.FindComponent<ComponentWalkAroundBehavior>());
                      continue;
                    default:
                      componentPlayer.ComponentGui.DisplaySmallMessage("错误:不存在‘" + strArray[1] + "’行为", Color.Yellow, false, false);
                      return;
                  }
                }
              }
              else if (word1 == "action")
              {
                string str3 = component1.ComponentCreatureModel.GetType().ToString();
                string[] strArray = word2.Split(',');
                if (strArray.Length < 2)
                {
                  componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + word2 + "’为无效的值", Color.Yellow, false, false);
                  break;
                }
                num1 = int.Parse(strArray[1]);
                if (str3 == "Game.ComponentFourLeggedModel")
                {
                  ComponentFourLeggedModel component2 = component1.Entity.FindComponent<ComponentFourLeggedModel>();
                  string str4 = strArray[0];
                  if (!(str4 == "head"))
                  {
                    if (!(str4 == "leg1"))
                    {
                      if (!(str4 == "leg2"))
                      {
                        if (!(str4 == "leg3"))
                        {
                          if (str4 == "leg4")
                            component2.m_legAngle4 = (float) num1;
                        }
                        else
                          component2.m_legAngle3 = (float) num1;
                      }
                      else
                        component2.m_legAngle2 = (float) num1;
                    }
                    else
                      component2.m_legAngle1 = (float) num1;
                  }
                  else
                    component2.m_headAngleY = (float) num1;
                }
                else if (str3 == "Game.ComponentBirdModel")
                {
                  ComponentBirdModel component3 = component1.Entity.FindComponent<ComponentBirdModel>();
                  string str5 = strArray[0];
                  if (!(str5 == "flyanimationspeed"))
                  {
                    if (!(str5 == "walkanimationspeed"))
                    {
                      if (str5 == "peckanimationspeed")
                        component3.m_peckAnimationSpeed = (float) num1;
                    }
                    else
                      component3.m_walkAnimationSpeed = (float) num1;
                  }
                  else
                    component3.m_flyAnimationSpeed = (float) num1;
                }
                else if (str3 == "Game.ComponentFishModel")
                {
                  ComponentFishModel component4 = component1.Entity.FindComponent<ComponentFishModel>();
                  string str6 = strArray[0];
                  if (!(str6 == "digindepth"))
                  {
                    if (!(str6 == "tailturn"))
                    {
                      if (str6 == "swimanimationspeed")
                        component4.m_swimAnimationSpeed = (float) num1;
                    }
                    else
                      component4.m_tailTurn = new Vector2((float) num1, (float) num1);
                  }
                  else
                    component4.m_digInDepth = (float) num1;
                }
                else if (str3 == "Game.ComponentFlightlessBirdModel")
                {
                  ComponentFlightlessBirdModel component5 = component1.Entity.FindComponent<ComponentFlightlessBirdModel>();
                  string str7 = strArray[0];
                  if (!(str7 == "leg1"))
                  {
                    if (!(str7 == "leg2"))
                    {
                      if (str7 == "head")
                        component5.m_headAngleY = (float) num1;
                    }
                    else
                      component5.m_legAngle2 = (float) num1;
                  }
                  else
                    component5.m_legAngle1 = (float) num1;
                }
                else if (str3 == "Game.ComponentHumanModel")
                {
                  ComponentHumanModel component6 = component1.Entity.FindComponent<ComponentHumanModel>();
                  Vector2 vector2 = new Vector2((float) int.Parse(strArray[1]), (float) int.Parse(strArray[2]));
                  string str8 = strArray[0];
                  if (!(str8 == "head"))
                  {
                    if (!(str8 == "hand1"))
                    {
                      if (!(str8 == "hand2"))
                      {
                        if (!(str8 == "leg1"))
                        {
                          if (str8 == "leg2")
                            component6.m_legAngles2 = vector2;
                        }
                        else
                          component6.m_legAngles1 = vector2;
                      }
                      else
                        component6.m_handAngles2 = vector2;
                    }
                    else
                      component6.m_handAngles1 = vector2;
                  }
                  else
                    component6.m_headAngles = vector2;
                }
              }
              else if (word1 == "sound")
              {
                component1.ComponentCreatureSounds.m_idleSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_painSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_attackSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_coughSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_moanSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_pukeSound = "Audio/Creatures/" + word2;
                component1.ComponentCreatureSounds.m_sneezeSound = "Audio/Creatures/" + word2;
              }
              else if (word1 == "airdrag")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentBody.AirDrag = vector2;
              }
              else if (word1 == "waterdrag")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentBody.WaterDrag = vector2;
              }
              else if (word1 == "lookorder")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentLocomotion.LookOrder = vector2;
              }
              else if (word1 == "turnorder")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentLocomotion.TurnOrder = vector2;
              }
              else if (word1 == "vrlookorder")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentLocomotion.VrLookOrder = new Vector2?(vector2);
              }
              else if (word1 == "walkorder")
              {
                Vector2 vector2 = new Vector2((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10));
                component1.ComponentLocomotion.WalkOrder = new Vector2?(vector2);
              }
              else if (word1 == "vrmoveorder")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentLocomotion.VrMoveOrder = new Vector3?(vector3);
              }
              else if (word1 == "swimorder")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentLocomotion.SwimOrder = new Vector3?(vector3);
              }
              else if (word1 == "flyorder")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentLocomotion.FlyOrder = new Vector3?(vector3);
              }
              else if (word1 == "boxsize")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentBody.BoxSize = vector3;
              }
              else if (word1 == "velocity")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentBody.m_velocity = vector3;
              }
              else if (word1 == "collisionvelocitychange")
              {
                Vector3 vector3 = new Vector3((float) (int.Parse(word2.Split(',')[0]) / 10), (float) (int.Parse(word2.Split(',')[1]) / 10), (float) (int.Parse(word2.Split(',')[2]) / 10));
                component1.ComponentBody.CollisionVelocityChange = vector3;
              }
              else
              {
                bool flag2 = true;
                if (word2.Contains("false") || word2.Contains("true"))
                  flag2 = false;
                if (flag2)
                {
                  try
                  {
                    num1 = int.Parse(word2);
                    num2 = (float) num1 / 10f;
                  }
                  catch
                  {
                    componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + word2 + "’为无效的值", Color.Yellow, false, false);
                    break;
                  }
                }
                switch (word1)
                {
                  case "accelerationfactor":
                    component1.ComponentLocomotion.AccelerationFactor = num2;
                    continue;
                  case "air":
                    component1.ComponentHealth.Air = num2;
                    continue;
                  case "aircapacity":
                    component1.ComponentHealth.AirCapacity = num2;
                    continue;
                  case "attackpower":
                    component1.Entity.FindComponent<ComponentMiner>(true).AttackPower = (float) num1;
                    continue;
                  case "attackresilience":
                    component1.ComponentHealth.AttackResilience = num2;
                    continue;
                  case "breathingmode":
                    component1.ComponentHealth.BreathingMode = num1 == 0 ? BreathingMode.Air : BreathingMode.Water;
                    continue;
                  case "canstrand":
                    component1.ComponentHealth.CanStrand = !(word2 == "false");
                    continue;
                  case "corpseduration":
                    component1.ComponentHealth.CorpseDuration = num2;
                    continue;
                  case "creativeflyspeed":
                    component1.ComponentLocomotion.CreativeFlySpeed = num2;
                    continue;
                  case "density":
                    component1.ComponentBody.Density = (float) num1;
                    continue;
                  case "fallresilience":
                    component1.ComponentHealth.FallResilience = num2;
                    continue;
                  case "fireresilience":
                    component1.ComponentHealth.FireResilience = num2;
                    continue;
                  case "flyspeed":
                    component1.ComponentLocomotion.FlySpeed = num2;
                    continue;
                  case "health":
                    component1.ComponentHealth.Health = num2;
                    continue;
                  case "healthchange":
                    component1.ComponentHealth.HealthChange = num2;
                    continue;
                  case "immersiondepth":
                    component1.ComponentBody.ImmersionDepth = num2;
                    continue;
                  case "immersionfactor":
                    component1.ComponentBody.ImmersionFactor = num2;
                    continue;
                  case "inairwalkfactor":
                    component1.ComponentLocomotion.InAirWalkFactor = num2;
                    continue;
                  case "isflyenabled":
                    component1.ComponentLocomotion.IsCreativeFlyEnabled = !(word2 == "false");
                    continue;
                  case "isgravityenabled":
                    component1.ComponentBody.IsGravityEnabled = !(word2 == "false");
                    continue;
                  case "isgrounddragenabled":
                    component1.ComponentBody.IsGroundDragEnabled = !(word2 == "false");
                    continue;
                  case "isinvulnerable":
                    component1.ComponentHealth.IsInvulnerable = !(word2 == "false");
                    continue;
                  case "issmoothriseenabled":
                    component1.ComponentBody.IsSmoothRiseEnabled = !(word2 == "false");
                    continue;
                  case "issneaking":
                    component1.ComponentBody.IsSneaking = !(word2 == "false");
                    continue;
                  case "jumporder":
                    component1.ComponentLocomotion.JumpOrder = num2;
                    continue;
                  case "jumpspeed":
                    component1.ComponentLocomotion.JumpSpeed = num2;
                    continue;
                  case "ladderspeed":
                    component1.ComponentLocomotion.LadderSpeed = num2;
                    continue;
                  case "mass":
                    component1.ComponentBody.Mass = num2;
                    continue;
                  case "slipspeed":
                    component1.ComponentLocomotion.SlipSpeed = new float?(num2);
                    continue;
                  case "stuntime":
                    component1.ComponentLocomotion.StunTime = num2;
                    continue;
                  case "swimspeed":
                    component1.ComponentLocomotion.SwimSpeed = num2;
                    continue;
                  case "turnspeed":
                    component1.ComponentLocomotion.TurnSpeed = num2;
                    continue;
                  case "walkspeed":
                    component1.ComponentLocomotion.WalkSpeed = num2;
                    continue;
                  case "waterswayangle":
                    component1.ComponentBody.WaterSwayAngle = num2;
                    continue;
                  case "waterturnspeed":
                    component1.ComponentBody.WaterTurnSpeed = num2;
                    continue;
                  default:
                    componentPlayer.ComponentGui.DisplaySmallMessage("错误:creaturedata指令不存在属性关键字：" + word1, Color.Yellow, false, false);
                    continue;
                }
              }
            }
            catch (Exception ex)
            {
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + word2 + "’为无效的值", Color.Yellow, false, false);
              break;
            }
          }
        }
      }
    }

    public void ChangeClothesdata(string[] words, ComponentPlayer componentPlayer)
    {
      string word1 = words[1];
      int index = int.Parse(words[2]);
      string word2 = words[3];
      try
      {
        if (word1 == "isouter")
          ClothingBlock.m_clothingData[index].IsOuter = !(word2 == "false");
        else if (word1 == "canbedyed")
        {
          ClothingBlock.m_clothingData[index].CanBeDyed = !(word2 == "false");
        }
        else
        {
          int num1 = int.Parse(word2);
          float num2 = (float) num1 / 10f;
          switch (word1)
          {
            case "armorprotection":
              ClothingBlock.m_clothingData[index].ArmorProtection = num2;
              break;
            case "densitymodifier":
              ClothingBlock.m_clothingData[index].DensityModifier = num2;
              break;
            case "insulation":
              ClothingBlock.m_clothingData[index].Insulation = num2;
              break;
            case "layer":
              ClothingBlock.m_clothingData[index].Layer = num1;
              break;
            case "movementspeedfactor":
              ClothingBlock.m_clothingData[index].MovementSpeedFactor = num2;
              break;
            case "playerlevelrequired":
              ClothingBlock.m_clothingData[index].PlayerLevelRequired = num1;
              break;
            case "steedmovementspeedfactor":
              ClothingBlock.m_clothingData[index].SteedMovementSpeedFactor = num2;
              break;
            case "sturdiness":
              ClothingBlock.m_clothingData[index].Sturdiness = (float) num1;
              break;
            default:
              componentPlayer.ComponentGui.DisplaySmallMessage("错误:clothesdata指令不存在属性关键字：" + word1, Color.Yellow, false, false);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        componentPlayer.ComponentGui.DisplaySmallMessage("错误:‘" + word2 + "’为无效的值", Color.Yellow, false, false);
      }
    }
  }
}
