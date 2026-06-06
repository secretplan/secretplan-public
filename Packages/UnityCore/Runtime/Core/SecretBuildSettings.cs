using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SecretPlan.Core
{
    [CreateAssetMenu(menuName = "SecretPlan/BuildSettings", fileName = "BuildSettings")]
    public class SecretBuildSettings : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField]
        private string _applicationName = "My Game";

        [SerializeField]
        private List<SceneAsset?> _scenesToBuild = new();

        public string AppName => _applicationName;
        public string[] ScenePathsToBuild =>
            _scenesToBuild.Where(a => a != null).Select(AssetDatabase.GetAssetPath).ToArray();
#endif
    }
}