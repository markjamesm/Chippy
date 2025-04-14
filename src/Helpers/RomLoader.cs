namespace Chippy.Helpers;

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
            Console.WriteLine("2 - Exit Chippy");
            Console.Write("Enter your choice: ");

            var romChoice = Console.ReadLine();

            switch (romChoice)
            {
                case "1":
                    return romDirectory + "IBM-Logo.ch8";
                case "2":
                    Console.WriteLine("Exiting Chippy...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    Console.WriteLine("");
                    break;
            }
        }
    }
}