using System;
using ExTween;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Tweenables;
using SokoGame.World;

namespace Sokomatic;

public class EntityAnimationState
{
    private readonly ExNoise _noise;
    private float _elapsedTime;

    public EntityAnimationState()
    {
        _noise = new ExNoise(GameConstants.GlobalRandom.NextInt());
        _elapsedTime = 0f;
    }

    /// <summary>
    ///     Assigned by Entity every time we draw to the screen
    /// </summary>
    public EntityContinuousAnimation CurrentAnimation { get; set; }

    /// <summary>
    ///     Assigned by Entity
    /// </summary>
    public Color PrimaryColor { get; set; }

    public Vector2 PositionOffsetPercent { get; private set; }
    public float AngleDegrees { get; private set; }
    public float Scale { get; private set; } = 1f;
    public TweenableVector2 TweenablePositionOffsetPercent { get; } = new();
    public TweenableFloat TweenableAngleDegrees { get; } = new();
    public TweenableFloat TweenableScale { get; } = new(1f);
    public Color SecondaryColor { get; } = Colors.White;
    public TweenableFloat TweenableSecondaryColorPercent { get; } = new();
    public float SecondaryColorPercent { get; private set; }

    public void UpdateAnimation(float dt)
    {
        _elapsedTime += dt;

        if (CurrentAnimation == EntityContinuousAnimation.Water)
        {
            PositionOffsetPercent = PositionOffsetPercent with
            {
                X = MathF.Sin(RandomFloat(0) + _elapsedTime * 2) * 0.05f
            };
            SecondaryColorPercent = MathF.Cos(RandomFloat(1) + _elapsedTime * 4f) / 2f + 0.5f;
        }
        
        if (CurrentAnimation == EntityContinuousAnimation.Submerged)
        {
            AngleDegrees = MathF.Sin(RandomFloat(0) + _elapsedTime) * 5f;
        }
    }

    private float RandomFloat(int position)
    {
        return _noise.FloatAt(position) * 100f;
    }
}