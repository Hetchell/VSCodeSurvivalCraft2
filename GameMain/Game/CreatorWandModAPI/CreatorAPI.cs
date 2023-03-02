//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CreatorWandModAPI;
using CreatorWand2;
using Engine;
using Engine.Graphics;
using Game;
using Color = Engine.Color;

public class CreatorAPI
{
	public enum NumberPoint
	{
		One,
		Two,
		Three,
		Four
	}

	public enum OnekeyType
	{
		Tree,
		Build
	}

	public static Language Language;

	public static bool IsAddedToProject;

	public static IEnumerable<XElement> CreatorDisplayDataDialog;

	public static IEnumerable<XElement> CreatorDisplayDataUI;

	public bool oldMainWidget;

	public bool AirIdentify;

	public bool ClearBlock;

	public bool UnLimitedOfCreate;

	public bool RevokeSwitch = true;

	public ChunkData revokeData;

	public NumberPoint amountPoint = NumberPoint.Two;

	public NumberPoint numberPoint;

	public CreateBlockType CreateBlockType = CreateBlockType.Fast;

	public bool oneKeyGeneration;

	public OnekeyType onekeyType = OnekeyType.Build;

	public bool launch = true;

	public bool pasteRotate;

	public bool pasteLimit;

	public ComponentMiner componentMiner;

	public PrimitivesRenderer3D primitivesRenderer3D;

	public CreatorGenerationAlgorithm creatorGenerationAlgorithm;

	public List<Point3> Position { get; set; }

	public CreatorAPI(ComponentMiner componentMiner)
	{
		try
		{
			switch (ModsManager.modSettings.languageType)
			{
				case LanguageControl.LanguageType.en_US:
				case LanguageControl.LanguageType.ot_OT:
					CreatorMain.Language = (Language = Language.en_US);
					break;
				case LanguageControl.LanguageType.zh_CN:
					CreatorMain.Language = (Language = Language.zh_CN);
					break;
				default:
					CreatorMain.Language = (Language = Language.en_US);
					break;
			}
			XElement xElement = ContentManager.Get<XElement>("CreatorDisplay");
			CreatorDisplayDataDialog = from xe in xElement.Element("CreatorDisplayDialog").Elements("CreatorDisplayData")
									   where xe.Attribute("Language").Value == Language.ToString()
									   select xe;
			CreatorDisplayDataUI = xElement.Element("CreatorDisplayUI").Elements();
			ContentManager.Dispose("CreatorDisplay");
		}
		catch (Exception ex)
		{
			Log.Write(LogType.Warning, "Failed to read language, need to restart\n" + ex.Message);
			Language = Language.en_US;
		}
		creatorGenerationAlgorithm = new CreatorGenerationAlgorithm();
		this.componentMiner = componentMiner;
		Position = new List<Point3>(4)
		{
			new Point3(0, -1, 0),
			new Point3(0, -1, 0),
			new Point3(0, -1, 0),
			new Point3(0, -1, 0)
		};
	}

	public void OnUse(TerrainRaycastResult terrainRaycastResult)
	{
		Point3 point = terrainRaycastResult.CellFace.Point;
		ComponentPlayer componentPlayer = componentMiner.ComponentPlayer;
		if (!OnTouch.Touch(this, point))
		{
			return;
		}
		int cellValue = GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetCellValue(point.X, point.Y, point.Z);
		int num = Terrain.ExtractLight(cellValue);
		int num2 = Terrain.ExtractData(cellValue);
		int num3 = Terrain.ExtractContents(cellValue);
		if (BlocksManager.Blocks[num3] == null)
		{
			return;
		}
		if (numberPoint == NumberPoint.One)
		{
			Position[0] = point;
			try
			{
				componentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorAPIsetpoint1"), point.X, point.Y, point.Z, num3, cellValue, num, num2, SetFaceAndRotate.GetFace(cellValue), SetFaceAndRotate.GetRotate(cellValue), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetTemperature(point.X, point.Z), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetHumidity(point.X, point.Z)), Color.White, true, false);
			}
			catch (Exception message)
			{
				Log.Warning(message);
			}
			if (amountPoint == numberPoint)
			{
				return;
			}
			numberPoint = NumberPoint.Two;
		}
		else if (numberPoint == NumberPoint.Two)
		{
			Position[1] = point;
			try
			{
				componentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorAPIsetpoint2"), point.X, point.Y, point.Z, num3, cellValue, num, num2, SetFaceAndRotate.GetFace(cellValue), SetFaceAndRotate.GetRotate(cellValue), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetTemperature(point.X, point.Z), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetHumidity(point.X, point.Z)), Color.White, true, false);
			}
			catch (Exception message2)
			{
				Log.Warning(message2);
			}
			SetFaceAndRotate.SetFace(cellValue, SetFaceAndRotate.GetFace(cellValue));
			if (amountPoint == numberPoint)
			{
				numberPoint = NumberPoint.One;
			}
			else
			{
				numberPoint = NumberPoint.Three;
			}
		}
		else if (numberPoint == NumberPoint.Three)
		{
			Position[2] = point;
			try
			{
				componentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorAPIsetpoint3"), point.X, point.Y, point.Z, num3, cellValue, num, num2, SetFaceAndRotate.GetFace(cellValue), SetFaceAndRotate.GetRotate(cellValue), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetTemperature(point.X, point.Z), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetHumidity(point.X, point.Z)), Color.White, true, false);
			}
			catch (Exception message3)
			{
				Log.Warning(message3);
			}
			if (amountPoint == numberPoint)
			{
				numberPoint = NumberPoint.One;
			}
			else
			{
				numberPoint = NumberPoint.Four;
			}
		}
		else
		{
			if (numberPoint != NumberPoint.Four)
			{
				return;
			}
			Position[3] = point;
			try
			{
				componentPlayer.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorAPIsetpoint4"), point.X, point.Y, point.Z, num3, cellValue, num, num2, SetFaceAndRotate.GetFace(cellValue), SetFaceAndRotate.GetRotate(cellValue), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetTemperature(point.X, point.Z), GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetHumidity(point.X, point.Z)), Color.White, true, false);
			}
			catch (Exception message4)
			{
				Log.Warning(message4);
			}
			numberPoint = NumberPoint.One;
		}
		CreatorMain.Position = Position;
	}

