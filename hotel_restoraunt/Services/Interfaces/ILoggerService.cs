// hotel_restoraunt/Services/Interfaces/ILoggerService.cs
using System;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
        void LogDebug(string message);
    }
}