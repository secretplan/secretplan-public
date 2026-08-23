using System;
using ExTween;
using FriendShapedDistributable;
using FriendShapedPlugin;
using Godot;
using SecretPlanGodot.Core;
using SecretPlanGodot.Tweenables;

namespace BirdGame.UI;

public partial class LoadingScrim : Control
{
    public const float LongLoadTimeDuration = 10f;
    private bool _hasAppearedOnce;
    private bool _hasWarnedAboutLongLoad;
    private readonly ParentCore _parentCore = new();
    private bool _shouldBeVisible;
    private float _visibleTimer;
    private CoreState CoreState => _parentCore.State(this);

    public bool ShouldBeHidden { get; set; }

    public override void _EnterTree()
    {
        Resized += OnSizeChanged;
    }

    public override void _Ready()
    {
        OnStateChanged(false);
    }


    private void OnSizeChanged()
    {
        if (_shouldBeVisible)
        {
            // rerun self
            OnStateChanged(_shouldBeVisible);
        }
    }

    private void OnStateChanged(bool shouldBeVisible)
    {
        if (shouldBeVisible)
        {
            Visible = true;
            OnAppear();
        }
        else
        {
            if (!Visible)
            {
                // already invisible, no work to do
                return;
            }

            // _multiplex.Clear();
            OnDisappear();
        }
    }

    public override void _Process(double delta)
    {
        if (!Visible)
        {
            if (_hasWarnedAboutLongLoad)
            {
                // This will give us a sense of how long people sit in the loading screen before they break out
                // send telemetry for long load finished
            }

            _visibleTimer = 0f;
            _hasWarnedAboutLongLoad = false;
            return;
        }

        _visibleTimer += (float)delta;
        if (_visibleTimer > LongLoadTimeDuration && !_hasWarnedAboutLongLoad)
        {
            _hasWarnedAboutLongLoad = true;
            LocalClient.Print(
                $"Loading screen lasted for longer than {LongLoadTimeDuration} seconds, numbered flag: {CoreState.LoadingStatus.NumberedFlagValue} named flags: [{string.Join(", ", CoreState.LoadingStatus.AllNamedFlags())}]");

            // send telemetry about long load
        }

        if (!_shouldBeVisible)
        {
            Visible = false;
        }
    }

    private void OnAppear()
    {
        _hasAppearedOnce = true;
    }

    private void OnDisappear()
    {
        if (_hasAppearedOnce)
        {
            // prevents playing outsound on game start
            // play out sound
        }
    }

    public void SetState(bool shouldBeVisible)
    {
        if (shouldBeVisible != _shouldBeVisible)
        {
            OnStateChanged(shouldBeVisible);
        }

        _shouldBeVisible = shouldBeVisible;
    }
}