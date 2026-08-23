using DATA_ASSEMBLY.Distributable;
using DATA_ASSEMBLY.DistributableConfig;
using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocStringSearchPopup : SearchPopup<uint>
{
    private readonly CachedNode<RichTextLabel> _currentLocalizedText = new("Sidebar/Stack/CurrentLocalizedTextLabel");
    private readonly CachedNode<RichTextLabel> _currentSlugLabel = new("Sidebar/Stack/CurrentSlugLabel");


    protected override void OnPopupOpened()
    {
    }

    protected override void OnPopupClosed()
    {
    }

    public override void AfterProcess(double delta)
    {
        var viewport = GetViewport();
        var hoveredResult = viewport.GuiGetHoveredControl() as SearchResultButton;
        var focusedResult = viewport.GuiGetFocusOwner() as SearchResultButton;

        var finalResult = hoveredResult ?? focusedResult;
        
        if (finalResult == null)
        {
            return;
        }

        if (finalResult.Value is uint id)
        {
            SetCurrentReference(LocalizationServer.Instance.GetReferenceFromId(id));
        }
    }

    private void SetCurrentReference(LocalizedStringReference reference)
    {
        _currentSlugLabel.Get(this).BbcodeEnabled = true;
        _currentSlugLabel.Get(this).Text = "[color=yellow]"+ reference.Slug + "[/color]";
        _currentLocalizedText.Get(this).Text = reference.TranslatedWithFallbackLocale();
    }

    protected override void OnInitialize(uint startingKey)
    {
        SetCurrentReference(LocalizationServer.Instance.GetReferenceFromId(startingKey));
    }

    protected override uint GetEmptyValue()
    {
        return 0;
    }
}