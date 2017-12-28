using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SyncLib
{
    public class GDAF
    {
        public GDAF(string directory)
        {
            this.Root = Path.Combine(directory + "\\");
            this.Files = new List<string>();
            this.Dirs = new List<string>();
        }

        public string Root
        {
            get;
            set;
        }

        public List<string> Files { get; private set; }
        public List<string> Dirs { get; private set; }

        private string _RecycleBinPath { get { return Path.Combine(this.Root, "$RECYCLE.BIN"); } }
        private string _SysVolInfoPath { get { return Path.Combine(this.Root, "System Volume Information"); } }

        public void Read()
        {
            this.Files.Clear();
            this.Dirs.Clear();
            if (Directory.Exists(this.Root))
            {
                this.GetObjects(this.Root);
            }
        }

        private void GetObjects(string folder)
        {
            this.Files.AddRange(Directory.GetFiles(folder));
            foreach (string subDir in Directory.GetDirectories(folder))
            {
                try
                {
                    if (subDir != this._RecycleBinPath && subDir != this._SysVolInfoPath)
                    {
                        this.Dirs.Add(subDir);
                        this.GetObjects(subDir);
                    }
                }
                catch { }
            }
        }
    }
}