namespace SyncTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class SyncPairs
    {
        public SyncPairs()
        {
            this.Pairs = new List<Pair>();
        }

        public List<Pair> Pairs { get; set; }

        private List<Pair> _NotExistingFiles { get { return this.Pairs.Where(s => s.Operation == OpType.NotInA && s.IsFile).ToList(); } }
        private List<Pair> _NotExistingDirs { get { return this.Pairs.Where(s => s.Operation == OpType.NotInA && !s.IsFile).OrderBy(s => s.B).Reverse().ToList(); } }
        private List<Pair> _NewObjects { get { return this.Pairs.Where(s => s.Operation == OpType.NotInB).ToList(); } }
        private List<Pair> _ExistingObjects { get { return this.Pairs.Where(s => s.Operation == OpType.InBoth).ToList(); } }

        public void SyncIt()
        {
            Console.WriteLine("Execute operations...");
            this.GetStats(); // Statistik

            // alle nicht mehr vorhandenen Objekte löschen
            Console.WriteLine("Clean directory B...");

            // alle alten Dateien löschen
            foreach (var item in this.Pairs.Where(s => s.Operation == OpType.NotInA && s.IsFile))
            {
                item.Sync();
            }

            // alle alten Ordner reverse löschen
            foreach (var item in this.Pairs.Where(s => s.Operation == OpType.NotInA && !s.IsFile).OrderBy(s => s.B).Reverse())
            {
                item.Sync();
            }

            // alle neuen Objekte kopieren
            Console.WriteLine("Copy new files to directory B...");
            foreach (var item in this.Pairs.Where(s => s.Operation == OpType.NotInB))
            {
                item.Sync();
            }

            // alle vorhandenen Objekte abgleichen
            Console.WriteLine("Compare existing files with directory B...");
            foreach (var item in this.Pairs.Where(s => s.Operation == OpType.InBoth))
            {
                item.Sync();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Synchronisation was successfull!");
        }

        private void GetStats()
        {
            Console.WriteLine();
            Console.WriteLine("Files to delete: " + this._NotExistingFiles.Count);
            Console.WriteLine("Directories to delete: " + this._NotExistingDirs.Count);
            Console.WriteLine("Objects to copy: " + this._NewObjects.Count);
            Console.WriteLine("Objects to synchronize: " + this._ExistingObjects.Count);
            Console.WriteLine();
        }
    }
}