using Engine;
using Engine.Graphics;
using Game;
using GameEntitySystem;
using System.Collections.Generic;
using System.Xml.Linq;
using TemplatesDatabase;
using Color = Engine.Color;
using TerrainContentsGenerator22 = Game.TerrainChunkGeneratorProviderActive;

namespace CreatorWandModAPI
{
    public class EditWorldDialog : Dialog
    {
        private readonly ComponentPlayer player;

        private readonly ButtonWidget paletteButton;

        private readonly RectangleWidget blocksTextureIcon;

        private readonly LabelWidget blocksTextureLabel;

        private readonly LabelWidget blocksTextureDetails;

        private readonly ButtonWidget blocksTextureButton;

        private readonly ButtonWidget supernaturalCreaturesButton;

        private readonly ButtonWidget environmentBehaviorButton;

        private readonly ButtonWidget timeOfDayButton;

        private readonly ButtonWidget weatherEffectsButton;

        private readonly ButtonWidget adventureRespawnButton;

        private readonly ButtonWidget adventureSurvivalMechanicsButton;

        private readonly LabelWidget terrainGenerationLabel;

        private readonly ButtonWidget terrainGenerationButton;

        private readonly SliderWidget seaLevelOffsetSlider;

        private readonly SliderWidget temperatureOffsetSlider;

        private readonly SliderWidget humidityOffsetSlider;

        private readonly SliderWidget biomeSizeSlider;

        private readonly Widget islandTerrainPanel;

        private readonly SliderWidget islandSizeNS;

        private readonly SliderWidget islandSizeEW;

        private readonly Widget flatTerrainPanel;

        private readonly Widget continentTerrainPanel;

        private readonly SliderWidget flatTerrainLevelSlider;

        private readonly BlockIconWidget flatTerrainBlock;

        private readonly ButtonWidget flatTerrainBlockButton;

        private readonly CheckboxWidget flatTerrainMagmaOceanCheckbox;

        private readonly ButtonWidget OKButton;

        private readonly ButtonWidget UpdataWorldButton;

        private readonly ButtonWidget UpdataButton;

        private readonly WorldSettings worldSettings;

        private readonly float[] islandSizes = new float[20]
        {
            30f,
            40f,
            50f,
            60f,
            80f,
            100f,
            120f,
            150f,
            200f,
            250f,
            300f,
            400f,
            500f,
            600f,
            800f,
            1000f,
            1200f,
            1500f,
            2000f,
            2500f
        };

        private readonly float[] biomeSizes = new float[7]
        {
            0.33f,
            0.5f,
            0.75f,
            1f,
            1.5f,
            2f,
            3f
        };

        private readonly BlocksTexturesCache blockTexturesCache = new BlocksTexturesCache();

        public EditWorldDialog(CreatorAPI creatorAPI)
        {
            player = creatorAPI.componentMiner.ComponentPlayer;
            XElement node = ContentManager.Get<XElement>("Dialog/EditWorld");
            LoadChildren(this, node);
            worldSettings = player.Project.FindSubsystem<SubsystemGameInfo>(throwOnError: true).WorldSettings;
            OKButton = Children.Find<ButtonWidget>("OK");
            OKButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditWorld", "OK");
            UpdataWorldButton = Children.Find<ButtonWidget>("Reload");
            UpdataWorldButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditWorld", "Reload");
            UpdataButton = Children.Find<ButtonWidget>("Refresh");
            UpdataButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "EditWorld", "Refresh");
            paletteButton = Children.Find<ButtonWidget>("Palette");
            blocksTextureIcon = Children.Find<RectangleWidget>("BlocksTextureIcon");
            blocksTextureLabel = Children.Find<LabelWidget>("BlocksTextureLabel");
            blocksTextureDetails = Children.Find<LabelWidget>("BlocksTextureDetails");
            blocksTextureButton = Children.Find<ButtonWidget>("BlocksTextureButton");
            supernaturalCreaturesButton = Children.Find<ButtonWidget>("SupernaturalCreatures");
            environmentBehaviorButton = Children.Find<ButtonWidget>("EnvironmentBehavior");
            timeOfDayButton = Children.Find<ButtonWidget>("TimeOfDay");
            weatherEffectsButton = Children.Find<ButtonWidget>("WeatherEffects");
            adventureRespawnButton = Children.Find<ButtonWidget>("AdventureRespawn");
            adventureSurvivalMechanicsButton = Children.Find<ButtonWidget>("AdventureSurvivalMechanics");
            terrainGenerationLabel = Children.Find<LabelWidget>("TerrainGenerationLabel");
            terrainGenerationButton = Children.Find<ButtonWidget>("TerrainGeneration");
            seaLevelOffsetSlider = Children.Find<SliderWidget>("SeaLevelOffset");
            temperatureOffsetSlider = Children.Find<SliderWidget>("TemperatureOffset");
            humidityOffsetSlider = Children.Find<SliderWidget>("HumidityOffset");
            biomeSizeSlider = Children.Find<SliderWidget>("BiomeSize");
            islandTerrainPanel = Children.Find<Widget>("IslandTerrainPanel");
            islandSizeNS = Children.Find<SliderWidget>("IslandSizeNS");
            islandSizeEW = Children.Find<SliderWidget>("IslandSizeEW");
            flatTerrainPanel = Children.Find<Widget>("FlatTerrainPanel");
            continentTerrainPanel = Children.Find<Widget>("ContinentTerrainPanel");
            flatTerrainLevelSlider = Children.Find<SliderWidget>("FlatTerrainLevel");
            flatTerrainBlock = Children.Find<BlockIconWidget>("FlatTerrainBlock");
            flatTerrainBlockButton = Children.Find<ButtonWidget>("FlatTerrainBlockButton");
            flatTerrainMagmaOceanCheckbox = Children.Find<CheckboxWidget>("MagmaOcean");
            islandSizeEW.MinValue = 0f;
            islandSizeEW.MaxValue = islandSizes.Length - 1;
            islandSizeEW.Granularity = 1f;
            islandSizeNS.MinValue = 0f;
            islandSizeNS.MaxValue = islandSizes.Length - 1;
            islandSizeNS.Granularity = 1f;
            biomeSizeSlider.MinValue = 0f;
            biomeSizeSlider.MaxValue = biomeSizes.Length - 1;
            biomeSizeSlider.Granularity = 1f;
            LoadProperties(this, node);
        }

