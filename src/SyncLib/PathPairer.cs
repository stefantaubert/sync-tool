namespace SyncTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    internal class PathPairer
    {
        /// <summary>
        /// Ordnet die eingelesenen Pfade als Paare
        /// </summary>
        /// <param name="g"></param>
        public PathPairer(GetPaths g)
        {
            this.Paths = g;
            this.Pairs = new List<Pair>();
        }

        public GetPaths Paths { get; set; }
        public List<Pair> Pairs { get; set; }

        public void PairIt()
        {
            Console.WriteLine("Process informations...");
            this.Pairs = new List<Pair>();
            // alle Ordner in A mit denen in B abgleichen
            for (int i = 0; i < Paths.A_D.Count; i++)
            {
                Pair p = new Pair(this.Paths.A, this.Paths.B);
                p.A = Paths.A + Paths.A_D[i];
                p.IsFile = false;
                int inB = Paths.B_D.BinarySearch(Paths.A_D[i]);
                if (inB >= 0)
                {
                    p.B = Paths.B + Paths.B_D[inB];
                }

                Pairs.Add(p);
            }

            // alle Dateien in A mit denen in B abgleichen
            for (int i = 0; i < Paths.A_F.Count; i++)
            {
                Pair p = new Pair(this.Paths.A, this.Paths.B);
                p.A = Paths.A + Paths.A_F[i];
                p.IsFile = true;
                int inB = Paths.B_F.BinarySearch(Paths.A_F[i]);
                if (inB >= 0)
                {
                    p.B = Paths.B + Paths.B_F[inB];
                }

                Pairs.Add(p);
            }

            // alle Dateien in B die nicht in A sind
            for (int i = 0; i < Paths.B_F.Count; i++)
            {
                Pair p = new Pair(this.Paths.A, this.Paths.B);
                p.B = Paths.B + Paths.B_F[i];
                p.IsFile = true;
                int inA = Paths.A_F.BinarySearch(Paths.B_F[i]);
                if (inA < 0)
                {
                    Pairs.Add(p);
                }
            }

            // alle Ordner in B die nicht in A sind
            for (int i = 0; i < Paths.B_D.Count; i++)
            {
                Pair p = new Pair(this.Paths.A, this.Paths.B);
                p.B = Paths.B + Paths.B_D[i];
                p.IsFile = false;
                int inA = Paths.A_D.BinarySearch(Paths.B_D[i]);
                if (inA < 0)
                {
                    Pairs.Add(p);
                }
            }

            Console.WriteLine("Processing finished...");
        }
    }
}