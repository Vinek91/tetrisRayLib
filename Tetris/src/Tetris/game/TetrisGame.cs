using Raylib_cs;
using Tetris.src.Tetris.Helpers;
using Tetris.src.Tetris;
using System.Numerics;

namespace Tetris.src.Game
{

    public class TetrisGame
    {
        private Piece[] pieces;
        private Piece[] piecesNext;
        private int[] descentSpeed;
        private bool paused;
        private bool gameOver;
        private int score;
        private int linesDestroyed;
        private bool showTetrisMessage;
        private int tetrisMessageCounter;
        private List<Player> players;

        private Music music;

        private Sound dropSound;
        private Sound moveSound;
        private Sound rotateSound;
        private Sound oneLineSound;
        private Sound fallPieceSound;

        private string mainTheme;

        private Texture2D background;


        public TetrisGame()
        {
            Raylib.InitWindow(800, 700, "Tetris avec Raylib en C#");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(60);

            InitializeGame();


        }

        private void InitializeGame()
        {
            InitializePieces();
            InitializeDescentSpeed();
            InitializeSounds();

            paused = false;
            gameOver = false;
            score = 0;
            linesDestroyed = 0;
            showTetrisMessage = false;
            tetrisMessageCounter = 0;
            players = Scoreboard.LoadScoresFromFile();

            background = Raylib.LoadTexture("assets/img/background.png");
        }

        private void InitializePieces()
        {
            pieces = new Piece[]
            {
                new Piece(Shapes.LShape, Color.Orange, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.JShape, Color.Blue, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.TShape, Color.Purple, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.ZShape, Color.Red, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.SShape, Color.Green, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.IShape, Color.SkyBlue, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.SquareShape, Color.Yellow, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),

            };

            piecesNext = new Piece[]
            {
                new Piece(Shapes.LShape, Color.Orange, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.JShape, Color.Blue, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.TShape, Color.Purple, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.ZShape, Color.Red, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.SShape, Color.Green, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.IShape, Color.SkyBlue, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),
                new Piece(Shapes.SquareShape, Color.Yellow, 30, GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight),

            };
        }

        private void InitializeDescentSpeed()
        {
            descentSpeed = new int[]
            {
                55, 50, 45, 40, 35, 30, 25, 20, 15, 10, 4
            };
        }

        private void InitializeSounds()
        {
            string musicPath = "assets/sound";

            mainTheme = $"{musicPath}/Tetris.mp3";

            dropSound = Raylib.LoadSound($"{musicPath}/drop.wav");
            moveSound = Raylib.LoadSound($"{musicPath}/move.mp3");
            Raylib.SetSoundVolume(moveSound, 0.3f);

            rotateSound = Raylib.LoadSound($"{musicPath}/rotate.mp3");
            Raylib.SetSoundVolume(rotateSound, 0.3f);

            oneLineSound = Raylib.LoadSound($"{musicPath}/oneLine.mp3");
            Raylib.SetSoundVolume(oneLineSound, 0.3f);

            fallPieceSound = Raylib.LoadSound($"{musicPath}/fall.mp3");
            Raylib.SetSoundVolume(fallPieceSound, 0.3f);
        }



