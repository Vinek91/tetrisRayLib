namespace Tetris.GlobalSettings
{
    public static class GlobalSettings
    {
        public static int PlayAreaX = 20;
        public static int PlayAreaY = 40;
        public static int PlayAreaWidth = 300;
        public static int PlayAreaHeight = 600;
        public static int[,] Grid = new int[10, 20];

        public static bool IsLineComplete(int row)
        {
            for (int col = 0; col < 10; col++)
            {
                if (Grid[col, row] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static void RemoveLine(int row)
        {
            for (int r = row; r > 0; r--)
            {
                for (int c = 0; c < 10; c++)
                {
                    Grid[c, r] = Grid[c, r - 1];
                }
            }

            for (int c = 0; c < 10; c++)
            {
                Grid[c, 0] = 0;
            }
        }

        public static bool IsPieceOutOfBounds(Piece.Piece piece)
        {
            if (piece == null || piece.Shape == null)
            {
                throw new ArgumentNullException("piece", "Piece or Piece.Shape is null.");
            }

            int N = piece.Shape.GetLength(0);
            int cellSize = piece.Size;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int gridX = (piece.X - PlayAreaX) / cellSize + j;
                        int gridY = (piece.Y - PlayAreaY) / cellSize + i;

                        // Check if the piece is out of bounds
                        if (gridX < 0 || gridX >= 10 || gridY >= 20 || (gridY >= 0 && Grid[gridX, gridY] == 1))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void PlacePiece(Piece.Piece piece)
        {
            int N = piece.Shape.GetLength(0);
            int cellSize = piece.Size;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int gridX = (piece.X - PlayAreaX) / cellSize + j;
                        int gridY = (piece.Y - PlayAreaY) / cellSize + i;


                        Grid[gridX, gridY] = 1;
                    }
                }
            }
        }

    }
}
