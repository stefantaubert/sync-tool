namespace SyncTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Reflection;

    public class Controller
    {
        public Controller()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("----- Sync-Tool -----");
            Console.WriteLine("---------------------");
            Console.WriteLine();
        }

        public void Start()
        {
            string a = string.Empty, b = a, path;
            path = "settings.txt";
            if (File.Exists(path))
            {
                string[] paths = File.ReadAllLines(path);
                if (paths.Length > 1)
                {
                    a = paths[0];
                    b = paths[1];
                }
            }

            if (!this.Validate(a, b))
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the origin directory (A):");
                Console.WriteLine();
                a = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter the destination directory (B):");
                Console.WriteLine();
                b = Console.ReadLine();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Origin directory (A): " + a);
                Console.WriteLine("Destination directory (B): " + b);
            }

            //a = "D:\\Testumgebung\\A";
            //b = "D:\\Testumgebung\\B";
            //a = "D:\\";
            //b = "Z:\\";

            if (!this.Validate(a, b))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid inputs!");
                Console.WriteLine();
                this.Start();
            }
            else
            {
                GetPaths g = new GetPaths();
                g.A = a.TrimEnd('\\');
                g.B = b.TrimEnd('\\');
                g.ReadIt();

                PathPairer p = new PathPairer(g);
                p.PairIt();

                SyncPairs s = new SyncPairs();
                s.Pairs = p.Pairs;
                s.SyncIt();
                Console.ReadLine();
            }
        }

        private bool Validate(string dirA, string dirB)
        {
            bool exist = Directory.Exists(dirA) && Directory.Exists(dirB);
            bool notinclude = !dirA.StartsWith(dirB) && !dirB.StartsWith(dirA);
            return exist && notinclude;
        }
    }
}