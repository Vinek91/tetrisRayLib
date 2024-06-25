// Scoreboard.cs
using System.Collections.Generic;
using Raylib_cs;
using static System.Net.Mime.MediaTypeNames;

namespace Tetris.src.Tetris.Helpers
{
    public static class Scoreboard
    {
        public static void DrawHighScores(List<Player> players)
        {
            int yPosition = 400; // Start Y position for the first score entry
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
        public static void SaveScoresToFile(List<Player> players)
        {
            // Créer une chaîne pour stocker les scores
            List<string> scoreLines = new List<string>();

            // Ajouter chaque joueur sous forme de ligne dans la liste
            foreach (var player in players)
            {
                string playerNameScore = $"{player.Name}: {player.Score}";
                string playerLevel = $"Level ({player.Level})";
                string scoreLine = $"{playerNameScore} | {playerLevel}";
                scoreLines.Add(scoreLine);
            }

            // Obtenir le chemin du répertoire de la solution et ajouter le chemin relatif vers le dossier Data
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));
            string dataDir = Path.Combine(projectDir, "Data");

            // Assurez-vous que le dossier Data existe
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            // Nom du fichier avec le chemin complet
            string fileName = Path.Combine(dataDir, "scores.txt");

            // Utiliser StreamWriter pour écrire les scores dans le fichier
            using (StreamWriter sw = new StreamWriter(fileName, false)) // false pour écraser le fichier existant
            {
                foreach (var scoreLine in scoreLines)
                {
                    sw.WriteLine(scoreLine);
                }
            }
        }

        public static List<Player> LoadScoresFromFile()
        {
            List<Player> players = new List<Player>();

            // Obtenir le chemin du répertoire de la solution et ajouter le chemin relatif vers le dossier Data
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));
            string dataDir = Path.Combine(projectDir, "Data");
            string fileName = Path.Combine(dataDir, "scores.txt");

            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        string[] nameScorePart = parts[0].Split(':');
                        string[] levelPart = parts[1].Split('(');
                        if (nameScorePart.Length == 2 && levelPart.Length == 2)
                        {
                            string name = nameScorePart[0].Trim();
                            int score = int.Parse(nameScorePart[1].Trim());
                            int level = int.Parse(levelPart[1].Trim(' ', ')'));

                            players.Add(new Player { Name = name, Score = score, Level = level });
                        }
                    }
                }

                players = players.OrderByDescending(p => p.Score).Take(5).ToList();
            }

            return players;
        }

    }
}
