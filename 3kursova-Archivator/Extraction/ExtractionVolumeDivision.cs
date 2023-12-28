using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace _3kursova_Archivator
{
    public partial class Form1 : Form
    {
        public static bool DearchiveVolumes(List<string> volumePaths, string destinationFolder, Stopwatch stopwatch)
        {
            volumePaths.Sort();
            int volumeCounter = 1;

            foreach (var volumePath in volumePaths)
            {
                // Extract the contents of the volume directly into the destination folder
                ExtractVolume(volumePath, destinationFolder, volumeCounter);

                // Increment volume counter for the next volume
                volumeCounter++;
            }

            stopwatch.Stop();
            MessageBox.Show("Extraction completed successfully.", "Success");
            return true;
        }

        private static void ExtractVolume(string volumePath, string destinationFolder, int volumeCounter)
        {
            // Extract the contents of the volume directly into the destination folder
            using (ZipArchive volumeArchive = ZipFile.OpenRead(volumePath))
            {
                foreach (var entry in volumeArchive.Entries)
                {
                    string entryDestination = Path.Combine(destinationFolder, entry.FullName);
                    entryDestination = Path.GetFullPath(entryDestination);

                    // Ensure the destination directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(entryDestination));

                    if (File.Exists(entryDestination))
                    {
                        // If the file already exists, append the entry contents to the end
                        using (FileStream destinationFileStream = new FileStream(entryDestination, FileMode.Append))
                        using (Stream entryStream = entry.Open())
                        {
                            entryStream.CopyTo(destinationFileStream);
                        }
                    }
                    else
                    {
                        // If the file does not exist, extract the entry
                        entry.ExtractToFile(entryDestination, true);
                    }
                }
            }
        }
    }
}