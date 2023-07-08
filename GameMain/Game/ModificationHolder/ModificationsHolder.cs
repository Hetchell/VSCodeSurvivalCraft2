using Engine.Input;
using Game;
using System.Diagnostics;
using ModificationHolder;
using Engine;
using System.Threading.Tasks;
using Color = Engine.Color;
using Random = Game.Random;
using System.Runtime.CompilerServices;
using OpenTK.Platform.Windows;

namespace Survivalcraft.Game.ModificationHolder
{
    public class ModificationsHolder
    {
        public readonly struct MobBehaviour
        {
            private readonly static bool ChangeValue = true;
            public static readonly Func<float, float> range_func = p =>
            {
                return ChangeValue ? 50.0f : p;
            };
            public static readonly Func<float, float> range_maxr_f = p =>
            {
                return ChangeValue ? 50.0f : p;
            };
            public static readonly Func<float, float> chasetime_f = p =>
            {
                return ChangeValue ? 50.0f : p;
            };
            public static readonly Func<bool, bool> persistence_f = z =>
            {
                return ChangeValue || z;
            };
        }
        public const bool infighting = false;
        public const bool creative_angry = true;
        private static bool AllowFlyingAnimalState = false;
        public static bool AllowFlyingAnimal
        {
            get { return AllowFlyingAnimalState; }
        }
        public static readonly bool allowWolfDespawn = true;

        public static readonly bool disableDrops = true;

        private static bool FogEnableState = false;
        public static bool FogEnable
        {
            get { return FogEnableState; }
        }

        private int shots_fired = 0;

        public static readonly bool night_vision = true;
        public static readonly bool allowForUnrestrictedTravel = true;
        public static readonly bool allowFixedSpawnPoint = true;

        public static readonly bool constantSeed = false;
        public static readonly int seed = 1638478217;
        public static readonly float steppedLevelTravel = 10f;
        public static readonly String[] animalTypes = { "Wolf", "Hyena", "Lion" };

        public static readonly String[] mayNotDieFromExplosion = {"Wolf", "Hyena"};
        public static readonly float movementLimitPlayer = 100f * 100f;
        public static readonly float movementLimitAnimalsDerFlying = 300f;
        public static readonly Vector3 spawnfixed = new(0, 300, 0);

        public static readonly Func<Block, bool> explosionRep = block => block is AirBlock;

        public static readonly bool ableExplosionTileDropping = false;
        public static readonly bool ableExplodeAir = false;

        private static int repeat = 0;
        private static string somestr;

        public static readonly Func<Block, bool> explosivesPredicate = block => (block is SnowballBlock) || (block is FireworksBlock);



        public const int fireworks_yield = 1600;

        public static readonly Func<Block, int, int> powerPredicate = (block, power) => {
            if(block is SnowballBlock) {
                return power;
            }
            if(block is FireworksBlock) {
                return fireworks_yield;
            }
            return (int)Math.Floor(power * 0.5D);
        };
        private const bool modifydelayfirework = true;
        public static readonly Func<float, float> fireworkDelayF = u =>
        {
            if (modifydelayfirework)
            {
                return 10.0f;
            } else
            {
                return u;
            }
        };

        public static readonly Func<bool, List<int>> spawnEntryModificationFunction = constantSpawn => {
            int num = constantSpawn ? 18 : 24;
			int num2 = constantSpawn ? 4 : 3;
			num = num2 = 300;
            return new List<int>(new int[]{num, num2});
        };

        public static readonly int[] attempts = new int[] {
            10, //10
            2   //2
        };

        private static readonly List<String> predatorAndHostiles = Make(new List<String>(), lst => {
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
            lst.Add("Bull Shark");
            lst.Add("Tiger Shark");
            lst.Add("Great White Shark");
            lst.Add("Piranha");
            lst.Add("Orca");
            lst.Add("Wildboar");
            lst.Add("Werewolf");
        });

