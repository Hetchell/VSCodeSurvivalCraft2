using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Color = Engine.Color;

namespace Game
{
  public class Datahandle
  {
    public static Point3 CollideHandle(Vector3 v, Vector3 p, int type)
    {
      int x = (int) p.X;
      int y = (int) p.Y;
      int z = (int) p.Z;
      if (type == 1)
      {
        if ((double) v.X > 0.0)
          x = (int) MathUtils.Floor(p.X + 1.1f);
        else if ((double) v.X < 0.0)
          x = (int) MathUtils.Ceiling(p.X - 1.1f);
        if ((double) v.Y > 0.0)
          y = (int) ((double) p.Y + 1.10000002384186);
        else if ((double) v.Y < 0.0)
          y = (int) ((double) p.Y - 0.100000001490116);
        if ((double) v.Z > 0.0)
          z = (int) MathUtils.Floor(p.Z + 1.1f);
        else if ((double) v.Z < 0.0)
          z = (int) MathUtils.Ceiling(p.Z - 1.1f);
      }
      if (type == 2)
      {
        if ((double) v.X > 0.0)
          x = (int) MathUtils.Floor(p.X + 0.9f);
        else if ((double) v.X < 0.0)
          x = (int) MathUtils.Ceiling(p.X - 0.9f);
        if ((double) v.Y > 0.0)
          y = (int) ((double) p.Y + 0.899999976158142);
        else if ((double) v.Y < 0.0)
          y = (int) ((double) p.Y + 0.100000001490116);
        if ((double) v.Z > 0.0)
          z = (int) MathUtils.Floor(p.Z + 0.9f);
        else if ((double) v.Z < 0.0)
          z = (int) MathUtils.Ceiling(p.Z - 0.9f);
      }
      return new Point3(x, y, z);
    }

    public static int GetAngle(Vector3 v, string type)
    {
      int angle = 0;
      if (type == "horizontal")
      {
        Vector3 vector3_1 = v;
        Vector3 vector3_2 = new Vector3(0.0f, -1f, 0.0f);
        angle = (int) ((double) MathUtils.Acos((float) (((double) vector3_1.X * (double) vector3_2.X + (double) vector3_1.Y * (double) vector3_2.Y + (double) vector3_1.Z * (double) vector3_2.Z) / ((double) vector3_1.Length() * (double) vector3_2.Length()))) / 3.14 * 180.0);
      }
      else if (type == "vertical")
      {
        Vector3 vector3_3 = new Vector3(v.X, 0.0f, v.Z);
        Vector3 vector3_4 = new Vector3(1f, 0.0f, 0.0f);
        angle = (int) ((double) MathUtils.Acos((float) (((double) vector3_3.X * (double) vector3_4.X + (double) vector3_3.Y * (double) vector3_4.Y + (double) vector3_3.Z * (double) vector3_4.Z) / ((double) vector3_3.Length() * (double) vector3_4.Length()))) / 3.14 * 180.0);
        if ((double) vector3_3.Z > 0.0)
          angle = 360 - angle;
      }
      return angle;
    }

    public static int GetEntityid(Entity entity)
    {
      int entityid = 1;
      List<Entity> entityList = new List<Entity>();
      ComponentCreature component1 = entity.FindComponent<ComponentCreature>();
      ComponentBoat component2 = entity.FindComponent<ComponentBoat>();
      if (component1 != null)
      {
        foreach (Entity entity1 in GameManager.Project.Entities)
        {
          if (entity1.FindComponent<ComponentCreature>() != null)
            entityList.Add(entity1);
        }
      }
      if (component2 != null)
      {
        foreach (Entity entity2 in GameManager.Project.Entities)
        {
          if (entity2.FindComponent<ComponentBoat>() != null)
            entityList.Add(entity2);
        }
      }
      foreach (Entity entity3 in entityList)
      {
        if (entity == entity3)
          return entityid;
        ++entityid;
      }
      return 0;
    }

    public static string GetPathname(string name)
    {
      string systemPath = Engine.Storage.GetSystemPath(GameManager.m_worldInfo.DirectoryName);
      string pathname;
      if (File.Exists(systemPath + "/" + name))
      {
        pathname = systemPath + "/" + name;
      }
      else
      {
        string path = !(Environment.CurrentDirectory == "/") ? Engine.Storage.GetSystemPath("app:") + "/Command" : Engine.Storage.GetSystemPath("android:SurvivalCraft2.2") + "/Command";
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        pathname = path + "/" + name;
      }
      return pathname;
    }

