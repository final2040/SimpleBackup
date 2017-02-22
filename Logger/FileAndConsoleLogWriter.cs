using System.Text;

namespace Logs
{
    public class FileAndConsoleLogWriter:ILogWriter
    {
        readonly FileLogWriter _fileLogWriter = new FileLogWriter();
        readonly ConsoleLogWriter _consoleLogWriter = new ConsoleLogWriter();

        public void Write(string path, string contents, Encoding encoding)
        {
            _consoleLogWriter.Write(path,contents, encoding);
            _fileLogWriter.Write(path, contents,encoding);
        }
    }
}