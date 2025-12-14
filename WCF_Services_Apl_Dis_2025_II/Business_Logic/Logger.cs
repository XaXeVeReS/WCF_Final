using System;
using System.IO;
using System.Text;

namespace Business_Logic
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly object lockObj = new object();

        static Logger()
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string fullMessage = ex != null 
                ? $"{message} | Exception: {ex.GetType().Name} | {ex.Message} | StackTrace: {ex.StackTrace}"
                : message;
            Log("ERROR", fullMessage);
        }

        public static void LogDebug(string message)
        {
            Log("DEBUG", message);
        }

        public static void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        private static void Log(string level, string message)
        {
            lock (lockObj)
            {
                try
                {
                    string fileName = $"Log_{DateTime.Now:yyyy-MM-dd}.txt";
                    string filePath = Path.Combine(LogPath, fileName);
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}{Environment.NewLine}";

                    File.AppendAllText(filePath, logEntry, Encoding.UTF8);
                }
                catch
                {
                    // Si no se puede escribir el log, simplemente continuar
                }
            }
        }
    }
}