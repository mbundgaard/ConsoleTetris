namespace ConsoleTetris;

public class PlayerControls
{
    public ConsoleKey Left { get; init; }
    public ConsoleKey Right { get; init; }
    public ConsoleKey Rotate { get; init; }
    public ConsoleKey SoftDrop { get; init; }
    public ConsoleKey HardDrop { get; init; }

    public static PlayerControls Player1 => new()
    {
        Left = ConsoleKey.A,
        Right = ConsoleKey.D,
        Rotate = ConsoleKey.W,
        SoftDrop = ConsoleKey.S,
        HardDrop = ConsoleKey.D1
    };

    public static PlayerControls Player2 => new()
    {
        Left = ConsoleKey.LeftArrow,
        Right = ConsoleKey.RightArrow,
        Rotate = ConsoleKey.UpArrow,
        SoftDrop = ConsoleKey.DownArrow,
        HardDrop = ConsoleKey.Spacebar
    };

    public static PlayerControls SinglePlayer => new()
    {
        Left = ConsoleKey.LeftArrow,
        Right = ConsoleKey.RightArrow,
        Rotate = ConsoleKey.UpArrow,
        SoftDrop = ConsoleKey.DownArrow,
        HardDrop = ConsoleKey.Spacebar
    };
}
