using System;
using System.Configuration;
using System.Threading;

namespace SimpleBackupLibrary
{
    public class BackupManager
    {
        private IBackup _backupType;
        private readonly Configuration _config;

        public BackupManager(IBackup backupType, Configuration config)
        {
            _backupType = backupType;
            _config = config;
        }

        public void Start()
        {
            while (true)
            {
                if (DateTime.Now.TimeOfDay > _config.StartTimeOfDay 
                    && DateTime.Now.TimeOfDay < _config.EndTimeOfDay)
                {
                    _backupType.DoBackup();
                }
                Thread.Sleep(_config.BackupIntervalSeconds * 1000);
            }
        }
    }
}