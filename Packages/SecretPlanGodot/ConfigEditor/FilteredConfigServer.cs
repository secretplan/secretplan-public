using System;
using System.Collections.Generic;
using System.Linq;
using SecretPlanCore.Configuration;

namespace BirdGame.Core;

public static class FilteredConfigServer
{
    private static readonly Dictionary<Type, bool> _isExactTypeHiddenCache = new();
    private static readonly HashSet<Type> _hiddenTypes = new();

    public static IEnumerable<Config> GetAllInstances()
    {
        return ConfigServer.Instance.GetAllInstances().Where(instance => !IsTypeHidden(instance.GetType()));
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