using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Projects
{
    public class DirectoryManipulation : Project
    {
        public override void Execute()
        {
            string base_folder = Directory.GetCurrentDirectory();
            string storage_folder = Path.Combine(base_folder, "Storage");

            string[] folders = new string[]
            {
                "Tester",
                "Test"
            };

            foreach (string folder in folders)
            {
                string source_folder = Path.Combine(base_folder, folder);
                string destination_folder = Path.Combine(storage_folder, folder);

                MoveFolder(source_folder, destination_folder);
            }
        }

        private void MoveFolder(string sourceFolder, string destinationFolder)
        {
            if (Directory.Exists(sourceFolder))
            {
                if (Directory.Exists(destinationFolder))
                {
                    foreach (string file in Directory.GetFiles(sourceFolder))
                    {
                        File.Copy(file, file.Replace(sourceFolder, destinationFolder), true);
                    }

                    Directory.Delete(sourceFolder, true);
                }
                else
                {
                    Directory.Move(sourceFolder, destinationFolder);
                }
            }
        }
    }
}
