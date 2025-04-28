namespace Chippy;

public static class RomLoader
{
    public static string Load()
    {
        var romDir = GetRomDirectory();
        var romPath = GetRomPath(romDir);

        if (!File.Exists(romPath))
        {
            Console.WriteLine("Error: Failed to load rom. Exiting...");
            Environment.Exit(1);
        }
        
        return romPath;
    }
    
    private static string GetRomDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var romsDirectory = Path.Join(currentDirectory, "/roms/");

        return romsDirectory;
    }

    private static string GetRomPath(string romDirectory)
    {
        while (true)
        {
            Console.WriteLine("Select ROM to load:\n");
            Console.WriteLine("1 - CHIP-8 Splash Screen");
            Console.WriteLine("2 - IBM Logo");
            Console.WriteLine("3 - Corax+ Opcode Test");
            Console.WriteLine("4 - Flags Test");
            Console.WriteLine("5 - Beep Test");
            Console.WriteLine("6 - Exit Chippy\n");
            Console.Write("Enter your choice: ");

            var romChoice = Console.ReadLine();

            switch (romChoice)
            {
                case "1":
                    return romDirectory + "chip8-logo.ch8";
                case "2":
                    return romDirectory + "ibm-logo.ch8";
                case "3":
                    return romDirectory + "corax+.ch8";
                case "4":
                    return romDirectory + "flags.ch8";
                case "5":
                    return romDirectory + "beep-test.ch8";
                case "6":
                    Console.WriteLine("Exiting Chippy...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid input. Try again.");
                    Console.WriteLine("");
                    break;
            }
        }
    }
}