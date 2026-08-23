using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

/// <summary>
///     Describes a FieldEditor that just has a text field
/// </summary>
public abstract partial class FieldEditorText : FieldEditor
{
    private readonly CachedNode<LineEdit> _lineEdit = new("LineEdit");
    protected string _cachedText = string.Empty;
    protected ConfigField? ConfigField { get; private set; }
    private LineEdit LineEdit => _lineEdit.Get(this);

    public override void Initialize(ConfigField configField)
    {
        ConfigField = configField;
        LineEdit.Text = configField.GetValue()?.ToString();
        _cachedText = LineEdit.Text ?? string.Empty;

        LineEdit.SelectAllOnFocus = true;
        LineEdit.KeepEditingOnTextSubmit = false;

        Initialize2();
    }

    private void OnEditToggled(bool toggledOn)
    {
        if (toggledOn)
        {
            _cachedText = LineEdit.Text;
        }
        else
        {
            OnLineEditSubmitted(LineEdit.Text);
            LineEdit.Text = ConfigField?.GetValue()?.ToString();
        }
    }

    protected void OnTextChanged(string newText)
    {
        if (IsValidText(newText))
        {
            OnLineEditSubmitted(newText);
        }
    }

    public sealed override void _EnterTree()
    {
        // LineEdit.TextSubmitted += OnLineEditSubmitted;
        LineEdit.EditingToggled += OnEditToggled;
        LineEdit.TextChanged += OnTextChanged;

        EnterTree2();
    }


    public sealed override void _ExitTree()
    {
        // LineEdit.TextSubmitted -= OnLineEditSubmitted;
        LineEdit.EditingToggled -= OnEditToggled;
        LineEdit.TextChanged -= OnTextChanged;

        ExitTree2();
    }

    public virtual void EnterTree2()
    {
    }

    public virtual void ExitTree2()
    {
    }

    private void OnLineEditSubmitted(string newText)
    {
        if (IsValidText(newText))
        {
            OnSubmitted(newText);
        }
        else
        {
            LineEdit.Text = _cachedText;
        }
    }

    protected abstract bool IsValidText(string newText);
    protected abstract void OnSubmitted(string newText);
    protected abstract void Initialize2();
}