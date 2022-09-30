using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zomForm;
using zomFormNew;

namespace zamboni
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] commandLineArgs)
        {
#if DEBUG
 //           Debugger.Launch();
#endif
            string str1 = "UI";
            List<string> paths = new List<string>();
            List<string> outFileNames = new List<string>();
            string str2 = "";
            string str3 = "";
            bool useGroups = true;
            bool compress = true;
            bool forceUnencrypted = false;
            bool useSubDirectories = true;

            //Check commands
            for (int index = 0; index < commandLineArgs.Length; ++index)
            {
                switch (commandLineArgs[index])
                {
                    case "-outName":
                        outFileNames.Add(commandLineArgs[index + 1]);
                        ++index;
                        break;
                    case "-noSubDirUsage":
                        useSubDirectories = false;
                        break;
                    case "-noGroups":
                        useGroups = false;
                        break;
                    case "-c":
                        compress = true;
                        break;
                    case "-compress":
                        compress = true;
                        break;
                    case "-nocompress":
                        compress = false;
                        break;
                    case "-indir":
                        str2 = commandLineArgs[index + 1];
                        ++index;
                        break;
                    case "-list":
                        paths.Add(commandLineArgs[index + 1]);
                        ++index;
                        break;
                    case "-outdir":
                        str3 = commandLineArgs[index + 1];
                        ++index;
                        break;
                    case "-v":
                        forceUnencrypted = true;
                        break;
                    case "-extract":
                        str1 = "extract";
                        break;
                    case "-pack":
                        str1 = "pack";
                        break;
                    default:
                        paths.Add(commandLineArgs[index]);
                        break;
                }
            }

            //Proceed to UI if set to that and there's no paths, but if paths and no other options, default to extraction.
            if(paths.Count > 0 && str1 == "UI")
            {
                str1 = "extractNoTxt";
                if(paths.Count > 1)
                {
                    str3 = null;
                }
            } else if (str1 == "UI")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new NicerForm());
            }

            //Packing, only supports older ice and not as robust as ice.exe
            if (str2 == "" && paths.Count == 0)
            {
                Console.Out.WriteLine("requires -indir <path to files to pack/unpack> and -list <path to list filename>");
            } else
            {
                string strOut;

                switch(str1)
                {
                    case "pack":
                        for (int i = 0; i < paths.Count; i++)
                        {
                            var path = paths[i];

                            if (Directory.Exists(path))
                            {
                                string outName = null;
                                if (outFileNames.Count - 1 >= i)
                                {
                                    outName = outFileNames[i];
                                }
                                UILogic.packIceFromDirectoryToFile(path,
                                    UILogic.ReadWhiteList(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "group1.txt")),
                                    useSubDirectories, compress, true, forceUnencrypted, null);
                            }
                        }
                        break;
                    case "extractNoTxt": //Extract all ice from paths given
                        Parallel.ForEach<string>(paths, (Action<string>)(path =>
                        {
                            if (str3 == null || str3 == "")
                            {
                                strOut = "";
                            }
                            else
                            {
                                strOut = str3;
                            }

                            if (str2 == "")
                            {
                                str2 = Path.GetDirectoryName(path);
                            }
                            string result = Form1.ExtractIce(str2, strOut, path, useGroups);
                            if (result != null)
                            {
                                File.WriteAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\log.txt", result);
                            }
                        }
                        ));
                        break;
                    case "list":
                        StringBuilder sb = new StringBuilder();
                        Parallel.ForEach<string>(paths, (Action<string>)(path =>
                        {
                            if (str2 == "")
                            {
                                str2 = Path.GetDirectoryName(path);
                            }
                            sb.Append(Form1.ListIce(str2, path));
                        }
                        ));

                        File.WriteAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "IceFileList.txt", sb.ToString());
                        break;
                }
            }
        }
    }
}
