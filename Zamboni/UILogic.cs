using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zamboni;

namespace zomFormNew
{
    public class UILogic
    {
        public static void packIceFromDirectoryToFile(string path, List<string> group1WhiteList, bool searchSub, bool compress, bool useFolderGroup, bool forceUnencrypted = false, string newFilename = null)
        {
            if (!Directory.Exists(path))
                return;
            loadFilesFromDirectory(path, searchSub, group1WhiteList, out byte[][] groupOneIn, out byte[][] groupTwoIn, useFolderGroup, true);

            byte[] rawData = new IceV4File((new IceHeaderStructures.IceArchiveHeader()).GetBytes(), groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypted);
            newFilename = GetTrueFileName(path, newFilename);

            File.WriteAllBytes(newFilename, rawData);
        }

        public static byte[] packIceFromDirectoryToMemory(string path, List<string> group1WhiteList, bool searchSub, bool compress, bool useFolderGroup, bool forceUnencrypted = false)
        {
            if (!Directory.Exists(path))
                return null;
            loadFilesFromDirectory(path, searchSub, group1WhiteList, out byte[][] groupOneIn, out byte[][] groupTwoIn, useFolderGroup, true);

            byte[] rawData = new IceV4File((new IceHeaderStructures.IceArchiveHeader()).GetBytes(), groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypted);

            return rawData;
        }

        public static byte[] packIceFromMemoryToMemory(string[] fileNames, byte[][] files, List<string> group1WhiteList, bool compress, Dictionary<string, int> presetGroups, bool forceUnencrypted = false)
        {
            loadFilesFromMemory(fileNames, files, group1WhiteList, presetGroups, out byte[][] groupOneIn, out byte[][] groupTwoIn, true);

            byte[] rawData = new IceV4File((new IceHeaderStructures.IceArchiveHeader()).GetBytes(), groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypted);
            
            return rawData;
        }

        public static void packIceFromMemoryToFile(string path, string[] fileNames, byte[][] files, List<string> group1WhiteList, bool compress, Dictionary<string, int> presetGroups, bool forceUnencrypted = false, string newFilename = null)
        {
            loadFilesFromMemory(fileNames, files, group1WhiteList, presetGroups, out byte[][] groupOneIn, out byte[][] groupTwoIn, true);

            byte[] rawData = new IceV4File((new IceHeaderStructures.IceArchiveHeader()).GetBytes(), groupOneIn, groupTwoIn).getRawData(compress, forceUnencrypted);
            GetTrueFileName(path, newFilename);

            File.WriteAllBytes(newFilename, rawData);
        }

        public static List<string> ReadWhiteList(string group1WhiteListName)
        {
            List<string> group1WhiteList = new List<string>();
            if (group1WhiteListName != null && File.Exists(group1WhiteListName))
            {
                group1WhiteList = new List<string>(File.ReadAllLines(group1WhiteListName));
                for (int i = group1WhiteList.Count - 1; i > 0; i--)
                {
                    if (group1WhiteList[i].Contains("//") || group1WhiteList[i].Contains(";") || group1WhiteList[i].Contains(" ") || group1WhiteList[i] == "" 
                        || group1WhiteList[i].Contains(Encoding.UTF8.GetString(new byte[] { 9 })))
                    {
                        group1WhiteList.RemoveAt(i);
                    }
                }
            }

            return group1WhiteList;
        }

        public static string GetTrueFileName(string path, string newFilename)
        {
            if (newFilename == null && newFilename != "")
            {
                newFilename = path + ".ice";
            }
            else
            {
                //Differentiate between a full file path and a filename
                if (!newFilename.Contains(":"))
                {
                    newFilename = path.Replace(Path.GetFileName(path), newFilename);
                }
            }

            return newFilename;
        }

        public static void loadFilesFromDirectory(string path, bool searchSub, List<string> group1WhiteList, out byte[][] group1, out byte[][] group2, bool useFolderGroup, bool headerless = true)
        {
            string[] files;
            if (searchSub == true)
            {
                files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).ToArray();
            } else
            {
                files = Directory.GetFiles(path);
            }
            var group1temp = new List<byte[]>();
            var group2temp = new List<byte[]>();

            foreach(var currfile in files)
            {
                List<byte> file = new List<byte>(System.IO.File.ReadAllBytes(currfile));
                var realName = Path.GetFileName(currfile);
                //Add header as needed
                if (headerless == true)
                {
                    file.InsertRange(0, (new IceHeaderStructures.IceFileHeader(currfile, (uint)file.Count)).GetBytes());
                }

                //Sort basaed on loaded whitelist
                if (CheckIfGroup1(group1WhiteList, useFolderGroup, currfile, realName))
                {
                    group1temp.Add(file.ToArray());
                }
                else
                {
                    group2temp.Add(file.ToArray());
                }
            }

            group1 = group1temp.ToArray();
            group2 = group2temp.ToArray();
        }

        public static void loadFilesFromMemory(string[] fileNames, byte[][] fileData, List<string> group1WhiteList, Dictionary<string, int> presetGroups, out byte[][] group1, out byte[][] group2, bool headerless = true)
        {
            if(presetGroups == null)
            {
                presetGroups = new Dictionary<string, int>();
            }
            var group1temp = new List<byte[]>();
            var group2temp = new List<byte[]>();

            for(int i = 0; i < fileNames.Length; i++)
            {
                string fileName = fileNames[i];
                List<byte> file = new List<byte>(fileData[i]);
                
                //Add header as needed
                if (headerless == true)
                {
                    file.InsertRange(0, (new IceHeaderStructures.IceFileHeader(fileName, (uint)file.Count)).GetBytes());
                }

                //Sort based on loaded whitelist
                int presetGroup;
                if(presetGroups.TryGetValue(fileNames[i], out presetGroup) == false)
                {
                    presetGroup = 2;
                }

                if (group1WhiteList.Any(v => fileName.Contains(v)) || presetGroup == 1)
                {
                    group1temp.Add(file.ToArray());
                }
                else
                {
                    group2temp.Add(file.ToArray());
                }
            }

            group1 = group1temp.ToArray();
            group2 = group2temp.ToArray();
        }

        private static bool CheckIfGroup1(List<string> group1WhiteList, bool useFolderGroup, string currfile, string realName)
        {
            if (useFolderGroup && currfile.Contains("\\group"))
            {
                if (currfile.Contains("\\group1\\"))
                {
                    return true;
                }
            }
            else if (group1WhiteList.Any(v => realName.Contains(v)))
            {
                return true;
            }

            return false;
        }
    }
}
