using SecretPlanCore.Configuration;

namespace SecretPlanGodot.ConfigEditor;

public static class FilteredConfigServer
{
    private static readonly List<Func<Config, bool>> _filters = new();
    private static readonly Dictionary<Type, bool> _isExactTypeHiddenCache = new();
    private static readonly HashSet<Type> _hiddenTypes = new();

    /// <summary>
    ///     Filters return true if the config should be allowed, false if it should not
    /// </summary>
    public static void AddFilter(Func<Config, bool> filter)
    {
        _filters.Add(filter);
    }

    public static IEnumerable<Config> GetAllInstances()
    {
        return ConfigServer.Instance.GetAllInstances().Where(instance => !IsTypeHidden(instance.GetType()) && _filters.All(filter => filter(instance)));
    }

    public static IEnumerable<TConfig> GetAllInstancesOfType<TConfig>()
    {
        if (IsTypeHidden(typeof(TConfig)))
        {
            yield break;
        }

        foreach (var config in ConfigServer.Instance.GetAllInstancesOfType<TConfig>())
        {
            yield return config;
        }
    }

    public static bool IsTypeHidden(Type type)
    {
        if (_isExactTypeHiddenCache.TryGetValue(type, out var result))
        {
            return result;
        }

        foreach (var hiddenType in _hiddenTypes)
        {
            if (type.IsAssignableTo(hiddenType))
            {
                _isExactTypeHiddenCache[type] = true;
                return true;
            }
        }

        return false;
    }

    public static void HideType(Type type)
    {
        _hiddenTypes.Add(type);
    }

    public static IEnumerable<string> AllTypeIds()
    {
        foreach (var typeId in ConfigServer.Instance.AllTypeIds())
        {
            var type = ConfigServer.Instance.TypeFromId(typeId);
            if (type == null)
            {
                continue;
            }

            if (IsTypeHidden(type))
            {
                continue;
            }

            yield return typeId;
        }
    }

    public static bool IsTypeIdHidden(string typeId)
    {
        var type = ConfigServer.Instance.TypeFromId(typeId);
        if (type == null)
        {
            return false;
        }

        return _hiddenTypes.Contains(type);
    }
}