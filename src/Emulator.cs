namespace Chippy;

public class Emulator
{
    private readonly byte[] _memory = new byte[4096];

    // Data registers
    private readonly byte[] _v = new byte[16];

    // Address register
    private ushort _i;

    // Program counter, start at program memory.
    private int _pc = 0x200;

    private readonly Stack<int> _stack = new(16);
    private bool[,] _frameBuffer = new bool[32, 64];

    private byte _soundTimer;
    private byte _delayTimer;

    private byte? _currentKeypress;
    private byte? _currentKeyReleased;

    private readonly AudioEngine _audioEngine;
    private readonly Display _display;

    private const int ProgramStart = 0x200;
    private const int FontStart = 0;

    public Emulator(string romPath, AudioEngine audioEngine, Display display)
    {
        _audioEngine = audioEngine;
        _display = display;

        var romStream = File.ReadAllBytes(romPath);
        romStream.CopyTo(_memory, ProgramStart);
        FontSet.Fonts.CopyTo(_memory, FontStart);
    }

    public void Start()
    {
        while (true)
        {
            _currentKeypress = Display.ReadKeyDown();
            _currentKeyReleased = Display.ReadKeyUp();

            if (_delayTimer > 0)
            {
                _delayTimer -= 1;
            }

            if (_soundTimer > 0)
            {
                _soundTimer -= 1;
            }

            switch (_soundTimer)
            {
                case > 0: _audioEngine.Beep(); break;
                case <= 0: _audioEngine.StopBeep(); break;
            }

            // Default to 11 instructions per frame but should
            // make this adjustable 
            for (var i = 0; i < 11; i++)
            {
                Cycle();
            }

            // Fx18 controls the sound timer state at runtime, so the
            // value could change at any time during the instruction loop.
            // it might initiate audio or stop it prematurely. so this
            // check must happen again after the instruction loop and
            // before the delay.
            switch (_soundTimer)
            {
                case > 0: _audioEngine.Beep(); break;
                case <= 0: _audioEngine.StopBeep(); break;
            }

            _display.Render(_frameBuffer);
            _currentKeypress = null;
            _currentKeyReleased = null;
            Thread.Sleep(16);
        }
    }

    private void Cycle()
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

