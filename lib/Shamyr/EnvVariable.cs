using System;

namespace Shamyr
{
    public static class EnvVariable
    {
        public static string Get(string variableName)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            return Environment.GetEnvironmentVariable(variableName)
              ?? throw new InvalidOperationException($"Variable with name '{variableName}' doesn't exist in current environment.");
        }

        public static T Get<T>(string variableName, Func<string, T> transform)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value is null)
                throw new InvalidOperationException($"Variable with name '{variableName}' doesn't exist in current environment.");

            return transform(value);
        }

        public static int GetInt(string variableName)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value is null)
                throw new InvalidOperationException($"Variable with name '{variableName}' doesn't exist in current environment.");

            return int.Parse(value);
        }

        public static TimeSpan GetTimeSpan(string variableName)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value is null)
                throw new InvalidOperationException($"Variable with name '{variableName}' doesn't exist in current environment.");

            return TimeSpan.Parse(value);
        }

        public static string? TryGet(string variableName)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            return Environment.GetEnvironmentVariable(variableName);
        }

        public static string TryGet(string variableName, string defaultValue)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            return Environment.GetEnvironmentVariable(variableName) ?? defaultValue;
        }

        public static T? TryGet<T>(string variableName, Func<string, T> transform)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value == null)
                return default;

            return transform(value);
        }

        public static T TryGet<T>(string variableName, Func<string, T> transform, T defaultValue)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value == null)
                return defaultValue;

            return transform(value);
        }

        public static TimeSpan? TryGetTimeSpan(string variableName)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value is null)
                return null;

            return TimeSpan.Parse(value);
        }

        public static TimeSpan TryGetTimeSpan(string variableName, TimeSpan defaultValue)
        {
            if (variableName is null)
                throw new ArgumentNullException(nameof(variableName));

            var value = Environment.GetEnvironmentVariable(variableName);
            if (value is null)
                return defaultValue;

            return TimeSpan.Parse(value);
        }
    }
}
