using Raylib_cs;

namespace Tetris.src.Tetris
{
    public class Piece
    {
        public int[,] Shape { get; private set; }
        public Color[,] BlockColors { get; private set; }

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

            // Initialize BlockColors array with the same dimensions as Shape
            BlockColors = new Color[Shape.GetLength(0), Shape.GetLength(1)];
            for (int i = 0; i < Shape.GetLength(0); i++)
            {
                for (int j = 0; j < Shape.GetLength(1); j++)
                {
                    if (Shape[i, j] == 1)
                    {
                        BlockColors[i, j] = ShapeColor; // Default to ShapeColor
                    }
                }
            }
        }
        public void Draw(int offsetX, int offsetY)
        {
            // Calculate the effective X and Y positions within the play area
            int effectiveX = X - 20;
            int effectiveY = Y - 40;

            Color startColor = new Color(
                Math.Min(ShapeColor.R + 50, 255),
                Math.Min(ShapeColor.G + 50, 255),
                Math.Min(ShapeColor.B + 50, 255),
                ShapeColor.A);

            for (int i = 0; i < Shape.GetLength(0); i++)
            {
                for (int j = 0; j < Shape.GetLength(1); j++)
                {
                    if (Shape[i, j] == 1)
                    {
                        // Calculate the screen position where this cell should be drawn
                        int screenX = offsetX + (effectiveX / Size + j) * Size;
                        int screenY = offsetY + (effectiveY / Size + i) * Size;

                        // Interpolate the color for the gradient effect
                        float factor = (float)i / Shape.GetLength(0);
                        Color interpolatedColor = InterpolateColor(startColor, ShapeColor, factor);


                        // Draw the rectangle and outline
                        Raylib.DrawRectangle(screenX, screenY, Size, Size, interpolatedColor);
                        Color outlineColor = new Color(
                                                 Math.Max(interpolatedColor.R - 100, 0),
                                                 Math.Max(interpolatedColor.G - 100, 0),
                                                 Math.Max(interpolatedColor.B - 100, 0),
                                                 200); // Semi-transparent
                        Raylib.DrawRectangleLinesEx(new Rectangle(screenX, screenY, Size, Size), 2, outlineColor);
                    }
                }
            }
        }

        public void DrawPreview(int posX, int posY)
        {

            // Calculate the effective X and Y positions within the play area
            int effectiveX = X - 20;
            int effectiveY = Y - 40;

            Color startColor = new Color(
                Math.Min(ShapeColor.R + 50, 255),
                Math.Min(ShapeColor.G + 50, 255),
                Math.Min(ShapeColor.B + 50, 255),
                ShapeColor.A);

            for (int i = 0; i < Shape.GetLength(0); i++)
            {
                for (int j = 0; j < Shape.GetLength(1); j++)
                {
                    if (Shape[i, j] == 1)
                    {
                        // Calculate the screen position where this cell should be drawn
                        int screenX = posX + (effectiveX / Size + j) * Size;
                        int screenY = posY + (effectiveY / Size + i) * Size;

                        // Interpolate the color for the gradient effect
                        float factor = (float)i / Shape.GetLength(0);
                        Color interpolatedColor = InterpolateColor(startColor, ShapeColor, factor);


                        // Draw the rectangle and outline
                        Raylib.DrawRectangle(screenX, screenY, Size, Size, interpolatedColor);
                        Color outlineColor = new Color(
                                                 Math.Max(interpolatedColor.R - 100, 0),
                                                 Math.Max(interpolatedColor.G - 100, 0),
                                                 Math.Max(interpolatedColor.B - 100, 0),
                                                 200); // Semi-transparent
                        Raylib.DrawRectangleLinesEx(new Rectangle(screenX, screenY, Size, Size), 2, outlineColor);
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

        public Color InterpolateColor(Color startColor, Color endColor, float factor)
        {
            int r = (int)(startColor.R + factor * (endColor.R - startColor.R));
            int g = (int)(startColor.G + factor * (endColor.G - startColor.G));
            int b = (int)(startColor.B + factor * (endColor.B - startColor.B));
            int a = (int)(startColor.A + factor * (endColor.A - startColor.A));

            return new Color(r, g, b, a);
        }
        public Color GetBlockColor(int col, int row)
        {


            // Vérifier les limites de Shape
            if (col < 0 || col >= Shape.GetLength(1) || row < 0 || row >= Shape.GetLength(0))
            {
                // Gérer l'erreur ici, par exemple en renvoyant une couleur par défaut
                return Color.Gray; // Ou une autre couleur par défaut de votre choix
            }

            // Accès à Shape en utilisant les indices col et row
            if (Shape[row, col] == 1)
            {
                // Renvoyer la couleur correspondante du bloc
                return ShapeColor;
            }
            else
            {
                // Gérer le cas où Shape[row, col] != 1 si nécessaire
                return Color.Gray;
            }
        }



    }
}