        private static readonly Dictionary<String, int> scoreMapping = Make(new Dictionary<string, int>(), u => {
            int[] sc = new int[]{
                -5, -5, -100, 
                10, 10, 20, 20, 50, 5, 15, 31, 20, 20, 20, 16, 33, 100, 180, 22, 205,
                3,
                61
            };
            int i = 0;
            foreach(String name in predatorAndHostiles) {
                u.Add(name, sc[i]);
                u.Add("Constant " + name, sc[i] + 1);
                i++;
            }
        });


        public readonly struct SpawnListEntryNumbers {
            public static readonly int wolf_top = 3;
            public static readonly int wolf_bottom = 1;
            public static readonly int hyena_top = 2;
            public static readonly int hyena_bottom = 1;

            public static readonly int MAXIMUM_COUNT = 300;
        }

        private static String? target;
        private static readonly int bonus = 1310;
        private static int score = 0;
        public int Score
        {
            get { return shots_fired == 0 ? 0 : score / shots_fired; }
        }
        public double Effective
        {
            get {
                int mean = this.MeanScore();
                return mean == 0 ? 0 : (double)Score / mean;
            }
        }
#pragma warning disable IDE1006
        private List<String> creatureListGrabbed { get; set; }
#pragma warning restore IDE1006
        private readonly ComponentPlayer componentPlayer;
        private readonly ComponentGui componentGui;
        private readonly SubsystemTime gametime;

        public static readonly bool stopTime = true;

        //not used for now...
        public ModificationsHolder(ComponentPlayer componentPlayer, ComponentGui componentGui, SubsystemTime gametime)
        {
            this.componentPlayer = componentPlayer;
            this.componentGui = componentGui;
            this.gametime = gametime;
            this.creatureListGrabbed = new List<String>();
        }

        public void IncrementShotCount()
        {
            this.shots_fired++;
        }

        public int GetShots()
        {
            return this.shots_fired;
        }

        public void KeyboardActions(WidgetInput input)
        {
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
                if (ComponentBody.Repeat == 0)
                {
                    ComponentBody.NoclipState = true;
                    ComponentBody.Repeat++;
                }
                else
                {
                    --ComponentBody.Repeat;
                    ComponentBody.NoclipState = false;
                }
            }
            //allow animallist to fly
            if (input.IsKeyDownOnce(Key.Control))
            {
                AllowFlyingAnimalState = true;
            }
            //allow animallist to drop
            if (input.IsKeyDownOnce(Key.Tab))
            {
                AllowFlyingAnimalState = false;
            }
            if (input.IsKeyDownOnce(Key.J))
            {
               if (repeat == 0)
               {
                   FogEnableState = true;
                   repeat++;
               }
               else
               {
                   --repeat;
                   FogEnableState = false;
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
                        DefaultThrowBack("Maximum explosive power of 20000 reached!");
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
                        DefaultThrowBack("Minimum explosive power of 50 reached!");
                    }
                    SubsystemProjectiles.explosionPower -= downstep;
                    Debug.WriteLine("Explosion power is decreased to: " + SubsystemProjectiles.explosionPower);
                }
            }
            // if(input.IsKeyDownOnce(Key.L) && input.IsKeyDown(Key.Shift)) {
            //     if (repeatLight == 0)
            //    {
            //        night_vision = true;
            //        repeatLight++;
            //    }
            //    else
            //    {
            //        --repeatLight;
            //        night_vision = false;
            //    }
            //    LightingManager.Initialize();
            // }
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

        public void DefaultThrowBack(String msg) {
            this.componentGui.DisplaySmallMessage(msg, Color.LightGreen, true, false);
        }

