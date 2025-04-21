using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class ConnectFour
    {
        private Gameboard board;
        private Player[] players;
        private int currentPlayerIndex;
        private bool gameover;
        public void Start()
            
        {
            Console.WriteLine("Welcome to Connect Four!");
            InitializeGame();
            PlayGame();
        }
        private void InitaializeGame()
        {
          board = new GameBoard();
            gameOver = false;
            
            Console.WriteLine("Select game mode: ");
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
                board.Dispaly();
                Player currentPlayer = player[currentPlayerIndex];
                Console.WriteLine($"{currentPlayer.Name}'s turn (
                                  {currentPlayer.Symbol})");
                    try {
                        int column = currentPlayer.MakeMove(board);
                        board.DropPiece(column, currentPlayer.Symbol);
                        if (board.CheckWin(currentPlayer.Symbol))
                        {
                            EndGame($"{currentPlayer.Name}wins!);
                                    }
                                    else if (board.IsFull())
                                    {
                                        EndGame("The game is a draw!");
                                    }
                                    else
                                    {
                                        currentPlayerIndex = (currentPlayerIndex + 1) %
                                            player.Length;
                                    }
                                    }
                                    catch (InvalidMoveException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.ReadKey();
                                    }
                    }
                         }
                                    
        }
        private void EndGame(string message)
        {
            Consol.Clear();
            board.Display();
            Console.WriteLine(message);
            gameOver = true;

            Console.Writeline("Press any key to play again or Q to quit...");
            var key = Console.ReadKey();
            if (key.KeyChar != 'q' && key.KeyChar != 'Q')
            {
                Console.Clear();
                Start;
            }
        }
        private int GetValidInput(int min, int max)
        {
            int input;
            while(!int.TryParse(Console.ReadLine(), out input ) || input < min || input > max)
            {
                Console.Write($"Invalid input.Enter a number between {min} and {max}:");
            }
            return input;
        }
        }
                public class GameBoard
             {
         private const int Rows = 6;
        private  const int Columns = 7;
                 private char[,]grid;
                 public GameBoard()
                 {
                     grid = new Char[Rows, Columns];
                     InitilizeBoard();
                 }
                     private void InitializeBoard()
                     {
                         for (int row = 0; row < Rows; rows++)
                         {
                             for (int col =0; col < Columns; col++)
                             {
                                 grid[row,col]= '.';
                             }
                         }
                     }

                 public void Display()
                 {
                     Console.WriteLine("\n 1 2 3 4 5 6 7");
                     for (int row - 0; row < Rows; row++)
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
                     Console.WriteLine("--------------------------");
                 }
                 private ConsoleColor Get SymbolColor(char symbol)
                 {
                     switch (symbol)
                     {
                         case 'X' : return ConsoleColor.Yellow;
                             Case'O' :return ConsoleColor.Red;
                         default: return ConsoleColor.White;
                     }
                 }
                 public void DropPiece(int column, char symbol)
                 {
                     if (!Is ValidMove(column))
                     {
                         throw new InvalidMoveException("Invalid move. Column is either full or is out of the range..");
                     }
                     for (int row = Row - 1; row >=0; row--)
                     {
                         if(grid[row,column] == '.')
                         {
                             grid[row, column] = symbol;
                             break;
                         }
                     }
                 }
                  public bool IsValidMove(int Column)
                  {
                      return Column >= 0 && column < Cloumns && grid[0 , column]== ',';
                  }
                 public bool CheckWin(char symbol)
                 {
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

                     return CheckDiagonalWin(symbol);
                 }

                 private bool CheckDiagonalWin(char symbol)
                 {
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
                 

                 

                     
                     
                 
                 

                     
            static void Main(string[] args)
        {
        }
    }
}
    
