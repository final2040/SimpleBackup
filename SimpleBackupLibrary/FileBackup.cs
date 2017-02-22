using System;
using System.IO;
using System.Xml.Schema;

namespace SimpleBackupLibrary
{
    public class FileBackup : IBackup
    {
        private Configuration _configuration;
        private FileHelper _fileHelper;

        public FileBackup(Configuration configuration, FileHelper fileHelper)
        {
            this._configuration = configuration;
            this._fileHelper = fileHelper;
        }

        public void DoBackup()
        {
            Validate();

            string destinationDirectory = Path.Combine(_configuration.DestinationFolder, DateTime.Now.ToString("yyyy-MM-ddThh-mm-ss"));
            _fileHelper.CreateDirectory(destinationDirectory);

            Copy(_configuration.SourceFolder, destinationDirectory);
        }

        private void Copy(string sourcePath, string destinationPath)
        {
            foreach (string directory in _fileHelper.GetDirectories(sourcePath))
            {
                string destDirectory = directory.Replace(sourcePath, destinationPath);
                if (directory != _configuration.DestinationFolder)
                {
                    _fileHelper.CreateDirectory(destDirectory);
                    Copy(directory, destDirectory);
                }
            }
            foreach (string file in _fileHelper.GetFiles(sourcePath))
            {
                string destFile = file.Replace(sourcePath, destinationPath);
                _fileHelper.Copy(file, destFile, true);
            }
        }

        private void Validate()
        {
            if (!_fileHelper.DirectoryExists(_configuration.SourceFolder))
                throw new DirectoryNotFoundException($"No se pudo encontrar el directorio origen: {_configuration.SourceFolder}");
            if (!_fileHelper.DirectoryExists(_configuration.DestinationFolder))
                throw new DirectoryNotFoundException($"No se pudo encontrar el directorio origen: {_configuration.DestinationFolder}");
        }
    }
}