    public static string[] GetReplacewords(string[] words, char[] variable, int[] signals)
    {
      char[] chArray = variable;
      string[] replacewords = words;
      for (int index1 = 1; index1 < replacewords.Length; ++index1)
      {
        replacewords[index1] = replacewords[index1].Replace(chArray[0].ToString() ?? "", signals[0].ToString() ?? "");
        replacewords[index1] = replacewords[index1].Replace(chArray[1].ToString() ?? "", signals[1].ToString() ?? "");
        replacewords[index1] = replacewords[index1].Replace(chArray[2].ToString() ?? "", signals[2].ToString() ?? "");
        replacewords[index1] = replacewords[index1].Replace(chArray[3].ToString() ?? "", signals[3].ToString() ?? "");
        if (replacewords[index1].Contains(":"))
        {
          try
          {
            replacewords[index1] = replacewords[index1].Split(':')[0] + ":" + Datahandle.Expressionhandle(replacewords[index1].Split(':')[1]);
          }
          catch
          {
          }
        }
        else if (replacewords[index1].Contains(","))
        {
          string[] strArray = replacewords[index1].Split(',');
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            try
            {
              strArray[index2] = Datahandle.Expressionhandle(strArray[index2]);
            }
            catch
            {
            }
          }
          replacewords[index1] = string.Join(",", strArray);
        }
        else
        {
          try
          {
            replacewords[index1] = Datahandle.Expressionhandle(replacewords[index1]);
          }
          catch
          {
          }
        }
      }
      string.Join(" ", replacewords);
      return replacewords;
    }

    public static List<InformationTopic> GetInformationtopics()
    {
      List<InformationTopic> informationtopics = new List<InformationTopic>();
      foreach (XElement xelement in ContentManager.Get<XElement>("Information").Elements((XName) "Topic").ToList<XElement>())
        informationtopics.Add(new InformationTopic()
        {
          Title = xelement.Attribute((XName) "Title").Value,
          Details = xelement.Value.Trim('\n').Replace("\t", "")
        });
      return informationtopics;
    }

    public static List<Instruction> GetSearchInstructionlist(string keystr)
    {
      string lower1 = keystr.ToLower();
      List<Instruction> searchInstructionlist = new List<Instruction>();
      string str1 = ContentManager.Get<string>("InstructionList");
      ContentManager.Dispose("InstructionList");
      string str2 = str1.Replace("\n", "#");
      char[] chArray1 = new char[1]{ '#' };
      foreach (string str3 in str2.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str3.Split(chArray2);
        if (strArray.Length == 2)
        {
          string lower2 = strArray[0].ToLower();
          if (lower2.Contains(lower1) && !lower2.Contains("value") && !lower2.Contains("keyword"))
            searchInstructionlist.Add(new Instruction()
            {
              Name = strArray[0],
              Details = strArray[1],
              ExistSub = false
            });
        }
      }
      string str4 = ContentManager.Get<string>("SubInstructionList");
      ContentManager.Dispose("SubInstructionList");
      string str5 = str4.Replace("\n", "#");
      char[] chArray3 = new char[1]{ '#' };
      foreach (string str6 in str5.Split(chArray3))
      {
        char[] chArray4 = new char[1]{ '=' };
        string[] strArray = str6.Split(chArray4);
        if (strArray.Length == 2)
        {
          string lower3 = strArray[0].ToLower();
          if (lower3.Contains(lower1) && !lower3.Contains("value") && !lower3.Contains("keyword"))
            searchInstructionlist.Add(new Instruction()
            {
              Name = strArray[0],
              Details = strArray[1],
              ExistSub = false
            });
        }
      }
      return searchInstructionlist;
    }

    public static List<Instruction> GetInstructionlist(bool conditionSwitch)
    {
      List<Instruction> instructionList1 = new List<Instruction>();
      List<Instruction> instructionList2 = new List<Instruction>();
      string str1 = ContentManager.Get<string>("InstructionList");
      ContentManager.Dispose("InstructionList");
      string str2 = str1.Replace("\n", "#");
      char[] chArray1 = new char[1]{ '#' };
      foreach (string str3 in str2.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str3.Split(chArray2);
        if (strArray.Length == 2)
        {
          string str4 = strArray[0].Split(' ')[0];
          Instruction instruction = new Instruction()
          {
            Name = strArray[0],
            Details = strArray[1]
          };
          instruction.ExistSub = instruction.Name.Contains("value") || instruction.Name.Contains("keyword");
          if (!str4.Contains("?"))
            instructionList1.Add(instruction);
          else
            instructionList2.Add(instruction);
        }
      }
      return !conditionSwitch ? instructionList1 : instructionList2;
    }

    public static List<Instruction> GetSubInstructionlist(string keyname)
    {
      List<Instruction> subInstructionlist = new List<Instruction>();
      string str1 = ContentManager.Get<string>("SubInstructionList");
      ContentManager.Dispose("SubInstructionList");
      string str2 = str1.Replace("\n", "#");
      char[] chArray1 = new char[1]{ '#' };
      foreach (string str3 in str2.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str3.Split(chArray2);
        if (strArray.Length == 2)
        {
          if (strArray[0].Split(' ')[0] == keyname)
            subInstructionlist.Add(new Instruction()
            {
              Name = strArray[0],
              Details = strArray[1],
              ExistSub = false
            });
        }
      }
      return subInstructionlist;
    }

    public static Point3 Coordbodyhandle(Vector3 vector3)
    {
      int x = 0;
      int z = 0;
      if ((double) vector3.X > 0.0)
        x = (int) vector3.X;
      else if ((double) vector3.X < 0.0)
        x = (int) vector3.X - 1;
      if ((double) vector3.Z > 0.0)
        z = (int) vector3.Z;
      else if ((double) vector3.Z < 0.0)
        z = (int) vector3.Z - 1;
      int y = (int) vector3.Y;
      return new Point3(x, y, z);
    }

