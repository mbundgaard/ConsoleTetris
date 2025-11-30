namespace ConsoleTetris;

public class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly BoardCell[,] _cells;

    public Board()
    {
        _cells = new BoardCell[Height, Width];
        Clear();
    }

    public void Clear()
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                _cells[row, col] = BoardCell.Empty;
            }
        }
    }

    public BoardCell GetCell(int row, int col)
    {
        if (row < 0 || row >= Height || col < 0 || col >= Width)
            return BoardCell.Empty;
        return _cells[row, col];
    }

    public bool IsOccupied(int row, int col)
    {
        if (col < 0 || col >= Width || row >= Height)
            return true;
        if (row < 0)
            return false;
        return _cells[row, col].IsOccupied;
    }

    public bool CanPlace(Tetromino piece)
    {
        var shape = piece.CurrentShape;
        for (int row = 0; row < piece.Height; row++)
        {
            for (int col = 0; col < piece.Width; col++)
            {
                if (shape[row, col])
                {
                    int boardRow = piece.Y + row;
                    int boardCol = piece.X + col;

                    if (IsOccupied(boardRow, boardCol))
                        return false;
                }
            }
        }
        return true;
    }

    public void LockPiece(Tetromino piece)
    {
        var shape = piece.CurrentShape;
        for (int row = 0; row < piece.Height; row++)
        {
            for (int col = 0; col < piece.Width; col++)
            {
                if (shape[row, col])
                {
                    int boardRow = piece.Y + row;
                    int boardCol = piece.X + col;

                    if (boardRow >= 0 && boardRow < Height && boardCol >= 0 && boardCol < Width)
                    {
                        _cells[boardRow, boardCol] = BoardCell.Normal(piece.Type);
                    }
                }
            }
        }
    }

    public int ClearCompleteLines()
    {
        int linesCleared = 0;

        for (int row = Height - 1; row >= 0; row--)
        {
            if (IsLineComplete(row))
            {
                ClearLine(row);
                linesCleared++;
                row++; // Check this row again since rows shifted down
            }
        }

        return linesCleared;
    }

    public void ReceiveGarbage(int lineCount)
    {
        if (lineCount <= 0)
            return;

        // Push existing rows up
        for (int row = 0; row < Height - lineCount; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                _cells[row, col] = _cells[row + lineCount, col];
            }
        }

        // Add garbage lines at the bottom with random gaps
        for (int i = 0; i < lineCount; i++)
        {
            int garbageRow = Height - lineCount + i;
            int gapCol = Random.Shared.Next(Width);

            for (int col = 0; col < Width; col++)
            {
                _cells[garbageRow, col] = col == gapCol ? BoardCell.Empty : BoardCell.Garbage;
            }
        }
    }

    private bool IsLineComplete(int row)
    {
        for (int col = 0; col < Width; col++)
        {
            if (!_cells[row, col].IsOccupied)
                return false;
        }
        return true;
    }

    private void ClearLine(int clearedRow)
    {
        // Move all rows above down by one
        for (int row = clearedRow; row > 0; row--)
        {
            for (int col = 0; col < Width; col++)
            {
                _cells[row, col] = _cells[row - 1, col];
            }
        }

        // Clear the top row
        for (int col = 0; col < Width; col++)
        {
            _cells[0, col] = BoardCell.Empty;
        }
    }
}
