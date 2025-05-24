// hotel_restoraunt/Services/Implementations/LoggerService.cs
using System;
using System.IO;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class LoggerService : ILoggerService
    {
        private readonly string _logFolderPath;
        
        public LoggerService()
        {
            _logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            EnsureLogDirectoryExists();
        }
        
        private void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }
        }
        
        private void WriteLog(string level, string message, Exception exception = null)
        {
            try
            {
                string logFilePath = Path.Combine(_logFolderPath, $"application_{DateTime.Now:yyyyMMdd}.log");
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] [{level}] {message}";
                
                if (exception != null)
                {
                    logEntry += $"\nException: {exception.Message}";
                    logEntry += $"\nStackTrace: {exception.StackTrace}\n";
                }
                
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                
                // Також виводимо у консоль для зручності під час розробки
                if (level == "ERROR")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (level == "WARNING")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (level == "INFO")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                
                Console.WriteLine(logEntry);
                Console.ResetColor();
            }
            catch
            {
                // У разі помилки при логуванні просто ігноруємо
            }
        }
        
        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }
        
        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }
        
        public void LogError(string message, Exception exception = null)
        {
            WriteLog("ERROR", message, exception);
        }
        
        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }
    }
}