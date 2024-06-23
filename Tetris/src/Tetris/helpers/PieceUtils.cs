// PieceUtils.cs

namespace Tetris.src.Tetris.Helpers
{
    public static class PieceUtils
    {
        public static int CalculateLowestPieceRow(Piece piece)
        {
            int N = piece.Shape.GetLength(0);
            int lowestRow = 0;

            for (int i = N - 1; i >= 0; i--)
            {
                for (int j = 0; j < N; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        lowestRow = i;
                        break;
                    }
                }
                if (lowestRow != 0)
                    break;
            }

            return lowestRow;
        }
    }
}