        public override void Update()
        {
            if (UpdataButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
                SubsystemTerrain subsystemTerrain = GameManager.Project.FindSubsystem<SubsystemTerrain>();
                TerrainChunk[] allocatedChunks = subsystemTerrain.Terrain.AllocatedChunks;
                foreach (TerrainChunk terrainChunk in allocatedChunks)
                {
                    foreach (SubsystemBlockBehavior blockBehavior in GameManager.Project.FindSubsystem<SubsystemBlockBehaviors>().BlockBehaviors)
                    {
                        blockBehavior.OnChunkDiscarding(terrainChunk);
                    }

                    _ = terrainChunk.Coords;
                    _ = terrainChunk.Coords;
                    subsystemTerrain.Dispose();
                    ((Subsystem)subsystemTerrain).Load(new ValuesDictionary());
                    player.ComponentGui.DisplaySmallMessage(CreatorMain.Display_Key_Dialog("editworlddialogs"), Color.LightYellow, blinking: true, playNotificationSound: false);
                }
            }

            if (OKButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
            }

            if (UpdataWorldButton.IsClicked)
            {
                DialogsManager.HideDialog(this);
                WorldInfo worldInfo = GameManager.WorldInfo;
                GameManager.SaveProject(waitForCompletion: true, showErrorDialog: true);
                GameManager.DisposeProject();
                ScreensManager.SwitchScreen("GameLoading", worldInfo, null);
            }

            if (paletteButton.IsClicked)
            {
                DialogsManager.ShowDialog(null, new CreatorWandModAPIEditPaletteDialog(worldSettings.Palette));
            }

            Texture2D texture = blockTexturesCache.GetTexture(worldSettings.BlocksTextureName);
            blocksTextureIcon.Subtexture = new Subtexture(texture, Vector2.Zero, Vector2.One);
            (blocksTextureLabel).Text = (BlocksTexturesManager.GetDisplayName(worldSettings.BlocksTextureName));
            (blocksTextureDetails).Text = ($"{texture.Width}x{texture.Height}");
            if (blocksTextureButton.IsClicked)
            {
                BlocksTexturesManager.UpdateBlocksTexturesList();
                DialogsManager.ShowDialog(null, new ListSelectionDialog("Select Blocks Texture", BlocksTexturesManager.BlockTexturesNames, 64f, delegate (object item)
                {
                    ContainerWidget obj2 = (ContainerWidget)Widget.LoadWidget(this, ContentManager.Get<XElement>("Widgets/BlocksTextureItem"), null);
                    Texture2D texture2 = blockTexturesCache.GetTexture((string)item);
                    (obj2.Children.Find<LabelWidget>("BlocksTextureItem.Text")).Text = (BlocksTexturesManager.GetDisplayName((string)item));
                    (obj2.Children.Find<LabelWidget>("BlocksTextureItem.Details")).Text = ($"{texture2.Width}x{texture2.Height}");
                    obj2.Children.Find<RectangleWidget>("BlocksTextureItem.Icon").Subtexture = new Subtexture(texture2, Vector2.Zero, Vector2.One);
                    return obj2;
                }, delegate (object item)
                {
                    worldSettings.BlocksTextureName = (string)item;
                    SubsystemBlocksTexture subsystemBlocksTexture = GameManager.Project.FindSubsystem<SubsystemBlocksTexture>();
                    subsystemBlocksTexture.Dispose();
                    ((Subsystem)subsystemBlocksTexture).Load(new ValuesDictionary());
                }));
            }

            if (supernaturalCreaturesButton.IsClicked)
            {
                worldSettings.AreSupernaturalCreaturesEnabled = !worldSettings.AreSupernaturalCreaturesEnabled;
            }

            if (environmentBehaviorButton.IsClicked)
            {
                IList<int> enumValues = EnumUtils.GetEnumValues(typeof(EnvironmentBehaviorMode));
                worldSettings.EnvironmentBehaviorMode = (EnvironmentBehaviorMode)((enumValues.IndexOf((int)worldSettings.EnvironmentBehaviorMode) + 1) % enumValues.Count);
            }

            if (timeOfDayButton.IsClicked)
            {
                IList<int> enumValues2 = EnumUtils.GetEnumValues(typeof(TimeOfDayMode));
                worldSettings.TimeOfDayMode = (TimeOfDayMode)((enumValues2.IndexOf((int)worldSettings.TimeOfDayMode) + 1) % enumValues2.Count);
            }

            if (weatherEffectsButton.IsClicked)
            {
                worldSettings.AreWeatherEffectsEnabled = !worldSettings.AreWeatherEffectsEnabled;
            }

            if (adventureRespawnButton.IsClicked)
            {
                worldSettings.IsAdventureRespawnAllowed = !worldSettings.IsAdventureRespawnAllowed;
            }

            if (adventureSurvivalMechanicsButton.IsClicked)
            {
                worldSettings.AreAdventureSurvivalMechanicsEnabled = !worldSettings.AreAdventureSurvivalMechanicsEnabled;
            }

            if (terrainGenerationButton.IsClicked)
            {
                DialogsManager.ShowDialog(null, new ListSelectionDialog("Select World Type", EnumUtils.GetEnumValues(typeof(TerrainGenerationMode)), 56f, (object e) => ((TerrainGenerationMode)e).ToString(), delegate (object e)
                {
                    if (worldSettings.GameMode != 0 && ((TerrainGenerationMode)e == TerrainGenerationMode.FlatContinent || (TerrainGenerationMode)e == TerrainGenerationMode.FlatIsland))
                    {
                        DialogsManager.ShowDialog(null, new MessageDialog("Unavailable", "Flat terrain is only available in Creative Mode", "OK", null, null));
                    }
                    else
                    {
                        worldSettings.TerrainGenerationMode = (TerrainGenerationMode)e;
                        SubsystemTerrain subsystemTerrain2 = GameManager.Project.FindSubsystem<SubsystemTerrain>();
                        if ((TerrainGenerationMode)e == TerrainGenerationMode.FlatContinent || (TerrainGenerationMode)e == TerrainGenerationMode.FlatIsland)
                        {
                            subsystemTerrain2.TerrainContentsGenerator = new TerrainContentsGeneratorFlat(subsystemTerrain2);
                        }
                        else
                        {
                            subsystemTerrain2.TerrainContentsGenerator = new TerrainContentsGenerator22(subsystemTerrain2);
                        }
                    }
                }));
            }

            if (seaLevelOffsetSlider.IsSliding)
            {
                worldSettings.SeaLevelOffset = (int)seaLevelOffsetSlider.Value;
            }

            if (temperatureOffsetSlider.IsSliding)
            {
                worldSettings.TemperatureOffset = temperatureOffsetSlider.Value;
            }

            if (humidityOffsetSlider.IsSliding)
            {
                worldSettings.HumidityOffset = humidityOffsetSlider.Value;
            }

            if (biomeSizeSlider.IsSliding)
            {
                worldSettings.BiomeSize = biomeSizes[MathUtils.Clamp((int)biomeSizeSlider.Value, 0, biomeSizes.Length - 1)];
            }

            if (islandSizeEW.IsSliding)
            {
                worldSettings.IslandSize.X = islandSizes[MathUtils.Clamp((int)islandSizeEW.Value, 0, islandSizes.Length - 1)];
            }

            if (islandSizeNS.IsSliding)
            {
                worldSettings.IslandSize.Y = islandSizes[MathUtils.Clamp((int)islandSizeNS.Value, 0, islandSizes.Length - 1)];
            }

            if (flatTerrainLevelSlider.IsSliding)
            {
                worldSettings.TerrainLevel = (int)flatTerrainLevelSlider.Value;
            }

            if (flatTerrainBlockButton.IsClicked)
            {
                DialogsManager.ShowDialog(null, new ListSelectionDialog("Select Block", new int[20]
                {
                    8,
                    2,
                    7,
                    3,
                    67,
                    66,
                    4,
                    5,
                    26,
                    73,
                    21,
                    46,
                    47,
                    15,
                    62,
                    68,
                    126,
                    71,
                    1,
                    0
                }, 72f, delegate (object index)
                {
                    ContainerWidget obj = (ContainerWidget)Widget.LoadWidget(null, ContentManager.Get<XElement>("Widgets/SelectBlockItem"), null);
                    obj.Children.Find<BlockIconWidget>("SelectBlockItem.Block").Contents = (int)index;
                    (obj.Children.Find<LabelWidget>("SelectBlockItem.Text")).Text = (BlocksManager.Blocks[(int)index].GetDisplayName(null, Terrain.MakeBlockValue((int)index)));
                    return obj;
                }, delegate (object index)
                {
                    worldSettings.TerrainBlockIndex = (int)index;
                }));
            }

            if (flatTerrainMagmaOceanCheckbox.IsClicked)
            {
                worldSettings.TerrainOceanBlockIndex = ((worldSettings.TerrainOceanBlockIndex == 18) ? 92 : 18);
            }

            islandTerrainPanel.IsVisible = (worldSettings.TerrainGenerationMode == TerrainGenerationMode.Island);
            flatTerrainPanel.IsVisible = (worldSettings.TerrainGenerationMode == TerrainGenerationMode.FlatContinent);
            continentTerrainPanel.IsVisible = (worldSettings.TerrainGenerationMode == TerrainGenerationMode.Continent);
            flatTerrainLevelSlider.Value = worldSettings.TerrainLevel;
            flatTerrainLevelSlider.Text = worldSettings.TerrainLevel.ToString();
            flatTerrainBlock.Contents = worldSettings.TerrainBlockIndex;
            flatTerrainMagmaOceanCheckbox.IsChecked = (worldSettings.TerrainOceanBlockIndex == 92);
            seaLevelOffsetSlider.Value = worldSettings.SeaLevelOffset;
            seaLevelOffsetSlider.Text = WorldOptionsScreen.FormatOffset(worldSettings.SeaLevelOffset);
            temperatureOffsetSlider.Value = worldSettings.TemperatureOffset;
            temperatureOffsetSlider.Text = WorldOptionsScreen.FormatOffset(worldSettings.TemperatureOffset);
            humidityOffsetSlider.Value = worldSettings.HumidityOffset;
            humidityOffsetSlider.Text = WorldOptionsScreen.FormatOffset(worldSettings.HumidityOffset);
            biomeSizeSlider.Value = FindNearestIndex(biomeSizes, worldSettings.BiomeSize);
            biomeSizeSlider.Text = worldSettings.BiomeSize + "x";
            islandSizeEW.Value = FindNearestIndex(islandSizes, worldSettings.IslandSize.X);
            islandSizeEW.Text = worldSettings.IslandSize.X.ToString();
            islandSizeNS.Value = FindNearestIndex(islandSizes, worldSettings.IslandSize.Y);
            islandSizeNS.Text = worldSettings.IslandSize.Y.ToString();
            supernaturalCreaturesButton.Text = (worldSettings.AreSupernaturalCreaturesEnabled ? "Enabled" : "Disabled");
            environmentBehaviorButton.Text = worldSettings.EnvironmentBehaviorMode.ToString();
            timeOfDayButton.Text = worldSettings.TimeOfDayMode.ToString();
            weatherEffectsButton.Text = (worldSettings.AreWeatherEffectsEnabled ? "Enabled" : "Disabled");
            adventureRespawnButton.Text = (worldSettings.IsAdventureRespawnAllowed ? "Allowed" : "Not Allowed");
            adventureSurvivalMechanicsButton.Text = (worldSettings.AreAdventureSurvivalMechanicsEnabled ? "Enabled" : "Disabled");
            (terrainGenerationLabel).Text = (worldSettings.TerrainGenerationMode.ToString());
        }

        private int FindNearestIndex(IList<float> list, float v)
        {
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if ((double)MathUtils.Abs(list[i] - v) < (double)MathUtils.Abs(list[num] - v))
                {
                    num = i;
                }
            }

            return num;
        }
    }
}