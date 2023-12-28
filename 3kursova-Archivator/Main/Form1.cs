using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace _3kursova_Archivator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_choose_files_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Оберіть файли які треба архівувати";
            openFileDialog.Multiselect = true; // Allow multiple file selection
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file names
                foreach (string filePath in openFileDialog.FileNames)
                {
                    richTextBox1.AppendText(filePath + Environment.NewLine);
                }
            }
        }

        private void button_choose_folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected folder path
                string selectedFolderPath = folderBrowserDialog.SelectedPath;

                // Display the selected folder path in the RichTextBox
                richTextBox1.AppendText(selectedFolderPath + Environment.NewLine);
            }
        }

        private void button_archivate_Click(object sender, EventArgs e)
        {
            List<string> filePathsList = new List<string>();
            richTextBox2.Clear();
            // Split the text in richTextBox1 by newline characters
            foreach (string line in richTextBox1.Lines)
            {
                if (!string.IsNullOrWhiteSpace(line)) // Check if the line is not empty or just spaces
                {
                    string trimmedLine = line.Trim(); // Trim any leading/trailing spaces
                    if (File.Exists(trimmedLine) || Directory.Exists(trimmedLine))
                    {
                        if (!filePathsList.Contains(trimmedLine))
                        {
                            filePathsList.Add(trimmedLine); // Add to the list if it's not a duplicate
                        }
                        else
                        {
                            MessageBox.Show($"Файл '{line}' вже було обрано раніше.", "Помилка. Duplicate File");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Файл або тека '{line}' не існує.", "Помилка. Несіснуючий файл або тека");
                    }
                }
            }

            if (filePathsList.Count == 0)
            {
                MessageBox.Show("Не вибрано жодного файлу або теки для архівації.", "Помилка. Нічого не вибрано.");
                return; // Do not proceed with archiving
            }

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            string custom_archive_path = richTextBox4.Lines.FirstOrDefault(line =>
                !string.IsNullOrWhiteSpace(line) && !line.Any(c => invalidFileNameChars.Contains(c)));

            string archivePath = "D:\\KPI\\3 курс\\Курсова\\C#code\\Archives_created\\";

            // Check if the archive path contains an extension; if not, add .zip
            if (!string.IsNullOrEmpty(custom_archive_path))
            {
                if (!Path.HasExtension(custom_archive_path))
                {
                    custom_archive_path += ".zip";
                }
                archivePath += custom_archive_path;

                // Check if division into volumes is enabled (checkBox1 is checked)
                bool divideIntoVolumes = checkBox1.Checked;

                if (divideIntoVolumes)
                {
                    // Get the volume size from the ComboBox
                    string volumeSize = comboBox1.Text;
                    try
                    {
                        // Try to archive the files and log the result
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        long maxVolumeSize = ConvertVolumeSizeToBytes(volumeSize);
                        List<string> volumePaths = new List<string>(); // Store paths of individual volumes

                        bool archivingResult = ArchiveFilesWithVolumes(filePathsList, archivePath, maxVolumeSize, ref volumePaths, ref stopwatch);
                        stopwatch.Stop();
                        LogArchivingResultWithVolumes(archivePath, filePathsList, custom_archive_path, archivingResult, stopwatch, volumePaths);
                    }
                    catch (ArgumentException ex)
                    {
                        // Handle the exception (e.g., log it, display an error message)
                        MessageBox.Show($"Помилка при архівації з розділенням на томи: {ex.Message}", "Помилка!");
                    }
                }

                else
                {
                    // Try to archive the files and log the result
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    bool archivingResult = ArchiveFilesAndFolders(filePathsList, archivePath, ref stopwatch);
                    stopwatch.Stop();
                    LogArchivingResult(archivePath, filePathsList, custom_archive_path, archivingResult, stopwatch);
                }
            }
            else
            {
                MessageBox.Show("Ім'я архіву не задано або містить заборонені символи. Впевніться, що ім'я не містить: * \" / \\ < > : | ?", "Помилка. Оберіть корректне ім'я архіву!");
            }
        }

        private void LogArchivingResult(string archivePath, List<string> filePathsList, string custom_archive_path, bool success, Stopwatch stopwatch)
        {
            richTextBox2.Clear();
            if (success)
            {
                richTextBox2.AppendText("Successful archiving! Archive name: " + custom_archive_path + Environment.NewLine);
                richTextBox2.AppendText($"Archive Path: {archivePath}" + Environment.NewLine);
                long initialSize = GetTotalSizeOfFiles(filePathsList);
                long finalSize = new FileInfo(archivePath).Length;
                richTextBox2.AppendText($"Initial Size: {initialSize} bytes" + Environment.NewLine);
                richTextBox2.AppendText($"Final Size: {finalSize} bytes" + Environment.NewLine);
                richTextBox2.AppendText($"Time required for archivation: {stopwatch.Elapsed.TotalSeconds} seconds" + Environment.NewLine);
            }
            else
            {
                richTextBox2.AppendText("Archiving failed!" + Environment.NewLine);
            }
        }

        private void LogArchivingResultWithVolumes(string archivePath, List<string> filePathsList, string custom_archive_path, bool success, Stopwatch stopwatch, List<string> volumePaths)
        {
            richTextBox2.Clear();
            if (success)
            {
                richTextBox2.AppendText("Successful archiving with division into volumes! Archive base name: " + custom_archive_path + Environment.NewLine);
                richTextBox2.AppendText($"Volumes base Path: {archivePath}" + Environment.NewLine);
                long initialSize = GetTotalSizeOfFiles(filePathsList);
                long finalSize = GetTotalSizeOfFiles(volumePaths);
                richTextBox2.AppendText($"Initial Size: {initialSize} bytes" + Environment.NewLine);
                richTextBox2.AppendText($"Final Size: {finalSize} bytes" + Environment.NewLine);
                richTextBox2.AppendText($"Time required for archivation: {stopwatch.Elapsed.TotalSeconds} seconds" + Environment.NewLine);
                richTextBox2.AppendText($"Number of Volumes: {volumePaths.Count}" + Environment.NewLine);
            }
            else
            {
                richTextBox2.AppendText("Archiving with division into volumes failed!" + Environment.NewLine);
            }
        }

        private long GetTotalSizeOfFiles(List<string> filePaths)
        {
            long totalSize = 0;
            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    totalSize += new FileInfo(filePath).Length;
                }
                else if (Directory.Exists(filePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    totalSize += directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
                        .Sum(file => file.Length);
                }
            }
            return totalSize;
        }

        private void button_choose_archive_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Оберіть архів який треба розархівувати";
            openFileDialog.Filter = "ZIP (*.zip*)|*.zip*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected archive name
                string filePath = openFileDialog.FileName;
                richTextBox3.Text = filePath;
            }
        }

        private void button_dearchivate_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            // Check if division into volumes is enabled (checkBox2 is checked)
            bool isdividedIntoVolumes = checkBox2.Checked;
            string directoryPath = "";
            string archivePath = "";
            List<string> filePathsList = new List<string>();
            if (!isdividedIntoVolumes)
            {
                archivePath = richTextBox3.Lines.FirstOrDefault(line => !string.IsNullOrWhiteSpace(line));
            }
            else
            {
                // Define the pattern for the volume name
                string volumeNamePattern = @".*\.z\d+$";
                // Split the text in richTextBox5 by newline characters
                foreach (string line in richTextBox5.Lines)
                {
                    if (!string.IsNullOrWhiteSpace(line)) // Check if the line is not empty or just spaces
                    {
                        string trimmedLine = line.Trim(); // Trim any leading/trailing spaces
                        if (File.Exists(trimmedLine))
                        {
                            if (!filePathsList.Contains(trimmedLine))
                            {
                                // Check if the file name matches the specified pattern
                                if (Regex.IsMatch(Path.GetFileName(trimmedLine), volumeNamePattern))
                                {
                                    filePathsList.Add(trimmedLine);
                                }
                                else
                                {
                                    MessageBox.Show($"Невірне ім'я тому. Впевніться що обрано вірні файли томів, з правильним ім'ям та розширенням томів: '{line}'", "Error. Invalid Volume Name");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Файл '{line}' вже було обрано раніше.", "Помилка. Duplicate File");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Файл '{line}' не існує.", "Помилка. Неіснуючий файл або тека");
                            return;
                        }
                    }
                }

                if (filePathsList.Count == 0)
                {
                    MessageBox.Show("Не вибрано жодного файлу томів.", "Помилка. Нічого не вибрано.");
                    return; // Do not proceed with archiving
                }
            }

            if (!string.IsNullOrEmpty(archivePath) || filePathsList.Count != 0)
            {
                // Get the dearchivation folder
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.Description = "Оберіть шлях розархівації";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected folder path
                    directoryPath = folderBrowserDialog.SelectedPath;
                }

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    if (isdividedIntoVolumes)
                    {
                        // Try to extract the files and log the result
                        Stopwatch stopwatch = Stopwatch.StartNew();

                        bool extractionSuccess = DearchiveVolumes(filePathsList, directoryPath, stopwatch);
                        stopwatch.Stop();
                        // Log the extraction result
                        LogExtractionResultWithVolumes(directoryPath, extractionSuccess, stopwatch);
                    }
                    else
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        // Perform the extraction
                        bool extractionSuccess = ExtractArchive(archivePath, directoryPath, ref stopwatch);
                        stopwatch.Stop();
                        // Log the extraction result
                        LogExtractionResult(archivePath, directoryPath, extractionSuccess, stopwatch);
                    }
                }
                else
                {
                    MessageBox.Show("Оберіть шлях призначення розархівування!", "Помилка. Шлях розархівування не вибрано.");
                }
            }
            else
            {
                MessageBox.Show("Оберіть архів для розархівування!", "Помилка. Архів не вибрано.");
            }
        }

        private void LogExtractionResult(string archivePath, string directoryPath, bool success, Stopwatch stopwatch)
        {
            richTextBox2.Clear();
            if (success)
            {
                richTextBox2.AppendText("Successful extraction!" + Environment.NewLine);
                richTextBox2.AppendText($"Archive Path: {archivePath}" + Environment.NewLine);
                richTextBox2.AppendText($"Extraction Directory: {directoryPath}" + Environment.NewLine);
                richTextBox2.AppendText($"Time required for extraction: {stopwatch.Elapsed.TotalSeconds} seconds" + Environment.NewLine);
            }
            else
            {
                richTextBox2.AppendText("Extraction failed!" + Environment.NewLine);
            }
        }

        private void LogExtractionResultWithVolumes(string directoryPath, bool success, Stopwatch stopwatch)
        {
            richTextBox2.Clear();
            if (success)
            {
                richTextBox2.AppendText("Successful extraction of archive divided into volumes!" + Environment.NewLine);
                richTextBox2.AppendText($"Extraction Directory: {directoryPath}" + Environment.NewLine);
                richTextBox2.AppendText($"Time required for extraction: {stopwatch.Elapsed.TotalSeconds} seconds" + Environment.NewLine);
            }
            else
            {
                richTextBox2.AppendText("Extraction of archive divided into volumes failed!" + Environment.NewLine);
            }
        }

        private void button_choose_volumes_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Оберіть томи які треба розпакувати";
            openFileDialog.Multiselect = true; // Allow multiple file selection
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file names
                foreach (string filePath in openFileDialog.FileNames)
                {
                    richTextBox5.AppendText(filePath + Environment.NewLine);
                }
            }
        }
    }
}