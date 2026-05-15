namespace Service
{
    public interface IMessageService
    {
        void ShowMessage(string message);
        void ShowError(string error);
    }
}