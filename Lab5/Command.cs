namespace Lab5;

public class Command
{
    public readonly string Name;
    private readonly Action _action;

    public Command(string name, Action action)
    {
        Name = name;
        _action = action;
    }

    public void Invoke()
    {
        _action.Invoke();
    }

    public static Command Create(string name, Action action)
    {
        return new Command(name, action);
    }
}