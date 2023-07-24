using Engine.Input;
using Game;
using System.Diagnostics;
using ModificationHolder;
using Engine;
using System.Threading.Tasks;
using Color = Engine.Color;
using Random = Game.Random;
using static ModificationHolder.ChunkGeneratorOverworldProvider;

namespace Survivalcraft.Game.ModificationHolder
{
    public class ModificationsHolder
    {
        public static bool returnNullMessageOfTheDay = true;
        public static bool allowFlyingAnimal = false;
        public static bool allowWolfDespawn = true;

        public static bool disableDrops = true;
        public static bool fogEnable = false;

        public static bool night_vision = true;
        public static bool allowForUnrestrictedTravel = true;
        public static bool allowFixedSpawnPoint = true;

        public static bool constantSeed = false;
        public static int seed = 1638478217;
        public static float steppedLevelTravel = 10f;
        public static String[] animalTypes = { "Wolf", "Hyena", "Lion" };

        public static String[] mayNotDieFromExplosion = {"Wolf", "Hyena"};
        public static float movementLimitPlayer = 100f * 100f;
        public static float movementLimitAnimalsDerFlying = 300f;
        public static Vector3 spawnfixed = new Vector3(0, 300, 0);

        public static Func<Block, bool> explosionRep = block => block is AirBlock;

        public static bool ableExplosionTileDropping = false;
        public static bool ableExplodeAir = false;

        private static int repeat = 0;
        private static int repeatCom = 0;

        private static int repeatLight;
        private static string somestr;
        private ComponentPlayer componentPlayer;

        private static ComponentGui gui_handler;
        public static Func<Block, bool> explosivesPredicate = block => (block is SnowballBlock) || (block is FireworksBlock);

        public static int score = 0;

        public static Func<Block, int, int> powerPredicate = (block, power) => {
            if(block is SnowballBlock) {
                return power;
            }
            if(block is FireworksBlock) {
                return 2100;
            }
            return (int)Math.Floor(power * 0.5D);
        };

        public static Func<bool, List<int>> spawnEntryModificationFunction = constantSpawn => {
            int num = constantSpawn ? 18 : 24;
			int num2 = constantSpawn ? 4 : 3;
			num = num2 = 300;
            return new List<int>(new int[]{num, num2});
        };

        public static int[] attempts = new int[] {
            10, //10
            2   //2
        };

        private static List<String> predatorAndHostiles = make(new List<String>(), lst => {
            //lst.Add("Brown Cattle");
            lst.Add("Brown Bull");
            lst.Add("Black Bull");
            //lst.Add("Black Cattle");
            lst.Add("White Bull");
            lst.Add("Gray Wolf");
            lst.Add("Coyote");
            lst.Add("Brown Bear");
            lst.Add("Black Bear");
            lst.Add("Polar Bear");
            lst.Add("Rhino");
            lst.Add("Tiger");
            lst.Add("White Tiger");
            lst.Add("Lion");
            lst.Add("Jaguar");
            lst.Add("Leopard");
            lst.Add("Hyena");
            lst.Add("Cave Bear");
            lst.Add("Cave Tiger");
            lst.Add("Cave Lion");
            lst.Add("Cave Jaguar");
            lst.Add("Cave Leopard");
            lst.Add("Cave Hyena");
            lst.Add("Bull Shark");
            lst.Add("Tiger Shark");
            lst.Add("Great White Shark");
            lst.Add("Piranha");
            lst.Add("Orca");
            lst.Add("Wildboar");
        });

        private static Dictionary<String, int> scoreMapping = make(new Dictionary<string, int>(), u => {
            int[] sc = new int[]{
                -5, -5, -100, 
                10, 10, 20, 20, 50, 5, 15, 31, 20, 20, 20, 16, 40, 77, 50, 80, 63, 51, 33, 100, 180, 22, 205,
                3
            };
            int i = 0;
            foreach(String name in predatorAndHostiles) {
                u.Add(name, sc[i]);
                u.Add("Constant " + name, sc[i] + 1);
                i++;
            }
        });


        public struct SpawnListEntryNumbers {
            public static int wolf_top = 3;
            public static int wolf_bottom = 1;
            public static int hyena_top = 2;
            public static int hyena_bottom = 1;

            public static int MAXIMUM_COUNT = 300;
        }

        private static String target;

