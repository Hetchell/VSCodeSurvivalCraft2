// CreatorWandModAPI.CreatorWidget
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CreatorWandModAPI;
using CreatorWand2;
using Engine;
using Game;
using TemplatesDatabase;
using Color = Engine.Color;

public class CreatorWidget : CanvasWidget
{
	private readonly ComponentPlayer player;

	private readonly CreatorAPI creatorAPI;

	private readonly WorldSettings worldSettings;

	private readonly SubsystemTerrain subsystemTerrain;

	private readonly ButtonWidget SphereButton;

	private readonly ButtonWidget PrismButton;

	private readonly ButtonWidget PyramidButton;

	private readonly ButtonWidget CylindricalButton;

	private readonly ButtonWidget PillarsButton;

	private readonly ButtonWidget PrismColumnButton;

	private readonly ButtonWidget RectangularButton;

	private readonly ButtonWidget CircleButton;

	private readonly ButtonWidget MazeButton;

	private readonly ButtonWidget SpiralButton;

	private readonly ButtonWidget LevelSetButton;

	private readonly ButtonWidget TransferButton;

	private readonly ButtonWidget SetPositionButton;

	private readonly ButtonWidget RevokeButton;

	private readonly ButtonWidget SetSpawn;

	private readonly ButtonWidget SetLinkButton;

	private readonly ButtonWidget RemoveItemButton;

	private readonly ButtonWidget RemoveAnimalButton;

	private readonly ButtonWidget MountainButton;

	private readonly ButtonWidget SetModeButton;

	private readonly ButtonWidget SetButton;

	private readonly ButtonWidget ClearCacheButton;

	private readonly ButtonWidget CopyPasteButton;

	private readonly ButtonWidget OnekeyButton;

	private readonly ButtonWidget ReplaceButton;

	private readonly ButtonWidget ModButton;

	private readonly ButtonWidget EditRegionButton;

	private readonly ButtonWidget EditWorldButton;

	private readonly ButtonWidget PenetrateButton;

	private readonly ButtonWidget TerrainTestButton;

	private readonly ButtonWidget FillingButton;

	private readonly ButtonWidget PavageButton;

	private readonly ButtonWidget ClearBlockButton;

	private readonly ButtonWidget SetPositionCarefulButton;

	private readonly ButtonWidget AdjustPositionButton;

	private readonly ButtonWidget SetDifficultyButton;

	private readonly ButtonWidget ThreePointPlaneButton;

	private readonly ButtonWidget FourPointSpaceButton;

	private readonly ButtonWidget LightWorldButton;

	private readonly ButtonWidget WeatherButton;

	private readonly ButtonWidget LangugeButton;

	private int ViewButton;

	private bool IsRain;

