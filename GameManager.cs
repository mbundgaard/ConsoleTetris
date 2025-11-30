namespace ConsoleTetris;

/// <summary>
/// Orchestrates game flow: menu navigation, game mode selection, and the main game loops
/// for single and dual player modes. Coordinates input handling, game updates, rendering,
/// and garbage line exchange between players.
/// </summary>
public class GameManager
{
    private const int FrameIntervalMs = 16;

    private readonly ConsoleRenderer _renderer;
    private GameMode _currentMode;
    private PlayerGame? _player1;
    private PlayerGame? _player2;
    private bool _isRunning;

    public GameManager()
    {
        _renderer = new ConsoleRenderer();
    }

    public void Run()
    {
        _renderer.Initialize();
        _isRunning = true;

        while (_isRunning)
        {
            _currentMode = ShowMenu();
            if (!_isRunning)
                break;

            _renderer.Initialize();

            if (_currentMode == GameMode.SinglePlayer)
            {
                RunSinglePlayer();
            }
            else
            {
                RunDualPlayer();
            }
        }

        _renderer.Cleanup();
    }

    private GameMode ShowMenu()
    {
        int selectedIndex = 0;

        while (_isRunning)
        {
            _renderer.RenderMenu(selectedIndex);

            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = 1;
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex == 0 ? GameMode.SinglePlayer : GameMode.DualPlayer;
                    case ConsoleKey.Escape:
                        _isRunning = false;
                        return GameMode.SinglePlayer;
                }
            }

            Thread.Sleep(FrameIntervalMs);
        }

        return GameMode.SinglePlayer;
    }

    private void RunSinglePlayer()
    {
        _player1 = new PlayerGame(PlayerControls.SinglePlayer);
        _player1.Start();
        bool gameOverScreenShown = false;

        while (_isRunning)
        {
            // Process input
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    _renderer.Initialize(); // Clear screen before returning to menu
                    return;
                }

                if (_player1.IsGameOver)
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        _player1.Restart();
                        _renderer.Initialize();
                        gameOverScreenShown = false;
                    }
                    continue;
                }

                _player1.ProcessInput(key.Key);
            }

            // Update
            if (!_player1.IsGameOver)
            {
                _player1.Update();
            }

            // Render
            if (_player1.IsGameOver)
            {
                if (!gameOverScreenShown)
                {
                    _renderer.RenderSinglePlayerGameOver(_player1.Stats);
                    gameOverScreenShown = true;
                }
            }
            else
            {
                _renderer.RenderSinglePlayer(_player1);
            }

            Thread.Sleep(FrameIntervalMs);
        }
    }

    private void RunDualPlayer()
    {
        _player1 = new PlayerGame(PlayerControls.Player1);
        _player2 = new PlayerGame(PlayerControls.Player2);
        _player1.Start();
        _player2.Start();
        bool gameOverScreenShown = false;

        while (_isRunning)
        {
            bool eitherGameOver = _player1.IsGameOver || _player2.IsGameOver;

            // Process input
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    _renderer.Initialize(); // Clear screen before returning to menu
                    return;
                }

                if (eitherGameOver)
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        _player1.Restart();
                        _player2.Restart();
                        _renderer.Initialize();
                        gameOverScreenShown = false;
                    }
                    continue;
                }

                // Route input to appropriate player
                _player1.ProcessInput(key.Key);
                _player2.ProcessInput(key.Key);
            }

            // Update
            if (!eitherGameOver)
            {
                _player1.Update();
                _player2.Update();

                // Handle garbage sending between players
                int garbage1 = _player1.ConsumeGarbageToSend();
                int garbage2 = _player2.ConsumeGarbageToSend();

                if (garbage1 > 0)
                {
                    _player2.ReceiveGarbage(garbage1);
                }
                if (garbage2 > 0)
                {
                    _player1.ReceiveGarbage(garbage2);
                }
            }

            // Render
            if (eitherGameOver)
            {
                if (!gameOverScreenShown)
                {
                    _renderer.RenderDualPlayerGameOver(_player1, _player2);
                    gameOverScreenShown = true;
                }
            }
            else
            {
                _renderer.RenderDualPlayer(_player1, _player2);
            }

            Thread.Sleep(FrameIntervalMs);
        }
    }
}
