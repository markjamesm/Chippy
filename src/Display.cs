using Raylib_cs;

namespace Chippy;

public class Display
{
    private const int ScreenWidth = 640;
    private const int ScreenHeight = 320;
    
    public Display()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Chippy - CHIP-8 Emulator");
    }

    public void Render(bool[,] frameBuffer)
    {
        if (Raylib.WindowShouldClose())
        {
            Raylib.CloseWindow();
            Environment.Exit(0);
        }
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        for (var y = 0; y < 32; y++)
        {
            for (var x = 0; x < 64; x++)
            {
                Raylib.DrawRectangle(x * 10, y * 10, 10, 10, 
                                    frameBuffer[y, x] ? Color.Blue : Color.White);
            }
        }

        Raylib.EndDrawing();
    }
}