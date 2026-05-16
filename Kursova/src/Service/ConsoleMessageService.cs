namespace Service
{
    public class ConsoleMessageService : IMessageService
    {
        public void ShowMessage(string message) => Console.WriteLine($"[INFO]: {message}");
        public void ShowError(string error) => Console.Error.WriteLine($"[ERROR]: {error}");
    }

}
