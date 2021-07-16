using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zomForm;

namespace zomFormNew
{
    public partial class NicerForm : Form
    {
        public bool groupFolders = true;
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
                Form1.ExtractIce(Path.GetDirectoryName(file), "", file, groupFolders);
            }
        }

        private void extractIceButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a PSO2 ICE file";
            fileDialog.FileName = "";

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            string result = Form1.ExtractIce(Path.GetDirectoryName(fileDialog.FileName), Path.GetDirectoryName(fileDialog.FileName), fileDialog.FileName, groupFolders);
            if(result != null)
            {
                File.WriteAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "log.txt", result);
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
            Form1.ExtractIceFromPath(batchFolderBrowserDialog.FileName, basePath, exportPath, groupFolders, searchSubCheck.Checked);
        }

        private void noGroupFolders_CheckedChanged(object sender, EventArgs e)
        {
            groupFolders = useGroupFolders.Checked;
        }
    }
}
