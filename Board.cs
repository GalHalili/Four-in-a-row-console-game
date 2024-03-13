namespace Ex02_01.GameLogic
{
    internal struct Board
    {
        private readonly eMatrixCellType[,] r_MatrixBoard;
        private readonly int[] r_CountFullCellsPerColumnArray;

        public Board(int i_NumOfRows, int i_NumOfCols)
        {
            r_MatrixBoard = new eMatrixCellType[i_NumOfRows, i_NumOfCols];
            r_CountFullCellsPerColumnArray = new int[i_NumOfCols];
        }

        public eMatrixCellType[,] MatrixBoard
        {
            get
            {
                return r_MatrixBoard;
            }
        }

        public int[] CountFullCellsPerColumnArray
        {
            get
            {
                return r_CountFullCellsPerColumnArray;
            }
        }

        public bool CheckIfColumnIsFull(int i_ColumnNum)
        {
            bool isFullColumn = CountFullCellsPerColumnArray[i_ColumnNum - 1] >= MatrixBoard.GetLength(0);

            return isFullColumn;
        }

        public void CleanGameBoard()
        {
            for (int row = 0; row < MatrixBoard.GetLength(0); row++)
            {
                for (int col = 0; col < MatrixBoard.GetLength(1); col++)
                {
                    MatrixBoard[row, col] = eMatrixCellType.Blank;
                }
            }
        }

        public void ResetCountFullCellsPerColumnArray()
        {
            for (int i = 0; i < CountFullCellsPerColumnArray.Length; i++)
            {
                CountFullCellsPerColumnArray[i] = 0;
            }
        }
    }
}