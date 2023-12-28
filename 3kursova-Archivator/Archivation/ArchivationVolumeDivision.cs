using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Windows.Forms;

namespace _3kursova_Archivator
{
    public partial class Form1 : Form
    {
        private bool ArchiveFilesWithVolumes(List<string> filePathsList, string archiveName, long maxVolumeSize, ref List<string> volumePaths, ref Stopwatch stopwatch)
        {
            try
            {
                int volumeCounter = 1;

                foreach (string path in filePathsList)
                {
                    if (File.Exists(path))
                    {
                        // Handle files
                        ArchiveFile(path, ref volumeCounter, volumePaths, archiveName, maxVolumeSize);
                    }
                    else if (Directory.Exists(path))
                    {
                        // Handle directories and their contents
                        ArchiveFolderWithVolumes(path, ref volumeCounter, volumePaths, archiveName, maxVolumeSize);
                    }
                }

                stopwatch.Stop();
                MessageBox.Show("Archiving completed successfully.", "Success");
                return true;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                MessageBox.Show($"Error: {ex.Message}", "Error");
                return false;
            }
        }

        private void ArchiveFile(string path, ref int volumeCounter, List<string> volumePaths, string archiveName, long maxVolumeSize)
        {
            //long fileSize = GetCompressedFileSize(path);
            long fileSize = GetUncompressedFileSize(path);
            // Keep track of the current position in the source file
            long sourceFilePosition = 0;

            while (sourceFilePosition < fileSize)
            {
                // Check if there is a volume available
                if (volumePaths.Count >= volumeCounter)
                {
                    long currentVolumeSize = 0;
                    string volumePath = volumePaths[volumeCounter - 1]; // Get the last volume
                    string tempFilePath;

                    using (FileStream volumeFileStream = new FileStream(volumePath, FileMode.Open))
                    {
                        // Create a temporary file to store the updated contents
                        tempFilePath = "D:\\KPI\\3 курс\\Курсова\\C#code\\Archives_created\\tempfilearchtemp.tmp";

                        using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create))
                        using (ZipArchive tempArchive = new ZipArchive(tempFileStream, ZipArchiveMode.Create))
                        {
                            // Read from the existing volume and copy to the temporary file
                            using (ZipArchive volumeArchive = new ZipArchive(volumeFileStream, ZipArchiveMode.Read))
                            {
                                foreach (var entry in volumeArchive.Entries)
                                {
                                    var tempEntry = tempArchive.CreateEntry(entry.FullName);
                                    using (Stream entryStream = entry.Open())
                                    using (Stream tempEntryStream = tempEntry.Open())
                                    {
                                        entryStream.CopyTo(tempEntryStream);
                                    }
                                }
                            }

                            // Create a new entry in the temporary file
                            ZipArchiveEntry newEntry = tempArchive.CreateEntry(Path.GetFileName(path));

                            using (Stream newEntryStream = newEntry.Open())
                            using (FileStream fileStream = File.Open(path, FileMode.Open))
                            {
                                // Seek to the position in the source file
                                fileStream.Seek(sourceFilePosition, SeekOrigin.Begin);

                                byte[] buffer = new byte[8192];
                                int bytesRead;

                                long remainingSpace = maxVolumeSize - tempFileStream.Length;

                                while (remainingSpace > 0 && sourceFilePosition < fileSize)
                                {
                                    bytesRead = fileStream.Read(buffer, 0, (int)Math.Min(buffer.Length, remainingSpace));
                                    if (bytesRead <= 0)
                                        break;

                                    newEntryStream.Write(buffer, 0, bytesRead);
                                    currentVolumeSize += bytesRead;

                                    // Update the remaining space
                                    remainingSpace = maxVolumeSize - tempFileStream.Length;
                                }

                                // Update the source file position after the loop
                                sourceFilePosition += currentVolumeSize;
                            }
                        }

                        // Close the initial volume file stream before attempting to delete or rename
                        volumeFileStream.Close();
                    }

                    // Delete the initial volume file
                    File.Delete(volumePath);

                    // Rename the temporary file to the initial volume file
                    File.Move(tempFilePath, volumePath);

                    // Check if the current volume is full
                    using (FileStream volumeFileStreamAgain = new FileStream(volumePath, FileMode.Open))
                    {
                        if (volumeFileStreamAgain.Length >= maxVolumeSize)
                        {
                            volumeCounter++;
                        }
                    }
                }
                else
                {
                    long currentVolumeSize = 0;
                    // Create a new volume
                    string volumeName = $"{archiveName}.z{volumeCounter:D2}";
                    string volumePath = Path.Combine(Path.GetDirectoryName(archiveName), volumeName);
                    volumePaths.Add(volumePath);

                    using (FileStream volumeFileStream = new FileStream(volumePath, FileMode.Create))
                    using (ZipArchive volumeArchive = new ZipArchive(volumeFileStream, ZipArchiveMode.Create))
                    {
                        ZipArchiveEntry entry = volumeArchive.CreateEntry(Path.GetFileName(path));

                        using (FileStream fileStream = File.Open(path, FileMode.Open))
                        using (Stream entryStream = entry.Open())
                        {
                            // Seek to the position in the source file
                            fileStream.Seek(sourceFilePosition, SeekOrigin.Begin);

                            byte[] buffer = new byte[8192];
                            int bytesRead;

                            while (volumeFileStream.Length < maxVolumeSize && sourceFilePosition < fileSize)
                            {
                                bytesRead = fileStream.Read(buffer, 0, (int)Math.Min(buffer.Length, maxVolumeSize - volumeFileStream.Length));
                                if (bytesRead <= 0)
                                    break;

                                entryStream.Write(buffer, 0, bytesRead);
                                currentVolumeSize += bytesRead;
                            }
                        }

                        sourceFilePosition += currentVolumeSize;

                        // Check if the current volume is full
                        if (volumeFileStream.Length >= maxVolumeSize)
                        {
                            volumeCounter++;
                        }
                    }
                }
            }
        }

        private void ArchiveFolderWithVolumes(string folderPath, ref int volumeCounter, List<string> volumePaths, string archiveName, long maxVolumeSize)
        {
            foreach (string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = GetRelativePath(folderPath, filePath);

                // Combine the root folder name with the relative path
                string folderedRelativePath = Path.Combine(Path.GetFileName(folderPath), relativePath);

                // Add file to the current volume
                ArchiveFileToVolume(filePath, folderedRelativePath, maxVolumeSize, volumePaths, ref volumeCounter, archiveName);
            }
        }

        private void ArchiveFileToVolume(string path, string folderedRelativePath, long maxVolumeSize, List<string> volumePaths, ref int volumeCounter, string archiveName)
        {
            //long fileSize = GetCompressedFileSize(path);
            long fileSize = GetUncompressedFileSize(path);
            // Keep track of the current position in the source file
            long sourceFilePosition = 0;

            while (sourceFilePosition < fileSize)
            {
                // Check if there is a volume available
                if (volumePaths.Count >= volumeCounter)
                {
                    long currentVolumeSize = 0;
                    string volumePath = volumePaths[volumeCounter - 1]; // Get the last volume
                    string tempFilePath;

                    using (FileStream volumeFileStream = new FileStream(volumePath, FileMode.Open))
                    {
                        // Create a temporary file to store the updated contents
                        tempFilePath = "D:\\KPI\\3 курс\\Курсова\\C#code\\Archives_created\\tempfilearchtemp.tmp";

                        using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create))
                        using (ZipArchive tempArchive = new ZipArchive(tempFileStream, ZipArchiveMode.Create))
                        {
                            // Read from the existing volume and copy to the temporary file
                            using (ZipArchive volumeArchive = new ZipArchive(volumeFileStream, ZipArchiveMode.Read))
                            {
                                foreach (var entry in volumeArchive.Entries)
                                {
                                    var tempEntry = tempArchive.CreateEntry(entry.FullName);
                                    using (Stream entryStream = entry.Open())
                                    using (Stream tempEntryStream = tempEntry.Open())
                                    {
                                        entryStream.CopyTo(tempEntryStream);
                                    }
                                }
                            }

                            // Create a new entry in the temporary file
                            ZipArchiveEntry newEntry = tempArchive.CreateEntry(folderedRelativePath);

                            using (Stream newEntryStream = newEntry.Open())
                            using (FileStream fileStream = File.Open(path, FileMode.Open))
                            {
                                // Seek to the position in the source file
                                fileStream.Seek(sourceFilePosition, SeekOrigin.Begin);

                                byte[] buffer = new byte[8192];
                                int bytesRead;

                                long remainingSpace = maxVolumeSize - tempFileStream.Length;

                                while (remainingSpace > 0 && sourceFilePosition < fileSize)
                                {
                                    bytesRead = fileStream.Read(buffer, 0, (int)Math.Min(buffer.Length, remainingSpace));
                                    if (bytesRead <= 0)
                                        break;

                                    newEntryStream.Write(buffer, 0, bytesRead);
                                    currentVolumeSize += bytesRead;

                                    // Update the remaining space
                                    remainingSpace = maxVolumeSize - tempFileStream.Length;
                                }

                                // Update the source file position after the loop
                                sourceFilePosition += currentVolumeSize;
                            }
                        }

                        // Close the initial volume file stream before attempting to delete or rename
                        volumeFileStream.Close();
                    }

                    // Delete the initial volume file
                    File.Delete(volumePath);

                    // Rename the temporary file to the initial volume file
                    File.Move(tempFilePath, volumePath);

                    // Check if the current volume is full
                    using (FileStream volumeFileStreamAgain = new FileStream(volumePath, FileMode.Open))
                    {
                        if (volumeFileStreamAgain.Length >= maxVolumeSize)
                        {
                            volumeCounter++;
                        }
                    }
                }
                else
                {
                    long currentVolumeSize = 0;
                    // Create a new volume
                    string volumeName = $"{archiveName}.z{volumeCounter:D2}";
                    string volumePath = Path.Combine(Path.GetDirectoryName(archiveName), volumeName);
                    volumePaths.Add(volumePath);

                    using (FileStream volumeFileStream = new FileStream(volumePath, FileMode.Create))
                    using (ZipArchive volumeArchive = new ZipArchive(volumeFileStream, ZipArchiveMode.Create))
                    {
                        ZipArchiveEntry entry = volumeArchive.CreateEntry(folderedRelativePath);

                        using (FileStream fileStream = File.Open(path, FileMode.Open))
                        using (Stream entryStream = entry.Open())
                        {
                            // Seek to the position in the source file
                            fileStream.Seek(sourceFilePosition, SeekOrigin.Begin);

                            byte[] buffer = new byte[8192];
                            int bytesRead;

                            while (volumeFileStream.Length < maxVolumeSize && sourceFilePosition < fileSize)
                            {
                                bytesRead = fileStream.Read(buffer, 0, (int)Math.Min(buffer.Length, maxVolumeSize - volumeFileStream.Length));
                                if (bytesRead <= 0)
                                    break;

                                entryStream.Write(buffer, 0, bytesRead);
                                currentVolumeSize += bytesRead;
                            }
                        }

                        sourceFilePosition += currentVolumeSize;

                        // Check if the current volume is full
                        if (volumeFileStream.Length >= maxVolumeSize)
                        {
                            volumeCounter++;
                        }
                    }
                }
            }
        }

        private string GetRelativePath(string rootPath, string filePath)
        {
            return filePath.Substring(rootPath.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        private long CalculateUncompressedMaxVolumeSize(long maxCompressedSize)
        {
            string tempArchivePath = "D:\\KPI\\3 курс\\Курсова\\C#code\\Archives_created\\temp_archive_uncompressed_tmp.zip"; // Change this to your desired file name
            long uncompressedMaxVolumeSize = 0;

            try
            {
                using (FileStream tempArchiveFileStream = new FileStream(tempArchivePath, FileMode.Create))
                using (ZipArchive tempArchive = new ZipArchive(tempArchiveFileStream, ZipArchiveMode.Create))
                {
                    while (tempArchiveFileStream.Length < maxCompressedSize)
                    {
                        // Add an entry to the temp archive
                        string entryName = $"temp_entry_{Guid.NewGuid()}.txt";
                        ZipArchiveEntry entry = tempArchive.CreateEntry(entryName);
                        using (Stream entryStream = entry.Open())
                        {
                            // Fill the entry with some data
                            byte[] buffer = new byte[8192];
                            int bytesRead;

                            while (tempArchiveFileStream.Length < maxCompressedSize && (bytesRead = buffer.Length) > 0)
                            {
                                entryStream.Write(buffer, 0, bytesRead);
                                uncompressedMaxVolumeSize += bytesRead;
                            }
                        }
                    }
                }
                // Delete the temporary archive file
                File.Delete(tempArchivePath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

            return uncompressedMaxVolumeSize;
        }

        private long GetUncompressedFileSize(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return fileStream.Length;
            }
        }

        private long GetCompressedFileSize(string filePath)
        {
            using (MemoryStream ms = new MemoryStream())
            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create))
            {
                string entryName = Path.GetFileName(filePath);
                var entry = archive.CreateEntry(entryName);

                using (Stream entryStream = entry.Open())
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(entryStream);
                }

                return ms.Length;
            }
        }

        private long ConvertVolumeSizeToBytes(string volumeSize)
        {
            if (string.IsNullOrWhiteSpace(volumeSize))
            {
                // Invalid size, throw exception volume size is not specified
                throw new ArgumentException("Розмір не зазначено. Введіть розмір вірно, наприклад: ('1 GB', '1 MB', '500 KB').");
            }

            string size = volumeSize.Trim().ToLower();
            long multiplier = 1;

            if (size.EndsWith("kb"))
            {
                multiplier = 1024;
                size = size.Replace("kb", "");
            }
            else if (size.EndsWith("mb"))
            {
                multiplier = 1024 * 1024;
                size = size.Replace("mb", "");
            }
            else if (size.EndsWith("gb"))
            {
                multiplier = 1024 * 1024 * 1024;
                size = size.Replace("gb", "");
            }

            if (long.TryParse(size, out long sizeValue) && sizeValue > 0)
            {
                return sizeValue * multiplier;
            }

            // Invalid size, throw exception
            throw new ArgumentException("Розмір зазначено невірно. Введіть розмір вірно, наприклад: ('1 GB', '1 MB', '500 KB').");
        }
    }
}