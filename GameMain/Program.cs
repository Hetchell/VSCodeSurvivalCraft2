using Game;
using ModificationHolder.ContentPacker;
using Survivalcraft.Game.ModificationHolder;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace GameMain
{
    class Program
    {   
        private static readonly bool m_state = true;
        public static readonly String m_contentpak_key = 
            "tiTrKAXRpwuRhNI3gTkxIun6AyLxSZaIgEjVkyFWhD6w0QgwmN5YwykY2I" +
                "79OHIolI1r4ewZ2uEfStqC7GRDM8CRTNQTdg91pkOk" +
                "bnIPAiEp2EqkZWYPgPv6CNZpB3E1OuuBmR3ZzYEv8UMj" +
                "QxjyXZy1CEOD8guk3uiiPvyFaf5pS" + //"QxjyXZy1CEOD8guk3uiiPvyFaf5pS"
                "znSNWXbnhmAzTbi1TEGCyhxejMTB2" + //"znSNWXbnhmAzTbi1TEGCyhxejMTB2"
                "3KUgqNiskGlrHaIVNz83DXVGkvm";
        [STAThread]
        static void Main(string[] args)
        {
            string pth = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "";
            string currDir = CurrentDirectoryInCode(pth);
            Pak pak = new Pak(m_contentpak_key);
            DeleteAndReplenishOriginalContentPak(pth, currDir + "\\GameMain\\ContentsGame", pak);
            HandleModPaks(pth, currDir + "\\GameMain\\ContentsMod", pak);
            if (m_state){
                Console.WriteLine("Hello, World! Test");
                ModificationsHolder.UpdateCommand();
                ProgramGame.MainMethod();
            } else {
                CheckMethod(currDir);
            }
        }

        static void CheckMethod(String currDir){
            string txtfile = "GameMain\\Notes\\InstructionListFormat.txt";
            string path = string.Concat(currDir, "\\", txtfile);
            Console.WriteLine(string.Format("Your current file location is at {0}", path));
            string[] contains = File.ReadAllLines(path);
            Console.WriteLine(contains[80]);
        }

        private static String CurrentDirectoryInCode(String path)
        {
            String dir = String.Empty;
            //"D:\\C projects\\SurvivalCraftMiniGame\\bin\\Debug\\net7.0-windows"
            String[] dec = path.Split(new char[] { '\\' });
            for(int i = 0; i < dec.Length - 3; i++)
            {
                dir += dec[i] + (i == dec.Length - 4 ? "" : "\\");
            }
            return dir;
        }

        private static void DeleteAndReplenishOriginalContentPak(String assemblyLocation, String contentFolderLoc, Pak pak)
        {
            String dest = assemblyLocation + "\\Content.pak";
            String src = contentFolderLoc + "\\Content.pak";
            File.Delete(dest);
            if (!File.Exists(src))
            {
                pak.Pack(contentFolderLoc + "\\Content");
            }
            try
            {
                File.Move(src, dest);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured: " + e.Message);
            }
        }

        private static void HandleModPaks(String assemblyLocation, String contentFolderColleLoc, Pak pak)
        {
            String modHolder = assemblyLocation + "\\Mods";
            IEnumerable<String> filelist = Directory.GetFiles(modHolder, "*.pak");
            foreach ( String filepath in filelist)
            {
                String[] p = filepath.Split(new char[] { '\\' });
                String name = p[^1];
                String dest = filepath;
                String src = contentFolderColleLoc + "\\" + name; 
                int k = ".pak".Length;
                name = name[..^k];
                String srcFolder = contentFolderColleLoc + "\\" + name;
                Console.WriteLine("Pak name: " + name);
                Console.WriteLine("Folder source: " + srcFolder);
                Console.WriteLine("Source read: " + src);
                Console.WriteLine("Destination write: " + dest);
                Console.WriteLine("================================================");
                if(File.Exists(dest))
                {
                    File.Delete(dest);
                }
                pak.Pack(srcFolder);
                try
                {
                    File.Move(src, dest);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occured: " + e.Message);
                }
            }
        }
    }
}