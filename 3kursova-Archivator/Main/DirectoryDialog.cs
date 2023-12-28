using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _3kursova_Archivator
{
    public class CustomFolderBrowserDialog
    {
        private readonly FolderBrowserDialog folderBrowserDialog;
        private readonly OpenFileDialog openFileDialog;

        public CustomFolderBrowserDialog()
        {
            folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select Folders",
                ShowNewFolderButton = false,
            };

            openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Select Files",
                Filter = "All Files|*.*",
            };
        }

        public DialogResult ShowDialog()
        {
            DialogResult folderDialogResult = folderBrowserDialog.ShowDialog();
            if (folderDialogResult == DialogResult.OK)
            {
                DialogResult fileDialogResult = openFileDialog.ShowDialog();
                if (fileDialogResult == DialogResult.OK)
                {
                    return DialogResult.OK;
                }
                else
                {
                    // User canceled file selection, return only the selected folder.
                    return folderDialogResult;
                }
            }
            return folderDialogResult;
        }

        public List<string> SelectedItems
        {
            get
            {
                List<string> selectedItems = new List<string>();

                if (!string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    selectedItems.Add(folderBrowserDialog.SelectedPath);
                }

                selectedItems.AddRange(openFileDialog.FileNames.Select(fileName => fileName.Trim()));

                return selectedItems;
            }
        }
    }
}