	public CreatorWidget(CreatorAPI creatorAPI)
	{
		player = creatorAPI.componentMiner.ComponentPlayer;
		this.creatorAPI = creatorAPI;
		XElement node = ((!creatorAPI.oldMainWidget) ? ContentManager.Get<XElement>("NewCreatorAPIWidget") : ContentManager.Get<XElement>("CreatorAPIWidget"));
		LoadChildren(this, node);
		worldSettings = player.Project.FindSubsystem<SubsystemGameInfo>(true).WorldSettings;
		subsystemTerrain = player.Project.FindSubsystem<SubsystemTerrain>(true);
		SphereButton = Children.Find<ButtonWidget>("Sphere");
		SphereButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Sphere");
		PrismButton = Children.Find<ButtonWidget>("FCC Octahedron");
		PrismButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "FCC Octahedron");
		PyramidButton = Children.Find<ButtonWidget>("BCC Octahedron");
		PyramidButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "BCC Octahedron");
		CylindricalButton = Children.Find<ButtonWidget>("Cylinder");
		CylindricalButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Cylinder");
		PrismColumnButton = Children.Find<ButtonWidget>("FCC Square Column");
		PrismColumnButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "FCC Square Column");
		PillarsButton = Children.Find<ButtonWidget>("BCC Square Column");
		PillarsButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "BCC Square Column");
		RectangularButton = Children.Find<ButtonWidget>("Cuboid");
		RectangularButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Cuboid");
		CircleButton = Children.Find<ButtonWidget>("Torus");
		CircleButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Torus");
		MazeButton = Children.Find<ButtonWidget>("Maze");
		MazeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Maze");
		FillingButton = Children.Find<ButtonWidget>("Fill");
		FillingButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Fill");
		PavageButton = Children.Find<ButtonWidget>("Tile");
		PavageButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Tile");
		MountainButton = Children.Find<ButtonWidget>("Mountains");
		MountainButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Mountains");
		TransferButton = Children.Find<ButtonWidget>("Teleport");
		TransferButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Teleport");
		LevelSetButton = Children.Find<ButtonWidget>("Level");
		LevelSetButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Level");
		SetButton = Children.Find<ButtonWidget>("Mod Settings");
		SetButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Mod Settings");
		SpiralButton = Children.Find<ButtonWidget>("Spiral");
		SpiralButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Spiral");
		PenetrateButton = Children.Find<ButtonWidget>("Disable Collision");
		PenetrateButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Disable Collision");
		SetLinkButton = Children.Find<ButtonWidget>("Connect 2 Points");
		SetLinkButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Connect 2 Points");
		SetPositionButton = Children.Find<ButtonWidget>("Set the Point");
		SetPositionButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Set the Point");
		RevokeButton = Children.Find<ButtonWidget>("Undo");
		RevokeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Undo");
		SetSpawn = Children.Find<ButtonWidget>("Set the Spawn Point");
		SetSpawn.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Set the Spawn Point");
		EditWorldButton = Children.Find<ButtonWidget>("World Settings");
		EditWorldButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "World Settings");
		EditRegionButton = Children.Find<ButtonWidget>("Edit Chunk");
		EditRegionButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Edit Chunk");
		ModButton = Children.Find<ButtonWidget>("Custom Module");
		ModButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Custom Module");
		ReplaceButton = Children.Find<ButtonWidget>("Replace");
		ReplaceButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Replace");
		OnekeyButton = Children.Find<ButtonWidget>("One Click Generation");
		OnekeyButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "One Click Generation");
		CopyPasteButton = Children.Find<ButtonWidget>("Copy and Paste");
		CopyPasteButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Copy and Paste");
		ClearCacheButton = Children.Find<ButtonWidget>("Clear Cache");
		ClearCacheButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Clear Cache");
		SetModeButton = Children.Find<ButtonWidget>("Game Mode");
		SetModeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Game Mode");
		RemoveItemButton = Children.Find<ButtonWidget>("Remove Drops");
		RemoveItemButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Remove Drops");
		RemoveAnimalButton = Children.Find<ButtonWidget>("Despawn Entities");
		RemoveAnimalButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Despawn Entities");
		ClearBlockButton = Children.Find<ButtonWidget>("Clear Blocks");
		ClearBlockButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Clear Blocks");
		TerrainTestButton = Children.Find<ButtonWidget>("Terrain");
		TerrainTestButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Terrain");
		SetPositionCarefulButton = Children.Find<ButtonWidget>("Custom Point");
		SetPositionCarefulButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Custom Point");
		SetPositionCarefulButton.IsEnabled = false;
		AdjustPositionButton = Children.Find<ButtonWidget>("Point adjustment");
		AdjustPositionButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Point adjustment");
		AdjustPositionButton.IsEnabled = false;
		SetDifficultyButton = Children.Find<ButtonWidget>("Init Difficulty");
		SetDifficultyButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Init Difficulty");
		ThreePointPlaneButton = Children.Find<ButtonWidget>("Connect 3 Points");
		ThreePointPlaneButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Connect 3 Points");
		FourPointSpaceButton = Children.Find<ButtonWidget>("Connect 4 Points");
		FourPointSpaceButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Connect 4 Points");
		LightWorldButton = Children.Find<ButtonWidget>("Light Up Blocks");
		LightWorldButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Light Up Blocks");
		Children.Find<BevelledButtonWidget>("Coming Soon").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Coming Soon");
		Children.Find<BevelledButtonWidget>("Coming Soon").Color = Color.White;
		LangugeButton = Children.Find<ButtonWidget>("Language");
		LangugeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Language");
		WeatherButton = Children.Find<ButtonWidget>("Weather");
		WeatherButton.Text = "Weather";
		LangugeButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Language");
		TerrainTestButton.IsEnabled = false;
		IsRain = SubsystemWeather1.subsystem.m_precipitationStartTime < GameManager.Project.FindSubsystem<SubsystemGameInfo>(true).TotalElapsedGameTime;
		LoadProperties(this, node);
	}

	public override void Update()
	{
		if (Children.Find<BevelledButtonWidget>("Coming Soon").IsClicked)
		{
			ViewButton++;
		}
		try
		{
			if (WeatherButton.IsClicked)
			{
				IsRain = !IsRain;
				SubsystemWeather1.SetPrecipitationTime(IsRain);
			}
			else
			{
				bool flag = SubsystemWeather1.subsystem.m_precipitationStartTime < GameManager.Project.FindSubsystem<SubsystemGameInfo>(true).TotalElapsedGameTime;
				Vector3 position = player.ComponentBody.Position;
				string str = ((SubsystemWeather1.subsystem.GetPrecipitationShaftInfo((int)position.X, (int)position.Z).Type == PrecipitationType.Rain) ? CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Rain:") : CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "Snow:"));
				string str2 = ((SubsystemWeather1.subsystem.GetPrecipitationShaftInfo((int)position.X, (int)position.Z).Type == PrecipitationType.Rain) ? CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "No Rain:") : CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "NewCreatorAPIWidget", "No Snow:"));
				WeatherButton.Text = (flag ? (str + (SubsystemWeather1.subsystem.m_precipitationEndTime - GameManager.Project.FindSubsystem<SubsystemGameInfo>(true).TotalElapsedGameTime).ToString("0.0")) : (str2 + (SubsystemWeather1.subsystem.m_precipitationStartTime - GameManager.Project.FindSubsystem<SubsystemGameInfo>(true).TotalElapsedGameTime).ToString("0.0")));
				WeatherButton.Color = ((!flag) ? Color.White : ((SubsystemWeather1.subsystem.GetPrecipitationShaftInfo((int)position.X, (int)position.Z).Type == PrecipitationType.Rain) ? Color.Blue : Color.White));
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Err:" + ex);
		}
		switch (ViewButton)
		{
			case 0:
				{
					string vec;
					switch (CW2Matrix.Vector3ToFace(player.ComponentMiner.ComponentCreature.ComponentCreatureModel.EyeRotation.GetForwardVector()).X)
					{
						case 0:
							vec = "North";
							break;
						case 1:
							vec = "West";
							break;
						case 2:
							vec = "South";
							break;
						case 3:
							vec = "East";
							break;
						case 4:
							vec = "Up";
							break;
						default:
							vec = "Down";
							break;
					}
					Children.Find<BevelledButtonWidget>("Coming Soon").Text = "View: " + vec;
					break;
				}
			case 1:
				Children.Find<BevelledButtonWidget>("Coming Soon").Text = CreatorMain.version;
				break;
			case 2:
				Children.Find<BevelledButtonWidget>("Coming Soon").Text = "Coming Soon";
				break;
			default:
				Children.Find<BevelledButtonWidget>("Coming Soon").Text = "Coming Soon";
				ViewButton = 0;
				break;
		}
		if (SphereButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new SphereDialog(creatorAPI));
		}
		if (PrismButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new PrismDialog(creatorAPI));
		}
		if (PyramidButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new PyramidDialog(creatorAPI));
		}
		if (CylindricalButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new CylindricalDialog(creatorAPI));
		}
		if (PrismColumnButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new PrismColumnDialog(creatorAPI));
		}
		if (PillarsButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new PillarsDialog(creatorAPI));
		}
		if (RectangularButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new RectangularDialog(creatorAPI));
		}
		if (CircleButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new CircleDialog(creatorAPI));
		}
		if (MazeButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new MazeDialog(creatorAPI));
		}
		if (PavageButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new PavageDialog(creatorAPI));
		}
		if (FillingButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new NewFillingDialog(creatorAPI));
		}
		if (MountainButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new MountainDialog(creatorAPI));
		}
		if (TransferButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new TransferDialog(creatorAPI));
		}
		if (LevelSetButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new LevelSetDialog(creatorAPI));
		}
		if (SetButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new SetDialog(creatorAPI));
		}
		if (SpiralButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new SpiralDialog(creatorAPI));
		}
		if (PenetrateButton.IsClicked)
		{
			if (CreatorMain.Penetrate)
			{
				foreach (int item in CreatorMain.PenetrateBlocksID)
				{
					BlocksManager.Blocks[item].IsCollidable = true;
				}
				CreatorMain.PenetrateBlocksID.Clear();
			}
			else
			{
				worldSettings.EnvironmentBehaviorMode = EnvironmentBehaviorMode.Static;
				Block[] blocks = BlocksManager.Blocks;
				foreach (Block block in blocks)
				{
					if (block.IsCollidable)
					{
						block.IsCollidable = false;
						CreatorMain.PenetrateBlocksID.Add(block.BlockIndex);
					}
				}
			}
			CreatorMain.Penetrate = !CreatorMain.Penetrate;
		}
		if (LightWorldButton.IsClicked)
		{
			if (CreatorMain.LightWorld)
			{
				foreach (int item2 in CreatorMain.LightWorldBlockID)
				{
					BlocksManager.Blocks[item2].DefaultEmittedLightAmount = 0;
				}
				CreatorMain.LightWorldBlockID.Clear();
			}
			else
			{
				Block[] blocks = BlocksManager.Blocks;
				foreach (Block block2 in blocks)
				{
					if (block2.DefaultEmittedLightAmount == 0)
					{
						block2.DefaultEmittedLightAmount = 15;
						CreatorMain.LightWorldBlockID.Add(block2.BlockIndex);
					}
				}
			}
			CreatorMain.LightWorld = !CreatorMain.LightWorld;
		}
		LightWorldButton.Color = ((!CreatorMain.LightWorld) ? Color.White : Color.Yellow);
		PenetrateButton.Color = ((!CreatorMain.Penetrate) ? Color.White : Color.Yellow);
		if (CreatorMain.Position != null)
		{
			if (CreatorMain.Position[0].Y > 0 && CreatorMain.Position[1].Y > 0)
			{
				EditRegionButton.IsEnabled = true;
			}
			else
			{
				EditRegionButton.IsEnabled = false;
			}
		}
		else
		{
			EditRegionButton.IsEnabled = false;
		}
		if (EditRegionButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new NewEditChunkDialog(creatorAPI));
		}
		if (EditWorldButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new EditWorldDialog(creatorAPI));
		}
		if (ClearBlockButton.IsClicked)
		{
			player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("creatorWidgetclear"), Color.LightRed, true, false);
			creatorAPI.ClearBlock = !creatorAPI.ClearBlock;
		}
		ClearBlockButton.Color = ((!creatorAPI.ClearBlock) ? Color.White : Color.Yellow);
		if (SetLinkButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new TwoPointLineDialog(creatorAPI));
		}
		if (SetPositionButton.IsClicked)
		{
			Vector3 position2 = player.ComponentBody.Position;
			Point3 point3 = new Point3((int)MathUtils.Floor( position2.X), (int)MathUtils.Floor(position2.Y), (int)MathUtils.Floor(position2.Z));
			DialogsManager.ShowDialog(null, new ListSelectionDialog(CreatorMain.Display_Key_Dialog("creatorWidgetsetpt"), new int[4] { 1, 2, 3, 4 }, 56f, (object e) => string.Format("{0}{1}", CreatorMain.Display_Key_Dialog("creatorWidgetsetp1"), (int)e), delegate (object e)
			{
				creatorAPI.Position[(int)e - 1] = point3;
				player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorWidgetsetpd"), (int)e, point3.X, point3.Y, point3.Z), Color.Yellow, true, true);
			}));
		}
		if (creatorAPI.RevokeSwitch)
		{
			if (creatorAPI.revokeData == null)
			{
				RevokeButton.IsEnabled = false;
			}
			else
			{
				RevokeButton.IsEnabled = true;
			}
		}
		else
		{
			RevokeButton.IsEnabled = false;
		}
		if (RevokeButton.IsClicked)
		{
			RevokeButton.IsEnabled = false;
			creatorAPI.revokeData = null;
			subsystemTerrain.Dispose();
			subsystemTerrain.Load(new ValuesDictionary());
			player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("creatorWidgetundo"), Color.Yellow, true, true);
		}
		if (SetSpawn.IsClicked)
		{
			Vector3 position3 = player.ComponentBody.Position;
			player.PlayerData.SpawnPosition = position3 + new Vector3(0f, 0.1f, 0f);
			player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorWidgetrp"), (int)position3.X, (int)position3.Y, (int)position3.Z), Color.Yellow, true, true);
		}
		if (ModButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new ModDialog(creatorAPI));
		}
		if (ReplaceButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new NewReplaceDialog(creatorAPI));
		}
		if (SetModeButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new SetModeDialog(creatorAPI));
		}
		if (OnekeyButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new OnekeyGenerationDialog(creatorAPI));
		}
		if (CopyPasteButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new CopyPasteDialog(creatorAPI));
		}
		ClearCacheButton.IsEnabled = Directory.Exists(CreatorMain.CacheDirectory);
		if (ClearCacheButton.IsClicked)
		{
			if (CreatorMain.CacheDirectory.Delete())
			{
				for (int i = 0; i < 4; i++)
				{
					creatorAPI.Position[i] = new Point3(0, -1, 0);
				}
				player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("creatorWidgetcls"), Color.Yellow, true, true);
			}
			else
			{
				player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("creatorWidgetclf"), Color.Red, true, true);
			}
		}
		if (RemoveAnimalButton.IsClicked)
		{
			int num = 0;
			foreach (ComponentCreature creature in player.Project.FindSubsystem<SubsystemCreatureSpawn>(true).Creatures)
			{
				if (!(creature is ComponentPlayer))
				{
					creature.ComponentSpawn.Despawn();
					num++;
				}
			}
			player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorWidgetcla"), num), Color.Yellow, true, false);
		}
		if (RemoveItemButton.IsClicked)
		{
			int num2 = 0;
			foreach (Pickable pickable in GameManager.Project.FindSubsystem<SubsystemPickables>(true).Pickables)
			{
				pickable.Count = 0;
				pickable.ToRemove = true;
				num2++;
			}
			player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("creatorWidgetcld"), num2), Color.Yellow, true, false);
		}
		bool isClicked = TerrainTestButton.IsClicked;
		if (SetDifficultyButton.IsClicked)
		{
			Vector3 position4 = player.ComponentBody.Position;
			new Point3((int)position4.X, (int)position4.Y, (int)position4.Z);
			int[] items = new int[3] { 0, 1, 2 };
			string[] difference = new string[3]
			{
				CreatorMain.Display_Key_Dialog("creatorWidgetde"),
				CreatorMain.Display_Key_Dialog("creatorWidgetdm"),
				CreatorMain.Display_Key_Dialog("creatorWidgetdh")
			};
			DialogsManager.ShowDialog(null, new ListSelectionDialog(CreatorMain.Display_Key_Dialog("creatorWidgetseld"), items, 56f, (object e) => difference[(int)e], delegate (object e)
			{
				worldSettings.StartingPositionMode = (StartingPositionMode)e;
				player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("creatorWidgetdd") + difference[(int)e], Color.Yellow, true, true);
			}));
		}
		bool isClicked2 = SetPositionCarefulButton.IsClicked;
		bool isClicked3 = AdjustPositionButton.IsClicked;
		if (ThreePointPlaneButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new ThreePointToPlaneDialog(creatorAPI));
		}
		if (FourPointSpaceButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new FourPointSpaceDialog(creatorAPI));
		}
		if (LangugeButton.IsClicked)
		{
			DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new MessageDialog("Choose a Language", "Please confirm if you want to change the language", "OK", "Cancel", delegate (MessageDialogButton result)
			{
				if (result == MessageDialogButton.Button1)
				{
					List<Tuple<string, Action>> list = new List<Tuple<string, Action>>();
					for (int num3 = Enum.GetNames(typeof(Language)).GetLength(0) - 1; num3 >= 0; num3--)
					{
						int a = num3;
						try
						{
							list.Add(new Tuple<string, Action>(Enum.GetName(typeof(Language), num3).ToString(), delegate
							{
								CreatorMain.Language = (CreatorAPI.Language = (Language)Enum.ToObject(typeof(Language), a));
								CreatorAPI.IsAddedToProject = false;
								Log.Warning(((Language)Enum.ToObject(typeof(Language), a)).ToString() + "  " + a + " is " + CreatorAPI.Language);
							}));
						}
						catch (Exception ex2)
						{
							DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new MessageDialog("Exception", "ERR:" + ex2, "OK", null, null));
						}
					}
					DialogsManager.ShowDialog(player.ViewWidget.GameWidget, new ListSelectionDialog("Choose a Language", list, 60f, (object t) => ((Tuple<string, Action>)t).Item1, delegate (object t)
					{
						((Tuple<string, Action>)t).Item2();
					}));
				}
			}));
		}
		if (!CreatorAPI.IsAddedToProject)
		{
			base.ParentWidget.Children.Clear();
			DialogsManager.HideAllDialogs();
		}
	}

	public static void Dismiss(bool result)
	{
		CreatorAPI.IsAddedToProject = false;
	}
}
