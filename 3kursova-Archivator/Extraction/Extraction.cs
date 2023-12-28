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
        private bool ExtractArchive(string archivePath, string directoryPath, ref Stopwatch stopwatch)
        {
            if (!File.Exists(archivePath))
            {
                MessageBox.Show("Selected archive file does not exist.", "Error");
                return false;
            }
            if (!Directory.Exists(directoryPath))
            {
                stopwatch.Stop();
                DialogResult result = MessageBox.Show("The extraction directory does not exist. Do you want to create it?", "Directory Not Found", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create the directory if the user agrees
                    Directory.CreateDirectory(directoryPath);
                    stopwatch.Start();
                }
                else
                {
                    return false; // User canceled or chose not to create the directory
                }
            }

            try
            {
                using (FileStream archiveStream = new FileStream(archivePath, FileMode.Open))
                using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        string entryPath = Path.Combine(directoryPath, entry.FullName);

                        if (File.Exists(entryPath))
                        {
                            stopwatch.Stop();
                            DialogResult result = MessageBox.Show($"A file with the same name already exists: {entry.FullName}. Do you want to overwrite it?", "File Exists", MessageBoxButtons.YesNo);
                            if (result == DialogResult.No)
                            {
                                continue; // Skip the file if the user chose not to overwrite
                            }
                            stopwatch.Start();
                        }

                        // Ensure the directory for the entry exists
                        Directory.CreateDirectory(Path.GetDirectoryName(entryPath));

                        if (!string.IsNullOrEmpty(entry.Name))
                        {
                            entry.ExtractToFile(entryPath, true);
                        }
                    }
                }
                stopwatch.Stop();
                MessageBox.Show("Extraction completed successfully.", "Success");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час розархівації: \"{ex.Message}\". Впевніться, що вхідні дані вірні, обраний архів має допустиме ім'я та розширення.", "Помилка");
                return false;
            }
        }
    }
}
