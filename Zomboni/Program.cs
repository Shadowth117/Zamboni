// Decompiled with JetBrains decompiler
// Type: zamboni.Program
// Assembly: zamboni, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 73B487C9-8F41-4586-BEF5-F7D7BFBD4C55
// Assembly location: D:\Downloads\zamboni_ngs (3)\zamboni.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using zomForm;
using zomFormNew;

namespace zomboni
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] commandLineArgs)
        {
            string str1 = "UI";
            List<string> paths = new List<string>();
            string str2 = "";
            string str3 = "";
            bool useGroups = true;
            bool compress = false;
            bool forceUnencrypted = false;

            //Check commands
            for (int index = 0; index < commandLineArgs.Length; ++index)
            {
                switch (commandLineArgs[index])
                {
                    case "-noGroups":
                        useGroups = false;
                        break;
                    case "-c":
                        compress = true;
                        break;
                    case "-compress":
                        compress = true;
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
                    case "extract":
                        str1 = "extract";
                        break;
                    case "pack":
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

                if (str1 == "pack")
                {
                    foreach(var path in paths)
                    {
                        
                        if(str3 == null || str3 == "")
                        {
                            strOut = path + "_ext";
                        } else
                        {
                            strOut = str3;
                        }


                        if (str2 == "")
                        {
                            str2 = Path.GetDirectoryName(path);
                        }
                        Form1.buildIceFromDirectory(File.ReadAllLines(path), str2, strOut, compress, forceUnencrypted);
                    }
                }
                //Extract all ice named in the text file.
                else if(str1 == "extract")
                {
                    foreach (var path in paths)
                    {
                        if (str3 == null || str3 == "")
                        {
                            strOut = path + "_ext";
                        }
                        else
                        {
                            strOut = str3;
                        }

                        if (str2 == "")
                        {
                            str2 = Path.GetDirectoryName(path);
                        }
                        Form1.exportFilesToDirectory(File.ReadAllLines(path), str2, strOut);
                    }
                }
                //Extract all ice from paths given
                else if (str1 == "extractNoTxt")
                {
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
                    Form1.ExtractIce(str2, strOut, path, useGroups);
                    }
                    ));
                }
            }
        }
    }
}
