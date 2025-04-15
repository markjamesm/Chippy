using Chippy.Helpers;

namespace Chippy;

class Program
{
    static void Main(string[] args)
    {
        var rom = RomLoader.Load();
        var emulator = new Emulator(rom);

        while (true)
        {
            emulator.Cycle();
            Thread.Sleep(16);
        }
    }
}