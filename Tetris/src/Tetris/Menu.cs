using System;
using System.Collections.Generic;
using Raylib_cs;

namespace Tetris.src
{
    public static class Menu
    {
        public static void DrawMainMenu()
        {
            // Main menu code goes here
            // Example:
            Raylib.DrawText("Tetris Game", 320, 100, 40, Color.White);
            Raylib.DrawText("Press Enter to Start", 280, 250, 20, Color.White);
        }

        public static void DrawPauseMenu()
        {
            // Pause menu code goes here
            // Example:
            Raylib.DrawRectangle(300, 200, 200, 100, Color.Gray);
            Raylib.DrawText("Paused", 350, 220, 30, Color.Black);
            Raylib.DrawText("Press P to Resume", 310, 270, 20, Color.Black);
        }

        // Additional menu functionalities can be added as needed
    }
}
