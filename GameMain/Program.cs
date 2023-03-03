using Game;
using Survivalcraft.Game.ModificationHolder;
using System.IO;

namespace GameMain
{
    class Program
    {   
        private static bool state = false;
        [STAThread]
        static void Main(string[] args)
        {
            if(state){
                Console.WriteLine("Hello, World! Test");
                ModificationsHolder.UpdateCommand();
                ProgramGame.MainMethod();
            } else {
                CheckMethod();
            }
        }

        static void CheckMethod(){
            string txtfile = "GameMain\\Notes\\InstructionListFormat.txt";
            string currDir = Directory.GetCurrentDirectory();
            string path = string.Concat(currDir, "\\", txtfile);
            Console.WriteLine(string.Format("Your current file location is at {0}", path));
            string[] contains = File.ReadAllLines(path);
            Console.WriteLine(contains[98]);
        }
    }
}