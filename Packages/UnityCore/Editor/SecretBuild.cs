using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SecretPlan.Core.Editor
{
    public static class SecretBuild
    {
        [MenuItem("Build/Build Game With Addressables")]
        public static void BuildGameWithAddressables()
        {
            // to run in command line:
            // /Path/To/Unity -batchmode -quit -projectPath "$(pwd)" -executeMethod BuildScript.BuildGameWithAddressables  BuildTarget=StandaloneOSX

            var buildFolder = ".build";
            var buildTargetString = GetCommandLineArg("BuildTarget");
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (!string.IsNullOrEmpty(buildTargetString))
            {
                if (!Enum.TryParse(buildTargetString, out buildTarget))
                {
                    Debug.LogError($"Invalid BuildTarget: {buildTargetString}");
                    return;
                }
            }

            var buildSettings = ScriptableObjectUtility.LoadSingletonScriptableObject<SecretBuildSettings>();

            if (buildSettings == null)
            {
                Debug.LogError("No build settings, please create one");
                return;
            }

            if (!Directory.Exists(buildFolder))
            {
                Directory.CreateDirectory(buildFolder);
            }

            BuildAddressables();

            BuildPlayer(buildFolder, buildTarget, buildSettings);
        }

        private static void BuildAddressables()
        {
            Debug.Log("Building Addressables...");
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
            Debug.Log("Addressables build complete.");
        }

        private static void BuildPlayer(string outputFolder, BuildTarget target, SecretBuildSettings buildSettings)
        {
            Debug.Log("Building game...");

            var scenes = buildSettings.ScenePathsToBuild;

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = Path.Combine(outputFolder, GetExecutableName(buildSettings.AppName, target)),
                target = target,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize} bytes at {outputFolder}");
            }
            else
            {
                Debug.LogError($"Build failed: {summary.result}");
            }
        }

        private static string? GetCommandLineArg(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (arg.StartsWith(name + "=", StringComparison.OrdinalIgnoreCase))
                {
                    return arg.Substring(name.Length + 1);
                }
            }

            return null;
        }

        private static string GetExecutableName(string appName, BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return $"{appName}.exe";
                case BuildTarget.StandaloneOSX:
                    return $"{appName}.app";
                case BuildTarget.StandaloneLinux64:
                    return $"{appName}.x86_64";
                default:
                    return "BuildOutput";
            }
        }
    }
}