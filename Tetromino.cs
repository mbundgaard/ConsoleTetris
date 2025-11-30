namespace ConsoleTetris;

/// <summary>
/// Defines the seven standard Tetris piece types: I, J, L, O, S, T, and Z.
/// </summary>
public enum TetrominoType
{
    I, J, L, O, S, T, Z
}

/// <summary>
/// Represents a Tetris piece (tetromino) with its type, position, and rotation state.
/// Contains all rotation matrices for each piece type following the standard Tetris
/// rotation system (SRS-like). Supports random piece generation.
/// </summary>
public class Tetromino
{
    public TetrominoType Type { get; }
    public int X { get; set; }
    public int Y { get; set; }
    public int RotationState { get; private set; }

    private readonly bool[][,] _rotations;

    public Tetromino(TetrominoType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
        RotationState = 0;
        _rotations = GetRotations(type);
    }

    public bool[,] CurrentShape => _rotations[RotationState];

    public int Width => CurrentShape.GetLength(1);
    public int Height => CurrentShape.GetLength(0);

    public void RotateClockwise()
    {
        RotationState = (RotationState + 1) % _rotations.Length;
    }

    public void RotateCounterClockwise()
    {
        RotationState = (RotationState - 1 + _rotations.Length) % _rotations.Length;
    }

    public Tetromino Clone()
    {
        var clone = new Tetromino(Type, X, Y);
        clone.RotationState = RotationState;
        return clone;
    }

    private static bool[][,] GetRotations(TetrominoType type)
    {
        return type switch
        {
            TetrominoType.I => new[]
            {
                new[,]
                {
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false },
                    { false, false, false, false }
                },
                new[,]
                {
                    { false, false, true, false },
                    { false, false, true, false },
                    { false, false, true, false },
                    { false, false, true, false }
                },
                new[,]
                {
                    { false, false, false, false },
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false }
                },
                new[,]
                {
                    { false, true, false, false },
                    { false, true, false, false },
                    { false, true, false, false },
                    { false, true, false, false }
                }
            },
            TetrominoType.J => new[]
            {
                new[,]
                {
                    { true, false, false },
                    { true, true, true },
                    { false, false, false }
                },
                new[,]
                {
                    { false, true, true },
                    { false, true, false },
                    { false, true, false }
                },
                new[,]
                {
                    { false, false, false },
                    { true, true, true },
                    { false, false, true }
                },
                new[,]
                {
                    { false, true, false },
                    { false, true, false },
                    { true, true, false }
                }
            },
            TetrominoType.L => new[]
            {
                new[,]
                {
                    { false, false, true },
                    { true, true, true },
                    { false, false, false }
                },
                new[,]
                {
                    { false, true, false },
                    { false, true, false },
                    { false, true, true }
                },
                new[,]
                {
                    { false, false, false },
                    { true, true, true },
                    { true, false, false }
                },
                new[,]
                {
                    { true, true, false },
                    { false, true, false },
                    { false, true, false }
                }
            },
            TetrominoType.O => new[]
            {
                new[,]
                {
                    { true, true },
                    { true, true }
                }
            },
            TetrominoType.S => new[]
            {
                new[,]
                {
                    { false, true, true },
                    { true, true, false },
                    { false, false, false }
                },
                new[,]
                {
                    { false, true, false },
                    { false, true, true },
                    { false, false, true }
                },
                new[,]
                {
                    { false, false, false },
                    { false, true, true },
                    { true, true, false }
                },
                new[,]
                {
                    { true, false, false },
                    { true, true, false },
                    { false, true, false }
                }
            },
            TetrominoType.T => new[]
            {
                new[,]
                {
                    { false, true, false },
                    { true, true, true },
                    { false, false, false }
                },
                new[,]
                {
                    { false, true, false },
                    { false, true, true },
                    { false, true, false }
                },
                new[,]
                {
                    { false, false, false },
                    { true, true, true },
                    { false, true, false }
                },
                new[,]
                {
                    { false, true, false },
                    { true, true, false },
                    { false, true, false }
                }
            },
            TetrominoType.Z => new[]
            {
                new[,]
                {
                    { true, true, false },
                    { false, true, true },
                    { false, false, false }
                },
                new[,]
                {
                    { false, false, true },
                    { false, true, true },
                    { false, true, false }
                },
                new[,]
                {
                    { false, false, false },
                    { true, true, false },
                    { false, true, true }
                },
                new[,]
                {
                    { false, true, false },
                    { true, true, false },
                    { true, false, false }
                }
            },
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    public static Tetromino CreateRandom(int x, int y)
    {
        var types = Enum.GetValues<TetrominoType>();
        var type = types[Random.Shared.Next(types.Length)];
        return new Tetromino(type, x, y);
    }
}
