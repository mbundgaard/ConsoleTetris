namespace ConsoleTetris;

/// <summary>
/// Manages the game state for a single player. Handles piece spawning, movement,
/// rotation, soft/hard drops, gravity timing, line clearing, and garbage exchange.
/// Each player instance maintains its own board, current/next pieces, and statistics.
/// </summary>
public class PlayerGame
{
    private const int FallIntervalMs = 500;
    private static readonly int[] GarbageTable = { 0, 0, 1, 2, 4 }; // Index = lines cleared

    private readonly Board _board;
    private readonly GameStats _stats;
    private readonly PlayerControls _controls;
    private Tetromino? _currentPiece;
    private Tetromino? _nextPiece;
    private bool _isGameOver;
    private DateTime _lastFallTime;
    private int _pendingGarbageToSend;

    public Board Board => _board;
    public GameStats Stats => _stats;
    public Tetromino? CurrentPiece => _currentPiece;
    public Tetromino? NextPiece => _nextPiece;
    public bool IsGameOver => _isGameOver;
    public PlayerControls Controls => _controls;
    public int PendingGarbageToSend => _pendingGarbageToSend;

    public PlayerGame(PlayerControls controls)
    {
        _board = new Board();
        _stats = new GameStats();
        _controls = controls;
    }

    public void Start()
    {
        _isGameOver = false;
        _nextPiece = null;
        SpawnNewPiece();
        _lastFallTime = DateTime.UtcNow;
    }

    public void ProcessInput(ConsoleKey key)
    {
        if (_isGameOver || _currentPiece == null)
            return;

        if (key == _controls.Left)
        {
            TryMove(-1, 0);
        }
        else if (key == _controls.Right)
        {
            TryMove(1, 0);
        }
        else if (key == _controls.Rotate)
        {
            TryRotate();
        }
        else if (key == _controls.SoftDrop)
        {
            if (TryMove(0, 1))
            {
                _lastFallTime = DateTime.UtcNow;
            }
        }
        else if (key == _controls.HardDrop)
        {
            HardDrop();
        }
    }

    public void Update()
    {
        if (_isGameOver || _currentPiece == null)
            return;

        var now = DateTime.UtcNow;
        if ((now - _lastFallTime).TotalMilliseconds >= FallIntervalMs)
        {
            _lastFallTime = now;

            if (!TryMove(0, 1))
            {
                LockCurrentPiece();
            }
        }
    }

    public void Restart()
    {
        _board.Clear();
        _stats.Reset();
        _isGameOver = false;
        _nextPiece = null;
        _pendingGarbageToSend = 0;
        SpawnNewPiece();
        _lastFallTime = DateTime.UtcNow;
    }

    public int ConsumeGarbageToSend()
    {
        int garbage = _pendingGarbageToSend;
        _pendingGarbageToSend = 0;
        return garbage;
    }

    public void ReceiveGarbage(int lineCount)
    {
        _board.ReceiveGarbage(lineCount);
    }

    private bool TryMove(int deltaX, int deltaY)
    {
        if (_currentPiece == null)
            return false;

        _currentPiece.X += deltaX;
        _currentPiece.Y += deltaY;

        if (!_board.CanPlace(_currentPiece))
        {
            _currentPiece.X -= deltaX;
            _currentPiece.Y -= deltaY;
            return false;
        }

        return true;
    }

    private void TryRotate()
    {
        if (_currentPiece == null)
            return;

        _currentPiece.RotateClockwise();

        if (!_board.CanPlace(_currentPiece))
        {
            _currentPiece.RotateCounterClockwise();
        }
    }

    private void HardDrop()
    {
        if (_currentPiece == null)
            return;

        while (_board.CanPlace(_currentPiece))
        {
            _currentPiece.Y++;
        }

        _currentPiece.Y--;
        LockCurrentPiece();
    }

    private void LockCurrentPiece()
    {
        if (_currentPiece == null)
            return;

        _board.LockPiece(_currentPiece);
        int linesCleared = _board.ClearCompleteLines();
        _stats.AddClearedLines(linesCleared);

        // Calculate garbage to send
        if (linesCleared > 0 && linesCleared < GarbageTable.Length)
        {
            _pendingGarbageToSend += GarbageTable[linesCleared];
        }

        SpawnNewPiece();
    }

    private void SpawnNewPiece()
    {
        if (_nextPiece != null)
        {
            _currentPiece = _nextPiece;
        }
        else
        {
            _currentPiece = Tetromino.CreateRandom(0, 0);
        }

        _currentPiece.X = (Board.Width - _currentPiece.Width) / 2;
        _currentPiece.Y = 0;

        _nextPiece = Tetromino.CreateRandom(0, 0);

        if (!_board.CanPlace(_currentPiece))
        {
            _isGameOver = true;
            _currentPiece = null;
        }
    }
}
