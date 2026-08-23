using System.Collections.Generic;
using System.Reflection;
using DATA_ASSEMBLY.DistributableConfig;
using Godot;
using SecretPlanCore.Configuration;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ConfigInstancePane : Control
{
    private readonly CachedNode<Button> _closeButton = new("TitleArea/CloseButton");
    private readonly CachedNode<Control> _content = new("FieldsScroll/PaneContent");
    private readonly CachedNode<Button> _duplicateButton = new("TitleArea/DuplicateButton");
    private readonly CachedNode<Button> _renameButton = new("TitleArea/RenameButton");
    private readonly CachedAncestor<ConfigEditor> _rootEditor = new();
    private readonly CachedNode<ConfigInstanceTitleDisplay> _title = new("TitleArea/Title");
    private Config? _instance;

    public void Initialize(Config configInstance)
    {
        _instance = configInstance;
        _title.Get(this).Initialize(configInstance);
        ContentRoot.QueueFreeAllChildren();

        foreach (var field in GetFieldsOfConfig(configInstance))
        {
            var fieldDisplay =
                GodotUtilities.InstantiateScene<NamedFieldDisplay>("res://FriendShapedPlugin/ConfigEditor/Scenes/NamedFieldVertical.tscn");
            fieldDisplay.Initialize(field.HumanReadableName);
            fieldDisplay.AddEditor(ConfigEditor.CreateEditorForConfigField(field));
            ContentRoot.AddChild(fieldDisplay);
        }

        var configLevelCustomEditors = configInstance.GetType().GetCustomAttributes<CustomEditorAttribute>(true);
        foreach (var configLevelCustomEditor in configLevelCustomEditors)
        {
            var cachedPackedScene = configLevelCustomEditor.Scene.CachedPackedScene;
            var scene = cachedPackedScene.LoadAndInstantiate();
            if (scene is not ICanInitializeFromConfig editor || !editor.Initialize(configInstance))
            {
                LocalClient.Error(
                    $"Attempted to use config-level custom editor {cachedPackedScene.Path}, but could not initialize it");

                // since we're not using this scene, free it before we forget about it
                scene.QueueFree();
            }
            else
            {
                ContentRoot.AddChild(scene);
            }
        }
    }

    private Control ContentRoot => _content.Get(this);

    public override void _EnterTree()
    {
        _closeButton.Get(this).Pressed += ClosePane;
        _duplicateButton.Get(this).Pressed += DuplicateButtonPressed;
        _renameButton.Get(this).Pressed += RenameButtonPressed;

        ConfigServer.Instance.EditorConfigChanged += OnConfigChanged;
    }

    public override void _ExitTree()
    {
        _closeButton.Get(this).Pressed -= ClosePane;
        _duplicateButton.Get(this).Pressed -= DuplicateButtonPressed;
        _renameButton.Get(this).Pressed -= RenameButtonPressed;
        
        ConfigServer.Instance.EditorConfigChanged -= OnConfigChanged;
    }

    private void OnConfigChanged(uint uid)
    {
        var instance = ConfigServer.Instance.GetInstanceUntyped(uid);
        if (instance == null)
        {
            ClosePane();
        }
    }

    private void DuplicateButtonPressed()
    {
        if (_instance == null)
        {
            return;
        }

        _rootEditor.GetOrNull(this)?.PromptDuplicateConfig(_instance);
    }
    
    private void RenameButtonPressed()
    {
        if (_instance == null)
        {
            return;
        }

        _rootEditor.GetOrNull(this)?.PromptRenameConfig(_instance);
    }

    private void ClosePane()
    {
        QueueFree();
    }

    private IEnumerable<ConfigField> GetFieldsOfConfig(Config configInstance)
    {
        void OnChange()
        {
            MarkDirty(configInstance);
        }

        return ConfigField.GetFieldsOfObject(configInstance, OnChange);
    }

    private void MarkDirty(Config configInstance)
    {
        _rootEditor.GetOrNull(this)?.MarkDirty(configInstance.Uid());
    }

    /// <summary>
    /// When a config is marked dirty and then saved, we then finish by letting the pane know it's been cleaned.
    /// </summary>
    public void NotifyCleaned()
    {
        foreach (var child in ContentRoot.GetAllDescendants())
        {
            if (child is ICanNotifyOfCleaned cleanable)
            {
                cleanable.OnNotifyOfClean();
            }
        }
    }
}