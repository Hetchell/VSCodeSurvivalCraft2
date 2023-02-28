using Game;
using Survivalcraft.Game.ModificationHolder;

namespace GameMain
{
    class Program
    {   
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World! Test");
            ModificationsHolder.UpdateCommand();
            ProgramGame.MainMethod();
        }
    }
}