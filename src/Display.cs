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
        if (Raylib.IsKeyDown(KeyboardKey.One))
        {
            return ConvertKeyToByte(KeyboardKey.One);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Two))
        {
            return ConvertKeyToByte(KeyboardKey.Two);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Three))
        {
            return ConvertKeyToByte(KeyboardKey.Three);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Four))
        {
            return ConvertKeyToByte(KeyboardKey.Four);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Q))
        {
            return ConvertKeyToByte(KeyboardKey.Q);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            return ConvertKeyToByte(KeyboardKey.W);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.E))
        {
            return ConvertKeyToByte(KeyboardKey.E);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.R))
        {
            return ConvertKeyToByte(KeyboardKey.R);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            return ConvertKeyToByte(KeyboardKey.A);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            return ConvertKeyToByte(KeyboardKey.S);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            return ConvertKeyToByte(KeyboardKey.D);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.F))
        {
            return ConvertKeyToByte(KeyboardKey.F);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Z))
        {
            return ConvertKeyToByte(KeyboardKey.Z);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.X))
        {
            return ConvertKeyToByte(KeyboardKey.X);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.C))
        {
            return ConvertKeyToByte(KeyboardKey.C);
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.V))
        {
            return ConvertKeyToByte(KeyboardKey.V);
        }

        return null;
    }

    private static byte? ConvertKeyToByte(KeyboardKey key)
    {
        return key switch
        {
            KeyboardKey.One => 0x1,
            KeyboardKey.Two => 0x2,
            KeyboardKey.Three => 0x3,
            KeyboardKey.Four => 0xC,
            KeyboardKey.Q => 0x4,
            KeyboardKey.W => 0x5,
            KeyboardKey.E => 0x6,
            KeyboardKey.R => 0xD,
            KeyboardKey.A => 0x7,
            KeyboardKey.S => 0x8,
            KeyboardKey.D => 0x9,
            KeyboardKey.F => 0xE,
            KeyboardKey.Z => 0xA,
            KeyboardKey.X => 0x0,
            KeyboardKey.C => 0xB,
            KeyboardKey.V => 0xF,
            _ => null
        };
    }
}