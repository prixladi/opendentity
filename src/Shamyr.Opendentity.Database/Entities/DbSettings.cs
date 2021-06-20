namespace Shamyr.Opendentity.Database.Entities
{
    public class DbSettings
    {
        public const string _InitKey = "init";

        public string Key { get; set; } = default!;
        public string? Value { get; set; }
    }
}
