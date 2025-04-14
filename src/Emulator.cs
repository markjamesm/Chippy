namespace Chippy;

public class Emulator
{
    private byte[] _memory = new byte[4096];

    // Data registers
    private byte[] _v = new byte[16];
    
    // Address register
    private ushort _i = 0;

    // Program counter
    private int pc = 0;
    
    private bool[,] _frameBuffer = new bool[32, 64];
    
    public Emulator(string romPath)
    {
        var romStream = File.ReadAllBytes(romPath);
        romStream.CopyTo(_memory, 0x200);
        FontSet.Fonts.CopyTo(_memory, 0);
    }
}