    public static int Colorhandle(Color c)
    {
      Color color1 = c;
      List<Color> colorList = new List<Color>();
      colorList.Add(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      colorList.Add(new Color(181, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      colorList.Add(new Color((int) byte.MaxValue, 181, (int) byte.MaxValue, (int) byte.MaxValue));
      colorList.Add(new Color(160, 181, (int) byte.MaxValue, (int) byte.MaxValue));
      colorList.Add(new Color((int) byte.MaxValue, 240, 160, (int) byte.MaxValue));
      colorList.Add(new Color(181, (int) byte.MaxValue, 181, (int) byte.MaxValue));
      colorList.Add(new Color((int) byte.MaxValue, 181, 160, (int) byte.MaxValue));
      colorList.Add(new Color(181, 181, 181, (int) byte.MaxValue));
      colorList.Add(new Color(112, 112, 112, (int) byte.MaxValue));
      colorList.Add(new Color(32, 112, 112, (int) byte.MaxValue));
      colorList.Add(new Color(112, 32, 112, (int) byte.MaxValue));
      colorList.Add(new Color(26, 52, 128, (int) byte.MaxValue));
      colorList.Add(new Color(87, 54, 31, (int) byte.MaxValue));
      colorList.Add(new Color(24, 116, 24, (int) byte.MaxValue));
      colorList.Add(new Color(136, 32, 32, (int) byte.MaxValue));
      colorList.Add(new Color(24, 24, 24, (int) byte.MaxValue));
      int[] numArray = new int[16];
      int index1 = 0;
      foreach (Color color2 in colorList)
      {
        int num1 = ((int) color1.R + (int) color2.R) / 2;
        int num2 = ((int) color1.R - (int) color2.R) * ((int) color1.R - (int) color2.R);
        int num3 = ((int) color1.G - (int) color2.G) * ((int) color1.G - (int) color2.G);
        int num4 = ((int) color1.B - (int) color2.B) * ((int) color1.B - (int) color2.B);
        int num5 = (2 + num1 / 256) * num2 + 4 * num3 + (2 + ((int) byte.MaxValue - num1) / 256) * num4;
        numArray[index1] = num5;
        ++index1;
      }
      int num6 = 100000000;
      int num7 = 0;
      for (int index2 = 0; index2 < numArray.Length; ++index2)
      {
        if (numArray[index2] < num6)
        {
          num6 = numArray[index2];
          num7 = index2;
        }
      }
      return num7;
    }

    public static string Expressionhandle(string str)
    {
      string str1 = str;
      int startIndex = 0;
      string val1 = "";
      Stack numStatck = new Stack();
      Stack operStatck = new Stack();
      do
      {
        string str2 = str1.Substring(startIndex, 1);
        if (Datahandle.isNum(str2))
        {
          val1 += str2;
          if (startIndex == str1.Length - 1 || !Datahandle.isNum(str1.Substring(startIndex + 1, 1)))
            numStatck.push((object) val1);
        }
        else
        {
          val1 = "";
          if (operStatck.isEmpty())
          {
            operStatck.push((object) str2);
          }
          else
          {
            while (!operStatck.isEmpty() && Datahandle.getLevel(str2) <= Datahandle.getLevel(operStatck.getTop()?.ToString() ?? ""))
            {
              int val2 = Datahandle.Calculate(numStatck, operStatck);
              numStatck.push((object) val2);
            }
            operStatck.push((object) str2);
          }
        }
        ++startIndex;
      }
      while (startIndex != str1.Length);
      while (!operStatck.isEmpty())
      {
        int val3 = Datahandle.Calculate(numStatck, operStatck);
        numStatck.push((object) val3);
      }
      return ((int) MathUtils.Round(double.Parse(numStatck.pop()?.ToString() ?? ""))).ToString() ?? "";
    }

    public static bool isNum(string ch) => !(ch == "+") && !(ch == "-") && !(ch == "*") && !(ch == "/");

    public static int getLevel(string oper) => "*".Equals(oper) || "/".Equals(oper) ? 1 : 0;

    public static int Calculate(Stack numStatck, Stack operStatck)
    {
      int num1 = 0;
      if (numStatck.getTop() != null)
        num1 = int.Parse(numStatck.pop()?.ToString() ?? "");
      int num2 = 0;
      if (numStatck.getTop() != null)
        num2 = int.Parse(numStatck.pop()?.ToString() ?? "");
      string str1 = "";
      if (operStatck.getTop() != null)
        str1 = operStatck.pop()?.ToString() ?? "";
      int num3 = 0;
      string str2 = str1.Substring(0, 1);
      if (!(str2 == "+"))
      {
        if (!(str2 == "-"))
        {
          if (!(str2 == "*"))
          {
            if (str2 == "/")
              num3 = num2 / num1;
          }
          else
            num3 = num1 * num2;
        }
        else
          num3 = num2 - num1;
      }
      else
        num3 = num1 + num2;
      return num3;
    }
  }
}
