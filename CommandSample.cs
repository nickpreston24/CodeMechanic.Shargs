using System.Text.RegularExpressions;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Shargs;

public class CommandSample : Enumeration
{
    public string args_line { get; set; } = string.Empty;

    public CommandValidations validations { get; set; } = new();

    public static CommandSample RazorHatGeneration = new CommandSample(
        1,
        nameof(RazorHatGeneration),
        @"--razorhat web -n dummyapp -o /home/nick/Desktop/projects/samples/sharpify -v"
    );

    public static CommandSample GrepSearch = new CommandSample(
        2,
        nameof(GrepSearch),
        @"--grep-search -i ."
    );

    public static CommandSample DotnetRun = new CommandSample(
        3,
        nameof(DotnetRun),
        @"dotnet run --no-self-contained -v q --os linux --no-restore --interactive"
    );

    public static CommandSample DotnetAddSln = new CommandSample(
        4,
        nameof(DotnetAddSln),
        @"--add-sln 3"
    ); //todo: Make this a command after you update the regex to include commands again.

    public static CommandSample Hello = new CommandSample(0, nameof(Hello), @"--hello");

    public static CommandSample DDGR_CLI = new CommandSample(
        5,
        nameof(DDGR_CLI),
        @"ddgr --json -N 10"
    );

    public static CommandSample SnapInstaller = new CommandSample(
        6,
        nameof(SnapInstaller),
        @"snap install --devmode --channel=foo --prefer --name=bar -v q"
    );

    public CommandSample(
        int id,
        string name,
        string argsLine = ""
        //, CommandValidations validations = null
    )
        : base(id, name)
    {
        this.args_line = argsLine;

        var tokens = Regex.Split(argsLine, @"[,\s""'=]");
        var args = Regex.Split(argsLine, @"\s");
        var arguments = new ArgsMap(args);

        var gmix = RegexOptions.Compiled
                   | RegexOptions.IgnoreCase
                   | RegexOptions.Multiline
                   | RegexOptions.IgnorePatternWhitespace;

        validations = new CommandValidations()
        {
            // https://regex101.com/r/IRmjr7/1
            expected_flag_count = Regex.Count(args_line
                , @"(--\w+(-\w+)*|-\w+)"),
            expected_command_count = Regex.Count(args_line
                , @"^((\b(?<!-)\w+(?!-)\b)\s*)+", gmix)
        };

        foreach (var token in tokens)
        {
            bool cmd_exists = arguments.HasCommand(token);
            bool flag_exists = arguments.HasFlag(token);
            bool value_exists = arguments.HasValue(token);

            if (cmd_exists) validations.actual_command_count++;
            if (flag_exists) validations.actual_flag_count++;
            if (value_exists) validations.actual_value_count++;
        }
    }

    public override string ToString()
    {
        return args_line;
    }

    public static List<CommandSample> GetAllCommandSamples()
    {
        var commands = Enumeration.GetAll<CommandSample>().ToList();
        return commands;
    }
}