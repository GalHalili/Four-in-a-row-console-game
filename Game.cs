using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex02_01.GameLogic
{
    internal class Game
    {
        private const int MinBoardSize = 4;
        private const int MaxBoardSize = 8;
        private const int MinMovesNumToHaveFourInARow = 7;
        private const int MinMovesNumToHaveThreeInARow = 5;
        private readonly Board r_GameBoard;
        private readonly PlayerInfo r_Player1;
        private readonly PlayerInfo r_Player2;
        private PlayerInfo m_CurrentPlayer;
        private int m_MovesCounter = 0;

        public Game(int i_NumOfRows, int i_NumOfCols, string i_Player2Type)
        {
            r_GameBoard = new Board(i_NumOfRows, i_NumOfCols);
            r_Player1 = new PlayerInfo();
            r_Player2 = new PlayerInfo(checkIfInputEqualToComputerType(i_Player2Type), eCoinType.P2);
            m_CurrentPlayer = r_Player1;
        }

        public Board GameBoard
        {
            get
            {
                return r_GameBoard;
            }
        }

        public PlayerInfo Player1
        {
            get
            {
                return r_Player1;
            }
        }

        public PlayerInfo Player2
        {
            get
            {
                return r_Player2;
            }
        }

        public PlayerInfo CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
            set
            {
                m_CurrentPlayer = value;
            }
        }

        private int MovesCounter
        {
            get
            {
                return m_MovesCounter;
            }
            set
            {
                m_MovesCounter = value;
            }
        }

        public static bool CheckIfLegalBoardSize(int i_UserInput)
        {
            bool isLogicalSizeOfBoard = i_UserInput >= MinBoardSize && i_UserInput <= MaxBoardSize;

            return isLogicalSizeOfBoard;
        }

        public static bool CheckIfLegalPlayerType(int i_UserInput)
        {
            return Enum.IsDefined(typeof(ePlayerType), i_UserInput);
        }

        private static bool checkIfInputEqualToComputerType(string i_InputStr)
        {
            bool isInputEqualToComputer = false;

            if (Enum.TryParse(i_InputStr, out ePlayerType userChoice))
            {
                isInputEqualToComputer = userChoice == ePlayerType.Computer;
            }

            return isInputEqualToComputer;
        }

        public bool CheckIfThereIsFourInARow()
        {
            int rows = GameBoard.MatrixBoard.GetLength(0);
            int cols = GameBoard.MatrixBoard.GetLength(1);
            bool IsItPossibleToHaveFourInARow = MovesCounter >= MinMovesNumToHaveFourInARow;
            bool isFourInARow = false;

            if (IsItPossibleToHaveFourInARow)
            {
                // Check horizontally
                for (int i = 0; i < rows && !isFourInARow; i++)
                {
                    for (int j = 0; j <= cols - 4 && !isFourInARow; j++)
                    {
                        isFourInARow = checkIfThereIsASequancOfThreeInHorizontal(i, j) && GameBoard.MatrixBoard[i, j] == GameBoard.MatrixBoard[i, j + 3];
                    }
                }

                // Check vertically
                for (int j = 0; j < cols && !isFourInARow; j++)
                {
                    for (int i = 0; i <= rows - 4 && !isFourInARow; i++)
                    {
                        isFourInARow = checkIfThereIsASequancOfThreeInVertical(i, j) && GameBoard.MatrixBoard[i, j] == GameBoard.MatrixBoard[i + 3, j];
                    }
                }

                // Check diagonally (top-right to bottom-left)
                for (int i = 0; i <= rows - 4 && !isFourInARow; i++)
                {
                    for (int j = 0; j <= cols - 4 && !isFourInARow; j++)
                    {
                        isFourInARow = checkIfThereIsASequancOfThreeInRightToLeftDiagonal(i, j) && GameBoard.MatrixBoard[i, j] == GameBoard.MatrixBoard[i + 3, j + 3];
                    }
                }

                // Check diagonally (top-left to bottom-right)
                for (int i = 0; i <= rows - 4 && !isFourInARow; i++)
                {
                    for (int j = cols - 1; j >= 3 && !isFourInARow; j--)
                    {
                        isFourInARow = checkIfThereIsASequancOfThreeInLeftToRightDiagonal(i, j) && GameBoard.MatrixBoard[i, j] == GameBoard.MatrixBoard[i + 3, j - 3];
                    }
                }
            }

            return isFourInARow;
        }

        public bool CheckIfMoveIsLegal(int i_ColumnNumToInsertACoin)
        {
            bool isLegalMove = true;

            if (CurrentPlayer.PlayerCoin == eCoinType.P1 || (CurrentPlayer.PlayerCoin == eCoinType.P2 && !Player2.IsComputerPlayer))
            {
                isLegalMove = checkIfInputInRangeOfBoard(i_ColumnNumToInsertACoin) && !checkIfColumnInGameBoardIsFull(i_ColumnNumToInsertACoin);
            }

            return isLegalMove;
        }

        public bool CheckIfUserInputOfAnotherGameIsLegal(int i_InputFromUser)
        {
            return Enum.IsDefined(typeof(ePlayerWantsAnotherGameType), i_InputFromUser);
        }

        public bool CheckIfUserInputForAnotherGameIsYes(string i_InputFromUserStr)
        {
            bool isInputEqualToYes = false;

            if (Enum.TryParse(i_InputFromUserStr, out ePlayerWantsAnotherGameType userChoice))
            {
                isInputEqualToYes = userChoice == ePlayerWantsAnotherGameType.Yes;
            }

            return isInputEqualToYes;
        }

        public bool CheckIfBoardIsFull()
        {
            return MovesCounter == GameBoard.MatrixBoard.Length;
        }

        private bool checkIfColumnInGameBoardIsFull(int i_ColumnNum)
        {
            bool isColumnInBoardFull = GameBoard.CheckIfColumnIsFull(i_ColumnNum);

            return isColumnInBoardFull;
        }

        private bool checkIfInputInRangeOfBoard(int i_ColumnNumToInsertACoin)
        {
            bool isInRangeOfBoard = i_ColumnNumToInsertACoin > 0 && i_ColumnNumToInsertACoin <= GameBoard.MatrixBoard.GetLength(1);

            return isInRangeOfBoard;
        }

        public void UpdatePoints(bool i_IsCurrentPlayerWin)
        {
            if (i_IsCurrentPlayerWin)
            {
                if (CurrentPlayer.PlayerCoin == eCoinType.P1)
                {
                    Player1.CurrentPoints += 1;
                }
                else
                {
                    Player2.CurrentPoints += 1;
                }
            }
            else if (!CheckIfBoardIsFull()) //User entered Q
            {
                if (CurrentPlayer.PlayerCoin == eCoinType.P1)
                {
                    Player2.CurrentPoints += 1;
                }
                else
                {
                    Player1.CurrentPoints += 1;
                }
            }//else - Tie
        }

        public void UpdateBoardAccordingToPlayerMove(int i_ColumnNumToInsertACoin)
        {
            if (CurrentPlayer.IsComputerPlayer)
            {
                i_ColumnNumToInsertACoin = chooseComputerMove();
            }

            int rowToInsertCoin = GameBoard.CountFullCellsPerColumnArray[i_ColumnNumToInsertACoin - 1];
            if (CurrentPlayer.PlayerCoin == eCoinType.P1)
            {
                GameBoard.MatrixBoard[rowToInsertCoin, i_ColumnNumToInsertACoin - 1] = eMatrixCellType.P1;
            }
            else
            {
                GameBoard.MatrixBoard[rowToInsertCoin, i_ColumnNumToInsertACoin - 1] = eMatrixCellType.P2;
            }

            GameBoard.CountFullCellsPerColumnArray[i_ColumnNumToInsertACoin - 1] += 1;
            MovesCounter += 1;
        }

        public void SwitchPlayer()
        {
            if (CurrentPlayer.PlayerCoin == eCoinType.P1)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
        }

        private int chooseARandomMove()
        {
            int columnNum = GameBoard.MatrixBoard.GetLength(1);
            List<int> arrayOfEmptyColumns = new List<int>();

            while (columnNum > 0)
            {
                if (!checkIfColumnInGameBoardIsFull(columnNum))
                {
                    arrayOfEmptyColumns.Add(columnNum);
                }

                columnNum--;
            }

            Random random = new Random();
            int randomIndex = random.Next(arrayOfEmptyColumns.Count);
            int randomColumn = arrayOfEmptyColumns[randomIndex];

            return randomColumn;
        }

        public void ActivateAnotherGame()
        {
            GameBoard.CleanGameBoard();
            GameBoard.ResetCountFullCellsPerColumnArray();
            CurrentPlayer = Player1;
            MovesCounter = 0;
        }

        private bool checkIfThereIsASequancOfThreeInHorizontal(int i_Row, int i_Col)
        {
            bool isThreeInHorizontal = GameBoard.MatrixBoard[i_Row, i_Col] != eMatrixCellType.Blank &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row, i_Col + 1] &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row, i_Col + 2];

            return isThreeInHorizontal;
        }

        private bool checkIfThereIsASequancOfThreeInVertical(int i_Row, int i_Col)
        {
            bool isThreeInVertical = GameBoard.MatrixBoard[i_Row, i_Col] != eMatrixCellType.Blank &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 1, i_Col] &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 2, i_Col];

            return isThreeInVertical;
        }

        private bool checkIfThereIsASequancOfThreeInRightToLeftDiagonal(int i_Row, int i_Col)
        {
            bool isThreeInLeftToRightDiagonal = GameBoard.MatrixBoard[i_Row, i_Col] != eMatrixCellType.Blank &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 1, i_Col + 1] &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 2, i_Col + 2];

            return isThreeInLeftToRightDiagonal;
        }

        private bool checkIfThereIsASequancOfThreeInLeftToRightDiagonal(int i_Row, int i_Col)
        {
            bool isThreeInRightToLeftDiagonal = GameBoard.MatrixBoard[i_Row, i_Col] != eMatrixCellType.Blank &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 1, i_Col - 1] &&
                            GameBoard.MatrixBoard[i_Row, i_Col] == GameBoard.MatrixBoard[i_Row + 2, i_Col - 2];

            return isThreeInRightToLeftDiagonal;
        }

        private int chooseComputerMove()
        {
            int numOfRows = GameBoard.MatrixBoard.GetLength(0);
            int numOfCols = GameBoard.MatrixBoard.GetLength(1);
            bool IsItPossibleToHaveThreeInARow = MovesCounter >= MinMovesNumToHaveThreeInARow;
            bool isThreeInARow = false;
            int potentialColToInsertACoin = -1;

            if (IsItPossibleToHaveThreeInARow)
            {
                // Check horizontally
                for (int row = 0; row < numOfRows && !isThreeInARow; row++)
                {
                    for (int col = 0; col <= numOfCols - 4 && !isThreeInARow; col++)
                    {
                        if (checkIfThereIsASequancOfThreeInHorizontal(row, col))
                        {
                            if (GameBoard.CountFullCellsPerColumnArray[col + 3] == row)
                            {
                                potentialColToInsertACoin = col + 4;
                                isThreeInARow = true;
                            }
                            else if (col != 0 && GameBoard.CountFullCellsPerColumnArray[col - 1] == row)
                            {
                                potentialColToInsertACoin = col;
                                isThreeInARow = true;
                            }
                        }
                    }
                }

                // Check vertically
                for (int col = 0; col < numOfCols && !isThreeInARow; col++)
                {
                    for (int row = 0; row <= numOfRows - 4 && !isThreeInARow; row++)
                    {
                        isThreeInARow = checkIfThereIsASequancOfThreeInVertical(row, col) && GameBoard.CountFullCellsPerColumnArray[col] == row + 3;
                        if (isThreeInARow)
                        {
                            potentialColToInsertACoin = col + 1;
                        }
                    }
                }

                // Check diagonally (top-right to bottom-left)
                for (int row = 0; row <= numOfRows - 4 && !isThreeInARow; row++)
                {
                    for (int col = 0; col <= numOfCols - 4 && !isThreeInARow; col++)
                    {
                        if (checkIfThereIsASequancOfThreeInRightToLeftDiagonal(row, col))
                        {
                            if (GameBoard.CountFullCellsPerColumnArray[col + 3] == row + 3)
                            {
                                potentialColToInsertACoin = col + 4;
                                isThreeInARow = true;
                            }
                            else if (col != 0 && row != 0 && GameBoard.CountFullCellsPerColumnArray[col - 1] == row - 1)
                            {
                                potentialColToInsertACoin = col;
                                isThreeInARow = true;
                            }
                        }
                    }
                }

                // Check diagonally (top-left to bottom-right)
                for (int row = 0; row <= numOfRows - 4 && !isThreeInARow; row++)
                {
                    for (int col = numOfCols - 1; col >= 3 && !isThreeInARow; col--)
                    {
                        if (checkIfThereIsASequancOfThreeInLeftToRightDiagonal(row, col))
                        {
                            if (GameBoard.CountFullCellsPerColumnArray[col - 3] == row + 3)
                            {
                                potentialColToInsertACoin = col - 2;
                                isThreeInARow = true;
                            }
                        }
                    }
                }
            }

            if (!IsItPossibleToHaveThreeInARow || !isThreeInARow)
            {
                potentialColToInsertACoin = chooseARandomMove();
            }

            return potentialColToInsertACoin;
        }
    }
} 