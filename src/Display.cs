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
            return ConvertKeypressToByte(KeyboardKey.One);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Two))
        {
            return ConvertKeypressToByte(KeyboardKey.Two);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Three))
        {
            return ConvertKeypressToByte(KeyboardKey.Three);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Four))
        {
            return ConvertKeypressToByte(KeyboardKey.Four);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            return ConvertKeypressToByte(KeyboardKey.Q);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.W))
        {
            return ConvertKeypressToByte(KeyboardKey.W);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.E))
        {
            return ConvertKeypressToByte(KeyboardKey.E);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            return ConvertKeypressToByte(KeyboardKey.R);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            return ConvertKeypressToByte(KeyboardKey.A);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            return ConvertKeypressToByte(KeyboardKey.S);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            return ConvertKeypressToByte(KeyboardKey.D);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.F))
        {
            return ConvertKeypressToByte(KeyboardKey.F);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Z))
        {
            return ConvertKeypressToByte(KeyboardKey.Z);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.X))
        {
            return ConvertKeypressToByte(KeyboardKey.X);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.C))
        {
            return ConvertKeypressToByte(KeyboardKey.C);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.V))
        {
            return ConvertKeypressToByte(KeyboardKey.V);
        }

        return null;
    }

    private static byte? ConvertKeypressToByte(KeyboardKey key)
    {
        return key switch
        {
            KeyboardKey.One => 0x0,
            KeyboardKey.Two => 0x1,
            KeyboardKey.Three => 0x2,
            KeyboardKey.Four => 0x3,
            KeyboardKey.Q => 0x4,
            KeyboardKey.W => 0x5,
            KeyboardKey.E => 0x6,
            KeyboardKey.R => 0x7,
            KeyboardKey.A => 0x8,
            KeyboardKey.S => 0x9,
            KeyboardKey.D => 0xA,
            KeyboardKey.F => 0xB,
            KeyboardKey.Z => 0xC,
            KeyboardKey.X => 0xD,
            KeyboardKey.C => 0xE,
            KeyboardKey.V => 0xF,
            _ => null
        };
    }
}