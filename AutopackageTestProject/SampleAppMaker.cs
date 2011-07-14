

namespace AutopackageTestProject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CoApp.Toolkit.Extensions;
    using CoApp.Toolkit.Utility;
    using AutopackageTestProject.Properties;
    using System.IO;
    using System.Reflection;
    using CoApp.Toolkit.Scripting.Languages.PropertySheet;
    class SampleAppMaker
    {
        private Dictionary<string, string> inVars = new Dictionary<string, string>();
        private readonly List<TestTarget> sharedLibraryTargets = new List<TestTarget>();
        private readonly List<TestTarget> applicationTargets = new List<TestTarget>();
        private Random rand = new Random();
        private string tempFolder { get; set; }
        private string certPath { get; set; }
        private string pkPath { get; set; }
        private string publicKeyToken { get; set; }
        

        private ProcessUtility cl_x86;
        private ProcessUtility cl_x64;
        private ProcessUtility lib_x86;
        private ProcessUtility lib_x64;
        private ProcessUtility csc_msil;
        private ProcessUtility al_msil;
        private ProcessUtility cl;
        private ProcessUtility lib;
        private ProcessUtility pktExtract;

        private SampleAppMaker(string tempFolder)
        {
            int a, b;
            a = rand.Next(100000);
            b = a + 1;
            inVars["a"] = a.ToString();
            inVars["b"] = b.ToString();
            this.tempFolder = tempFolder;
        }

        private string GetItemName(string wholename)
        {
            return wholename.Split('(')[0].ToLower();
        }

        private string GetItemVersion(string wholename)
        {
            wholename = wholename.Replace("{a}", inVars["a"]).Replace("{b}", inVars["b"]);
            return wholename.Split('(')[1].Split(')')[0].ToLower();
        }


        // A list of archs and autopkg files and their
        public Dictionary<string, IEnumerable<string>> Build()
        {
            var output = new Dictionary<string, IEnumerable<string>>();
            

            //load the input
            Load(new string[] {"--load-config={0}".format(WriteIni())});
            ExtractCert();
            output["x86"] = BuildX86();
           


            


            return null;
        }

        private void ExtractCert()
        {
            certPath = Path.Combine(tempFolder, "CoAppTest.pfx");
            File.WriteAllBytes(certPath, Resources.CoAppTestCert);
            pkPath = Path.Combine(tempFolder, "CoAppTest.pk");
            File.WriteAllBytes(pkPath, Resources.CoAppTestCertPublicKey);

            var cerPath = Assembly.GetExecutingAssembly().ExtractFileResourceToTemp("CoAppTest.cer");
            pktExtract.Exec("-nologo -quiet {0}", cerPath);
            publicKeyToken = pktExtract.StandardOut.Trim();
        }


        private IEnumerable<string> BuildX86()
        {
            clearTargetBuildFlags();
            var output = new List<string>();
            var setEnv = ProgramFinder.ProgramFiles.ScanForFile("setenv.cmd");
            if (string.IsNullOrEmpty(setEnv))
                throw new Exception("Cannot locate SDK SetEnv command. Please install the Windows SDK");

            var se = new ProcessUtility("cmd.exe");
            se.Exec(@"/c ""{0}"" /{1} & set ", setEnv, "x86");


            foreach (var x in se.StandardOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                if (x.Contains("="))
                {
                    var v = x.Split('=');
                    Environment.SetEnvironmentVariable(v[0], v[1]);
                }

            var target_cpu = Environment.GetEnvironmentVariable("TARGET_CPU");
            if (string.IsNullOrEmpty(target_cpu) || (target_cpu == "x64") )
            {
                
                    throw new Exception(
                        "Cannot set the SDK environment. Please install the Windows SDK and use the setenv.cmd command to set your environment");
            }

            

            foreach (var dll in sharedLibraryTargets)
            {
                CreateDLL(dll, "x86", output);
            }

            foreach (var exe in applicationTargets)
            {
                CreateEXE(exe, "x86", output);
            }



            return null;
        }

        private IEnumerable<string> BuildX64()
        {
            clearTargetBuildFlags();
            var setEnv = ProgramFinder.ProgramFiles.ScanForFile("setenv.cmd");
            if (string.IsNullOrEmpty(setEnv))
                throw new Exception("Cannot locate SDK SetEnv command. Please install the Windows SDK");

            var se = new ProcessUtility("cmd.exe");
            se.Exec(@"/c ""{0}"" /{1} & set ", setEnv,  "x64");


            foreach (var x in se.StandardOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                if (x.Contains("="))
                {
                    var v = x.Split('=');
                    Environment.SetEnvironmentVariable(v[0], v[1]);
                }

            var target_cpu = Environment.GetEnvironmentVariable("TARGET_CPU");
            if (string.IsNullOrEmpty(target_cpu) || (target_cpu == "x86"))
            {
                throw new Exception(
                        "Cannot set the SDK environment. Please install the Windows SDK and use the setenv.cmd command to set your environment");
            }


            return null;
        }

        private IEnumerable<string> BuildMSIL()
        {

            clearTargetBuildFlags();
            var setEnv = ProgramFinder.ProgramFiles.ScanForFile("VSVARS32.bat");
            if (string.IsNullOrEmpty(setEnv))
               throw new Exception("Cannot locate VCVARS32.bat command. Please install something?");

            var se = new ProcessUtility(setEnv);
            se.Exec("");

            return null;
        }

        private void clearTargetBuildFlags()
        {

            foreach (var dll in sharedLibraryTargets)
            {
                dll.Built = false;
            }

            foreach (var exe in applicationTargets)
            {
                exe.Built = false;
            }
        }


        private void CreateEXE(TestTarget exe, string arch, List<string> autopkgFiles)
        {
            if (arch == "msil")
            {
                CreateMSILBinary(exe, Resources.MSILAppTemplate, ".exe", arch, @" /out:""{0}"" /target:exe /delaysign+ /keyfile:""{1}"" ""{2}""", autopkgFiles);
            }
            else
            {
                CreateBinary(exe, Resources.AppTemplate, ".exe", arch, @" ""{0}"" ""/Fe{1}"" /DISOLATION_AWARE_ENABLED=1 /TC /Y- ", autopkgFiles);
            }

        }



        private void CreateDLL(TestTarget dll, string arch, List<string> autopkgFiles)
        {
            if (arch == "msil")
            {
                CreateMSILBinary(dll, Resources.MSILSharedLibTemplate, ".dll", arch,  @" /out:""{0}"" /target:library /delaysign+ /keyfile:""{1}"" ""{2}""", autopkgFiles);
            }
            else
            {
                CreateBinary(dll, Resources.SharedLibTemplate, ".dll", arch,
                             @" ""{0}"" ""/Fe{1}"" /DISOLATION_AWARE_ENABLED=1 /LD /TC /Y- ",  autopkgFiles);
            }
        }

        private void CreateBinary(TestTarget binary, string binaryTemplate, string extension, string arch, string commandLine, List<string> autopkgFiles)
        {

            var outputPath = Path.Combine(this.tempFolder, arch);
            if (binary.Built)
                return;

            foreach (var dep in binary.Dependencies)
            {
                CreateDLL(dep, arch, autopkgFiles);
            }

            if (binary.Ignore)
                return;

            //Console.WriteLine("Building Binary [{0}-{1}]", binary.Name, binary.Version);

            var importLibrary = new StringBuilder();
            var callLibrary = new StringBuilder();

            foreach (var dll in binary.Dependencies)
            {
                importLibrary.Append(Resources.LibraryReferenceTemplate.Replace("[$LIBNAME]", dll.Name).Replace("[$LIBVERSION]", dll.Version).Replace("[$PUBLICKEYTOKEN]", publicKeyToken).Replace("[$ARCH]", arch));
                callLibrary.Append(Resources.LibraryFunctionCallTemplate.Replace("[$LIBNAME]", dll.Name).Replace("[$LIBVERSION]", dll.Version).Replace("[$PUBLICKEYTOKEN]", publicKeyToken).Replace("[$ARCH]", arch));
            }

            var tempFolder = Path.Combine(Path.GetTempPath(), binary.Name + "-" + binary.Version);
            Directory.CreateDirectory(tempFolder);
            var outputBinaryFolder = Path.Combine(outputPath, binary.Name + "-" + binary.Version);
            Directory.CreateDirectory(outputBinaryFolder);
            Environment.CurrentDirectory = outputBinaryFolder;

            foreach (var dll in binary.Dependencies)
            {
                var defFile = Resources.ModuleDefinitionFileTemplate + "print_" + dll.Name;
                File.WriteAllText(Path.Combine(tempFolder, dll.Name + ".def"), defFile);
               

                if (lib.Exec("/out:{0} /def:{1} /machine:{2}", Path.Combine(tempFolder, dll.Name + ".lib"), Path.Combine(tempFolder, dll.Name + ".def"), arch == "x86" ? arch : "x64") != 0)
                    throw new Exception(lib.StandardOut);

                commandLine = @"{0} /link ""{1}""".format(commandLine, Path.Combine(tempFolder, dll.Name + ".lib"));
            }

            // make C file
            var binaryText = binaryTemplate.Replace("[$LIBNAME]", binary.Name).Replace("[$LIBVERSION]", binary.Version).Replace("[$IMPORT_LIBRARY]", importLibrary.ToString()).Replace("[$CALL_LIBRARY]", callLibrary.ToString()).Replace("[$PUBLICKEYTOKEN]", publicKeyToken).Replace("[$ARCH]", arch);
            var tempCFile = Path.Combine(tempFolder, binary.Name + ".c");
            File.WriteAllText(tempCFile, binaryText);

            // compile it
            var outputBinary = Path.Combine(outputBinaryFolder, binary.Name + extension);
            if (cl.Exec(commandLine, tempCFile, outputBinary) != 0)
                throw new Exception(cl.StandardOut);

            binary.Built = true;

            //TODO create the autopkg file

            var propSheet = new PropertySheet();
            //propSheet.Keys
        }


        public void CreateMSILBinary(TestTarget binary, string binaryTemplate, string extension, string arch, string commandLine, List<string> autopkgFiles)
        {

            var outputPath = Path.Combine(this.tempFolder, arch);
            if (binary.Built)
                return;

            foreach (var dep in binary.Dependencies)
            {
                CreateDLL(dep, arch, autopkgFiles);
            }

            if (binary.Ignore)
                return;

            //Console.WriteLine("Building Binary [{0}-{1}]", binary.Name, binary.Version);

            //var importLibrary = new StringBuilder();
            var callLibrary = new StringBuilder();

            foreach (var dll in binary.Dependencies)
            {
                //importLibrary.Append(LibraryReferenceTemplate.Replace("[$LIBNAME]", dll.Name).Replace("[$LIBVERSION]", dll.Version).Replace("[$PUBLICKEYTOKEN]", publicKeyToken).Replace("[$ARCH]", arch));
                callLibrary.Append(Resources.MSILLibraryFunctionCallTemplate.Replace("[$LIBNAME]", dll.Name).Replace("[$LIBVERSION]", dll.Version));
            }

            var tempFolder = Path.Combine(Path.GetTempPath(), binary.Name + "-" + binary.Version);
            Directory.CreateDirectory(tempFolder);
            var outputBinaryFolder = Path.Combine(outputPath, binary.Name + "-" + binary.Version);
            Directory.CreateDirectory(outputBinaryFolder);
            Environment.CurrentDirectory = outputBinaryFolder;

            foreach (var dll in binary.Dependencies)
            {
                var currentRef = Path.Combine(Path.GetTempPath(), dll.Name + "-" + dll.Version);
                commandLine = @"{0} /reference:""{1}""".format(commandLine, Path.Combine(currentRef, dll.Name + ".dll"));
            }


            var binaryText = binaryTemplate.Replace("[$LIBNAME]", binary.Name).Replace("[$LIBVERSION]", binary.Version).Replace("[$CALL_LIBRARY]", callLibrary.ToString()).Replace("[$PUBLICKEYTOKEN]", publicKeyToken).Replace("[$ARCH]", arch);
            var tempCSFile = Path.Combine(tempFolder, binary.Name + ".cs");
            File.WriteAllText(tempCSFile, binaryText);




            // compile it
            var outputBinary = Path.Combine(outputBinaryFolder, binary.Name + extension);
            if (csc_msil.Exec(commandLine, outputBinary, pkPath, tempCSFile) != 0)
                throw new Exception(csc_msil.StandardOut);

            binary.Built = true;
            //TODO: create autopkg
        }



        private string WriteIni()
        {
            string inputFile = Path.Combine(tempFolder, "PackageMaker.ini");
            File.WriteAllText(inputFile, Resources.PackageMaker);
            return inputFile;
        }


        private void Load(string[] args)
        {
            var options = args.Switches();
            var parameters = args.Parameters();

            foreach (var arg in args)
            {
                var argumentParameters = options[arg];

                if (arg.StartsWith("exe-"))
                {
                    var appname = arg.Substring(4);

                    var t = AddEXE(GetItemName(appname), GetItemVersion(appname));
                    t.Ignore = false;
                    foreach (var item in argumentParameters)
                    {
                        t.Dependencies.Add(AddDLL(GetItemName(item), GetItemVersion(item)));
                    }
                    break;
                }

                else if (arg.StartsWith("dll-"))
                {
                    var dllname = arg.Substring(4);

                    var t = AddDLL(GetItemName(dllname), GetItemVersion(dllname));
                    t.Ignore = false;
                    foreach (var item in argumentParameters)
                    {
                        if (!String.IsNullOrEmpty(item))
                            t.Dependencies.Add(AddDLL(GetItemName(item), GetItemVersion(item)));
                    }
                    break;
                }

                else if (arg.StartsWith("policy-"))
                {
                    var dllname = arg.Substring(7);
                    var t = AddDLL(GetItemName(dllname), GetItemVersion(dllname));

                    foreach (var policyText in argumentParameters)
                    {
                        var policy = policyText.Split('-');
                        t.Policies.Add(new Tuple<string, string>(policy[0], policy.Length == 2 ? policy[1] : policy[0]));
                    }
                    break;
                }

            }

            cl_x86 = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("cl.exe", ExecutableInfo.x86, new[] { @"*\x86_amd64\*", @"*\x86_ia64\*" }));
            cl_x64 = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("cl.exe", ExecutableInfo.x64));

            //This only works if
            var cscpf = new ProgramFinder("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319");
            csc_msil = new ProcessUtility(cscpf.ScanForFile("csc.exe"));
            al_msil = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("al.exe"));

            lib_x86 = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("lib.exe", ExecutableInfo.x86, new[] { @"*\x86_amd64\*", @"*\x86_ia64\*" }));
            lib_x64 = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("lib.exe", ExecutableInfo.x64));

            pktExtract = new ProcessUtility(ProgramFinder.ProgramFilesAndDotNet.ScanForFile("pktExtract.exe"));
        }


        private static TestTarget Add(List<TestTarget> list, string name, string version)
        {
            var t = new TestTarget { Name = name, Version = version };
            foreach (var item in list)
            {
                if (item == t)
                    return item;
            }

            list.Add(t);
            return t;
        }

        private TestTarget AddDLL(string name, string version)
        {
            return Add(sharedLibraryTargets, name, version);
        }

        private TestTarget AddEXE(string name, string version)
        {
            return Add(applicationTargets, name, version);
        }

        

    }
}
