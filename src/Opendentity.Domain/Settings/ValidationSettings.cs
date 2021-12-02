namespace Opendentity.Domain.Settings;

public record ValidationSettings
{
    public int MaxPageSize { get; init; } = 200;

    public int MaxSearchLength { get; init; } = 50;
    public int MinSearchLength { get; init; } = 3;

    public int MaxUserNameLength { get; init; } = 60;
    public int MinUserNameLength { get; init; } = 5;

    public int MaxNameLength { get; init; } = 60;

    public int MinPasswordLength { get; init; } = 5;

    public int MaxEmailLength { get; init; } = 80;
}
