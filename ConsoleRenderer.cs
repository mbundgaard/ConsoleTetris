namespace ConsoleTetris;

public class ConsoleRenderer
{
    private const string NormalCell = "[]";
    private const string GarbageCell = "<>";
    private const string EmptyCell = ". ";
    private const string BorderLeft = "<!";
    private const string BorderRight = "!>";
    private const int LeftPanelWidth = 16;
    private const int BoardDisplayWidth = 24; // BorderLeft + 10*2 cells + BorderRight

    public void Initialize()
    {
        Console.CursorVisible = false;
        Console.Clear();
        // Set text color to #00FF00 (green) using ANSI escape code
        Console.Write("\x1b[38;2;0;255;0m");
    }

    public void RenderMenu(int selectedIndex)
    {
        Console.SetCursorPosition(0, 0);
        var sb = new System.Text.StringBuilder();

        // ASCII art logo
        string[] logo = {
            "  [][][][][][][][][]  [][][][][][][][][]  [][][][][][][][][]  [][][][][][][][][]    [][][]  [][][][][][][][][]",
            "  [][][][][][][][][]  [][][][][][][][][]  [][][][][][][][][]  [][][][][][][][][]    [][][]  [][][][][][][][][]",
            "        [][][]        [][][]                    [][][]        [][][]      [][][]    [][][]  [][][]",
            "        [][][]        [][][]                    [][][]        [][][]      [][][]    [][][]  [][][]",
            "        [][][]        [][][][][][]              [][][]        [][][][][][]          [][][]  [][][][][][][][][]",
            "        [][][]        [][][][][][]              [][][]        [][][][][][]          [][][]  [][][][][][][][][]",
            "        [][][]        [][][]                    [][][]        [][][]      [][][]    [][][]              [][][]",
            "        [][][]        [][][]                    [][][]        [][][]      [][][]    [][][]              [][][]",
            "        [][][]        [][][][][][][][][]        [][][]        [][][]      [][][]    [][][]  [][][][][][][][][]",
            "        [][][]        [][][][][][][][][]        [][][]        [][][]      [][][]    [][][]  [][][][][][][][][]"
        };

        const string margin = "    "; // 4 character right margin
        int logoWidth = logo[0].Length;
        int menuWidth = logoWidth;
        string Pad(string text) => margin + new string(' ', Math.Max(0, (menuWidth - text.Length) / 2)) + text;

        sb.AppendLine();
        foreach (var line in logo)
        {
            sb.AppendLine(margin + line);
        }
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();

        string single = selectedIndex == 0 ? ">>  Single Player  <<" : "    Single Player    ";
        string dual = selectedIndex == 1 ? ">>  Dual Player    <<" : "    Dual Player      ";

        sb.AppendLine(Pad(single));
        sb.AppendLine();
        sb.AppendLine(Pad(dual));
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad("Press Enter to start"));
        sb.AppendLine(Pad("Esc to exit"));

