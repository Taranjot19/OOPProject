using System;
using System.Collections.Generic;

namespace ConnectFour
{
    // Main game class
    public class ConnectFourGame
    {
        private GameBoard board;
        private Player[] players;
        private int currentPlayerIndex;
        private bool gameOver;

        public void Start()
        {
            Console.WriteLine("Welcome to Connect Four!");
            InitializeGame();
            PlayGame();
        }

        private void InitializeGame()
        {
            board = new GameBoard();
            gameOver = false;

            Console.WriteLine("Select game mode:");
            Console.WriteLine("1. Two Players");
            Console.WriteLine("2. Player vs Computer");
            Console.Write("Enter choice (1-2): ");

            int choice = GetValidInput(1, 2);

            players = new Player[2];
            players[0] = new HumanPlayer('X', "Player 1");

            if (choice == 1)
            {
                players[1] = new HumanPlayer('O', "Player 2");
            }
            else
            {
                players[1] = new ComputerPlayer('O', "Computer");
            }

            currentPlayerIndex = 0;
        }

        private void PlayGame()
        {
            while (!gameOver)
            {
                Console.Clear();
                board.Display();

                Player currentPlayer = players[currentPlayerIndex];
                Console.WriteLine($"{currentPlayer.Name}'s turn ({currentPlayer.Symbol})");

                try
                {
                    int column = currentPlayer.MakeMove(board);
                    board.DropPiece(column, currentPlayer.Symbol);

                    if (board.CheckWin(currentPlayer.Symbol))
                    {
                        EndGame($"{currentPlayer.Name} wins!");
                    }
                    else if (board.IsFull())
                    {
                        EndGame("The game is a draw!");
                    }
                    else
                    {
                        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                    }
                }
                catch (InvalidMoveException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
            }
        }

        private void EndGame(string message)
        {
            Console.Clear();
            board.Display();
            Console.WriteLine(message);
            gameOver = true;

            Console.WriteLine("Press any key to play again or Q to quit...");
            var key = Console.ReadKey();
            if (key.KeyChar != 'q' && key.KeyChar != 'Q')
            {
                Console.Clear();
                Start();
            }
        }

        private int GetValidInput(int min, int max)
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || input < min || input > max)
            {
                Console.Write($"Invalid input. Enter a number between {min} and {max}: ");
            }
            return input;
        }
    }

    // Game board class using 2D array
    public class GameBoard
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private char[,] grid;

        public GameBoard()
        {
            grid = new char[Rows, Columns];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    grid[row, col] = '.';
                }
            }
        }

        public void Display()
        {
            Console.WriteLine("\n 1 2 3 4 5 6 7");
            for (int row = 0; row < Rows; row++)
            {
                Console.Write("|");
                for (int col = 0; col < Columns; col++)
                {
                    Console.ForegroundColor = GetSymbolColor(grid[row, col]);
                    Console.Write(grid[row, col]);
                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------");
        }

        private ConsoleColor GetSymbolColor(char symbol)
        {
            switch (symbol)
            {
                case 'X': return ConsoleColor.Green;
                case 'O': return ConsoleColor.Blue;
                default: return ConsoleColor.White;
            }
        }

        public void DropPiece(int column, char symbol)
        {
            if (!IsValidMove(column))
            {
                throw new InvalidMoveException("Invalid move. Column is either full or out of range.");
            }

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, column] == '.')
                {
                    grid[row, column] = symbol;
                    break;
                }
            }
        }

        public bool IsValidMove(int column)
        {
            return column >= 0 && column < Columns && grid[0, column] == '.';
        }

        public bool CheckWin(char symbol)
        {
            // Check horizontal
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row, col + 1] == symbol &&
                        grid[row, col + 2] == symbol &&
                        grid[row, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check vertical
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col] == symbol &&
                        grid[row + 2, col] == symbol &&
                        grid[row + 3, col] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check diagonals
            return CheckDiagonalWin(symbol);
        }

        private bool CheckDiagonalWin(char symbol)
        {
            // Top-left to bottom-right
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col + 1] == symbol &&
                        grid[row + 2, col + 2] == symbol &&
                        grid[row + 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Bottom-left to top-right
            for (int row = 3; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row - 1, col + 1] == symbol &&
                        grid[row - 2, col + 2] == symbol &&
                        grid[row - 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsFull()
        {
            for (int col = 0; col < Columns; col++)
            {
                if (grid[0, col] == '.')
                {
                    return false;
                }
            }
            return true;
        }
    }

    // Abstract player class
    public abstract class Player
    {
        public char Symbol { get; }
        public string Name { get; }

        protected Player(char symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }

        public abstract int MakeMove(GameBoard board);
    }

    // Human player class
    public class HumanPlayer : Player
    {
        public HumanPlayer(char symbol, string name) : base(symbol, name) { }

        public override int MakeMove(GameBoard board)
        {
            Console.Write($"Enter column (1-7): ");
            int column;
            while (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7)
            {
                Console.Write("Invalid input. Enter a number between 1 and 7: ");
            }
            return column - 1; // Convert to 0-based index
        }
    }

    // Computer player class
    public class ComputerPlayer : Player
    {
        private static readonly Random random = new Random();

        public ComputerPlayer(char symbol, string name) : base(symbol, name) { }

        public override int MakeMove(GameBoard board)
        {
            Console.WriteLine("Computer is thinking...");
            System.Threading.Thread.Sleep(1000);

            // Try to find a winning move
            for (int col = 0; col < 7; col++)
            {
                if (board.IsValidMove(col))
                {
                    var testBoard = board.Clone();
                    testBoard.DropPiece(col, Symbol);
                    if (testBoard.CheckWin(Symbol))
                    {
                        return col;
                    }
                }
            }

            // If no winning move, choose randomly
            List<int> validColumns = new List<int>();
            for (int col = 0; col < 7; col++)
            {
                if (board.IsValidMove(col))
                {
                    validColumns.Add(col);
                }
            }

            return validColumns[random.Next(validColumns.Count)];
        }
    }

    // Custom exception for invalid moves
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message) { }
    }

    // Extension method for cloning the board
    public static class GameBoardExtensions
    {
        public static GameBoard Clone(this GameBoard original)
        {
            var clone = new GameBoard();
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (original.IsValidMove(col))
                    {
                        clone.DropPiece(col, original.GetCell(row, col));
                    }
                }
            }
            return clone;
        }

        public static char GetCell(this GameBoard board, int row, int col)
        {
            var gridField = typeof(GameBoard).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var grid = (char[,])gridField.GetValue(board);
            return grid[row, col];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var game = new ConnectFourGame();
            game.Start();
        }
    }
}