        public static void keyboardActions(WidgetInput input, ComponentPlayer componentPlayer)
        {
            Color color = Color.Yellow;
            ComponentMiner componentMiner = componentPlayer.ComponentMiner;
            ComponentGui gui_throwback = componentPlayer.ComponentGui;
            ITerrainContentsGenerator terrain = componentPlayer.m_subsystemTerrain.TerrainContentsGenerator;
            DebuggerHelper debugger = new DebuggerHelper(componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage, componentMiner);
            gui_handler = gui_throwback;
            if (input.IsKeyDownOnce(Key.UpArrow) && !input.IsKeyDown(Key.B))
            {
                ComponentInput.speed++;
                Debug.WriteLine("Speed is increased");
            }
            if (input.IsKeyDownOnce(Key.DownArrow) && !input.IsKeyDown(Key.B))
            {
                ComponentInput.speed--;
                Debug.WriteLine("Speed is decreased");
            }
            if (input.IsKeyDown(Key.LeftArrow))
            {
                ComponentInput.state = true;
                ComponentInput.step = ModificationsHolder.steppedLevelTravel;
            }
            else if (input.IsKeyDown(Key.RightArrow))
            {
                ComponentInput.state = true;
                ComponentInput.step = -ModificationsHolder.steppedLevelTravel;
            }
            else
            {
                ComponentInput.state = false;
            }
            if (input.IsKeyDownOnce(Key.N))
            {
                //Console.WriteLine("output is " + repeat);
                if (ComponentInput.repeat == 0)
                {
                    ComponentInput.noclipState = true;
                    ComponentInput.repeat++;
                }
                else
                {
                    --ComponentInput.repeat;
                    ComponentInput.noclipState = false;
                }
            }
            //allow animallist to fly
            if (input.IsKeyDownOnce(Key.Control))
            {
                ModificationsHolder.allowFlyingAnimal = true;
            }
            //allow animallist to drop
            if (input.IsKeyDownOnce(Key.Tab))
            {
                ModificationsHolder.allowFlyingAnimal = false;
            }
            if (input.IsKeyDownOnce(Key.J))
            {
               if (repeat == 0)
               {
                   fogEnable = true;
                   repeat++;
               }
               else
               {
                   --repeat;
                   fogEnable = false;
               }
            }
            //up or down explosive power
            int exppow = SubsystemProjectiles.explosionPower;
            if(input.IsKeyDown(Key.B)) {
                if (input.IsKeyDown(Key.UpArrow))
                {
                    int upstep = 100;
                    if(exppow >= 20000) {
                        upstep = 0;
                        defaultThrowBack("Maximum explosive power of 20000 reached!", gui_throwback);
                        SubsystemProjectiles.explosionPower = Math.Min(exppow, 20000);
                    }
                    SubsystemProjectiles.explosionPower += upstep;
                    Debug.WriteLine("Explosion power increased to: " + SubsystemProjectiles.explosionPower);
                }
                if (input.IsKeyDown(Key.DownArrow))
                {
                    int downstep = 100;
                    if(exppow <= 50) {
                        downstep = 0;
                        defaultThrowBack("Minimum explosive power of 50 reached!", gui_throwback);
                    }
                    SubsystemProjectiles.explosionPower -= downstep;
                    Debug.WriteLine("Explosion power is decreased to: " + SubsystemProjectiles.explosionPower);
                }
            }
            if(input.IsKeyDownOnce(Key.R)) {
               Point3 point3 = Datahandle.Coordbodyhandle(componentMiner.ComponentCreature.ComponentBody.Position);
               DebugHelper(componentMiner, point3, debugger, terrain);
            }
        }

        private static bool CheckSpawnChunks(ITerrainContentsGenerator terrain, Point3 point) {
            Vector3 spawn_pos = terrain.FindCoarseSpawnPosition();
            //survivalcraft spawn chunk is in 800 block radius from point. Don't ask me why it is so large. 
            return Vector3.Distance(spawn_pos, new(point.X, point.Y, point.Z)) < 800;
        }