    #region Decode

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
            case 0x0000: Decode0000(opcode); break;
            case 0x1000: Execute1Nnn(nnn); break;
            case 0x2000: Execute2Nnn(nnn); break;
            case 0x3000: Execute3Xnn(x, nn); break;
            case 0x4000: Execute4Xnn(x, nn); break;
            case 0x5000: Execute5Xy0(x, y); break;
            case 0x6000: Execute6Xnn(x, nn); break;
            case 0x7000: Execute7Xnn(x, nn); break;
            case 0x8000: Decode8000(opcode, x, y); break;
            case 0x9000: Execute9Xy0(x, y); break;
            case 0xA000: ExecuteAnnn(nnn); break;
            case 0xB000: ExecuteBnnn(nnn); break;
            case 0xC000: ExecuteCxnn(x, nn); break;
            case 0xD000: ExecuteDxyn(x, y, n); break;
            case 0xE000: DecodeEx00(opcode, x); break;
            case 0xF000: DecodeFx00(opcode, x); break;
        }
    }

    private void Decode0000(uint opcode)
    {
        switch (opcode & 0x0FFF)
        {
            case 0x00E0: Execute00E0(); break;
            case 0x00EE: Execute00Ee(); break;
        }
    }

    private void Decode8000(uint opcode, byte x, byte y)
    {
        switch (opcode & 0x000F)
        {
            case 0x0000: Execute8Xy0(x, y); break;
            case 0x0001: Execute8Xy1(x, y); break;
            case 0x0002: Execute8Xy2(x, y); break;
            case 0x0003: Execute8Xy3(x, y); break;
            case 0x0004: Execute8Xy4(x, y); break;
            case 0x0005: Execute8Xy5(x, y); break;
            case 0x0006: Execute8Xy6(x, y); break;
            case 0x0007: Execute8Xy7(x, y); break;
            case 0x000E: Execute8Xye(x, y); break;
        }
    }

    private void DecodeEx00(uint opcode, byte x)
    {
        switch (opcode & 0x00F0)
        {
            case 0x0090: ExecuteEx9E(x); break;
            case 0x00A0: ExecuteExA1(x); break;
        }
    }

    private void DecodeFx00(uint opcode, byte x)
    {
        switch (opcode & 0x00FF)
        {
            case 0x0007: ExecuteFx07(x); break;
            case 0x000A: ExecuteFx0A(x); break;
            case 0x0015: ExecuteFx15(x); break;
            case 0x0018: ExecuteFx18(x); break;
            case 0x001E: ExecuteFx1E(x); break;
            case 0x0029: ExecuteFx29(x); break;
            case 0x0033: ExecuteFx33(x); break;
            case 0x0055: ExecuteFx55(x); break;
            case 0x0065: ExecuteFx65(x); break;
        }
    }

    #endregion

    #region Execute

    private void Execute00E0() => _frameBuffer = new bool[32, 64];
    private void Execute00Ee() => _pc = _stack.Pop();
    private void Execute1Nnn(ushort nnn) => _pc = nnn;

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

    private void Execute6Xnn(byte x, byte nn) => _v[x] = nn;
    private void Execute7Xnn(byte x, byte nn) => _v[x] += nn;
    private void Execute8Xy0(byte x, byte y) => _v[x] = _v[y];
    private void Execute8Xy1(byte x, byte y) => _v[x] |= _v[y];
    private void Execute8Xy2(byte x, byte y) => _v[x] &= _v[y];
    private void Execute8Xy3(byte x, byte y) => _v[x] ^= _v[y];

    private void Execute8Xy4(byte x, byte y)
    {
        var result = _v[x] + _v[y];

        _v[x] = (byte)result;

        _v[0xF] = result > 255 ? (byte)1 : (byte)0;
    }

    private void Execute8Xy5(byte x, byte y)
    {
        var result = _v[x] - _v[y];
        _v[x] = (byte)result;
        _v[0xF] = result >= 0 ? (byte)1 : (byte)0;
    }

    private void Execute8Xy6(byte x, byte y)
    {
        var bitShiftedOut = (byte)(_v[y] & 0x1);
        _v[x] = (byte)(_v[y] >> 1);
        _v[0xF] = bitShiftedOut;
    }

    private void Execute8Xy7(byte x, byte y)
    {
        var result = _v[y] - _v[x];
        _v[x] = (byte)result;
        _v[0xF] = result >= 0 ? (byte)1 : (byte)0;
    }

    private void Execute8Xye(byte x, byte y)
    {
        var bitShiftedOut = (byte)(_v[y] >> 7);
        _v[x] = (byte)(_v[y] << 1);
        _v[0xF] = bitShiftedOut;
    }

    private void Execute9Xy0(byte x, byte y)
    {
        if (_v[x] != _v[y])
        {
            _pc += 2;
        }
    }

    private void ExecuteAnnn(ushort nnn) => _i = nnn;
    private void ExecuteBnnn(ushort nnn) => _pc = nnn + _v[0];

    private void ExecuteCxnn(byte x, byte nn)
    {
        var random = (byte)Random.Shared.Next(0, 256);
        _v[x] = (byte)(random & nn);
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

                var screenPixel = _frameBuffer[yCoord + row, xCoord + col];

                if (currentPixel && screenPixel)
                {
                    _v[0xF] = 1;
                }

                // XOR does the same thing as 
                // Framebuffer[xCoord + col, yCoord + row] = !Framebuffer[xCoord + col, yCoord + row];
                _frameBuffer[yCoord + row, xCoord + col] ^= true;
            }
        }
    }

    private void ExecuteEx9E(byte x)
    {
        var currentKeyPressed = Display.ReadKeyDown();

        if (currentKeyPressed.HasValue)
        {
            if (currentKeyPressed.Value == _v[x])
            {
                _pc += 2;
            }
        }
    }

    private void ExecuteExA1(byte x)
    {
        var currentKeyPressed = Display.ReadKeyDown();

        if (currentKeyPressed != _v[x])
        {
            _pc += 2;
        }
    }

    private void ExecuteFx07(byte x) => _v[x] = _delayTimer;

    private void ExecuteFx0A(byte x)
    {
        if (!_currentKeypress.HasValue)
        {
            _pc -= 2;
        }

        if (_currentKeypress.HasValue)
        {
            if (_currentKeyReleased.HasValue) {
                _v[x] = _currentKeypress.Value;
            }
        }
    }

    private void ExecuteFx15(byte x) => _delayTimer = _v[x];

    private void ExecuteFx18(byte x)
    {
        _soundTimer = _v[x];
    }

    private void ExecuteFx1E(byte x) => _i += _v[x];

    private void ExecuteFx29(byte x) => _i = _memory[_v[x]];

    private void ExecuteFx33(byte x)
    {
        _memory[_i] = (byte)(_v[x] / 100);
        _memory[_i + 1] = (byte)(_v[x] / 10 % 10);
        _memory[_i + 2] = (byte)(_v[x] % 10);
    }

    // Save registers to memory
    private void ExecuteFx55(byte x)
    {
        for (var register = 0; register <= x; register++)
        {
            _memory[_i + register] = _v[register];
        }
    }

    // Load registers to memory
    private void ExecuteFx65(byte x)
    {
        for (var register = 0; register <= x; register++)
        {
            _v[register] = _memory[_i + register];
        }
    }

    #endregion
}