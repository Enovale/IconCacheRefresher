using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IconCacheRefresher
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("You are run as admin.");
            Console.WriteLine("Killing file explorerer...");

            int attempts = 0;
            while (Process.GetProcessesByName("explorer").Length > 0 && attempts < 5)
            {
                Console.WriteLine($"Attempt #{attempts}");
                Process.Start("taskkill", "/F /IM explorer.exe");
                Thread.Sleep(1000);
                attempts++;
            }

            string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string ExplorerPath = Path.Combine(Directory.GetParent(AppDataPath).FullName, @"Local\Microsoft\Windows\Explorer");

            Console.WriteLine("Deleting cache...");

            FileInfo[] cacheFiles = new DirectoryInfo(ExplorerPath).GetFiles("iconcache*");

            foreach(FileInfo file in cacheFiles)
            {
                try
                {
                    file.Delete();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error Ocurred! " + e.ToString());

                    Console.WriteLine("Restarting Explorer...");
                    Process.Start("explorer.exe");

                    Console.WriteLine("Icon Cache failed to wipe. Please press a key to continue...");

                    Console.ReadKey();
                    return;
                }
            }

            Thread.Sleep(3000);

            Console.WriteLine("Restarting Explorer...");
            Process.Start("explorer.exe");

            Console.WriteLine("Icon Cache wiped. Please press a key to continue...");

            Console.ReadKey();
        }
    }
}
