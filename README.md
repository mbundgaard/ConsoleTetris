# Console Tetris

A classic Tetris clone built in C# for the console, featuring single and dual player modes with garbage line attacks.

## Inspiration

This project recreates the look and feel of the original Tetris from 1984, developed by Alexey Pajitnov on the Electronika 60.

<p align="center">
  <img src="https://tetris.wiki/images/1/1d/Tetris_E60_title.png" alt="Original Tetris title screen on Electronika 60" width="400"/>
  <img src="https://tetris.wiki/images/e/e9/Tetris_E60_ingame.png" alt="Original Tetris gameplay on Electronika 60" width="400"/>
</p>

*The original Tetris on Electronika 60 (1984) - Title screen and gameplay*

Learn more: [Tetris (Electronika 60) - TetrisWiki](https://tetris.wiki/Tetris_(Electronika_60))

## Features

- Single Player and Dual Player modes
- ASCII art title screen with menu
- Classic 10x20 board
- All 7 tetrominoes (I, O, T, S, Z, J, L) with rotation
- Line clearing with classic scoring system
- Next piece preview
- Level progression (every 10 lines)
- Green retro-style text (#00FF00)

### Dual Player Mode

- Side-by-side competitive gameplay
- Garbage line attacks: clearing multiple lines sends garbage to opponent
- Game ends when first player tops out
- Winner determined by score

## Controls

### Single Player / Player 2

| Key | Action |
|-----|--------|
| Left Arrow | Move left |
| Right Arrow | Move right |
| Up Arrow | Rotate |
| Down Arrow | Soft drop |
| Space | Hard drop |

### Player 1 (Dual Player Mode)

| Key | Action |
|-----|--------|
| A | Move left |
| D | Move right |
| W | Rotate |
| S | Soft drop |
| 1 | Hard drop |

### General

| Key | Action |
|-----|--------|
| Enter | Start game / Restart |
| Esc | Return to menu / Exit |

## Scoring

Points are awarded based on lines cleared simultaneously, multiplied by the current level:

| Lines | Points | Name |
|-------|--------|------|
| 1 | 40 x level | Single |
| 2 | 100 x level | Double |
| 3 | 300 x level | Triple |
| 4 | 1200 x level | Tetris |

## Garbage System (Dual Player)

Clearing multiple lines sends garbage rows to your opponent:

| Lines Cleared | Garbage Sent |
|---------------|--------------|
| 1 (Single) | 0 |
| 2 (Double) | 1 |
| 3 (Triple) | 2 |
| 4 (Tetris) | 4 |

- Garbage lines appear at the bottom of the opponent's board
- Each garbage row has one random gap
- Garbage blocks display as `<>` vs normal blocks `[]`

## Requirements

- .NET 8.0

## Running the Game

```bash
dotnet run
```

## Project Structure

| File | Description |
|------|-------------|
| `Program.cs` | Entry point |
| `GameManager.cs` | Menu and game mode orchestration |
| `PlayerGame.cs` | Individual player game state and logic |
| `Board.cs` | Grid state, collision detection, garbage lines |
| `BoardCell.cs` | Cell state (empty, normal, garbage) |
| `Tetromino.cs` | Piece definitions and rotation states |
| `ConsoleRenderer.cs` | ASCII rendering for menu and gameplay |
| `GameStats.cs` | Score, lines, and level tracking |
| `GameMode.cs` | Game mode enumeration |
| `PlayerControls.cs` | Control scheme definitions |

## Acknowledgements

Inspired by the original Tetris created by Alexey Pajitnov in 1984.

## Disclaimer

This is a fan-made project for educational purposes. 
Tetris® is a registered trademark of The Tetris Company, LLC.
This project is not affiliated with or endorsed by The Tetris Company.