        public static void DisplayImpulse(Vector3 impulse, ComponentBody componentBody) {
            float x = impulse.X;
            float y = impulse.Y;
            float z = impulse.Z;
            float t = impulse.LengthSquared();
            string sign_j = y < 0 ? " - " : " + ";
            string sign_k = z < 0 ? " - " : " + ";
            if(t > 0.001 && componentBody.getCreature() != null) {
                Debug.WriteLine("Type of [" + componentBody.getCreature().DisplayName.ToLower() + "] given impulse of " + x + "î" + sign_j + Math.Abs(y) + "ĵ" + sign_k + Math.Abs(z) + "k̂ Ns.");
            }
        }

        public static unsafe void GetDamage(ComponentBody body, Random random, float* damage) {
            bool result = false;
            for (int i = 0; i < mayNotDieFromExplosion.Length; i++)
            {
                bool isNull = body.getCreature() == null;
                bool k = true;
                if (!isNull)
                {
                    k = body.getCreature().DisplayName.Contains(mayNotDieFromExplosion[i]);
                }
                result = result || k;
            }
            if (result) {
                float r = random.NormalFloat(0.48f, 0.87f);
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

        public Action<Vector3, IInventory> ProcessDeath(ComponentCreature creature, ComponentHealth health) {
            String cause = health.CauseOfDeath;
            if(cause == "Blasted by explosion") {
                this.creatureListGrabbed.Add(creature.DisplayName);
                ScoreKeeper(creature);
                DefaultThrowBack(creature.DisplayName + " has exploded!");
            }
            if (disableDrops) {
                return (pos, inv) => {};
            } else {
                return (pos, inv) => inv.DropAllItems(pos);
            }
        }

        public static void ScoreKeeper(ComponentCreature creature) {
            String name = creature.DisplayName;
            int i;
            if (name == target) {
                    i = bonus + GetPoint(target);
            } else {
                    i = GetPoint(name);
            }
            score += i;
        }

        public static int GetPoint(String key) {
            return scoreMapping.GetValueOrDefault<String, int>(key, -200);
        }

        public void ChangeTarget(SubsystemTime time, Random rand) {
            int time_delay = rand.Int(30, 90);
            int list_length = predatorAndHostiles.Count - 1;
            int idx = rand.Int(0, list_length);
            String name = predatorAndHostiles[idx];
            if (time.PeriodicGameTimeEvent(time_delay, 2)) {
                target = name;
                this.componentGui.DisplayLargeMessage("Target: " + target, "Worth: " + (int)(GetPoint(target) + bonus), time_delay, 0f);
            }
        }

        public int MeanScore()
        {
            int s = 0;  
            foreach(String name in this.creatureListGrabbed) {
                s += GetPoint(name);
            }
            int length = this.creatureListGrabbed.Count;
            return s / length;
        }

        public static T Make<T>(T t, Action<T> item) {
            item(t);
            return t;
        }

        public static bool FlyingLogicAnimal(ComponentCreature creature)
        {
            bool result = false;
            for (int i = 0; i < ModificationsHolder.animalTypes.Length; i++)
            {
                result = result || creature.DisplayName.Contains(ModificationsHolder.animalTypes[i]);
            }
            return result;
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
            public static readonly float ShoreFluctuationMul = 1.0f;
            /* insert description */
            public static readonly float ShoreFluctuationScMul = 1.0f;

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
            public static readonly float OceanSlope = 0.006f;
            /* insert description */
            public static readonly float SlopeVar = 0.004f;
            /* insert description */
            public static readonly float IslandFreq = 0.01f;
            /* insert description */
            public static readonly float RiversStrgth = 1f;
            public static readonly float[] constants = new float[]
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

        public class ScoreKeeping
        {
            private int totalScore;
            private int worlds_visited;
            public ScoreKeeping() { 
                this.totalScore = 0;
                this.worlds_visited = 0;
            }

            public void UpdateWorldCount()
            {
                this.worlds_visited++;
            }

            public void UpdateTotalScore(int hits, int avg)
            {
                this.totalScore += hits * avg;
            }

            public int[] GetValues()
            {
                return new int[] { totalScore, worlds_visited };
            }
        }
    }
}
