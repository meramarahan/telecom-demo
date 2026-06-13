using System.Diagnostics.CodeAnalysis;

namespace Telecom.Services
{
    [ExcludeFromCodeCoverage]
    public class Message
    {
        public string Command { get; set; } = string.Empty;
        public object? Payload { get; set; }
        public Action<object?>? OnSuccess { get; set; }
        public Action<Exception>? OnError { get; set; }
    }
}