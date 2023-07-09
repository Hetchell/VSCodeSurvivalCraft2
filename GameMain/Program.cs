using Game;
using Survivalcraft.Game.ModificationHolder;
using System.IO;
using System.Text;
using Colors = Engine.Color;
using ModificationHolder.ContentPacker;

namespace GameMain
{
    class Program
    {   
        private static bool m_state = true;
        public static string contents;
        public static string[] instructionsall;
        public static readonly String contentpak_key = "tiTrKAXRpwuRhNI3gTkxIun6AyLxSZaIgEjVkyFWhD6w0QgwmN5YwykY2I79OHIolI1r4ewZ2uEfStqC7GRDM8CRTNQTdg91pkOkbnIPAiEp2EqkZWYPgPv6CNZpB3E1OuuBmR3ZzYEv8UMjQxjyXZy1CEOD8guk3uiiPvyFaf5pSznSNWXbnhmAzTbi1TEGCyhxejMTB23KUgqNiskGlrHaIVNz83DXVGkvm";
        [STAThread]
        static void Main(string[] args)
        {
            string pth = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "";
            //string pth = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\TextFile1.txt";
            contents = ModificationsHolder.getMessageOfTheDayXMLString(pth + "\\Message.txt", "");
            string txtfile = "GameMain\\Notes\\InstructionListFormat.txt";
            string currDir = Directory.GetCurrentDirectory();
            string path = string.Concat(currDir, "\\", txtfile);
            Console.WriteLine(string.Format("Your current file location is at {0}", path));
            string[] contains = File.ReadAllLines(path);
            instructionsall = ProcessInstructions(contains);
            deleteAndReplenishOriginalContentPak(pth, currDir + "\\GameMain\\ContentsGame");
            if(m_state){
                Console.WriteLine("Hello, World! Test");
                ModificationsHolder.UpdateCommand();
                ProgramGame.MainMethod();
            } else {
                CheckMethod();
            }
        }

        private static void CheckMethod(){
        
        }

        private static string[] ProcessInstructions(string[] containing){
            List<string> instruction = new List<string>();
            List<string> subinstruction = new List<string>();
            bool force = false;
            foreach(string linecontent in containing){
                if(!linecontent.Contains("<Sep/>") && !force){
                    //we are in instructions field 
                    if(!linecontent.Contains("<")){
                        //we do not look for <
                        instruction = instruction.Append(string.Concat(linecontent, "\n")).ToList<string>();
                    }
                } else {
                    //we are in subinstruction
                    //set force to true redirect content to subinstruction
                    force = true;
                    if(!linecontent.Contains("<")){
                        subinstruction = subinstruction.Append(string.Concat(linecontent, "\n")).ToList<string>();
                    }
                }
            }
            string instructionlist = string.Concat(instruction).Replace("\r", "");
            string subinstructionlist = string.Concat(subinstruction).Replace("\r", "");
            //remove \n at end
            instructionlist = instructionlist.Remove(instructionlist.Length - 1);
            subinstructionlist = subinstructionlist.Remove(subinstructionlist.Length - 1);
            return new string[]{
                instructionlist,
                subinstructionlist
            };
        }

        private static void deleteAndReplenishOriginalContentPak(String assemblyLocation, String contentFolderLoc) {
            String dest = assemblyLocation + "\\Nyaa\\Content.pak";
            String src = contentFolderLoc + "\\Content.pak";
            File.Delete(dest);
            if(!File.Exists(src)) {
                new Pak(contentFolderLoc + "\\Content");
            }
            try {
                File.Move(src, dest);
            } catch (Exception e) {
                Console.WriteLine("An error occured: " + e.Message);
            }
        }
    }
}