namespace Chippy;

public class Emulator
{
    private byte[] _memory = new byte[4096];

    // Data registers
    private byte[] _v = new byte[16];
    
    // Address register
    private ushort _i = 0;

    // Program counter, start at program memory.
    private int _pc = 0x200;
    
    private bool[,] _frameBuffer = new bool[32, 64];
    
    public Emulator(string romPath)
    {
        var romStream = File.ReadAllBytes(romPath);
        romStream.CopyTo(_memory, 0x200);
        FontSet.Fonts.CopyTo(_memory, 0);
    }

    public void Cycle()
    {
        var opcode = FetchOpcode();
        Decode(opcode);
    }

    private uint FetchOpcode()
    {
        var opcode = (uint)_memory[_pc] << 8 | _memory[_pc + 1];
        _pc++;
        
        return opcode;
    }

    private void Decode(uint opcode)
    {
        var x = (byte)((opcode & 0x0F00) >> 8);
        var y = (byte)((opcode & 0x00F0) >> 4);
        var n = (byte)(opcode & 0x000F);
        var nn = (byte)(opcode & 0x00FF);
        var nnn = (ushort)(opcode & 0x0FFF);
        
        Console.WriteLine($"x: {x:X4} y: {y:X4} n: {n:X4} nn: {nn:X4} nnn: {nnn:X4}");
    }
}