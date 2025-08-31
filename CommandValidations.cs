namespace CodeMechanic.Shargs;

public class CommandValidations
{
    public int expected_flag_count { get; set; } = 0;
    public int expected_command_count { get; set; } = 0;
    public int actual_flag_count { get; set; } = 0;
    public int actual_command_count { get; set; } = 0;

    public int actual_value_count { get; set; } = 0;
    public int expected_value_count { get; set; } = 0;

    public double percent_valid_values =>
        (actual_command_count - expected_command_count) / expected_command_count * 100.00;

    public double percent_valid_commands =>
        (actual_command_count - expected_command_count) / expected_command_count * 100.00;

    public double percent_valid_flags =>
        (actual_flag_count - expected_flag_count) / expected_flag_count * 100.00;
}