using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SyncTool
{
    internal class Pair
    {
        public Pair(string rootA, string rootB)
        {
            this.Root_A = rootA;
            this.Root_B = rootB;
            this.A = this.B = string.Empty;
            this.IsFile = false;
            this.Length = 0;
        }

        public string Root_A { get; set; }
        public string Root_B { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public bool IsFile { get; set; }
        public long Length { get; set; }
        public bool _ExistsInA { get { return !string.IsNullOrWhiteSpace(this.A); } }
        public bool _ExistsInB { get { return !string.IsNullOrWhiteSpace(this.B); } }

        FileSystemInfo InfoA { get; set; }
        FileSystemInfo InfoB { get; set; }

        public OpType Operation
        {
            get
            {
                if (this._ExistsInA && this._ExistsInB)
                {
                    return OpType.InBoth;
                }
                else if (this._ExistsInA && !this._ExistsInB)
                {
                    return OpType.NotInB;
                }
                else if (!this._ExistsInA && this._ExistsInB)
                {
                    return OpType.NotInA;
                }
                else
                {
                    return OpType.None;
                }
            }
        }

        public void Sync()
        {
            try
            {
                if (this.IsFile)
                {
                    this.SyncFile();
                }
                else
                {
                    this.SyncDir();
                }

                return;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("No access on file/directory!\n\n" + this.A + "\nor\n" + this.B);
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("Filename is too long!\n\n" + this.B);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File/Directory not found!\n\n" + this.A);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured!\n\n" + this.A + "\n\n" + ex.ToString());
            }
        }

        private void SyncDir()
        {
            switch (this.Operation)
            {
                case OpType.NotInA:
                    this.InfoB = new DirectoryInfo(this.B);
                    this.RemoveWriteProtection();
                    Directory.Delete(this.B);
                    break;
                case OpType.NotInB:
                    this.InfoA = new DirectoryInfo(this.A);
                    string b = this.Root_B + this.A.Substring(this.Root_A.Length);
                    Directory.CreateDirectory(b);
                    this.InfoB = new DirectoryInfo(b);
                    this.ApplyAttributes();
                    break;
                case OpType.InBoth:
                    this.InfoA = new DirectoryInfo(this.A);
                    this.InfoB = new DirectoryInfo(this.B);
                    this.RemoveWriteProtection();
                    this.ApplyAttributes();
                    break;
            }
        }

        private void SyncFile()
        {
            switch (this.Operation)
            {
                case OpType.NotInA:
                    this.InfoB = new FileInfo(this.B);
                    this.RemoveWriteProtection();
                    File.Delete(this.B);
                    break;
                case OpType.NotInB:
                    this.InfoA = new FileInfo(this.A);
                    string b = this.Root_B + this.A.Substring(this.Root_A.Length);
                    Directory.CreateDirectory(Path.GetDirectoryName(b));
                    File.Copy(this.A, b, true);
                    this.InfoB = new FileInfo(b);
                    this.ApplyAttributes();
                    break;
                case OpType.InBoth:
                    this.InfoA = new FileInfo(this.A);
                    this.InfoB = new FileInfo(this.B);
                    this.RemoveWriteProtection();
                    if (this.InfoA.LastWriteTime != this.InfoB.LastWriteTime ||
                        (this.InfoA as FileInfo).Length != (this.InfoB as FileInfo).Length)
                    {
                        File.Copy(this.A, this.B, true);
                    }
                    this.ApplyAttributes();
                    break;
            }
        }

        /// <summary>
        /// Hebt den Schreibschutz von b auf
        /// </summary>
        private void RemoveWriteProtection()
        {
            this.InfoB.Attributes &= ~FileAttributes.ReadOnly;
        }

        private void ApplyAttributes()
        {
            if (this.InfoB.LastAccessTime != this.InfoA.LastAccessTime)
                this.InfoB.LastAccessTime = this.InfoA.LastAccessTime;
            if (this.InfoB.LastWriteTime != this.InfoA.LastWriteTime)
                this.InfoB.LastWriteTime = this.InfoA.LastWriteTime;
            if (this.InfoB.CreationTime != this.InfoA.CreationTime)
                this.InfoB.CreationTime = this.InfoA.CreationTime;
            if (this.InfoB.Attributes != this.InfoA.Attributes)
                this.InfoB.Attributes = this.InfoA.Attributes;
        }
    }
}