using Game;
using Survivalcraft.Game.ModificationHolder;
using System.IO;
using Colors = Engine.Color;

namespace GameMain
{
    class Program
    {   
        private static bool state = true;
        public static string contents;
        [STAThread]
        static void Main(string[] args)
        {
            string pth = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\Message.txt";
            //string pth = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\TextFile1.txt";
            contents = ModificationsHolder.getMessageOfTheDayXMLString(pth, "");
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
            foreach(var instruction in contains.Where(i => i.Contains("value="))){
                Console.Write(instruction + "\n");
            }
        }
    }
}