        private static void DebugHelper(ComponentMiner componentMiner, Point3 point, DebuggerHelper debugger, ITerrainContentsGenerator terrain) {
            int x = point.X;
            int z = point.Z;
            int chunkX = x >> 4;
            int chunkZ = z >> 4;
            string loc = CheckSpawnChunks(terrain, point) ? "Within spawn chunk" : "Outside spawn chunk";
            SubsystemTerrain terrain_char = componentMiner.m_subsystemTerrain;
            SubsystemGameInfo info = terrain_char.SubsystemGameInfo;
            WorldSettings settings = info.WorldSettings;
            Terrain terrainchunkaccessor = terrain_char.Terrain;
            TerrainChunk terrainchunk = terrainchunkaccessor.GetChunkAtCoords(chunkX, chunkZ);
            int[] slicecontenthashes = terrainchunk.SliceContentsHashes;
            String time = "Time Passed = " + info.TotalElapsedGameTime;
            String seed = "World seed : " + info.WorldSeed;
            String worldname = "World name: " + settings.Name;
            String islandsize = "Island size: " + settings.IslandSize;
            String statistics = "SR: " + settings.ShoreRoughness;
            statistics += ", BS : " + settings.BiomeSize;
            statistics += ", Toff: " + settings.TemperatureOffset;
            statistics += ", Hoff: " + settings.HumidityOffset;
            string chunkloc = "Chunk Location = (" + chunkX + "," + chunkZ + ")";
            String from_terraingenerator = terrain.GetType(debugger);
            string terraininfo = "SliceContentHashSum: " + Calculator.HashSpecSum(slicecontenthashes);
            terraininfo += ", T(x, z): " + Calculator.AvgArrayWithFunction(terrainchunk.Shafts, Terrain.ExtractTemperature); 
            terraininfo += ", H(x, z): " + Calculator.AvgArrayWithFunction(terrainchunk.Shafts, Terrain.ExtractHumidity);
            terraininfo += ", \nF{}: " + Calculator.ToArrayString(terrainchunk.FogEnds);
            if (terrain is ChunkGeneratorOverworldProvider) {
                terraininfo += "\nNoiseFactors: " + from_terraingenerator;
                terraininfo += ", Generator: " + PassGeneratorType(x, z, terrainchunk).ToString();
            } else {
                terraininfo = "TerrainGenInfo: " + from_terraingenerator;
            }
            debugger
               .AddToDebugger(chunkloc)
               .AddToDebugger(loc)
               .AddToDebugger(time)
               .AddToDebugger(seed + "#" + worldname)
               .AddToDebugger(islandsize + "," + statistics)
               .AddToDebugger(terraininfo)
               .Print();
        }

        private static WorldProviderState PassGeneratorType(int x, int z, TerrainChunk terrainChunk) {
            int marker = (Math.Abs(z) - 1) / 8;
            marker %= 2;
            if (marker == 0) {
                if (z < 0) {
                    return terrainChunk.provider_state_pass2;
                }
                return terrainChunk.provider_state_pass1;
            }
            if (marker == 1) {
                if (z < 0) {
                    return terrainChunk.provider_state_pass1;
                }
                return terrainChunk.provider_state_pass2;
            }
            throw new Exception("Marker value somehow became wrong!");
        }

        public static void UpdateCommand(){
            // Task.Run(() => {
            //     while(true){
            //         string input = Console.ReadLine();
            //         if(input.Contains("Exit")){
            //             Window.Close();
            //         }
            //     }
            // });
        }

        public static void defaultThrowBack(String msg, ComponentGui instance) {
            instance.DisplaySmallMessage(msg, Color.LightGreen, true, false);
        }

        public static void displayImpulse(Vector3 impulse, ComponentBody componentBody) {
            float x = impulse.X;
            float y = impulse.Y;
            float z = impulse.Z;
            float t = impulse.LengthSquared();
            string sign_j = y < 0 ? " - " : " + ";
            string sign_k = z < 0 ? " - " : " + ";
            if(t > 0.001) {
                Debug.WriteLine("Type of [" + componentBody.getCreature().DisplayName.ToLower() + "] given impulse of " + x + "î" + sign_j + Math.Abs(y) + "ĵ" + sign_k + Math.Abs(z) + "k̂ Ns.");
            }
        }

        public static unsafe void getDamage(ComponentBody body, Random random, float* damage) {
            bool result = false;
            for (int i = 0; i < mayNotDieFromExplosion.Length; i++)
            {
                bool s = false;
                if (body.getCreature() != null) {
                    s = body.getCreature().DisplayName.Contains(mayNotDieFromExplosion[i]);
                }
                result = result || s;
            }
            if(result) {
                float r = random.NormalFloat(0.48f, 1.97f);
                r = Math.Abs(r);
                if(r < 0.5f) {
                    *damage += 0.2f;
                } else if (r < 1.0f) {
                    *damage /= 5.0f;
                } else {
                    *damage = 0.0f;
                }
                random.skip((int)*damage);
            }
        }

        public static Action<Vector3, IInventory> processDeath(ComponentCreature creature, ComponentHealth health) {
            String cause = health.CauseOfDeath;
            if(cause == "Blasted by explosion") {
                scoreKeeper(creature, cause);
                defaultThrowBack(creature.DisplayName + " has exploded!", gui_handler);
            }
            if (disableDrops) {
                return (pos, inv) => {};
            } else {
                return (pos, inv) => inv.DropAllItems(pos);
            }
        }

