using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SecretPlan.Core.Editor
{
    public class SceneBookmarksWindow : EditorWindow
    {
        private SceneAsset? _requestedScene;

        private void OnGUI()
        {
            HandleRequests();

            var sceneBookmarkConfig = ScriptableObjectUtility.LoadSingletonScriptableObject<SceneBookmarkConfig>();

            if (sceneBookmarkConfig == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();

            foreach (var sceneAsset in sceneBookmarkConfig.Scenes)
            {
                if (sceneAsset == null)
                {
                    continue;
                }

                if (GUILayout.Button(sceneAsset.name))
                {
                    _requestedScene = sceneAsset;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void HandleRequests()
        {
            if (_requestedScene.IsNotNull())
            {
                if (!Application.isPlaying)
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_requestedScene));
                }
                else
                {
                    var address = AddressableUtilities.GetAddress(_requestedScene);
                    if (address != null)
                    {
                        SceneTransition.LoadScene(address);
                    }
                    else
                    {
                        Debug.Log($"Could not find address of {_requestedScene}, loading without addressables");
                        SceneManager.LoadScene(_requestedScene.name);
                    }
                }
            }

            _requestedScene = null;
        }

        [MenuItem("SecretPlan/Scene Bookmarks")]
        public static void ShowWindow()
        {
            GetWindow<SceneBookmarksWindow>("Scene Bookmarks");
        }
    }
}