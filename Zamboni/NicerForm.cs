using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zamboni;
using zomForm;

namespace zomFormNew
{
    public partial class NicerForm : Form
    {
        public NicerForm()
        {
            InitializeComponent();
            this.DragEnter += new DragEventHandler(ZamboniDragEnter);
            this.DragDrop += new DragEventHandler(ZamboniDragDrop);
        }

        private void ZamboniDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void ZamboniDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach(var file in files)
            {
                Form1.ExtractIce(Path.GetDirectoryName(file), "", file, useGroupFolders.Checked);
            }
        }

        private void extractIceButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a PSO2 ICE file";
            fileDialog.FileName = "";

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            string result = Form1.ExtractIce(Path.GetDirectoryName(fileDialog.FileName), Path.GetDirectoryName(fileDialog.FileName), fileDialog.FileName, useGroupFolders.Checked);
            if(result != null)
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "log.txt"), result);
            }
        }

        private void batchExtractIceButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog batchFolderBrowserDialog = new CommonOpenFileDialog();
            batchFolderBrowserDialog.IsFolderPicker = true;
            batchFolderBrowserDialog.Title = "Select a base directory";

            if (batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string basePath = batchFolderBrowserDialog.FileName;
            string exportPath = basePath + "\\";
            Directory.GetFiles(batchFolderBrowserDialog.FileName);
            Form1.ExtractIceFromPath(batchFolderBrowserDialog.FileName, basePath, exportPath, useGroupFolders.Checked, searchSubCheck.Checked);
        }

        private void packIceButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog batchFolderBrowserDialog = new CommonOpenFileDialog();
            batchFolderBrowserDialog.IsFolderPicker = true;
            batchFolderBrowserDialog.Title = "Select a base directory";

            if (batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            UILogic.packIceFromDirectoryToFile(batchFolderBrowserDialog.FileName, 
                UILogic.ReadWhiteList(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "group1.txt")),
                searchSubCheck.Checked, compressCheckBox.Checked, useGroupFolders.Checked, false, null);
        }

        private void batchListIceContentsButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog batchFolderBrowserDialog = new CommonOpenFileDialog();
            batchFolderBrowserDialog.IsFolderPicker = true;
            batchFolderBrowserDialog.Title = "Select a base directory";

            if (batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string basePath = batchFolderBrowserDialog.FileName;
            string exportPath = basePath + "\\";
            Directory.GetFiles(batchFolderBrowserDialog.FileName);
            Form1.ListIceFromPath(batchFolderBrowserDialog.FileName, basePath, exportPath, useGroupFolders.Checked, searchSubCheck.Checked);
        }
    }
}