        Console.Write(sb.ToString());
    }

    public void RenderSinglePlayer(PlayerGame player)
    {
        Console.SetCursorPosition(0, 0);

        var display = BuildDisplayGrid(player.Board, player.CurrentPiece);

        var leftPanel = BuildStatsPanel(player.Stats, player.NextPiece);

        var rightPanel = new Dictionary<int, string>
        {
            { 2, "<-     LEFT" },
            { 3, "->     RIGHT" },
            { 4, "UP     ROTATE" },
            { 5, "DOWN   SOFT DROP" },
            { 6, "SPACE  HARD DROP" }
        };

        const string hMargin = "                                "; // 32 characters
        var sb = new System.Text.StringBuilder();

        // 3 lines vertical margin
        for (int i = 0; i < 3; i++)
        {
            sb.AppendLine();
        }

        for (int row = 0; row < Board.Height; row++)
        {
            sb.Append(hMargin);
            sb.Append(leftPanel.GetValueOrDefault(row, "").PadRight(LeftPanelWidth));
            sb.Append(BorderLeft);
            for (int col = 0; col < Board.Width; col++)
            {
                sb.Append(GetCellString(display[row, col]));
            }
            sb.Append(BorderRight);
            sb.Append("      ");
            sb.Append(rightPanel.GetValueOrDefault(row, ""));
            sb.AppendLine();
        }

        int interiorWidth = Board.Width * 2;
        sb.Append(hMargin);
        sb.Append(new string(' ', LeftPanelWidth));
        sb.Append(BorderLeft);
        sb.Append(new string('=', interiorWidth));
        sb.AppendLine(BorderRight);

        sb.Append(hMargin);
        sb.Append(new string(' ', LeftPanelWidth + 2));
        for (int i = 0; i < interiorWidth / 2; i++)
        {
            sb.Append("\\/");
        }
        sb.AppendLine();

        Console.Write(sb.ToString());
    }

    public void RenderSinglePlayerGameOver(GameStats stats)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        const string margin = "    "; // 4 character margin (same as menu)
        const int logoWidth = 110; // Same width as menu logo
        string Pad(string text) => margin + new string(' ', Math.Max(0, (logoWidth - text.Length) / 2)) + text;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad("*** GAME OVER ***"));
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad($"SCORE: {stats.Score}"));
        sb.AppendLine(Pad($"LINES: {stats.TotalLines}"));
        sb.AppendLine(Pad($"LEVEL: {stats.Level}"));
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad("Press Enter to restart"));
        sb.AppendLine(Pad("Esc for menu"));

        Console.Write(sb.ToString());
    }

    public void RenderDualPlayer(PlayerGame player1, PlayerGame player2)
    {
        Console.SetCursorPosition(0, 0);

        var display1 = BuildDisplayGrid(player1.Board, player1.CurrentPiece);
        var display2 = BuildDisplayGrid(player2.Board, player2.CurrentPiece);

        var leftPanel1 = BuildStatsPanel(player1.Stats, player1.NextPiece);
        var leftPanel2 = BuildStatsPanel(player2.Stats, player2.NextPiece);

        const string hMargin = "            "; // 12 characters to center dual board
        var sb = new System.Text.StringBuilder();

        // 3 lines vertical margin
        for (int i = 0; i < 3; i++)
        {
            sb.AppendLine();
        }

        // Headers
        sb.Append(hMargin);
        sb.Append(new string(' ', LeftPanelWidth + 6));
        sb.Append("PLAYER 1");
        sb.Append(new string(' ', BoardDisplayWidth + LeftPanelWidth));
        sb.AppendLine("PLAYER 2");
        sb.AppendLine();

        for (int row = 0; row < Board.Height; row++)
        {
            sb.Append(hMargin);
            // Player 1
            sb.Append(leftPanel1.GetValueOrDefault(row, "").PadRight(LeftPanelWidth));
            sb.Append(BorderLeft);
            for (int col = 0; col < Board.Width; col++)
            {
                sb.Append(GetCellString(display1[row, col]));
            }
            sb.Append(BorderRight);

            sb.Append("      "); // Gap between boards

            // Player 2
            sb.Append(leftPanel2.GetValueOrDefault(row, "").PadRight(LeftPanelWidth));
            sb.Append(BorderLeft);
            for (int col = 0; col < Board.Width; col++)
            {
                sb.Append(GetCellString(display2[row, col]));
            }
            sb.Append(BorderRight);

            sb.AppendLine();
        }

        // Floors
        int interiorWidth = Board.Width * 2;
        sb.Append(hMargin);
        sb.Append(new string(' ', LeftPanelWidth));
        sb.Append(BorderLeft);
        sb.Append(new string('=', interiorWidth));
        sb.Append(BorderRight);
        sb.Append("      ");
        sb.Append(new string(' ', LeftPanelWidth));
        sb.Append(BorderLeft);
        sb.Append(new string('=', interiorWidth));
        sb.AppendLine(BorderRight);

        // Base decorations
        sb.Append(hMargin);
        sb.Append(new string(' ', LeftPanelWidth + 2));
        for (int i = 0; i < interiorWidth / 2; i++)
        {
            sb.Append("\\/");
        }
        sb.Append("      ");
        sb.Append(new string(' ', LeftPanelWidth + 2));
        for (int i = 0; i < interiorWidth / 2; i++)
        {
            sb.Append("\\/");
        }
        sb.AppendLine();

        Console.Write(sb.ToString());
    }

    public void RenderDualPlayerGameOver(PlayerGame player1, PlayerGame player2)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        const string margin = "    "; // 4 character margin (same as menu)
        const int logoWidth = 110; // Same width as menu logo
        string Pad(string text) => margin + new string(' ', Math.Max(0, (logoWidth - text.Length) / 2)) + text;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad("*** GAME OVER ***"));
        sb.AppendLine();

        // Determine winner by score
        string winner;
        if (player1.Stats.Score > player2.Stats.Score)
            winner = "PLAYER 1 WINS!";
        else if (player2.Stats.Score > player1.Stats.Score)
            winner = "PLAYER 2 WINS!";
        else
            winner = "IT'S A TIE!";

        sb.AppendLine(Pad(winner));
        sb.AppendLine();
        sb.AppendLine();

        // Show both players' stats side by side
        sb.AppendLine(Pad("PLAYER 1          PLAYER 2"));
        sb.AppendLine(Pad($"SCORE: {player1.Stats.Score,-10}SCORE: {player2.Stats.Score}"));
        sb.AppendLine(Pad($"LINES: {player1.Stats.TotalLines,-10}LINES: {player2.Stats.TotalLines}"));
        sb.AppendLine(Pad($"LEVEL: {player1.Stats.Level,-10}LEVEL: {player2.Stats.Level}"));
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Pad("Press Enter to restart"));
        sb.AppendLine(Pad("Esc for menu"));

        Console.Write(sb.ToString());
    }

    public void Cleanup()
    {
        Console.CursorVisible = true;
        // Reset text color to default
        Console.Write("\x1b[0m");
    }

    private BoardCell[,] BuildDisplayGrid(Board board, Tetromino? currentPiece)
    {
        var display = new BoardCell[Board.Height, Board.Width];

        for (int row = 0; row < Board.Height; row++)
        {
            for (int col = 0; col < Board.Width; col++)
            {
                display[row, col] = board.GetCell(row, col);
            }
        }

        if (currentPiece != null)
        {
            var shape = currentPiece.CurrentShape;
            for (int row = 0; row < currentPiece.Height; row++)
            {
                for (int col = 0; col < currentPiece.Width; col++)
                {
                    if (shape[row, col])
                    {
                        int boardRow = currentPiece.Y + row;
                        int boardCol = currentPiece.X + col;

                        if (boardRow >= 0 && boardRow < Board.Height &&
                            boardCol >= 0 && boardCol < Board.Width)
                        {
                            display[boardRow, boardCol] = BoardCell.Normal(currentPiece.Type);
                        }
                    }
                }
            }
        }

        return display;
    }

    private string GetCellString(BoardCell cell)
    {
        if (!cell.IsOccupied)
            return EmptyCell;
        return cell.IsGarbage ? GarbageCell : NormalCell;
    }

    private Dictionary<int, string> BuildStatsPanel(GameStats stats, Tetromino? nextPiece)
    {
        var panel = new Dictionary<int, string>
        {
            { 1, $"LINES: {stats.TotalLines}" },
            { 2, $"LEVEL: {stats.Level}" },
            { 3, $"SCORE: {stats.Score}" },
            { 5, "NEXT:" }
        };

        if (nextPiece != null)
        {
            var nextShape = nextPiece.CurrentShape;
            for (int row = 0; row < nextPiece.Height; row++)
            {
                var rowStr = new System.Text.StringBuilder();
                rowStr.Append("  ");
                for (int col = 0; col < nextPiece.Width; col++)
                {
                    rowStr.Append(nextShape[row, col] ? NormalCell : "  ");
                }
                panel[7 + row] = rowStr.ToString();
            }
        }

        return panel;
    }
}
