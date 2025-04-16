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
    
    private Stack<int> _stack = new(16);
    
    private const int ProgramStart = 0x200;
    private const int FontStart = 0;
    
    public bool[,] FrameBuffer { get; private set; } = new bool[32, 64];
    
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
        var opcode = (uint)(_memory[_pc] << 8 | _memory[_pc + 1]);
        _pc += 2;
        
        return opcode;
    }

    private void Decode(uint opcode)
    {
        // x & y refer to registers
        var x = (byte)((opcode & 0x0F00) >> 8);
        var y = (byte)((opcode & 0x00F0) >> 4);
        
        // n: hex bite, nn: hex nibble
        var n = (byte)(opcode & 0x000F);
        var nn = (byte)(opcode & 0x00FF);
        
        // nnn refers to a hexadecimal memory address.
        var nnn = (ushort)(opcode & 0x0FFF);

        switch (opcode & 0xF000)
        {
            case 0x0000:
                Decode0(opcode);
                break;
            case 0x1000:
                Execute1Nnn(nnn);
                break;
            case 0x2000:
                Execute2Nnn(nnn);
                break;
            case 0x3000:
                Execute3Xnn(x, nn);
                break;
            case 0x4000:
                Execute4Xnn(x, nn);
                break;
            case 0x5000:
                Execute5Xy0(x, y);
                break;
            case 0x6000:
                Execute6Xnn(x, nn);
                break;
            case 0x7000:
                Execute7Xnn(x, nn);
                break;
            case 0x8000:
                Decode8Xy0(opcode);
                break;
            case 0xA000:
                ExecuteAnnn(nnn);
                break;
            case 0xD000:
                ExecuteDxyn(x, y, n);
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
            case 0x00EE:
                Execute00Ee();
                break;
        }
    }

    private void Decode8Xy0(uint opcode)
    {
        switch (opcode & 0x000F)
        {
            case 0x0000:
                break;
            case 0x0001:
                break;
            case 0x0002:
                break;
            case 0x0003:
                break;
            case 0x0004:
                break;
            case 0x0005:
                break;
            case 0x0006:
                break;
            case 0x0007:
                break;
            case 0x000E:
                break;
        }
    }

    private void Execute00E0()
    {
        FrameBuffer = new bool[32, 64];
    }

    private void Execute00Ee()
    {
        _pc = _stack.Pop();
    }

    private void Execute1Nnn(ushort nnn)
    {
        _pc = nnn;
    }

    private void Execute2Nnn(ushort nnn)
    {
        _stack.Push(_pc);
        _pc = nnn;
    }

    private void Execute3Xnn(byte x, ushort nn)
    {
        if (_v[x] == nn)
        {
            _pc += 2;
        }
    }

    private void Execute4Xnn(byte x, ushort nn)
    {
        if (_v[x] != nn)
        {
            _pc += 2;
        }
    }

    private void Execute5Xy0(byte x, byte y)
    {
        if (_v[x] == _v[y])
        {
            _pc += 2;
        }
    }

    private void Execute6Xnn(byte x, byte nn)
    {
        _v[x] = nn;
    }

    private void Execute7Xnn(byte x, byte nn)
    {
        _v[x] += nn;
    }

    private void ExecuteAnnn(ushort nnn)
    {
        _i = nnn;
    }

    private void ExecuteDxyn(byte x, byte y, byte n)
    {
        // Modulo to wrap coords rather than clip on screen
        var xCoord = _v[x] % 64;
        var yCoord = _v[y] % 32;

        _v[0xF] = 0;
        
        for (var row = 0; row < n; row++)
        {
            if (row + yCoord >= 32)
            {
                break;
            }
            
            var spriteByte = _memory[_i + row];

            // CHIP-8 sprites are always a byte, so we can always
            // assume 8 bits per row.
            for (var col = 0; col < 8; col++)
            {
                if (col + xCoord >= 64)
                {
                    break;
                }

                var currentPixel = (spriteByte & (0x80 >> col)) != 0;

                if (!currentPixel)
                {
                    continue;
                }
                
                var screenPixel = FrameBuffer[yCoord + row, xCoord + col];

                if (currentPixel && screenPixel)
                {
                    _v[0xF] = 1;
                }

                // XOR does the same thing as 
                // Framebuffer[xCoord + col, yCoord + row] = !Framebuffer[xCoord + col, yCoord + row];
                FrameBuffer[yCoord + row, xCoord + col] ^= true;
            }
        }
    }
}