﻿using System.Drawing;
using RandomizerCore.Controllers;
using RandomizerCore.Randomizer.Logic.Options;

namespace MinishCapRandomizerCLI;

internal static class GenericCommands
{
    //Not Null
#pragma warning disable CS8618
    internal static ShufflerController ShufflerController;
#pragma warning restore CS8618
    private static string? _cachedLogicPath;
    private static string? _cachedPatchPath;
    private static bool _strict = false;
    
    internal static void LoadRom(string? path = null)
    {
        Console.Write("Please enter the path to your Minish Cap Rom: ");
        try
        {
            var input = path ?? Console.ReadLine();
            if (input != null) ShufflerController.LoadRom(input);
            Console.WriteLine("ROM Loaded Successfully!");
        }
        catch
        {
            PrintError("Failed to load ROM! Please check your file path and make sure you have read access.");
        }
    }

    internal static void Seed(string? option = null, string? seedStr = null)
    {
        Console.Write("Would you like a random seed or a set seed? (R or S): ");
        var input = option ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            switch (input.ToLowerInvariant())
            {
                case "r":
                    var rand = new Random();
                    var rSeed = rand.Next();
                    ShufflerController.SetRandomizationSeed(rSeed);
                    Console.WriteLine($"Seed {rSeed} set successfully!");
                    break;
                case "s":
                    Console.Write("Please enter the seed you want to use: ");
                    var seedString = seedStr ?? Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(seedString) || !int.TryParse(seedString, out var seed))
                    {
                        PrintError("Invalid seed entered!");
                        break;
                    }
                    
                    ShufflerController.SetRandomizationSeed(seed);
                    Console.WriteLine("Seed set successfully!");
                    break;
                default:
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
            }
        }
        else PrintError("Invalid Input!");
    }

    internal static void LoadLogic(string? logicFile = null)
    {
        Console.Write("Please enter the path to the Logic File you want to use (leave empty to use default logic): ");
        try
        {
            _cachedLogicPath = logicFile ?? Console.ReadLine();
            ShufflerController.LoadLogicFile(_cachedLogicPath);
            Console.WriteLine("Logic file loaded successfully!");
        }
        catch
        {
            PrintError("Failed to load Logic File! Please check your file path and make sure you have read access.");
        }
    }

    internal static void LoadPatch(string? patchFile = null)
    {        
        Console.Write("Please enter the path to the Patch File you want to use (leave empty to use default patch): ");
        _cachedPatchPath = patchFile ?? Console.ReadLine();
        Console.WriteLine("Patch file loaded successfully!");
    }

    internal static void LoadSettings(string? settings = null)
    {
        Console.Write("Please enter the setting string to load: ");
        var input = settings ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(input)) ShufflerController.LoadSettingsFromSettingString(input);
        Console.WriteLine("Settings loaded successfully!");
    }

    // This option is not supported for use by command files, use the settings string option instead
    internal static void Options()
    {
        Console.WriteLine("Options for current logic file:");
        var options = ShufflerController.GetLogicOptions();
        for (var i = 0; i < options.Count; )
        {
            var option = options[i];
            Console.WriteLine($"{++i}) Type: {option.GetOptionUiType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
        }

        Console.Write("Please enter the number of the setting you would like to change, enter \"Exit\" to stop editing, or enter \"List\" to list all of the options again: ");
        var input = Console.ReadLine();
        
        while (string.IsNullOrEmpty(input) || !input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < options.Count; )
                    {
                        var option = options[i];
                        Console.WriteLine($"{++i}) Type: {option.GetOptionUiType()}, Option Name: {option.NiceName}, Setting Type: {option.Type}, Value: {GetOptionValue(option)}");
                    }
                }
                else if (int.TryParse(input, out var num))
                {
                    if (--num >= 0 && num < options.Count)
                    {
                        EditOption(options[num]);
                    }
                    else
                    {
                        PrintError("Number ouf of range!");
                    }
                }
                else
                {
                    if (options.Exists(option => String.Equals(option.Name, input)))
                    {
                        var option = options.Find(option => String.Equals(option.Name, input));
                        if (option != null)
                        {
                            EditOption(option);
                        }
                        else
                        {
                            PrintError($"Unknown Option {input}!");
                        }
                    }
                    else
                    {
                        PrintError($"Unknown Option {input}!");
                    }
                }
            }
            else
            {
                PrintError("Invalid Input!");
            }
            Console.Write("Please enter the number of the setting you would like to change, enter \"Exit\" to stop editing, or enter \"List\" to list all of the options again: ");
            input = Console.ReadLine();
        }
    }

    internal static void Logging(string? option = null, string? verbosityOption = null, string? logPath = null)
    {
        Console.WriteLine("1) Logger verbosity");
        Console.WriteLine("2) Logger output file");
        Console.WriteLine("3) Force publish log transactions using current verbosity and output file");
        Console.Write("Enter the number of the logger option you want to change: ");
        var input = option ?? Console.ReadLine();
        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i))
        {
            PrintError("Invalid Input!");
            return;
        }

        switch (i)
        {
            case 1:
            {
                Console.WriteLine("Note: One logger transaction has many logs: info logs, warning logs, error logs, and exception logs");
                Console.WriteLine("1) Publish all logger transactions");
                Console.WriteLine("2) Publish only transactions that contain errors or warnings");
                Console.Write("Enter the number for your desired verbosity: ");
                input = verbosityOption ?? Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out i) || (i != 1 && i != 2))
                {
                    PrintError("Invalid Input!");
                    return;
                }
                ShufflerController.SetLoggerVerbosity(i == 1);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 2:
            {
                Console.Write("Please enter the path to save logs after randomization: ");
                input = logPath ?? Console.ReadLine();
                if (input != null) ShufflerController.SetLogOutputPath(input);
                Console.WriteLine("Logger updated successfully!");
                break;
            }
            case 3:
                Console.WriteLine(ShufflerController.PublishLogs());
                break;
            default:
                PrintError("Invalid Input!");
                return;
        }
    }

    internal static bool Randomize(string? randomizationAttempts = null)
    {
        Console.Write("How many times would you like the randomizer to attempt to generate a new seed if randomization fails? ");
        var attemptsStr = randomizationAttempts ?? Console.ReadLine();
        if (!string.IsNullOrEmpty(attemptsStr) || !int.TryParse(attemptsStr, out var attempts) || attempts <= 0) attempts = 1;

        ShufflerController.LoadLocations(_cachedLogicPath);
        var result = ShufflerController.Randomize(attempts);
        if (result.WasSuccessful)
        {
            Console.WriteLine("Randomization successful!");
            return true;
        }

        PrintError($"Randomization failed! Error: {result.ErrorMessage}");
        return false;
    }

    internal static void SaveRom(string? output = null)
    {        
        Console.Write("Please enter the path to save the ROM (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            ShufflerController.SaveAndPatchRom(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-ROM.gba" : input);
            Console.WriteLine("Rom saved successfully!");
        }
        catch
        {
            PrintError("Failed to save ROM! Please check your file path and make sure you have write access.");
        }
    }

    internal static void SaveSpoiler(string? output = null)
    {        
        Console.Write("Please enter the path to save the spoiler (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            ShufflerController.SaveSpoiler(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Spoiler.txt" : input);
            Console.WriteLine("Spoiler saved successfully!");
        }
        catch
        {
            PrintError("Failed to save spoiler! Please check your file path and make sure you have write access.");
        }
    }

    internal static void SavePatch(string? output = null)
    {
        Console.Write("Please enter the path to save the patch (blank for default): ");
        try
        {
            var input = output ?? Console.ReadLine();
            ShufflerController.CreatePatch(string.IsNullOrEmpty(input) ? $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}MinishRandomizer-Patch.bps" : input, _cachedPatchPath);
            Console.WriteLine("Patch saved successfully!");
        }
        catch
        {
            PrintError("Failed to save patch! Please check your file path and make sure you have write access.");
        }
    }

    internal static void GetSettingString()
    {
        Console.WriteLine("Setting String:");
        Console.WriteLine(ShufflerController.GetSettingsString());
    }

    internal static void PatchRom()
    {

        Console.Write("Please enter the path of the rom patch: ");
        var patch = Console.ReadLine();
        
        Console.Write("Please enter the path to save the ROM to: ");
        var rom = Console.ReadLine();

        if (rom == null || patch == null)
        {
            Console.WriteLine("Failed to save patch! Please check your file paths and make sure you have read/write access.");
        }
        else
        {
            var result = ShufflerController.SaveRomPatch(patch, rom);

            if(result)
            {
                Console.WriteLine("Rom patched successfully!");
            }
            else
            {
                PrintError("Failed to patch ROM! Please check your file paths and make sure you have read/write access. "+result.ErrorMessage);
            }
        }
    }

    internal static void CreatePatch()
    {
        Console.Write("Please enter the path of the patched rom: ");
        var rom = Console.ReadLine();
            
        Console.Write("Please enter the path to save the patch to: ");
        var patch = Console.ReadLine();

        if (rom == null || patch == null)
        {
            Console.WriteLine("Failed to save patch! Please check your file paths and make sure you have read/write access.");
        }
        else
        {
            var result = ShufflerController.SaveRomPatch(patch, rom);

            if(result)
            {
                Console.WriteLine("Patch saved successfully!");
            }
            else
            {
                PrintError("Failed to save patch! Please check your file paths and make sure you have read/write access. "+result.ErrorMessage);
            }
        }
    }

    internal static void Strict()
    {
        _strict = !_strict;
        Console.WriteLine($"Toggled strict mode {(_strict ? "on" : "off")}");
    }

    internal static string GetOptionValue(LogicOptionBase option)
    {
        switch (option)
        {
            case LogicFlag:
            {
                return option.Active.ToString();
            }
            case LogicDropdown dropdown:
            {
                return dropdown.Selection;
            }
            case LogicColorPicker colorPicker:
            {
                return colorPicker.Active ? colorPicker.DefinedColor.ToString() : "Vanilla";
            }
            case LogicNumberBox box:
            {
                return box.Value;
            }
        }
        return "";
    }

    internal static void EditOption(LogicOptionBase option)
    {
        switch (option)
        {
            case LogicFlag:
            {
                Console.WriteLine("1) Enabled");
                Console.WriteLine("2) Disabled");
                Console.Write("Enter the number of the option to set the flag to: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || (i != 0 && i != 1 && i != 2))
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                option.Active = i == 1;
                Console.WriteLine("Flag set successfully!");
                break;
            }
            case LogicDropdown dropdown:
            {
                var keys = dropdown.Selections.Keys.ToList();
                for (var i = 0; i < keys.Count; )
                {
                    var selection = keys[i];
                    Console.WriteLine($"{++i}) {selection}");
                }
                
                Console.Write("Enter the number of the option you want for the dropdown: ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var o) || o < 1 || o > keys.Count)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                dropdown.Selection = keys[o - 1];
                Console.WriteLine("Dropdown option set successfully!");
                break;
            }
            case LogicColorPicker colorPicker:
            {
                Console.WriteLine("1) Use Vanilla Color");
                Console.WriteLine("2) Use Default Color");
                Console.WriteLine("3) Use Random Color");
                Console.WriteLine("4) Enter ARGB Color Code");
                Console.Write("Enter the number of the option you want for the color picker: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i is < 1 or > 4)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                switch (i)
                {
                    case 1:
                        colorPicker.Active = false;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 2:
                        colorPicker.Active = true;
                        colorPicker.DefinedColor = colorPicker.BaseColor;
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 3:
                        colorPicker.Active = true;
                        colorPicker.PickRandomColor();
                        Console.WriteLine("Color set successfully!");
                        break;
                    case 4:
                        Console.Write("Please enter the ARGB color string you wish to use: ");
                        var argb = Console.ReadLine();
                        if (string.IsNullOrEmpty(argb) || !int.TryParse(argb, out var color))
                        {
                            if (!argb.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                PrintError("Invalid Input!");
                            }
                            break;
                        }

                        colorPicker.Active = true;
                        colorPicker.DefinedColor = Color.FromArgb(color);
                        Console.WriteLine("Color set successfully!");
                        break;
                }
                break;
            }
            case LogicNumberBox box:
            {
                Console.Write($"Please enter a number from {box.MinValue} to {box.MaxValue} for {box.NiceName}: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var i) || i < box.MinValue || i > box.MaxValue)
                {
                    if (!input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        PrintError("Invalid Input!");
                    }
                    break;
                }

                box.Value = input;
                Console.WriteLine("Number box value set successfully!");
                break;
            }
        }
    }

    internal static void PrintError(string msg)
    {
        Console.Error.WriteLine(msg);
        if(_strict)
        {
            Environment.Exit(1);
        }
    }
}
