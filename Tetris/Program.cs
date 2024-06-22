using System;
using System.IO;
using Raylib_cs;
using Tetris.GlobalSettings;
using Tetris.Piece;
using Tetris.Shape;

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 700, "Tetris avec Raylib en C#");
        Raylib.SetTargetFPS(60);

        Piece[] pieces = new Piece[]
        {
            new Piece(Shapes.LShape, Color.Orange, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
            new Piece(Shapes.TShape, Color.Purple, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
            new Piece(Shapes.ZShape, Color.Red, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
            new Piece(Shapes.SShape, Color.Green, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
            new Piece(Shapes.IShape, Color.SkyBlue, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
            new Piece(Shapes.SquareShape, Color.Yellow, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight)
        };

        Raylib.InitAudioDevice();
        string musicPath = "./resources/Tetris.mp3";

        if (!File.Exists(musicPath))
        {
            Console.WriteLine($"Music file not found at {musicPath}");
        }
        else
        {
            Music music = Raylib.LoadMusicStream(musicPath);
            Raylib.PlayMusicStream(music);

            Random rnd = new Random();
            int selectedPieceIndex = rnd.Next(0, pieces.Length);
            pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
            pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;  // Ensure starting at the top of the play area
            bool pieceLocked = false;
            int framesPerDescent = 30;
            int frameCounter = 0;
            bool isDownKeyPressed = false;  // Variable pour suivre l'état de la touche bas

            while (!Raylib.WindowShouldClose())
            {
                Raylib.UpdateMusicStream(music);

                frameCounter++;

                if (frameCounter >= framesPerDescent)
                {
                    frameCounter = 0;

                    if (!pieceLocked && !isDownKeyPressed)  // Vérifier également si la touche bas n'est pas maintenue enfoncée
                    {
                        int lowestPieceRow = CalculateLowestPieceRow(pieces[selectedPieceIndex]);

                        pieces[selectedPieceIndex].Y += pieces[selectedPieceIndex].Size;

                        if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                        {
                            pieces[selectedPieceIndex].Y -= pieces[selectedPieceIndex].Size;
                            pieceLocked = true;
                            GlobalSettings.PlacePiece(pieces[selectedPieceIndex]);

                            for (int row = 0; row < 20; row++)
                            {
                                if (GlobalSettings.IsLineComplete(row))
                                {
                                    GlobalSettings.RemoveLine(row);
                                    row--;
                                }
                            }

                            selectedPieceIndex = rnd.Next(0, pieces.Length);
                            pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
                            pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;  // Ensure starting at the top of the play area
                            pieceLocked = false;
                        }
                    }
                }

                // Gestion de l'entrée utilisateur pour déplacer la pièce
                if (Raylib.IsKeyPressed(KeyboardKey.Left))
                {
                    pieces[selectedPieceIndex].X -= pieces[selectedPieceIndex].Size;
                    if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                    {
                        pieces[selectedPieceIndex].X += pieces[selectedPieceIndex].Size;
                    }
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Right))
                {
                    pieces[selectedPieceIndex].X += pieces[selectedPieceIndex].Size;
                    if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                    {
                        pieces[selectedPieceIndex].X -= pieces[selectedPieceIndex].Size;
                    }
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    pieces[selectedPieceIndex].Rotate();
                    if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                    {
                        // Handle out of bounds or overlapping situations after rotation
                        // Reset rotation if it causes issues
                        pieces[selectedPieceIndex].Rotate(); // Rotate back once
                        pieces[selectedPieceIndex].Rotate(); // Rotate back twice
                        pieces[selectedPieceIndex].Rotate(); // Rotate back thrice (original position)
                    }
                }
                else if (Raylib.IsKeyDown(KeyboardKey.Down))
                {
                    // Marquer que la touche bas est enfoncée
                    isDownKeyPressed = true;
                }
                else
                {
                    // Réinitialiser lorsque la touche bas est relâchée
                    isDownKeyPressed = false;
                }

                // Déplacer la pièce vers le bas si la touche bas est enfoncée
                if (isDownKeyPressed)
                {
                    pieces[selectedPieceIndex].Y += pieces[selectedPieceIndex].Size;
                    if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                    {
                        pieces[selectedPieceIndex].Y -= pieces[selectedPieceIndex].Size;
                        pieceLocked = true;
                        GlobalSettings.PlacePiece(pieces[selectedPieceIndex]);

                        for (int row = 0; row < 20; row++)
                        {
                            if (GlobalSettings.IsLineComplete(row))
                            {
                                GlobalSettings.RemoveLine(row);
                                row--;
                            }
                        }

                        selectedPieceIndex = rnd.Next(0, pieces.Length);
                        pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
                        pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;  // Ensure starting at the top of the play area
                        pieceLocked = false;
                        isDownKeyPressed = false;  // Réinitialiser une fois que la pièce est placée
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RayWhite);

                int gridSize = pieces[selectedPieceIndex].Size;

                // Dessin des lignes verticales de la grille
                for (int col = 0; col <= 10; col++)
                {
                    int posX = GlobalSettings.PlayAreaX + col * gridSize;
                    Raylib.DrawRectangle(posX, GlobalSettings.PlayAreaY, 1, GlobalSettings.PlayAreaHeight, Color.Black);
                }

                // Dessin des lignes horizontales de la grille
                for (int row = 0; row <= 20; row++)
                {
                    int posY = GlobalSettings.PlayAreaY + row * gridSize;
                    Raylib.DrawRectangle(GlobalSettings.PlayAreaX, posY, GlobalSettings.PlayAreaWidth, 1, Color.Black);
                }

                // Dessin des pièces déjà placées sur la grille
                for (int row = 0; row < 20; row++)
                {
                    for (int col = 0; col < 10; col++)
                    {
                        if (GlobalSettings.Grid[col, row] == 1)
                        {
                            int posX = GlobalSettings.PlayAreaX + col * gridSize;
                            int posY = GlobalSettings.PlayAreaY + row * gridSize;

                            Raylib.DrawRectangle(posX, posY, gridSize, gridSize, Color.Gray);
                            Raylib.DrawRectangleLines(posX, posY, gridSize, gridSize, Color.Black);
                        }
                    }
                }

                // Dessiner la pièce actuelle
                pieces[selectedPieceIndex].Draw(GlobalSettings.PlayAreaX, GlobalSettings.PlayAreaY);

                // Informations de débogage pour montrer la position de la pièce
                Raylib.DrawText($"Position de la pièce : {pieces[selectedPieceIndex].X}, {pieces[selectedPieceIndex].Y}", 100, 100, 12, Color.Blue);

                Raylib.EndDrawing();
            }

            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
    }

    private static int CalculateLowestPieceRow(Piece piece)
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
