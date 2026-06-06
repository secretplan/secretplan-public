using System.Collections.Generic;
using UnityEngine;

namespace SecretPlan.Core
{
    public static class GlobalTransform
    {
        private static readonly Dictionary<string, Transform?> _table = new();

        public static Transform Get(string name)
        {
            if (!Application.isPlaying || Application.exitCancellationToken.IsCancellationRequested)
            {
                return null!;
            }

            if (!_table.ContainsKey(name) || _table[name] == null)
            {
                _table[name] = new GameObject($"GlobalTransform[{name}]").transform;
            }

            return _table[name]!;
        }
    }
}