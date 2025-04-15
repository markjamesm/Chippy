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
    
    private const int ProgramStart = 0x200;
    private const int FontStart = 0;
    
    public Emulator(string romPath)
    {
        var romStream = File.ReadAllBytes(romPath);
        romStream.CopyTo(_memory, ProgramStart);
        FontSet.Fonts.CopyTo(_memory, FontStart);
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

        switch (opcode & 0xF000)
        {
            case 0x0000:
                Decode0(opcode);
                break;
            case 0x6000:
                Execute6Xnn(x, nn);
                break;
            case 0xA000:
                ExecuteAnnn(nnn);
                break;
        }
    }

    private void Decode0(uint opcode)
    {
        switch (opcode & 0x0FFF)
        {
            case 0x00E0:
                Execute00E0();
                break;
        }
    }

    private void Execute00E0()
    {
        _frameBuffer = new bool[32, 64];
    }

    private void Execute6Xnn(byte x, byte nn)
    {
        _v[x] = nn;
    }

    private void ExecuteAnnn(ushort nnn)
    {
        _i = nnn;
    }
}