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
    
    public static byte? ReadKeys()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.One))
        {
            return ConvertKeypressToScancode(KeyboardKey.One);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Two))
        {
            return ConvertKeypressToScancode(KeyboardKey.Two);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Three))
        {
            return ConvertKeypressToScancode(KeyboardKey.Three);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Four))
        {
            return ConvertKeypressToScancode(KeyboardKey.Four);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            return ConvertKeypressToScancode(KeyboardKey.Q);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.W))
        {
            return ConvertKeypressToScancode(KeyboardKey.W);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.E))
        {
            return ConvertKeypressToScancode(KeyboardKey.E);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            return ConvertKeypressToScancode(KeyboardKey.R);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            return ConvertKeypressToScancode(KeyboardKey.A);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            return ConvertKeypressToScancode(KeyboardKey.S);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            return ConvertKeypressToScancode(KeyboardKey.D);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.F))
        {
            return ConvertKeypressToScancode(KeyboardKey.F);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Z))
        {
            return ConvertKeypressToScancode(KeyboardKey.Z);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.X))
        {
            return ConvertKeypressToScancode(KeyboardKey.X);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.C))
        {
            return ConvertKeypressToScancode(KeyboardKey.C);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.V))
        {
            return ConvertKeypressToScancode(KeyboardKey.V);
        }

        return null;
    }

    private static byte ConvertKeypressToScancode(KeyboardKey key)
    {
        return key switch
        {
            KeyboardKey.One => 0x02,
            KeyboardKey.Two => 0x03,
            _ => 0x0
        };
    }
}