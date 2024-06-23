namespace Tetris.src.Tetris
{
    public static class Shapes
    {
        public static int[,] LShape = new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        public static int[,] TShape = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        public static int[,] ZShape = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        };

        public static int[,] SShape = new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 0, 0 }
        };

        public static int[,] IShape = new int[,]
        {
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };

        public static int[,] SquareShape = new int[,]
        {
            { 1, 1 },
            { 1, 1 }
        };


    }
}