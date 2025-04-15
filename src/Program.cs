using Chippy.Helpers;

namespace Chippy;

class Program
{
    static void Main(string[] args)
    {
        var rom = RomLoader.Load();
        var emulator = new Emulator(rom);
        var display = new Display();

        while (true)
        {
            emulator.Cycle();
            display.Render(emulator.FrameBuffer);
            Thread.Sleep(16);
        }
    }
}