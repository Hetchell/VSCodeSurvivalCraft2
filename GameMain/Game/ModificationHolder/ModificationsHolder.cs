using Engine.Input;
using Game;
using System.Diagnostics;
using ModificationHolder;
using Engine;
using System.Threading.Tasks;

namespace Survivalcraft.Game.ModificationHolder
{
    public class ModificationsHolder
    {
        public static bool allowFlyingAnimal = false;
        public static bool allowWolfDespawn = true;
        public static bool fogEnable = false;
        public static bool allowForUnrestrictedTravel = true;
        public static bool allowFixedSpawn = true;
        public static int seed = 1638478217;
        public static float steppedLevelTravel = 10f;
        public static String[] animalTypes = { "Wolf", "Hyena", "Lion" };
        public static float movementLimitPlayer = 100f * 100f;
        public static float movementLimitAnimalsDerFlying = 300f;
        public static Vector3 spawnfixed = new Vector3(0, 300, 0);

        private static int repeat = 0;
        private static int repeatCom = 0;
        private static string somestr;

        public static void keyboardActions(WidgetInput input)
        {
            if (input.IsKeyDownOnce(Key.UpArrow))
            {
                ComponentInput.speed++;
                Debug.WriteLine("Speed is increased");
            }
            if (input.IsKeyDownOnce(Key.DownArrow))
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
