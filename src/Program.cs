using Chippy.Helpers;

namespace Chippy;

class Program
{
    static void Main(string[] args)
    {
        var rom = RomLoader.Load();
        var audioEngine = new AudioEngine();
        var display = new Display();
        var emulator = new Emulator(rom, audioEngine, display);
        
        emulator.Start();
    }
}