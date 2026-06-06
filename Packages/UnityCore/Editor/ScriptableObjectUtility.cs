using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SecretPlan.Core.Editor
{
    public static class ScriptableObjectUtility
    {
        public static T? LoadSingletonScriptableObject<T>() where T : ScriptableObject
        {
            var bookmarkConfigName = AssetDatabase.FindAssets($"t: {typeof(T).Name}").FirstOrDefault();
            if (bookmarkConfigName == null)
            {
                return null;
            }

            var config =
                AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(bookmarkConfigName));
            
            return config;
        }
    }
}