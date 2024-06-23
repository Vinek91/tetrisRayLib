// Scoreboard.cs
using System.Collections.Generic;
using Raylib_cs;

namespace Tetris.src.Tetris.Helpers
{
    public static class Scoreboard
    {
        public static void DrawHighScores(List<Player> players)
        {
            int yPosition = 480; // Start Y position for the first score entry
            Raylib.DrawText("Meilleurs Joueurs :", 400, 330, 30, Color.Black);

            foreach (var player in players)
            {
                string playerNameScore = $"{player.Name}: {player.Score} |";
                string playerLevel = $" Level ({player.Level})";

                // Dessiner le nom et le score normalement
                Raylib.DrawText(playerNameScore, 400, yPosition, 20, Color.Black);

                // Dessiner le niveau dans une autre couleur
                Raylib.DrawText(playerLevel, 400 + Raylib.MeasureText(playerNameScore, 20), yPosition, 20, Color.Red);

                yPosition += 30; // Move to the next line for the next player
            }
        }
    }
}
