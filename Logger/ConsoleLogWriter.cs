using System;
using System.Text;

namespace Logs
{
    /// <summary>
    /// Clase que muestra el log del sistema en una consola de MS-Dos
    /// </summary>
    public class ConsoleLogWriter : ILogWriter
    {
        public void Write(string path, string contents, Encoding encoding)
        {
            Console.Write(contents);
        }
    }
}