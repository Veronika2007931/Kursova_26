namespace Command
{
    public interface ICommand
    {
        string Description { get; }
        void Execute();
    }
}