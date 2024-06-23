using Raylib_cs;

namespace Tetris.src.Tetris
{
    public class Piece
    {
        public int[,] Shape { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public int Rotation { get; private set; }
        public Color ShapeColor { get; set; }
        private int playAreaWidth;
        private int playAreaHeight;

        public Piece(int[,] shape, Color color, int size, int playAreaWidth, int playAreaHeight)
        {
            Shape = shape;
            ShapeColor = color;
            Size = size;
            this.playAreaWidth = playAreaWidth;
            this.playAreaHeight = playAreaHeight;
            X = playAreaWidth / 2 - Shape.GetLength(1) / 2 * size;
            Y = 0;
        }

        public void Draw(int offsetX, int offsetY)
        {
            // Calculate the effective X and Y positions within the play area
            int effectiveX = X - 20;
            int effectiveY = Y - 40;

            for (int i = 0; i < Shape.GetLength(0); i++)
            {
                for (int j = 0; j < Shape.GetLength(1); j++)
                {
                    if (Shape[i, j] == 1)
                    {
                        // Calculate the screen position where this cell should be drawn
                        int screenX = offsetX + (effectiveX / Size + j) * Size;
                        int screenY = offsetY + (effectiveY / Size + i) * Size;

                        // Draw the rectangle and outline
                        Raylib.DrawRectangle(screenX, screenY, Size, Size, ShapeColor);
                        Raylib.DrawRectangleLines(screenX, screenY, Size, Size, Color.Black);
                    }
                }
            }
        }
        public void DrawPreview(int posX, int posY)
        {
            // Offset for preview display
            int offsetX = posX;
            int offsetY = posY;

            // Clone the Shape array to avoid modifying the original Shape of the piece
            int[,] previewShape = (int[,])Shape.Clone();

            // Loop through the previewShape matrix to draw the preview
            for (int i = 0; i < previewShape.GetLength(0); i++)
            {
                for (int j = 0; j < previewShape.GetLength(1); j++)
                {
                    if (previewShape[i, j] == 1)
                    {
                        // Calculate the screen position where this cell should be drawn for preview
                        int screenX = offsetX + j * Size;
                        int screenY = offsetY + i * Size;

                        // Draw the rectangle and outline for preview
                        Raylib.DrawRectangle(screenX, screenY, Size, Size, ShapeColor);
                        Raylib.DrawRectangleLines(screenX, screenY, Size, Size, Color.Black);
                    }
                }
            }
        }



        public void Rotate()
        {
            int N = Shape.GetLength(0);
            int[,] newShape = new int[N, N];

            // Rotate the shape
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    newShape[j, N - 1 - i] = Shape[i, j];
                }
            }

            // Calculate the adjustment in X position
            int deltaX = (Shape.GetLength(1) - newShape.GetLength(1)) * Size / 2;
            X += deltaX;

            Shape = newShape;
        }




    }
}
