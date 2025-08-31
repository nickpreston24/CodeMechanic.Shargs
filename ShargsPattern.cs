using System.Text.RegularExpressions;
using CodeMechanic.RegularExpressions;

namespace CodeMechanic.Shargs;

public class ShargsPattern : RegexEnumBase
{
    public static ShargsPattern Cli = new ShargsPattern(
        1,
        nameof(Cli),
        @"(?<raw_commands>(([a-zA-Z]+)\s*){1,})|
(?<flag>-+[\w-]+)=?
(\s*(?<value>[a-zA-Z/\d.:?=\s_]+))? # Values after flags",
        uri: "https://regex101.com/r/TyNCZp/9"
    );

    public static ShargsPattern Command = new ShargsPattern(
        2,
        @"^(?<command>(?!-).)*",
        nameof(Command)
    );

    public static ShargsPattern Help = new ShargsPattern(
        3,
        nameof(Help),
        @"(?<raw_text>(?<flag>--?[\w-]+=?)(\s*(?<variable_name>[a-zA-Z/\d:\.]+))?)?",
        "https://regex101.com/r/hGTIO3/1"
    );

    protected ShargsPattern(int id, string name, string pattern,
        string uri = "")
        : base(id, name, pattern, uri)
    {
        this.CompiledRegex = new Regex(
            pattern,
            RegexOptions.Compiled
            | RegexOptions.Multiline
            | RegexOptions.IgnoreCase
            | RegexOptions.IgnorePatternWhitespace
        );
    }
}