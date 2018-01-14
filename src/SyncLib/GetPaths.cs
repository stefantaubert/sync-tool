namespace SyncTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Threading;

    internal class GetPaths
    {
        public GetPaths()
        {
            this.A = this.B = string.Empty;
            this.A_D = new List<string>();
            this.A_F = new List<string>();
            this.B_F = new List<string>();
            this.B_D = new List<string>();
        }

        public string A { get; set; }
        public string B { get; set; }
        public List<string> A_D { get; set; }
        public List<string> A_F { get; set; }
        public List<string> B_F { get; set; }
        public List<string> B_D { get; set; }

        public void ReadIt()
        {
            Console.WriteLine("Reading...");
            Thread t = new Thread(GetPathsB);
            t.Start();
            this.GetPathsA();
            t.Join();
        }

        private void GetPathsA()
        {
            GDAF g = new GDAF(this.A);
            g.Read();
            //this.A_D = new List<string>(this.Dir.GetDirectories(this.A, "*", SearchOption.AllDirectories));
            //this.A_F = new List<string>(this.Dir.GetFiles(this.A, "*", SearchOption.AllDirectories));
            this.A_D = g.Dirs;
            this.A_F = g.Files;

            for (int i = 0; i < this.A_D.Count; i++)
            {
                this.A_D[i] = this.A_D[i].Substring(this.A.Length);
            }

            for (int i = 0; i < this.A_F.Count; i++)
            {
                this.A_F[i] = this.A_F[i].Substring(this.A.Length);
            }

            this.A_D.Sort();
            this.A_F.Sort();
            Console.WriteLine("Reading of A finished.");
        }

        private void GetPathsB()
        {
            GDAF g = new GDAF(this.B);
            g.Read();
            //this.B_D = new List<string>(this.Dir.GetDirectories(this.B, "*", SearchOption.AllDirectories));
            //this.B_F = new List<string>(this.Dir.GetFiles(this.B, "*", SearchOption.AllDirectories));
            this.B_D = g.Dirs;
            this.B_F = g.Files;

            for (int i = 0; i < this.B_D.Count; i++)
            {
                this.B_D[i] = this.B_D[i].Substring(this.B.Length);
            }

            for (int i = 0; i < this.B_F.Count; i++)
            {
                this.B_F[i] = this.B_F[i].Substring(this.B.Length);
            }

            this.B_D.Sort();
            this.B_F.Sort();
            Console.WriteLine("Reading of B finisched.");
        }
    }
}
