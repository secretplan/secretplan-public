using System.Linq;
using DATA_ASSEMBLY.DistributableConfig;
using Godot;
using SecretPlan.Generated;
using SecretPlanCore.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocalizationTableEditor : Control, ICanInitializeFromConfig
{
    private readonly CachedNode<GridContainer> _gridContainer = new("GridContainer");
    private readonly CachedAncestor<ConfigEditor> _rootEditor = new();
    private LocalizationRootTable? _rootTable;

    private GridContainer GridContainer => _gridContainer.Get(this);

    public bool Initialize(Config config)
    {
        if (config is not LocalizationRootTable localizationRootTable)
        {
            return false;
        }

        _rootTable = localizationRootTable;

        GridContainer.QueueFreeAllChildren();

        var availableLocales = LocalizationServer.Instance.AvailableLocales(true).ToList();
        GridContainer.Columns = availableLocales.Count + 2;

        // Header
        GridContainer.AddChild(new Label { Text = "Slug" });

        GridContainer.AddChild(new Label { Text = "Id" });

        foreach (var locale in availableLocales)
        {
            GridContainer.AddChild(new Label { Text = locale.ReadOrDefault().LocalizedName });
        }

        // Body
        foreach (var id in localizationRootTable.IdTable.ReadOrDefault().AllIds())
        {
            GridContainer.AddChild(new Label { Text = localizationRootTable.IdTable.ReadOrDefault().GetSlug(id) });

            GridContainer.AddChild(new Label { Text = id.ToString() });

            foreach (var locale in availableLocales)
            {
                GridContainer.AddChild(new Label
                    { Text = LocalizationServer.Instance.GetTranslatedStringFromId(id, locale) });
            }
        }

        return true;
    }

    public override void _EnterTree()
    {
        if (_rootTable != null)
        {
            foreach (var uid in _rootTable.AllRelatedUids())
            {
                _rootEditor.GetOrNull(this)?.MarkDirty(uid);
            }
        }
    }
}