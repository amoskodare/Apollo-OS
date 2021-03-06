using System;
using System.Collections.Generic;
using System.Text;
using AIC_Framework;
using Apollo.Environment;
using Apollo.Internals;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;

namespace Apollo
{
    public class Init
    {
		public static Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
		public static void Start()
        {
			VFSManager.RegisterVFS(fs);
			fs.Initialize();
			AConsole.WriteLineEx("    Welcome to Apollo OS    ", ConsoleColor.White, ConsoleColor.Gray, true, false);
            AConsole.WriteLineEx("Press any key to continue...", ConsoleColor.White, ConsoleColor.Gray, true, false);
            AConsole.ReadKey(true);
            bool integchk = IntegrityCheck();
            if (integchk == true)
            {
                Console.WriteLine("File System integrity checks completed successfully!");
                Console.WriteLine("Continuing boot...");
            }
            else
            {
                Make_sys_dir();
            }
            if (Directory.GetFiles(KernelVariables.homedir).Length == 0)
            {
                UserInit();
            }
            else
            {
                string user = Directory.GetDirectories(KernelVariables.homedir)[0];
                env_vars.user = user;
            }
            if (!Directory.Exists(KernelVariables.bindir))
            {
                Directory.CreateDirectory(KernelVariables.currentdir);
                File.Create(KernelVariables.bindir + "vars.sys").Dispose();
            }
            else
            {
                usr_vars.ReadVars();
            }
            Console.WriteLine("SysGuard Checks proceeded.");
            env_vars.PressAnyKey("Press any key to continue boot...");
            Console.Clear();
            
        }
        private static void Make_sys_dir()
        {
            KernelVariables.SysDirList();
            foreach (string dir in KernelVariables.systemdirectory)
            {
                Console.WriteLine("Creating " + dir + "...");
                try
                {
                    fsfunc.mkdir(dir, true);
                }
                catch
                {
                    Console.WriteLine("Failed to create " + dir + " directory!");
                }
            }
        }

        /// <summary>
        /// TODO: Check if all directories exist in a foreach?
        /// </summary>
        /// <returns></returns>
        public static bool IntegrityCheck()
        {
            KernelVariables.SysDirList();
            if (!Directory.Exists(KernelVariables.etcdir))
            {
                Console.WriteLine("System directory <" + KernelVariables.etcdir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.etcdir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.homedir))
            {
                Console.WriteLine("System directory <" + KernelVariables.homedir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.homedir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.bindir))
            {
                Console.WriteLine("System directory <" + KernelVariables.bindir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.bindir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.devdir))
            {
                Console.WriteLine("System directory <" + KernelVariables.devdir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.devdir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.rootusrdir))
            {
                Console.WriteLine("System directory <" + KernelVariables.rootusrdir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.rootusrdir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.tmpdir))
            {
                Console.WriteLine("System directory <" + KernelVariables.tmpdir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.tmpdir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.usrdir))
            {
                Console.WriteLine("System directory <" + KernelVariables.usrdir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.usrdir, true);
                return false;
            }
            if (!Directory.Exists(KernelVariables.vardir))
            {
                Console.WriteLine("System directory <" + KernelVariables.vardir + "> is missing! Recreating directory...");
                fsfunc.mkdir(KernelVariables.vardir, true);
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void UserInit()
        {
            Console.WriteLine("It is dangerous to run some commands as a root user,\nso it's important that you use a non-admin user.");
            Console.Write ("Enter a new username for first user:");
            string user = Console.ReadLine();
            Console.Write("Enter user password: ");
            string pass = Console.ReadLine();
            Account.Accounts.Add(new Account(user, pass));
            env_vars.user = user;
        }
    }
}