        public void Run()
        {

            Music music = Raylib.LoadMusicStream(mainTheme);
            Raylib.PlayMusicStream(music);
            Random rnd = new Random();

            int selectedPieceIndex = rnd.Next(0, pieces.Length);
            int newSelectedPieceIndex = rnd.Next(0, pieces.Length);

            pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
            pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY; // Ensure starting at the top of the play area
            bool pieceLocked = false;

            int initialLevel = 0;

            int currentLevel = initialLevel;

            int framesPerDescent = 0;

            if (currentLevel < descentSpeed.Length)
            {
                framesPerDescent = descentSpeed[currentLevel];
            }
            else
            {
                framesPerDescent = descentSpeed[descentSpeed.Length - 1];
            }

            int frameCounter = 0;
            int totalPiecesPlaced = 0;

            bool isDownKeyPressed = false; // Variable pour suivre l'état de la touche bas
            bool gameOver = false; // Ajouter une variable pour suivre l'état du jeu

            string playerName = string.Empty;
            bool enterName = false;

            Texture2D background = Raylib.LoadTexture($"assets/img/background.png");

            // Dimensions de la texture d'arrière-plan
            float bgWidth = background.Width;
            float bgHeight = background.Height;

            // Facteur de zoom
            float scale = 0.5f; // Exemple : réduire à 50% de la taille d'origine

            // Calculer les dimensions redimensionnées
            float scaledWidth = bgWidth * scale;
            float scaledHeight = bgHeight * scale;

            // Rectangle de destination pour le dessin redimensionné
            Rectangle destRect = new Rectangle(0, 0, scaledWidth, scaledHeight);

            // Rectangle source : dessine toute la texture d'arrière-plan
            Rectangle sourceRect = new Rectangle(0, 0, bgWidth, bgHeight);

            List<Player> players = Scoreboard.LoadScoresFromFile();


            while (!Raylib.WindowShouldClose())
            {

                Raylib.UpdateMusicStream(music);

                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    paused = !paused;  // Toggle pause state on 'P' key press
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                if (!paused)
                {


                    Raylib.DrawTexturePro(background, sourceRect, destRect, new Vector2(0, 0), 0, Color.White);

                    frameCounter++;



                    // Mise à jour du niveau en fonction des lignes détruites
                    currentLevel = initialLevel + (linesDestroyed / 10);

                    // Mise à jour de la vitesse de descente en fonction du niveau actuel

                    if (currentLevel < descentSpeed.Length)
                    {
                        framesPerDescent = descentSpeed[currentLevel];
                    }
                    else
                    {
                        framesPerDescent = descentSpeed[descentSpeed.Length - 1];
                    }

                    if (frameCounter >= framesPerDescent && !gameOver)
                    {
                        frameCounter = 0;

                        if (!pieceLocked && !isDownKeyPressed)  // Vérifier également si la touche bas n'est pas maintenue enfoncée
                        {
                            int lowestPieceRow = PieceUtils.CalculateLowestPieceRow(pieces[selectedPieceIndex]);

                            pieces[selectedPieceIndex].Y += pieces[selectedPieceIndex].Size;

                            if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                            {
                                Raylib.PlaySound(fallPieceSound);
                                totalPiecesPlaced += 1;

                                pieces[selectedPieceIndex].Y -= pieces[selectedPieceIndex].Size;
                                pieceLocked = true;
                                Raylib.PlaySound(dropSound);
                                GlobalSettings.PlacePiece(pieces[selectedPieceIndex]);

                                score += 33;

                                int linesCleared = 0;
                                for (int row = 0; row < 20; row++)
                                {
                                    if (GlobalSettings.IsLineComplete(row))
                                    {
                                        GlobalSettings.RemoveLine(row);
                                        row--;
                                        linesCleared++;
                                    }
                                }
                                linesDestroyed += linesCleared;



                                // Mettre à jour le score en fonction du nombre de lignes complètes
                                switch (linesCleared)
                                {
                                    case 1:
                                        Console.WriteLine("line");
                                        Raylib.PlaySound(oneLineSound);
                                        score += 40;
                                        break;
                                    case 2:
                                        Raylib.PlaySound(oneLineSound);
                                        score += 100;
                                        break;
                                    case 3:
                                        Raylib.PlaySound(oneLineSound);
                                        score += 300;
                                        break;
                                    case 4:
                                        score += 1200;
                                        showTetrisMessage = true; // Set the Tetris flag
                                        tetrisMessageCounter = 180;
                                        break;
                                }

                                selectedPieceIndex = newSelectedPieceIndex;
                                newSelectedPieceIndex = rnd.Next(0, pieces.Length);

                                pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
                                pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;  // Ensure starting at the top of the play area
                                pieceLocked = false;

                                if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                                {
                                    gameOver = true;
                                    enterName = true;
                                }
                            }
                        }
                    }

                    if (Raylib.IsKeyPressed(KeyboardKey.Left) && !gameOver)
                    {
                        Raylib.PlaySound(moveSound);
                        pieces[selectedPieceIndex].X -= pieces[selectedPieceIndex].Size;
                        if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                        {
                            pieces[selectedPieceIndex].X += pieces[selectedPieceIndex].Size;
                        }
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.Right) && !gameOver)
                    {
                        Raylib.PlaySound(moveSound);
                        pieces[selectedPieceIndex].X += pieces[selectedPieceIndex].Size;
                        if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                        {
                            pieces[selectedPieceIndex].X -= pieces[selectedPieceIndex].Size;
                        }
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.Up) && !gameOver)
                    {
                        Raylib.PlaySound(rotateSound);
                        pieces[selectedPieceIndex].Rotate();
                        if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                        {
                            pieces[selectedPieceIndex].Rotate();
                            pieces[selectedPieceIndex].Rotate();
                            pieces[selectedPieceIndex].Rotate();
                        }
                    }
                    else if (Raylib.IsKeyDown(KeyboardKey.Down))
                    {
                        isDownKeyPressed = true;
                    }
                    else
                    {
                        isDownKeyPressed = false;
                    }

                    if (isDownKeyPressed && !gameOver)
                    {
                        pieces[selectedPieceIndex].Y += pieces[selectedPieceIndex].Size;
                        if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                        {

                            Raylib.PlaySound(fallPieceSound);
                            totalPiecesPlaced += 1;
                            pieces[selectedPieceIndex].Y -= pieces[selectedPieceIndex].Size;
                            pieceLocked = true;
                            GlobalSettings.PlacePiece(pieces[selectedPieceIndex]);
                            score += 33;

                            int linesCleared = 0;
                            for (int row = 0; row < 20; row++)
                            {
                                if (GlobalSettings.IsLineComplete(row))
                                {
                                    GlobalSettings.RemoveLine(row);
                                    row--;
                                    linesCleared++;
                                }
                            }

                            linesDestroyed += linesCleared;


                            switch (linesCleared)
                            {
                                case 1:
                                    Console.WriteLine("line");
                                    Raylib.PlaySound(oneLineSound);
                                    score += 40;
                                    break;
                                case 2:
                                    Raylib.PlaySound(oneLineSound);
                                    score += 100;
                                    break;
                                case 3:
                                    Raylib.PlaySound(oneLineSound);
                                    score += 300;
                                    break;
                                case 4:
                                    score += 1200;
                                    showTetrisMessage = true; // Set the Tetris flag
                                    tetrisMessageCounter = 180;
                                    break;
                            }

                            selectedPieceIndex = newSelectedPieceIndex;
                            newSelectedPieceIndex = rnd.Next(0, pieces.Length);
                            pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
                            pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;
                            pieceLocked = false;
                            isDownKeyPressed = false;

                            if (GlobalSettings.IsPieceOutOfBounds(pieces[selectedPieceIndex]))
                            {
                                gameOver = true;
                                enterName = true;
                            }
                        }
                    }
                    bool isFrenchKeyboard = Raylib.IsKeyDown(KeyboardKey.Z) && Raylib.IsKeyDown(KeyboardKey.W);





                    if (gameOver)
                    {
                        Raylib.DrawText("Game Over", 350, 300, 40, Color.Red);
                        Raylib.DrawText($"Score: {score}", 350, 350, 20, Color.Blue);

                        if (enterName)
                        {
                            Raylib.DrawText("Enter your name:", 350, 380, 20, Color.Black);
                            Raylib.DrawRectangle(350, 410, 130, 30, Color.LightGray);
                            Raylib.DrawText(playerName, 355, 415, 20, Color.Black);

                            InputHandler.HandleTextInput(ref playerName);

                            // Vérifier si la touche 'Entrée' est pressée ou 'Espace' pour valider le nom
                            if ((Raylib.IsKeyPressed(KeyboardKey.Enter) || (isFrenchKeyboard && Raylib.IsKeyPressed(KeyboardKey.M))) && playerName.Length > 0)
                            {
                                players.Add(new Player { Name = playerName, Score = score, Level = currentLevel });
                                players.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));

                                if (players.Count > 5)
                                    players = players.Take(5).ToList();

                                playerName = string.Empty;
                                enterName = false;
                            }
                        }
                        else
                        {
                            int buttonWidth = 200;
                            int buttonHeight = 50;
                            int buttonX = (Raylib.GetScreenWidth() - buttonWidth) / 2;
                            int buttonY = 400;

                            Raylib.DrawRectangle(buttonX, buttonY, buttonWidth, buttonHeight, Color.LightGray);
                            Raylib.DrawText("Recommencer", buttonX + 10, buttonY + 10, 20, Color.Black);

                            // Vérifier si le bouton gauche de la souris est cliqué pour recommencer le jeu
                            if (Raylib.IsMouseButtonPressed(MouseButton.Left) || (Raylib.IsKeyPressed(KeyboardKey.Enter)))
                            {
                                Vector2 mousePosition = Raylib.GetMousePosition();
                                if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight)))
                                {
                                    score = 0;
                                    linesDestroyed = 0;
                                    totalPiecesPlaced = 0;
                                    tetrisMessageCounter = 0;
                                    gameOver = false;
                                    Raylib.StopMusicStream(music);
                                    Raylib.PlayMusicStream(music);

                                    GlobalSettings.ClearGrid();
                                    selectedPieceIndex = rnd.Next(0, pieces.Length);
                                    pieces[selectedPieceIndex].X = GlobalSettings.PlayAreaX + (GlobalSettings.PlayAreaWidth / 2) - (pieces[selectedPieceIndex].Size * 2);
                                    pieces[selectedPieceIndex].Y = GlobalSettings.PlayAreaY;
                                }
                            }
                        }
                    }

                    else
                    {
                        int gridSize = pieces[selectedPieceIndex].Size;



                        Color transparentWhite = new Color(255, 255, 255, 178);

                        // Dessiner le rectangle avec la couleur choisie
                        Raylib.DrawRectangle(GlobalSettings.PlayAreaX, GlobalSettings.PlayAreaY,
                                             GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaHeight,
                                             transparentWhite);


                        // Dessiner les lignes du rectangle
                        // Example of drawing thicker lines (adjust thickness as needed)
                        const float lineThickness = 5.0f;
                        Raylib.DrawLineEx(
                            new Vector2(GlobalSettings.PlayAreaX, GlobalSettings.PlayAreaY),
                            new Vector2(GlobalSettings.PlayAreaX + GlobalSettings.PlayAreaWidth, GlobalSettings.PlayAreaY),
                            lineThickness,
                            Color.Pink
                        );
                        Color pieceColor = pieces[selectedPieceIndex].ShapeColor;


                        // Inside the main drawing loop where you draw the game grid
                        for (int row = 0; row < 20; row++)
                        {
                            for (int col = 0; col < 10; col++)
                            {
                                if (GlobalSettings.Grid[col, row] == 1)
                                {
                                    int posX = GlobalSettings.PlayAreaX + col * gridSize;
                                    int posY = GlobalSettings.PlayAreaY + row * gridSize;

                                    // Utilisez GetBlockColor avec les indices corrects
                                    Color blockColor = pieces[selectedPieceIndex].GetBlockColor(col, row);

                                    // Dessine le bloc avec la couleur récupérée
                                    Raylib.DrawRectangle(posX, posY, gridSize, gridSize, blockColor);

                                    // Dessine les contours du bloc
                                    Raylib.DrawRectangleLines(posX, posY, gridSize, gridSize, Color.Black);
                                }
                            }
                        }








                        if (showTetrisMessage)
                        {
                            Raylib.DrawText("Tetris!", 90, 200, 40, Color.Green);
                            tetrisMessageCounter--;
                            if (tetrisMessageCounter <= 0)
                            {
                                showTetrisMessage = false;
                            }
                        }





                        Raylib.DrawRectangle(390, 10, 320, 300, transparentWhite);  // Rectangle blanc autour du texte

                        const float lineThickness2 = 5.0f;
                        Raylib.DrawLineEx(
                            new Vector2(390, 10),
                            new Vector2(390, 10),
                            lineThickness,
                            Color.Pink
                        );

                        Raylib.DrawText($"Level: {currentLevel}", 400, 20, 30, Color.Black);
                        Raylib.DrawText($"Score: {score}", 400, 60, 20, Color.Black);
                        Raylib.DrawText($"Ligne(s) détruite : {linesDestroyed}", 400, 90, 20, Color.Black);
                        Raylib.DrawText($"Total pieces placée : {totalPiecesPlaced}", 400, 120, 20, Color.Black);
                        Raylib.DrawText($"Position de la pièce : {pieces[selectedPieceIndex].X}, {pieces[selectedPieceIndex].Y}", 100, 140, 12, Color.Blue);

                        if (players.Count != 0)
                        {
                            Scoreboard.DrawHighScores(players);
                        }

                        pieces[selectedPieceIndex].Draw(GlobalSettings.PlayAreaX, GlobalSettings.PlayAreaY);

                        Raylib.DrawText($"NEXT", 400, 170, 30, Color.Black);
                        piecesNext[newSelectedPieceIndex].DrawPreview(400, 220);



                    }

                    Raylib.EndDrawing();
                }
                else
                {

                    Raylib.DrawText("Paused", 350, 300, 40, Color.Red);
                    Raylib.EndDrawing();

                }




            }

            Raylib.UnloadMusicStream(music);


            Cleanup();
        
        }


        private void Cleanup()
        {
            Raylib.UnloadTexture(background);
            Raylib.StopMusicStream(music);
            Raylib.UnloadMusicStream(music);

            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
            // Sauvegarde des scores
            if (players.Count > 0)
            {
                Scoreboard.SaveScoresToFile(players);
            }
        }
      }
}