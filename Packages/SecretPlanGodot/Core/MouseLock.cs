using Godot;

namespace SecretPlanGodot.Core;

public class MouseLock
{
    private readonly HashSet<string> _tokens = new();
    private Input.MouseModeEnum _bottomLevelSetting = Input.MouseModeEnum.Visible;
    private int _freeCount;

    public Input.MouseModeEnum BottomLevelSetting
    {
        get => _bottomLevelSetting;
        set
        {
            _bottomLevelSetting = value;
            OnChanged();
        }
    }

    public Input.MouseModeEnum DesiredState { get; private set; }

    public void LockMouse()
    {
        _freeCount--;
        if (_freeCount < 0)
        {
            _freeCount = 0;
        }
        OnChanged();
    }

    public void FreeMouse()
    {
        _freeCount++;
        OnChanged();
    }

    public void OnChanged()
    {
        if (IsAtBottomLevel())
        {
            DesiredState = BottomLevelSetting;
        }
        else
        {
            DesiredState = Input.MouseModeEnum.Visible;
        }
    }

    public bool IsAtBottomLevel()
    {
        return _freeCount <= 0 && _tokens.Count == 0;
    }

    public bool IsMouseLocked()
    {
        return IsAtBottomLevel() && BottomLevelSetting != Input.MouseModeEnum.Visible;
    }

    public void RequestMouseLock(bool shouldLock)
    {
        if (shouldLock)
        {
            LockMouse();
        }
        else
        {
            FreeMouse();
        }
    }

    public void AddFreeMouseToken(string name)
    {
        _tokens.Add(name);
        OnChanged();
    }

    public void RemoveFreeMouseToken(string name)
    {
        _tokens.Remove(name);
        OnChanged();
    }
}