        public static void scoreKeeper(ComponentCreature creature, String CauseOfDeath) {
            String name = creature.DisplayName;
            int i;
            if (name == target) {
                    i = 310 + get(target);
            } else {
                    i = get(name);
            }
            score += i;
        }

        public static int get(String key) {
            return scoreMapping.GetValueOrDefault<String, int>(key, -200);
        }

        public static void changeTarget(SubsystemTime time, Random rand) {
            int time_delay = rand.Int(30, 90);
            int list_length = predatorAndHostiles.Count - 1;
            int idx = rand.Int(0, list_length);
            String name = predatorAndHostiles[idx];
            if (time.PeriodicGameTimeEvent(time_delay, 2)) {
                target = name;
                gui_handler.DisplayLargeMessage("Target: " + target, "Worth: " + (int)(get(target) + 310), time_delay, 0f);
            }
        }

        public static T make<T>(T t, Action<T> item) {
            item(t);
            return t;
        }

        public static class NoiseConstants
        {
            struct Turbulence{
            /* insert description */
            public static float MinTurbulence = 0.04f;
            /* insert description */
            public static float TurbulenceZero = 0.84f;
            /* insert description */
            public static float TurbulenceStrgth = 55f;
            /* insert description */
            public static float TurbulenceFreq = 0.03f;
            /* insert description */
            public static float TurbulenceOctaves = 16f;
            /* insert description */
            public static float TurbulencePersistence = 0.5f;
            }
            
            struct Bias{
                /* insert description */
                public static float DensityBias = 55f;
                 /* insert description */
                public static float HeightBias = 1f;
            }
            
            /* insert description */
            public static float ShoreFluctuationMul = 1.0f;
            /* insert description */
            public static float ShoreFluctuationScMul = 1.0f;

            struct Mountain{
                /* insert description */
                public static float MountainRangeFreq = 0.0006f;
                /* insert description */
                public static float MountainPct = 0.15f;
                /* insert description */
                public static float MountainsStrgth = 220f;
            }

            struct Hill{
                /* insert description */
                public static float HillPct = 0.32f;
            
                /* insert description */
                public static float HillFreq = 0.014f;
                /* insert description */
                public static float HillOct = 1f;
                /* insert description */
                public static float HillPersistence = 0.5f;
                /* insert description */
                public static float HillsStrgth = 32f;
            
            }
            
            /* insert description */
            public static float OceanSlope = 0.006f;
            /* insert description */
            public static float SlopeVar = 0.004f;
            /* insert description */
            public static float IslandFreq = 0.01f;
            /* insert description */
            public static float RiversStrgth = 1f;
            public static float[] constants = new float[]
            {
                Turbulence.MinTurbulence, //TGMinTurbulence0
                Turbulence.TurbulenceZero, //TGTurbulenceZero1
                Turbulence.TurbulenceStrgth, //TGTurbulenceStrength2
                Turbulence.TurbulenceFreq, //TGTurbulenceFreq3
                Turbulence.TurbulenceOctaves, //TGTurbulenceOctaves4
                Turbulence.TurbulencePersistence, //TGTurbulencePersistence5
                Bias.DensityBias, //TGDensityBias6
                ShoreFluctuationMul, //TGShoreFluctuationsMultiplier7
                ShoreFluctuationScMul, //TGShoreFluctuationsScalingMultiplier8
                Mountain.MountainRangeFreq, //TGMountainRangeFreq9
                OceanSlope, //this.TGOceanSlope = 10
                SlopeVar, //TGOceanSlopeVariation11
                IslandFreq, //this.TGIslandsFrequency = 12
                Hill.HillPct, //this.TGHillsPercentage = 13
                Mountain.MountainPct, //this.TGMountainsPercentage = 14 --> This one, with lower value, squeeze mountain width
                Hill.HillFreq, //this.TGHillsFrequency = 15
                Hill.HillOct, //this.TGHillsOctaves = 16
                Hill.HillPersistence, //this.TGHillsPersistence = 17
                Bias.HeightBias, //this.TGHeightBias = 18
                Hill.HillsStrgth, //this.TGHillsStrength = 19
                Mountain.MountainsStrgth, //this.TGMountainsStrength = 20
                RiversStrgth, //this.TGRiversStrength = 21
                1f //OceanLevelMultiplier22
            };
        }
    }
}
