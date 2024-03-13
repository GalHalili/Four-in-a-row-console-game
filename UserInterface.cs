using Ex02_01.GameLogic;
using System;
using System.Linq;

namespace Ex02_01.UI
{
    internal class UserInterface
    {
        private const int NumOfSpaces = 3;
        private const int NumOfCharAccurance = 1;

        public void StartGame()
        {
            Console.WriteLine(@"Hello!
Please enter the number of rows for the board size (between 4 and 8)");
            string numOfRowsInput = getValidMatrixSizeOrTypeOfPlayer("Rows");
            Console.WriteLine("Please enter the number of columns for the board size (between 4 and 8)");
            string numOfColsInput = getValidMatrixSizeOrTypeOfPlayer("Cols");
            Console.WriteLine(string.Format(@"If you want to play against another player press 1 
If you want to play against the computer press 2"));
            string againstPlayerInput = getValidMatrixSizeOrTypeOfPlayer("Against player");
            int numOfRows = int.Parse(numOfRowsInput);
            int numOfCols = int.Parse(numOfColsInput);
            Game game = new Game(numOfRows, numOfCols, againstPlayerInput);
            cleanAndPrintScreen(game);
            runGame(ref game);
        }

        private void runGame(ref Game io_Game)
        {   
            int columnNumToInsertACoin;
            bool isCurrentPlayerIsAWinner = false;
            
            Console.WriteLine("Please enter the number of the column in which you would like to insert a coin or Q to quit the game");
            string inputFromUser = Console.ReadLine();
            while (inputFromUser != "Q")
            {
                if (checkIfValidColumnOrValidInputOfAnotherGame(io_Game, inputFromUser, "column"))
                {
                    columnNumToInsertACoin = int.Parse(inputFromUser);
                }
                else
                {
                    inputFromUser = getValidColumnOrAnotherGameInput(io_Game, "column");
                    if (int.TryParse(inputFromUser, out int result))
                    {
                        columnNumToInsertACoin = int.Parse(inputFromUser);
                    }
                    else
                    {
                        break;
                    }
                }

                isCurrentPlayerIsAWinner = manageMoveAndCheckIfThereIsAWinner(ref io_Game, columnNumToInsertACoin);
                if (isCurrentPlayerIsAWinner)
                {
                    break;
                }

                io_Game.SwitchPlayer();
                if (io_Game.CurrentPlayer.IsComputerPlayer)
                {
                    isCurrentPlayerIsAWinner = manageMoveAndCheckIfThereIsAWinner(ref io_Game, columnNumToInsertACoin);
                    io_Game.SwitchPlayer();
                }

                if (isCurrentPlayerIsAWinner)
                {
                    break;
                }
                else if (io_Game.CheckIfBoardIsFull())
                {
                    Console.WriteLine("The game ended with a tie");
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter the number of the column in which you would like to insert a coin or Q to quit the game");
                    inputFromUser = Console.ReadLine();
                }
            }

            if (inputFromUser == "Q")
            {
                Console.WriteLine("You chose to quit the game");
                io_Game.UpdatePoints(isCurrentPlayerIsAWinner);
            }

            printPointsStatus(io_Game);
            if (checkIfUserWantsAnotherGame(io_Game))
            {
                io_Game.ActivateAnotherGame();
                cleanAndPrintScreen(io_Game);
                runGame(ref io_Game);
            }
            else
            {
                Console.WriteLine("You chose to end the game, Bye Bye!");
            }
        }

        private bool manageMoveAndCheckIfThereIsAWinner(ref Game io_Game, int i_ColumnNumToInsertACoin) 
        {
            io_Game.UpdateBoardAccordingToPlayerMove(i_ColumnNumToInsertACoin);
            cleanAndPrintScreen(io_Game);
            return checkAndHandleWinnerCase(ref io_Game);
        }

        private void cleanAndPrintScreen(Game i_Game)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard(i_Game);
        }

        private void printBoard(Game i_Game)
        {
            int numOfRowsInBoardGame = i_Game.GameBoard.MatrixBoard.GetLength(0);
            int numOfColsInBoardGame = i_Game.GameBoard.MatrixBoard.GetLength(1);
            string separatorLine = "====";

            separatorLine = string.Concat(Enumerable.Repeat(separatorLine, numOfColsInBoardGame));
            separatorLine = string.Concat(separatorLine, "=");
            for (int row = numOfRowsInBoardGame; row >= 0; row--)
            {
                if (row == numOfRowsInBoardGame)
                {
                    for (int currentColumn = 1; currentColumn <= numOfColsInBoardGame; currentColumn++)
                    {
                        Console.Write($"  {currentColumn} ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    for (int col = 0; col < numOfColsInBoardGame; col++)
                    {
                        if (numOfColsInBoardGame < row)
                        {
                            Console.Write(new string('|', NumOfCharAccurance));
                            Console.Write(new string(' ', NumOfSpaces));
                        }
                        else
                        {
                            GameLogic.eMatrixCellType cellType = i_Game.GameBoard.MatrixBoard[row, col];
                            if (cellType == GameLogic.eMatrixCellType.Blank)
                            {
                                Console.Write("|   ");
                            }
                            else if (cellType == GameLogic.eMatrixCellType.P1)
                            {
                                Console.Write("| {0} ", eMatrixCellsSimbolsType.X);
                            }
                            else
                            {
                                Console.Write("| {0} ", eMatrixCellsSimbolsType.O);
                            }
                        }
                    }

                    Console.WriteLine("|");
                    Console.WriteLine(separatorLine);
                }
            }
        }

        private void printPointsStatus(Game i_Game)
        {
            string typeOfPlayer2 = "Player2";

            if (i_Game.Player2.IsComputerPlayer)
            {
                typeOfPlayer2 = "Computer";
            }

            Console.WriteLine(string.Format(
@"The points status is:
Player1: {0}
{1}: {2}", i_Game.Player1.CurrentPoints, typeOfPlayer2, i_Game.Player2.CurrentPoints));
        }

        private static string getValidMatrixSizeOrTypeOfPlayer(string i_FlagStr)
        {
            string inputFromUser = Console.ReadLine();
            bool isValidInput = checkIfValidMatrixSizeOrTypeOfPlayer(inputFromUser, i_FlagStr);

            if (!isValidInput)
            {
                Console.WriteLine("Invalid number, try again!");
                inputFromUser = getValidMatrixSizeOrTypeOfPlayer(i_FlagStr);
            }

            return inputFromUser;
        }

        private static bool checkIfValidMatrixSizeOrTypeOfPlayer(string i_InputFromUser, string i_FlagStr)
        {
            bool isIntegerNumber = int.TryParse(i_InputFromUser, out int result);
            bool isValidInput = isIntegerNumber;

            if (isIntegerNumber)
            {
                int inputToInteger = int.Parse(i_InputFromUser);
                if (i_FlagStr == "Rows" || i_FlagStr == "Cols")
                {
                    isValidInput = GameLogic.Game.CheckIfLegalBoardSize(inputToInteger);
                }
                else if (i_FlagStr == "Against player")
                {
                    isValidInput = GameLogic.Game.CheckIfLegalPlayerType(inputToInteger);
                }
            }

            return isValidInput;
        }

        private string getValidColumnOrAnotherGameInput(Game i_Game, string i_FlagStr)
        {
            string i_InputColumnFromUser = Console.ReadLine();
            bool isValidColumnInput = checkIfValidColumnOrValidInputOfAnotherGame(i_Game, i_InputColumnFromUser, i_FlagStr);

            if (!isValidColumnInput)
            {
                i_InputColumnFromUser = getValidColumnOrAnotherGameInput(i_Game, i_FlagStr);
            }

            return i_InputColumnFromUser;
        }

        private bool checkIfValidColumnOrValidInputOfAnotherGame(Game i_Game, string i_InputFromUser, string i_FlagStr)
        {
            bool isUserWantsToQuit = i_InputFromUser == "Q" && i_FlagStr == "column";
            bool isValidInput = isUserWantsToQuit;

            if (!isUserWantsToQuit)
            {
                bool isIntegerNumber = int.TryParse(i_InputFromUser, out int result);
                isValidInput = isIntegerNumber;
                if (isIntegerNumber)
                {
                    int inputToInteger = int.Parse(i_InputFromUser);
                    if (i_FlagStr == "column")
                    {
                        isValidInput = i_Game.CheckIfMoveIsLegal(inputToInteger);
                    }
                    else if (i_FlagStr == "anotherGame")
                    {
                        isValidInput = i_Game.CheckIfUserInputOfAnotherGameIsLegal(inputToInteger);
                    }
                }

                if (!isValidInput)
                {
                    Console.WriteLine("Invalid input, try again!");
                }
            }

            return isValidInput;
        }

        private bool checkAndHandleWinnerCase(ref Game io_Game)
        {
            bool isCurrentPlayerIsAWinner = io_Game.CheckIfThereIsFourInARow();

            if (isCurrentPlayerIsAWinner)
            {
                io_Game.UpdatePoints(isCurrentPlayerIsAWinner);
                string winnerStr = io_Game.CurrentPlayer.IsComputerPlayer ? "Computer" : io_Game.CurrentPlayer.PlayerCoin.ToString();
                Console.WriteLine(string.Format("The winner is {0}", winnerStr));
            }

            return isCurrentPlayerIsAWinner;
        }

        private bool checkIfUserWantsAnotherGame(Game i_Game)
        {
            Console.WriteLine("If you want another game please press 1, otherwise press 2");
            string validInputFromUser = getValidColumnOrAnotherGameInput(i_Game, "anotherGame");
            bool isUserWantsAnotherGame = i_Game.CheckIfUserInputForAnotherGameIsYes(validInputFromUser);

            return isUserWantsAnotherGame;
        }
    }
}