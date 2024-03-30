namespace SimpleExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** TESTING STORE ******");

            Console.WriteLine("storing files.....");

            string saveAsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MvcDemo.dat");
            if (File.Exists(saveAsFile))
                File.Delete(saveAsFile);

            string[] files = [@"files-to-store\dummy-audio.mp3", @"files-to-store\dummy-document.pdf", @"files-to-store\dummy-image.webp"];

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("Starting " + Path.GetFileName(files[i]));
                FileDB.Core.DB.Store(saveAsFile, files[i]);
                Console.WriteLine("Ended " + Path.GetFileName(files[i]));
            }


            Console.WriteLine("retrieving files.....");

            string extractToPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extract");
            if (Directory.Exists(extractToPath))
                Directory.Delete(extractToPath, true);
            //re-create
            Directory.CreateDirectory(extractToPath);

            using (FileDB.Core.DB db = new FileDB.Core.DB(saveAsFile, FileAccess.Read))
            {
                var entities = db.ListFiles();
                foreach (FileDB.Core.EntryInfo? entity in entities)
                {
                    var filename = UniqueFilename(extractToPath, entity.FileName);
                    Console.WriteLine("Extrating.... " + filename);
                    db.Read(entity.ID, filename);
                }
            }

            Console.WriteLine("Done.");


            Console.ReadLine();
        }

        static string UniqueFilename(string dir, string filename)
        {
            var f = Path.Combine(dir, filename);
            var index = 1;
            while (File.Exists(f))
            {
                index++;
                f = Path.Combine(dir, Path.GetFileNameWithoutExtension(filename) + " (" + index + ")" + Path.GetExtension(filename));
            }
            return f;
        }
    }
}
