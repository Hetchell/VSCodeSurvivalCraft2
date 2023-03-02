using System.IO;

namespace CreatorWandModAPI
{
    public static class FileOperation
    {
        public static bool IsFileInUse(this string file)
        {
            bool result = true;
            try
            {
                new FileStream(file, FileMode.Open).Dispose();
                result = false;
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static Stream CreateStream(this Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0L;
            stream.Dispose();
            return memoryStream;
        }

        public static bool Delete(this string directory)
        {
            if (File.Exists(directory))
            {
                if (directory.IsFileInUse())
                {
                    return false;
                }

                File.Delete(directory);
                return true;
            }

            string[] directories = Directory.GetDirectories(directory);
            for (int i = 0; i < directories.Length; i++)
            {
                directories[i].Delete();
            }

            directories = Directory.GetFiles(directory);
            foreach (string text in directories)
            {
                if (text.IsFileInUse())
                {
                    return false;
                }

                File.Delete(text);
            }

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }

            return true;
        }

        public static FileStream CreateFile(this string file)
        {
            try
            {
                return new FileStream(file, FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                string[] array = file.Split('/', '\0');
                string text = "";
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (i == array.Length - 1)
                    {
                        text += array[i];
                        break;
                    }

                    text = text + array[i] + "/";
                }

                Directory.CreateDirectory(text);
                return new FileStream(file, FileMode.Create);
            }
        }
    }
}