namespace ConsoleTetris;

/// <summary>
/// Tracks player statistics including total lines cleared, score, and level.
/// Implements the classic Tetris scoring system where points scale with level
/// and clearing multiple lines at once yields bonus points (e.g., Tetris = 4
/// lines = 1200 * level).
/// </summary>
public class GameStats
{
    private static readonly int[] LinePoints = { 0, 40, 100, 300, 1200 };

    public int TotalLines { get; private set; }
    public int Score { get; private set; }
    public int Level => (TotalLines / 10) + 1;

    public void AddClearedLines(int linesCleared)
    {
        if (linesCleared <= 0 || linesCleared > 4)
            return;

        TotalLines += linesCleared;
        Score += LinePoints[linesCleared] * Level;
    }

    public void Reset()
    {
        TotalLines = 0;
        Score = 0;
    }
}
