namespace ConsoleTetris;

public struct BoardCell
{
    public bool IsOccupied { get; init; }
    public bool IsGarbage { get; init; }
    public TetrominoType? Type { get; init; }

    public static BoardCell Empty => new() { IsOccupied = false, IsGarbage = false, Type = null };

    public static BoardCell Normal(TetrominoType type) => new() { IsOccupied = true, IsGarbage = false, Type = type };

    public static BoardCell Garbage => new() { IsOccupied = true, IsGarbage = true, Type = null };
}