	public void CreateBlock(int x, int y, int z, int value, ChunkData chunkData = null)
	{
		if (RevokeSwitch && revokeData != null && revokeData.GetChunk(x, y) == null)
		{
			revokeData.CreateChunk(x, y, true);
		}
		CW2EntityManager.RemoveEntity(new Point3(x, y, z));
		switch (CreateBlockType)
		{
			case CreateBlockType.Normal:
				GameManager.Project.FindSubsystem<SubsystemTerrain>(true).ChangeCell(x, y, z, value);
				break;
			case CreateBlockType.Fast:
				SetBlock(x, y, z, value);
				break;
			case CreateBlockType.Catch:
				chunkData.SetBlock(x, y, z, value);
				break;
		}
	}

	public void CreateBlock(Point3 point3, int value, ChunkData chunkData = null)
	{
		if (RevokeSwitch && revokeData != null && revokeData.GetChunk(point3.X, point3.Z) == null)
		{
			revokeData.CreateChunk(point3.X, point3.Z, true);
		}
		CW2EntityManager.RemoveEntity(point3);
		switch (CreateBlockType)
		{
			case CreateBlockType.Normal:
				GameManager.Project.FindSubsystem<SubsystemTerrain>(true).ChangeCell(point3.X, point3.Y, point3.Z, value);
				break;
			case CreateBlockType.Fast:
				SetBlock(point3.X, point3.Y, point3.Z, value);
				break;
			case CreateBlockType.Catch:
				chunkData.SetBlock(point3, value);
				break;
		}
	}

	public void SetBlock(int x, int y, int z, int value)
	{
		try
		{
			SubsystemTerrain subsystemTerrain = componentMiner.Project.FindSubsystem<SubsystemTerrain>(true);
			if (!subsystemTerrain.Terrain.IsCellValid(x, y, z))
			{
				return;
			}
			TerrainChunk terrainChunk = subsystemTerrain.Terrain.GetChunkAtCell(x, z);
			if (terrainChunk == null)
			{
				if (!UnLimitedOfCreate)
				{
					return;
				}
				terrainChunk = subsystemTerrain.Terrain.AllocateChunk(x >> 4, z >> 4);
				while (terrainChunk.ThreadState < TerrainChunkState.Valid)
				{
					subsystemTerrain.TerrainUpdater.UpdateChunkSingleStep(terrainChunk, 15);
				}
			}
			terrainChunk.Cells[y + (x & 0xF) * 256 + (z & 0xF) * 256 * 16] = value;
			terrainChunk.ModificationCounter++;
			if (UnLimitedOfCreate)
			{
				terrainChunk.State = TerrainChunkState.Valid;
			}
			if (terrainChunk.State > TerrainChunkState.InvalidLight)
			{
				terrainChunk.State = TerrainChunkState.InvalidLight;
			}
			terrainChunk.WasDowngraded = true;
		}
		catch (Exception ex)
		{
			Log.Error(CreatorMain.Display_Key_Dialog("creatorAPIsetblock") + ex.Message);
		}
	}
}
