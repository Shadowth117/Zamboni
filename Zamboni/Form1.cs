// Decompiled with JetBrains decompiler
// Type: zamboni.Form1
// Assembly: zamboni, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 73B487C9-8F41-4586-BEF5-F7D7BFBD4C55
// Assembly location: D:\Downloads\zamboni_ngs (3)\zamboni.exe

using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zamboni;

namespace zomForm
{
    public class Form1 : Form
    {
        private const uint CharacterBlowfishKey = 2588334024;
        private IContainer components = (IContainer)null;
        private Button button1;
        private OpenFileDialog extractOpenDialog;
        private Button button2;
        private OpenFileDialog openHeaderDialog;
        private OpenFileDialog openGroup1Dialog;
        private OpenFileDialog openGroup2Dialog;
        private Button button3;
        private OpenFileDialog openListDialog1;
        private CommonOpenFileDialog batchFolderBrowserDialog;
        private Button button4;
        private Label label1;
        private CheckBox checkBox1;
        private TextBox textBox1;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private CheckBox compressCheckbox;
        private Button button9;
        private CheckBox forceUnencryptCheckbox;
        private Button button10;

        public Form1() => this.InitializeComponent();

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.extractOpenDialog.ShowDialog() != DialogResult.OK)
                return;
            using (Stream inStream = this.extractOpenDialog.OpenFile())
            {
                IceFile iceFile = IceFile.LoadIceFile(inStream);
                string str = Path.GetDirectoryName(this.extractOpenDialog.FileName) + "\\";
                string withoutExtension = Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName);
                string path1 = str;
                if (this.textBox1.Text != "")
                    path1 = !this.checkBox1.Checked ? this.textBox1.Text + "\\" : path1 + this.textBox1.Text + "\\";
                if (iceFile != null)
                {
                    FileStream fileStream = new FileStream(Path.Combine(path1, withoutExtension + ".hdr"), FileMode.Create);
                    fileStream.Write(iceFile.header, 0, iceFile.header.Length);
                    fileStream.Close();
                    if (iceFile.groupOneFiles != null && (uint)iceFile.groupOneFiles.Length > 0U)
                        Form1.writeGroupToDirectory(iceFile.groupOneFiles, Path.Combine(path1, withoutExtension + "_g1"));
                    if (iceFile.groupTwoFiles != null && (uint)iceFile.groupTwoFiles.Length > 0U)
                        Form1.writeGroupToDirectory(iceFile.groupTwoFiles, Path.Combine(path1, withoutExtension + "_g2"));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openHeaderDialog.ShowDialog() != DialogResult.OK)
                return;
            string path1 = Path.GetDirectoryName(this.extractOpenDialog.FileName) + "\\";
            string str = path1;
            if (this.textBox1.Text != "")
                str = !this.checkBox1.Checked ? this.textBox1.Text + "\\" : str + this.textBox1.Text + "\\";
            string withoutExtension = Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName);
            Stream input = this.openHeaderDialog.OpenFile();
            BinaryReader binaryReader = new BinaryReader(input);
            byte[] headerData = binaryReader.ReadBytes((int)input.Length);
            binaryReader.Close();
            byte[][] groupOneIn = new byte[0][];
            byte[][] groupTwoIn = new byte[0][];
            if (Directory.Exists(Path.Combine(path1, withoutExtension + "_g1")))
                groupOneIn = Form1.loadGroupFromDirectory(Path.Combine(path1, withoutExtension + "_g1"));
            if (Directory.Exists(Path.Combine(path1, withoutExtension + "_g2")))
                groupTwoIn = Form1.loadGroupFromDirectory(Path.Combine(path1, withoutExtension + "_g2"));
            byte[] rawData = new IceV4File(headerData, groupOneIn, groupTwoIn).getRawData(this.compressCheckbox.Checked, this.forceUnencryptCheckbox.Checked);
            FileStream fileStream = new FileStream(str + "\\" + withoutExtension + ".out", FileMode.Create);
            fileStream.Write(rawData, 0, rawData.Length);
            fileStream.Close();
        }

        private void buttonBatchExtract_Click(object sender, EventArgs e)
        {
            if (this.openListDialog1.ShowDialog() != DialogResult.OK || this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string fileName = this.openListDialog1.FileName;
            string basePath = this.batchFolderBrowserDialog.FileName + "\\";
            string exportPath = basePath;
            if (this.textBox1.Text != "")
                exportPath = !this.checkBox1.Checked ? this.textBox1.Text + "\\" : exportPath + this.textBox1.Text + "\\";
            Form1.exportFilesToDirectory(System.IO.File.ReadAllLines(fileName), basePath, exportPath);
        }

        public static void exportFilesToDirectory(
          string[] fileList,
          string basePath,
          string exportPath)
        {
            Parallel.ForEach<string>((IEnumerable<string>)fileList, (Action<string>)(currFile =>
            {
                string[] strArray = currFile.Split('\t');
                if (!System.IO.File.Exists(Path.Combine(basePath, strArray[0])))
                    return;
                IceFile iceFile = IceFile.LoadIceFile((Stream)new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(basePath, strArray[0]))));
                if (iceFile != null)
                {
                    FileStream fileStream = new FileStream(Path.Combine(exportPath, strArray[1]), FileMode.Create);
                    fileStream.Write(iceFile.header, 0, iceFile.header.Length);
                    fileStream.Close();
                    if (strArray[2] != "")
                        Form1.writeGroupToDirectory(iceFile.groupOneFiles, Path.Combine(exportPath, strArray[2]));
                    if (strArray[3] != "")
                        Form1.writeGroupToDirectory(iceFile.groupTwoFiles, Path.Combine(exportPath, strArray[3]));
                }
            }));
        }

        private static bool writeGroupToDirectory(byte[][] groupToWrite, string directory)
        {
            if (!Directory.Exists(directory) && groupToWrite != null && (uint)groupToWrite.Length > 0U)
                Directory.CreateDirectory(directory);
            for (int index = 0; index < groupToWrite.Length; ++index)
            {
                int int32 = BitConverter.ToInt32(groupToWrite[index], 16);
                string str = Encoding.ASCII.GetString(groupToWrite[index], 64, int32).TrimEnd(new char[1]);

                //System.IO.File.WriteAllBytes(directory + "\\" + str + ".full", groupToWrite[index]);

                int iceHeaderSize = BitConverter.ToInt32(groupToWrite[index], 0xC);
                int newLength = groupToWrite[index].Length - iceHeaderSize;
                byte[] file = new byte[newLength];
                Array.ConstrainedCopy(groupToWrite[index], iceHeaderSize, file, 0, newLength);
                System.IO.File.WriteAllBytes(directory + "\\" + str, file);
                file = null;
                groupToWrite[index] = null;
            }

            if(groupToWrite == null || (uint)groupToWrite.Length == 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private void batchPackClick(object sender, EventArgs e)
        {
            if (this.openListDialog1.ShowDialog() != DialogResult.OK || this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string[] fileList = System.IO.File.ReadAllLines(this.openListDialog1.FileName);
            string selectedPath = this.batchFolderBrowserDialog.FileName;
            string saveDirectory = selectedPath;
            if (this.textBox1.Text != "")
                saveDirectory = !this.checkBox1.Checked ? this.textBox1.Text : saveDirectory + this.textBox1.Text;
            Form1.buildIceFromDirectory(fileList, selectedPath, saveDirectory, this.compressCheckbox.Checked, this.forceUnencryptCheckbox.Checked);
        }

        public static void buildIceFromDirectory(
          string[] fileList,
          string baseDirectory,
          string saveDirectory,
          bool compress,
          bool forceUnencrypted = false)
        {
            Parallel.ForEach<string>((IEnumerable<string>)fileList, (Action<string>)(currFile =>
            {
                string[] strArray = currFile.Split('\t');
                if (!System.IO.File.Exists(Path.Combine(baseDirectory, strArray[1])))
                    return;
                Stream input = (Stream)System.IO.File.Open(Path.Combine(baseDirectory, strArray[1]), FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(input);
                byte[] headerData = binaryReader.ReadBytes((int)input.Length);
                binaryReader.Close();
                byte[][] groupOneIn = new byte[0][];
                byte[][] groupTwoIn = new byte[0][];
                if (strArray[2] != "" && Directory.Exists(Path.Combine(baseDirectory, strArray[2])))
                    groupOneIn = Form1.loadGroupFromDirectory(Path.Combine(baseDirectory, strArray[2]));
                if (strArray[3] != "" && Directory.Exists(Path.Combine(baseDirectory, strArray[3])))
                    groupTwoIn = Form1.loadGroupFromDirectory(Path.Combine(baseDirectory, strArray[3]));
                byte[] rawData = new IceV4File(headerData, groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypted);
                FileStream fileStream = new FileStream(saveDirectory + "\\" + strArray[0], FileMode.Create);
                fileStream.Write(rawData, 0, rawData.Length);
                fileStream.Close();
            }));
        }

        private static byte[][] loadGroupFromDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            byte[][] numArray = new byte[files.Length][];
            for (int index = 0; index < files.Length; ++index)
                numArray[index] = System.IO.File.ReadAllBytes(files[index]);
            return numArray;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.extractOpenDialog.ShowDialog() != DialogResult.OK)
                return;
            Stream inStream = this.extractOpenDialog.OpenFile();
            IceFile iceFile = IceFile.LoadIceFile(inStream);
            inStream.Close();
            string directoryName = Path.GetDirectoryName(this.extractOpenDialog.FileName);
            byte[][] groupOneFiles = iceFile.groupOneFiles;
            if (groupOneFiles != null)
            {
                if (!Directory.Exists(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_1"))
                    Directory.CreateDirectory(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_1");
                for (int index = 0; index < groupOneFiles.Length; ++index)
                {
                    int int32_1 = BitConverter.ToInt32(groupOneFiles[index], 16);
                    string str = Encoding.ASCII.GetString(groupOneFiles[index], 64, int32_1).TrimEnd(new char[1]);
                    if (str.EndsWith(".lua"))
                    {
                        int int32_2 = BitConverter.ToInt32(groupOneFiles[index], 12);
                        byte[] bytes = new byte[groupOneFiles[index].Length - int32_2];
                        Array.Copy((Array)groupOneFiles[index], int32_2, (Array)bytes, 0, bytes.Length);
                        System.IO.File.WriteAllBytes(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_1\\" + str, bytes);
                    }
                    else
                        System.IO.File.WriteAllBytes(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_1\\" + str, groupOneFiles[index]);
                }
            }
            byte[][] groupTwoFiles = iceFile.groupTwoFiles;
            if (groupTwoFiles != null)
            {
                if (!Directory.Exists(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_2") && (uint)groupTwoFiles.Length > 0U)
                    Directory.CreateDirectory(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_2");
                for (int index = 0; index < groupTwoFiles.Length; ++index)
                {
                    int int32 = BitConverter.ToInt32(groupTwoFiles[index], 16);
                    string str = Encoding.ASCII.GetString(groupTwoFiles[index], 64, int32).TrimEnd(new char[1]);
                    System.IO.File.WriteAllBytes(directoryName + "\\" + Path.GetFileNameWithoutExtension(this.extractOpenDialog.FileName) + "_group_2\\" + str, groupTwoFiles[index]);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string[] files = Directory.GetFiles(this.batchFolderBrowserDialog.FileName);
            StreamWriter streamWriter = new StreamWriter(this.batchFolderBrowserDialog.FileName + "\\file_contents.txt");
            bool flag = true;
            foreach (string path in files)
            {
                if (flag && !path.Contains("."))
                {
                    FileStream fileStream = System.IO.File.Open(path, FileMode.Open);
                    byte[] numArray = new byte[4];
                    fileStream.Read(numArray, 0, 4);
                    fileStream.Seek(0L, SeekOrigin.Begin);
                    string str1 = Encoding.ASCII.GetString(numArray);
                    streamWriter.WriteLine(Path.GetFileName(path) + "\t\t\t" + str1.Replace("\0", ""));
                    try
                    {
                        if (str1 == "ICE\0")
                        {
                            IceFile iceFile = (IceFile)new IceV4File((Stream)fileStream);
                            fileStream.Close();
                            if (iceFile != null)
                            {
                                if (iceFile.groupOneFiles != null)
                                {
                                    streamWriter.WriteLine("\tGroup 1:");
                                    byte[][] groupOneFiles = iceFile.groupOneFiles;
                                    for (int index = 0; index < groupOneFiles.Length; ++index)
                                    {
                                        int int32 = BitConverter.ToInt32(groupOneFiles[index], 16);
                                        string str2 = Encoding.ASCII.GetString(groupOneFiles[index], 64, int32).TrimEnd(new char[1]);
                                        streamWriter.WriteLine("\t\t" + str2);
                                    }
                                }
                                if (iceFile.groupOneFiles != null)
                                {
                                    streamWriter.WriteLine("\tGroup 2:");
                                    byte[][] groupTwoFiles = iceFile.groupTwoFiles;
                                    for (int index = 0; index < groupTwoFiles.Length; ++index)
                                    {
                                        int int32 = BitConverter.ToInt32(groupTwoFiles[index], 16);
                                        string str2 = Encoding.ASCII.GetString(groupTwoFiles[index], 64, int32).TrimEnd(new char[1]);
                                        streamWriter.WriteLine("\t\t" + str2);
                                    }
                                }
                                streamWriter.WriteLine();
                                streamWriter.WriteLine();
                            }
                        }
                        else
                            fileStream.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            streamWriter.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.openListDialog1.ShowDialog() != DialogResult.OK || this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            Form1.DownloadFilesFromList(System.IO.File.ReadAllLines(this.openListDialog1.FileName), this.batchFolderBrowserDialog.FileName + "\\");
        }

        public static void DownloadFilesFromList(string[] fileList, string directoryToSave)
        {
            StreamWriter streamWriter = new StreamWriter(directoryToSave + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString("d2") + "_" + DateTime.Now.Day.ToString("d2") + ".txt");
            WebClient webClient = new WebClient();
            foreach (string file in fileList)
            {
                char[] chArray = new char[1] { '\t' };
                string[] strArray = file.Split(chArray);
                try
                {
                    webClient.Headers.Add("user-agent", "AQUA_HTTP");
                    //webClient.DownloadFile("http://download.pso2.jp/patch_prod/head_rc_4161_masterbase/patches/" + "launcherlist.txt", directoryToSave + "\\" + "launcherlist.txt");
                    //webClient.DownloadFile("http://download.pso2.jp/patch_prod/v70000_rc_180_872FCD51/patches/" + "patchlist_region1st.txt", directoryToSave + "\\" + "patchlist_region1st.txt");
                    //webClient.DownloadFile("http://download.pso2.jp/patch_prod/head_rc_4161_masterbase/patches/data/win32/" + "00049841d10e1e00b22f84204f8b092e" + ".pat", directoryToSave + "\\" + "00049841d10e1e00b22f84204f8b092e");
                    //webClient.DownloadFile("http://download.pso2.jp/patch_prod/head_rc_4161_masterbase/patches/data/win32/" + strArray[0] + ".pat", directoryToSave + "\\" + strArray[0]);
                    webClient.DownloadFile("http://download.pso2.jp/patch_prod/v70000_rc_180_872FCD51/patches/data/win32reboot/9c/a433e75e9cef9c6d0a318bde62bda6" + ".pat", directoryToSave + "\\" + "9ca433e75e9cef9c6d0a318bde62bda6");
                    //webClient.DownloadFile("http://download.pso2.jp/patch_prod/patches/data/win32/" + strArray[0] + ".pat", directoryToSave + "\\" + strArray[0]);
                    streamWriter.WriteLine(strArray[0] + "\tpatches");
                    streamWriter.Flush();
                }
                catch (Exception ex1)
                {
                    try
                    {
                        webClient.Headers.Add("user-agent", "AQUA_HTTP");
                        webClient.DownloadFile("http://sub-download.pso2.jp/patch_prod/head_rc_4161_masterbase/patches/data/win32/" + strArray[0] + ".pat", directoryToSave + "\\" + strArray[0]);
                        streamWriter.WriteLine(strArray[0] + "\told patches");
                        streamWriter.Flush();
                    }
                    catch (Exception ex2)
                    {
                        streamWriter.WriteLine(strArray[0] + "\tfailed");
                        streamWriter.Flush();
                    }
                }
            }
            streamWriter.Close();
        }

        private void button8_Click(object sender, EventArgs e) => new BlewfishReversed(2788572218U).encrypt(new uint[2]);

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.openListDialog1.ShowDialog() != DialogResult.OK || this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string[] fileList = System.IO.File.ReadAllLines(this.openListDialog1.FileName);
            string selectedPath = this.batchFolderBrowserDialog.FileName;
            string saveDirectory = selectedPath;
            if (this.textBox1.Text != "")
                saveDirectory = !this.checkBox1.Checked ? this.textBox1.Text : saveDirectory + this.textBox1.Text;
            Form1.buildIceFromDirectorySequential(fileList, selectedPath, saveDirectory, this.compressCheckbox.Checked, this.forceUnencryptCheckbox.Checked);
        }

        public static void buildIceFromDirectorySequential(
          string[] fileList,
          string baseDirectory,
          string saveDirectory,
          bool compress,
          bool forceUnencrypt)
        {
            foreach (string file in fileList)
            {
                char[] chArray = new char[1] { '\t' };
                string[] strArray = file.Split(chArray);
                if (System.IO.File.Exists(Path.Combine(baseDirectory, strArray[1])))
                {
                    Stream input = (Stream)System.IO.File.Open(Path.Combine(baseDirectory, strArray[1]), FileMode.Open);
                    BinaryReader binaryReader = new BinaryReader(input);
                    byte[] headerData = binaryReader.ReadBytes((int)input.Length);
                    binaryReader.Close();
                    byte[][] groupOneIn = new byte[0][];
                    byte[][] groupTwoIn = new byte[0][];
                    if (strArray[2] != "" && Directory.Exists(Path.Combine(baseDirectory, strArray[2])))
                        groupOneIn = Form1.loadGroupFromDirectory(Path.Combine(baseDirectory, strArray[2]));
                    if (strArray[3] != "" && Directory.Exists(Path.Combine(baseDirectory, strArray[3])))
                        groupTwoIn = Form1.loadGroupFromDirectory(Path.Combine(baseDirectory, strArray[3]));
                    byte[] rawData = new IceV4File(headerData, groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypt);
                    FileStream fileStream = new FileStream(saveDirectory + "\\" + strArray[0], FileMode.Create);
                    fileStream.Write(rawData, 0, rawData.Length);
                    fileStream.Close();
                }
            }
        }

        public void button10_Click(object sender, EventArgs e)
        {
            if (this.batchFolderBrowserDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;
            string basePath = this.batchFolderBrowserDialog.FileName;
            string exportPath = basePath + "\\";
            if (this.textBox1.Text != "")
                exportPath = !this.checkBox1.Checked ? this.textBox1.Text + "\\" : exportPath + this.textBox1.Text + "\\";
            Directory.GetFiles(this.batchFolderBrowserDialog.FileName);
            ExtractIceFromPath(this.batchFolderBrowserDialog.FileName, basePath, exportPath, true, true);
        }

        public static void ExtractIceFromPath(string extractPath, string basePath, string exportPath, bool useGroups, bool searchSub, bool logging = true)
        {
            List<string> log = new List<string>();
            var option = SearchOption.AllDirectories;
            if (searchSub == false)
            {
                option = SearchOption.TopDirectoryOnly;
            }
            Parallel.ForEach<string>(Directory.EnumerateFiles(extractPath, "*.*", option), (Action<string>)(currFile =>
            {
                string result = ExtractIce(basePath, exportPath, currFile, useGroups);
                if(result != null)
                {
                    log.Add(result);
                }
            }));
            if(logging = true)
            {
                File.WriteAllLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\log.txt", log);
            }
        }

        public static string ExtractIce(string basePath, string exportPath, string currFile, bool useGroups)
        {
            byte[] buffer = System.IO.File.ReadAllBytes(currFile);
#if DEBUG
            Console.WriteLine(currFile);
#endif
            try
            {
                if (buffer.Length <= 127 || buffer[0] != (byte)73 || (buffer[1] != (byte)67 || buffer[2] != (byte)69) || buffer[3] != (byte)0 )
                {
                    buffer = null;
                    return null;
                }

                IceFile iceFile = IceFile.LoadIceFile((Stream)new MemoryStream(buffer));

                if (iceFile != null)
                {
                    string fileName = Path.GetFileName(currFile);
                    string directoryName = Path.GetDirectoryName(currFile);
                    string str1 = !directoryName.Equals(basePath) ? Path.GetFileName(directoryName) + "_" : "";
                    string str2 = Path.Combine(exportPath, str1 + Path.GetFileName(currFile) + "_ext");
                    if (!Directory.Exists(str2))
                        Directory.CreateDirectory(str2);

                    //using (FileStream fileStream = new FileStream(Path.Combine(str2, str1 + fileName + ".hdr"), FileMode.Create))
                      //  fileStream.Write(iceFile.header, 0, iceFile.header.Length);

                    string group1Path;
                    string group2Path;
                    if(useGroups)
                    {
                        group1Path = Path.Combine(exportPath, Path.Combine(str2, "group1"));
                        group2Path = Path.Combine(exportPath, Path.Combine(str2, "group2"));
                    } else
                    {
                        group1Path = str2;
                        group2Path = str2;
                    }

                    bool group1 = Form1.writeGroupToDirectory(iceFile.groupOneFiles, group1Path);
                    bool group2 = Form1.writeGroupToDirectory(iceFile.groupTwoFiles, group2Path);
                    if (group1 == false && group2 == false)
                    {
                        Console.WriteLine($"Neither group1 nor group2 was dumped from {Path.GetFileName(currFile)}.");
                    }
                } else
                {
                    Console.WriteLine("Ice reading failed");
                }
                iceFile = null;
            }
            catch (Exception ex)
            {
                string error = currFile + " could not be extracted";
                Console.WriteLine(currFile + " could not be extracted");
                return error;
            }
            buffer = null;
            return null;
        }

        public static void ListIceFromPath(string extractPath, string basePath, string exportPath, bool useGroups, bool searchSub)
        {
            StringBuilder log = new StringBuilder();
            var option = SearchOption.AllDirectories;
            if (searchSub == false)
            {
                option = SearchOption.TopDirectoryOnly;
            }
            Dictionary<string, StringBuilder> sbDict = new Dictionary<string, StringBuilder>();
            var filesE = Directory.EnumerateFiles(extractPath, "*.*", option);
            List<string> files = filesE.ToList();
            files.Sort();

            foreach(string str in files)
            {
                log.Append(ListIce(basePath, str));
            }
            File.WriteAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\IceFileList.txt", log.ToString());
        }

        public static StringBuilder ListIce(string basePath, string currFile)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = System.IO.File.ReadAllBytes(currFile);
            if(buffer.Length < 4)
            {
                var earlyOut = currFile.Replace(basePath, "") + " " + Path.GetExtension(currFile);
                earlyOut = earlyOut.Substring(1);
                sb.AppendLine(earlyOut);
                return sb;
            }

            int extTest = BitConverter.ToInt32(buffer, 0);
            string extension;

            switch(extTest)
            {
                case 0xC1C3C8: //HCA
                    extension = "HCA";
                    break;
                case 0x32534641: //AFS2
                    extension = "AWB";
                    break;
                default:
                    byte[] typeBuffer = new byte[4];
                    Array.Copy(buffer, 0, typeBuffer, 0, 4);
                    extension = Encoding.ASCII.GetString(typeBuffer);
                    break;
            }
            var zeroIndex = extension.IndexOf(char.MinValue);
            if(zeroIndex > 0)
            {
                extension = extension.Remove(zeroIndex);
            }
            extension = extension.Replace("?", "");

            var path = currFile.Replace(basePath, "");
            var stringOut = path + " " + extension;
            stringOut = stringOut.Substring(1);
            sb.AppendLine(stringOut);
#if DEBUG
            //      Console.WriteLine(currFile);
#endif
            try
            {
                if (buffer.Length <= 127 || buffer[0] != (byte)73 || (buffer[1] != (byte)67 || buffer[2] != (byte)69) || buffer[3] != (byte)0)
                {
                    buffer = null;
                    return sb;
                }

                IceFile iceFile = IceFile.LoadIceFile((Stream)new MemoryStream(buffer));

                if (iceFile != null)
                {
                    if(iceFile.groupOneFiles.Length > 0)
                    {
                        sb.AppendLine("  Group 1 Contents:");
                        foreach (var file in iceFile.groupOneFiles)
                        {
                            sb.AppendLine("    " + path + " " + IceFile.getFileName(file));
                        }
                    }

                    if (iceFile.groupTwoFiles.Length > 0)
                    {
                        sb.AppendLine("  Group 2 Contents:");
                        foreach (var file in iceFile.groupTwoFiles)
                        {
                            sb.AppendLine("    " + path + " " + IceFile.getFileName(file));
                        }
                    }
                }
                else
                {
                    sb.AppendLine(Path.GetFileName(currFile) + " could not be extracted");
                }
                iceFile = null;
            }
            catch (Exception ex)
            {
                sb.AppendLine(Path.GetFileName(currFile) + " could not be extracted");
            }
            return sb;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.extractOpenDialog = new OpenFileDialog();
            this.button2 = new Button();
            this.openHeaderDialog = new OpenFileDialog();
            this.openGroup1Dialog = new OpenFileDialog();
            this.openGroup2Dialog = new OpenFileDialog();
            this.button3 = new Button();
            this.openListDialog1 = new OpenFileDialog();
            this.batchFolderBrowserDialog = new CommonOpenFileDialog();
            batchFolderBrowserDialog.IsFolderPicker = true;
            this.button4 = new Button();
            this.label1 = new Label();
            this.checkBox1 = new CheckBox();
            this.textBox1 = new TextBox();
            this.button5 = new Button();
            this.button6 = new Button();
            this.button7 = new Button();
            this.button8 = new Button();
            this.compressCheckbox = new CheckBox();
            this.button9 = new Button();
            this.forceUnencryptCheckbox = new CheckBox();
            this.button10 = new Button();
            this.SuspendLayout();
            this.button1.Location = new Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Extract ICE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.extractOpenDialog.FileName = "openFileDialog1";
            this.button2.Location = new Point(13, 43);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Pack ICE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.openHeaderDialog.FileName = "openHeaderDialog";
            this.openGroup1Dialog.FileName = "openFileDialog2";
            this.openGroup2Dialog.FileName = "openFileDialog3";
            this.button3.Location = new Point(13, 118);
            this.button3.Name = "button3";
            this.button3.Size = new Size(91, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Batch Extract";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.buttonBatchExtract_Click);
            this.openListDialog1.FileName = "openFileDialog1";
            this.button4.Location = new Point(13, 147);
            this.button4.Name = "button4";
            this.button4.Size = new Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Batch pack";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.batchPackClick);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 208);
            this.label1.Name = "label1";
            this.label1.Size = new Size(67, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Export path: ";
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(86, 208);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(98, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Relative to files";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.textBox1.Location = new Point(13, 229);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(367, 20);
            this.textBox1.TabIndex = 7;
            this.button5.Location = new Point(95, 12);
            this.button5.Name = "button5";
            this.button5.Size = new Size(75, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Extract luas";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new EventHandler(this.button5_Click);
            this.button6.Location = new Point(305, 13);
            this.button6.Name = "button6";
            this.button6.Size = new Size(75, 23);
            this.button6.TabIndex = 9;
            this.button6.Text = "List all files";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new EventHandler(this.button6_Click);
            this.button7.Location = new Point(15, 177);
            this.button7.Name = "button7";
            this.button7.Size = new Size(104, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "Download files...";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new EventHandler(this.button7_Click);
            this.button8.Location = new Point(305, 43);
            this.button8.Name = "button8";
            this.button8.Size = new Size(75, 23);
            this.button8.TabIndex = 11;
            this.button8.Text = "Test character create";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new EventHandler(this.button8_Click);
            this.compressCheckbox.AutoSize = true;
            this.compressCheckbox.Location = new Point(200, 153);
            this.compressCheckbox.Name = "compressCheckbox";
            this.compressCheckbox.Size = new Size(72, 17);
            this.compressCheckbox.TabIndex = 12;
            this.compressCheckbox.Text = "Compress";
            this.compressCheckbox.UseVisualStyleBackColor = true;
            this.button9.Location = new Point(95, 148);
            this.button9.Name = "button9";
            this.button9.Size = new Size(99, 23);
            this.button9.TabIndex = 13;
            this.button9.Text = "Pack (sequential)";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new EventHandler(this.button9_Click);
            this.forceUnencryptCheckbox.AutoSize = true;
            this.forceUnencryptCheckbox.Location = new Point(278, 152);
            this.forceUnencryptCheckbox.Name = "forceUnencryptCheckbox";
            this.forceUnencryptCheckbox.Size = new Size(74, 17);
            this.forceUnencryptCheckbox.TabIndex = 14;
            this.forceUnencryptCheckbox.Text = "Vita Mode";
            this.forceUnencryptCheckbox.UseVisualStyleBackColor = true;
            this.button10.Location = new Point(115, 118);
            this.button10.Name = "button10";
            this.button10.Size = new Size(157, 23);
            this.button10.TabIndex = 15;
            this.button10.Text = "Extract All ICE files";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new EventHandler(this.button10_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(392, 266);
            this.Controls.Add((Control)this.button10);
            this.Controls.Add((Control)this.forceUnencryptCheckbox);
            this.Controls.Add((Control)this.button9);
            this.Controls.Add((Control)this.compressCheckbox);
            this.Controls.Add((Control)this.button8);
            this.Controls.Add((Control)this.button7);
            this.Controls.Add((Control)this.button6);
            this.Controls.Add((Control)this.button5);
            this.Controls.Add((Control)this.textBox1);
            this.Controls.Add((Control)this.checkBox1);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.button4);
            this.Controls.Add((Control)this.button3);
            this.Controls.Add((Control)this.button2);
            this.Controls.Add((Control)this.button1);
            this.Name = nameof(Form1);
            this.Text = nameof(Form1);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private class BruteForcer
        {
            private uint[] encrypted = new uint[2]
            {
        387260108U,
        2816776519U
            };
            private BlewFish temp = new BlewFish(0U);
            public uint startValue = 0;
            public uint endValue = 1073741824;

            public void AttemptRange()
            {
                for (uint startValue = this.startValue; startValue < this.endValue; ++startValue)
                {
                    this.temp.setKey(startValue);
                    uint[] numArray = this.temp.decrypt(this.encrypted);
                    if (numArray[0] == 0U && numArray[1] == 0U)
                        throw new Exception("Found key: " + startValue.ToString());
                }
            }
        }
    }
}
