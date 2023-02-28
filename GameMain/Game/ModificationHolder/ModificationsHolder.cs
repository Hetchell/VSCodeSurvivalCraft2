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
                ComponentInput.step = ModifierHolderOuterClass.steppedTravel;
            }
            else if (input.IsKeyDown(Key.RightArrow))
            {
                ComponentInput.state = true;
                ComponentInput.step = -ModifierHolderOuterClass.steppedTravel;
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
    }
}
