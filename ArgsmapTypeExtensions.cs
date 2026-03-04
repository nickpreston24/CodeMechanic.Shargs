using CodeMechanic.Types;

namespace CodeMechanic.Shargs;

public static class ArgsmapTypeExtensions
{
    /// <summary>
    /// Usage:
    ///     (var is_set, var baz) = argsmap.WithFlags(out int bar, "--foo");
    /// </summary>
    public static (bool is_assigned, int x) WithFlags(this ArgsMap argsmap, out int parsed_value, params string[] flags)
    {
        (bool is_assigned, string raw) = argsmap.WithFlags(flags);
        parsed_value = raw.ToInt(fallback: 0);
        return (is_assigned, parsed_value);
    }

    public static (bool is_assigned, double x) WithFlags(this ArgsMap argsmap, out double parsed, params string[] flags)
    {
        (bool is_assigned, string raw) = argsmap.WithFlags(flags);
        parsed = raw.ToDouble(fallback: 0);
        return (is_assigned, parsed);
    }
}