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
        private bool ArchiveFilesAndFolders(List<string> filePathsList, string archivePath, ref Stopwatch stopwatch)
        {
            try
            {
                if (File.Exists(archivePath))
                {
                    stopwatch.Stop();
                    // Display the custom form for archive exists options
                    using (FormArchiveExists archiveExistsForm = new FormArchiveExists())
                    {
                        DialogResult dialogResult = archiveExistsForm.ShowDialog();

                        if (dialogResult == DialogResult.Cancel)
                        {
                            // User chose to cancel
                            return false;
                        }
                        else if (dialogResult == DialogResult.Yes)
                        {
                            // User chose to add files to the existing archive
                            stopwatch.Start();
                            using (FileStream archiveStream = new FileStream(archivePath, FileMode.Open))
                            using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Update))
                            {
                                AddFilesAndFoldersToArchive(archive, filePathsList);
                            }
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            // User chose to replace the existing archive
                            stopwatch.Start();
                            File.Delete(archivePath);
                            using (FileStream archiveStream = new FileStream(archivePath, FileMode.Create))
                            using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
                            {
                                AddFilesAndFoldersToArchive(archive, filePathsList);
                            }
                        }
                    }
                }
                else
                {
                    // The archive does not exist, create a new one
                    using (FileStream archiveStream = new FileStream(archivePath, FileMode.Create))
                    using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
                    {
                        AddFilesAndFoldersToArchive(archive, filePathsList);
                    }
                }
                stopwatch.Stop();
                MessageBox.Show("Archiving completed successfully.", "Success");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час архівації: \"{ex.Message}\". Впевніться, що вхідні дані вірні, обрані файли мають допустимі ім'я та розширення.", "Помилка");
                return false;
            }
        }

        private void AddFilesAndFoldersToArchive(ZipArchive archive, List<string> filePathsList)
        {
            foreach (string filePath in filePathsList)
            {
                if (File.Exists(filePath))
                {
                    // If it's a file, add it to the archive
                    string relativeFilePath = Path.GetFileName(filePath);
                    archive.CreateEntryFromFile(filePath, relativeFilePath);
                }
                else if (Directory.Exists(filePath))
                {
                    // If it's a folder, add its contents to the archive
                    string folderName = new DirectoryInfo(filePath).Name;
                    AddDirectoryToArchive(archive, filePath, folderName);
                }
            }
        }

        private void AddDirectoryToArchive(ZipArchive archive, string sourceDirectory, string folderName)
        {
            foreach (string file in Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                string relativeFilePath = Path.Combine(folderName, file.Substring(sourceDirectory.Length + 1));
                archive.CreateEntryFromFile(file, relativeFilePath);
            }
        }
    }
}
