# Chippy

Chippy is a [CHIP-8](https://en.wikipedia.org/wiki/CHIP-8) emulator (technically an interpreter...) written in C#.

![Chippy - Chip-8 emulator flags test](https://github.com/user-attachments/assets/38930c75-d8c4-4901-8231-6a3b16336feb)

## Tests

Chippy passes the following tests from the [CHIP-8 test suite](https://github.com/Timendus/chip8-test-suite?tab=readme-ov-file):

- [CHIP-8 Splash Screen](https://github.com/Timendus/chip8-test-suite?tab=readme-ov-file#chip-8-splash-screen)
- [IBM Logo](https://github.com/Timendus/chip8-test-suite#ibm-logo)
- [Corax+ Opcode Test](https://github.com/Timendus/chip8-test-suite#corax-opcode-test)
- [Flags Test](https://github.com/Timendus/chip8-test-suite?tab=readme-ov-file#flags-test)
- [Beep Test](https://github.com/Timendus/chip8-test-suite?tab=readme-ov-file#beep-test)
- [Keypad Test](https://github.com/Timendus/chip8-test-suite#keypad-test)

## Instructions

To run, checkout the source and build. Chippy comes packaged with test ROMs that you can load via the interface on launch. Keyboard input maps to the CHIP-8's keyboard using the following keys:
```
1234
qwer
asdf
zxcv
```

## Screenshots

Chippy passing the Corax+ opcode test:

![Chippy - Passing Corax+ opcode test](https://github.com/user-attachments/assets/fd822186-6501-4946-bf4a-d324df607d8b)
