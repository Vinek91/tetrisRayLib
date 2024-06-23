// InputHandler.cs
using Raylib_cs;

namespace Tetris.src.Tetris.Helpers
{
    public static class InputHandler
    {
        public static void HandleTextInput(ref string playerName)
        {
            int key = Raylib.GetCharPressed();

            if (key >= 32 && key <= 125 && playerName.Length < 10) // Allowable character range and length check
            {
                playerName += (char)key;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Backspace)) // Backspace key (259 is the Raylib representation for backspace)
            {
                if (playerName.Length > 0)
                    playerName = playerName.Remove(playerName.Length - 1);
            }
        }
    }
}
