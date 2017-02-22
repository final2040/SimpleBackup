using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBackupLibrary
{
    public class FileHelper
    {
        public virtual bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public virtual void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public virtual void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            System.IO.File.Copy(sourceFileName, destFileName, overwrite);
        }

        public virtual string[] GetFiles(string path)
        {
            return System.IO.Directory.GetFiles(path);
        }

        public virtual string[] GetDirectories(string path)
        {
            return System.IO.Directory.GetDirectories(path);
        }
       
    }
}
