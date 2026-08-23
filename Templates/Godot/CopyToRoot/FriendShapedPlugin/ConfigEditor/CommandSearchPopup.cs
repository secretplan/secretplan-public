namespace FriendShapedPlugin.ConfigEditor;

public partial class CommandSearchPopup : SearchPopup<ConfigEditor.CommandPaletteCommand>
{
    protected override void OnPopupOpened()
    {
    }

    protected override void OnPopupClosed()
    {
    }

    public override void AfterProcess(double delta)
    {
    }

    protected override void OnInitialize(ConfigEditor.CommandPaletteCommand startingKey)
    {
    }

    protected override ConfigEditor.CommandPaletteCommand GetEmptyValue()
    {
        return new ConfigEditor.CommandPaletteCommand(string.Empty, () => { });
    }
}