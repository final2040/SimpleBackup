using System;
using System.Configuration;
using System.Runtime.InteropServices;
using Logs;

namespace SimpleBackupLibrary
{
   public class Configuration 
    {
        private static Configuration _manager;

        public Configuration()
        {
        }

        public virtual string SourceFolder { get; set; }
        public virtual string DestinationFolder { get; set; }
        public virtual int BackupIntervalSeconds { get; set; }
        public virtual TimeSpan StartTimeOfDay { get; set; }
        public virtual TimeSpan EndTimeOfDay { get; set; }


        public static Configuration GetConfig()
        {
            if (_manager == null) _manager = ReadConfig();
            return _manager;
        }

        public static Configuration ReadConfig()
        {
            string sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            string destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
            TimeSpan startTime;
            TimeSpan endTime;

            int time;

            if (string.IsNullOrWhiteSpace(sourceFolder))
                throw new ConfigurationErrorsException("Debe de especificar una carpeta origen");
            if (string.IsNullOrWhiteSpace(destinationFolder))
                throw new ConfigurationErrorsException("Debe de especificar una carpeta destino");
            if (!int.TryParse(ConfigurationManager.AppSettings["BackupIntervalSeconds"],out time))
                throw new ConfigurationErrorsException("El intervalo de tiempo debe de ser un entero válido");
            if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["StartTimeOfDay"], out startTime))
                throw new ConfigurationErrorsException("La hora inicial es invalida por favor utilize el formato hh:mm:ss");
            if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["EndTimeOfDay"], out endTime))
                throw new ConfigurationErrorsException("La hora inicial es invalida por favor utilize el formato hh:mm:ss");
            if (time == 0)
                throw new ConfigurationErrorsException("El tiempo debe de ser mayor a 0 segundos");
            if (endTime <= startTime)
                throw new ConfigurationErrorsException("La hora de finalización no puede ser mayor a la hora de comienzo");
            if (sourceFolder == destinationFolder)
                throw new ConfigurationErrorsException("El directorio origen y el directorio destino deben ser diferentes");

            return new Configuration()
            {
                SourceFolder = sourceFolder,
                DestinationFolder = destinationFolder,
                BackupIntervalSeconds = time,
                StartTimeOfDay = startTime,
                EndTimeOfDay = endTime
            };
        }

       public override string ToString()
       {
           return $"Carpeta Origen: {SourceFolder}{Environment.NewLine}" +
                  $"Carpeta Destino: {DestinationFolder}{Environment.NewLine}" +
                  $"Intervalo de tiempo (segundos): {BackupIntervalSeconds}{Environment.NewLine}" +
                  $"Horario de copia de seguridad: {StartTimeOfDay.ToString("c")} - {EndTimeOfDay.ToString("c")}";
       }
    }
}