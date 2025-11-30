namespace ConsoleTetris;

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
