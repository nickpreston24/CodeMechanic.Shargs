using System.Text.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.RegularExpressions;
using CodeMechanic.Types;

namespace CodeMechanic.Shargs;

public class ArgsMap : IArgsMap
{
    private string flattened_args;
    private readonly string[] args;
    public int Count => this.Arguments.Count;

    string[] IArgsMap.InitialArgs => args;

    public List<Argument> Arguments { get; set; } = new();

    public ArgsMap(params string[] args)
    {
        this.args = args;
        BuildArgsMap(args);
    }

    private void BuildArgsMap(string[] args, bool debug = false)
    {
        flattened_args = string.Join(' ', args);
        // flattened_args.Dump("args");
        if (debug)
            Console.WriteLine("flattened args: " + flattened_args);

        Arguments =
            flattened_args.Extract<Argument>(ShargsPattern.Cli.CompiledRegex);

        // if (this.HasFlag("--debug"))
        // {
        if (debug)
            Arguments.Dump("All Arguments (BuildArgsMap)", ignoreNulls: true);
        //     Arguments.Count.Dump("total arguments");
        // }
    }


    private static Regex flag = new Regex(@"-+\w+");

    // Brute force create the arguments
    // private List<Argument> RawDogCreateArguments(string[] args)
    // {
    //     List<Argument> arguments = new();
    //
    //     foreach (string raw_arg in args)
    //     {
    //         if (flag.IsMatch(raw_arg))
    //     }
    //
    //     return arguments;
    // }

    private Argument GetMatchingCmd(string command_name)
    {
        var matching_command = Arguments
            .FirstOrDefault(a =>
                a.commands
                    .Any(raw_command => raw_command.NotEmpty()
                                        && raw_command.Equals(command_name))
            );

        return matching_command;
    }

    public bool HasCommand(string command_name)
    {
        var matching_commands = GetMatchingCmd(command_name);
        return matching_commands != null;
    }

    public Argument WithCommand(string command_name)
    {
        var matching_commands = GetMatchingCmd(command_name);
        return matching_commands;
    }

    /// <summary>
    /// Usage:
    ///     (var is_files_flag_checked ,var files) = options.Matching("-f", "--files");
    ///
    ///     files.Dump(); // "file1", "file2" ...
    /// </summary>
    public Argument WithFlags(params string[] flags)
    {
        return Arguments
            .FirstOrDefault(a => flags.Any(f => a.Flag == f))
            .ToMaybe()
            .Case(some: (arg) => arg, none: () => new Argument());
    }

    public bool HasFlag(params string[] flags)
    {
        var all_flag_names = Arguments.Where(a => a.Flag.NotEmpty())
            .Select(a => a.Flag).ToArray();

        if (flags.IsNullOrEmpty() || all_flag_names.IsNullOrEmpty())
            return false;

        var flags_found = flags.Any(flag =>
            all_flag_names
                .Any(value => value
                    .Equals(flag, StringComparison.OrdinalIgnoreCase))
        );

        return flags_found;
    }

    public bool HasValue(params string[] values)
    {
        var all_flag_values = Arguments
            .Where(a => a.Value.NotEmpty())
            .Select(a => a.Value)
            .ToArray();

        if (values.IsNullOrEmpty() || all_flag_values.IsNullOrEmpty())
            return false;

        return values.Any(flag =>
            all_flag_values
                .Any(value => value
                    .Equals(flag, StringComparison.OrdinalIgnoreCase))
        );
    }
}

public interface IArgsMap
{
    public bool HasCommand(string command_name);
    public Argument WithCommand(string command_name);
    string[] InitialArgs { get; }

    /// <summary>
    /// Usage:
    ///     (var is_files_flag_checked ,var files) = options.Matching("-f", "--files");
    ///
    ///     files.Dump(); // "file1", "file2" ...
    /// </summary>
    public Argument WithFlags(params string[] flags);

    public bool HasFlag(params string[] flags);
}

public static class ArgumentExtensions
{
    public static (bool item1, int value) ToInt(this Argument arg)
    {
        var (has_flag, raw_value) = arg;
        int value = raw_value.ToInt();
        return (has_flag, value);
    }

    public static ArgsMap ToArgsMap(this string[] args) => new ArgsMap(args);

    public static bool
        HasAllOf(this IArgsMap arguments, params string[] args) =>
        args.All(a =>
            arguments
                .Dump("arguments in " + nameof(ToArgsMap))
                .HasCommand(a.Dump($"has command {a}?"))
            && arguments.HasFlag(a).Dump($"has flag {a}?")
        );

    public static bool
        HasAnyOf(this IArgsMap arguments, params string[] args) =>
        args.Any(a => arguments.HasCommand(a) || arguments.HasFlag(a));
}