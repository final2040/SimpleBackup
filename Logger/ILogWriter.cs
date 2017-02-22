using System.Text;

namespace Logs
{
    /// <summary>
    /// Interfaz para los modulos de Logeo
    /// </summary>
    public interface ILogWriter
    {
        void Write(string path, string contents, Encoding encoding);
    }
}