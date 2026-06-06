using System.Text;
using FuzzySharp;

namespace SecretPlanCore.Core;

public class FuzzyUtilities
{
    private static string Normalize(string input)
    {
        var stringBuilder = new StringBuilder();
        foreach (var character in input)
        {
            if (character is '_' or '-' or '/' or '\\' or '.')
            {
                // any of above symbols becomes space
                stringBuilder.Append(' ');
            }
            else if (char.IsUpper(character))
            {
                // capital letters get prepended with a space and become lower ("ThisConfig" becomes "this config")
                stringBuilder.Append(' ');
                stringBuilder.Append(char.ToLowerInvariant(character));
            }
            else
            {
                stringBuilder.Append(character);
            }
        }
        
        return stringBuilder.ToString().Trim();
    }

    public static IEnumerable<T> Rank<T>(
        string query,
        IEnumerable<T> items,
        Func<T, string> dataToString,
        int minScore = 50)
    {
        return items
            .Select(item =>
            {
                var entry = new Entry(dataToString(item));
                return (Entry: entry, Score: Score(query, entry), Data: item);
            })
            .Where(x => x.Score >= minScore)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Data);
    }

    private static int Score(string query, Entry entry)
    {
        query = Normalize(query);

        if (string.IsNullOrEmpty(query))
        {
            return 0;
        }

        var score = 0;

        // 1. Exact / prefix match (dominant)
        if (entry.Normalized.StartsWith(query))
        {
            score += 100;
        }

        // 2. Token prefix matches
        foreach (var token in entry.Tokens)
        {
            if (token.StartsWith(query))
            {
                score += 60;
            }
        }

        // 3. Initialism (very important)
        score += Fuzz.Ratio(query, entry.Initialism) * 2;

        // 4. Partial ratio (core fuzzy feel)
        score += Fuzz.PartialRatio(query, entry.Normalized);

        // 5. Token set (order independent)
        score += Fuzz.TokenSetRatio(query, entry.Normalized);

        // 6. Typo tolerance
        score += Fuzz.Ratio(query, entry.Normalized) / 2;

        // 7. Length penalty (prefer shorter)
        score -= entry.Normalized.Length;

        return score;
    }


    private struct Entry
    {
        public Entry(string display)
        {
            var normalized = Normalize(display);
            var tokens = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var initialism = string.Concat(tokens.Select(t => t[0]));

            Display = display;
            Initialism = initialism;
            Normalized = normalized;
            Tokens = tokens;
        }

        public string Display { get; }
        public string Normalized { get; }
        public string[] Tokens { get; }
        public string Initialism { get; }
    }
}