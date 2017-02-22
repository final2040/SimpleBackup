using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.SqlServer.Server;
using SimpleBackupLibrary;
using Configuration = SimpleBackupLibrary.Configuration;
using Logs;

namespace SimpleBackupApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logger = Logger.GetLogger();
            logger.LogWriter = new FileAndConsoleLogWriter();
            try
            {
             
                logger.WriteInformation("Incializando software de copia de seguridad...");
                logger.WriteInformation(Configuration.GetConfig().ToString());
                BackupManager manager = new BackupManager(new FileBackup(Configuration.GetConfig(), new FileHelper()), Configuration.GetConfig());
                manager.Start();
            }
            catch (Exception ex)
            {
                logger.WriteError("Ha ocurrido un error: {0} \r\n", ex.Message, ex.StackTrace);
            }
            Console.ReadLine();
        }
    }
}
