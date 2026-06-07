using System;
using System.Collections.Generic;
using ExTween;
using ExTween.Tweens;
using SecretPlanGodot.Core;
using SokoCore;
using SokoGame.Animation;
using SokoGame.World;

namespace Sokomatic;

public class GameSession
{
    private readonly FrameIdSource _frameIdSource = new();
    private readonly Stack<Frame> _previousFrames = new();
    private readonly SequenceTween _tween = new();
    private readonly EntityViewTable _viewTable = new();
    private Frame? _checkpointFrame;
    private Frame _currentFrame;
    private Frame? _pendingNextFrame;

    public GameSession()
    {
        _currentFrame = new Frame(_frameIdSource);

        CurrentFrame.AddEntity(EntityTemplate.Player(new GridPosition(2, 2)));
        CurrentFrame.AddEntity(EntityTemplate.Crate(new GridPosition(8, 2)));
        CurrentFrame.AddEntity(EntityTemplate.Crate(new GridPosition(9, 2)));
        CurrentFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(3, 3)));
        CurrentFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(4, 3)));
        CurrentFrame.AddEntity(EntityTemplate.Water(new GridPosition(5, 2)));
        CurrentFrame.AddEntity(EntityTemplate.Water(new GridPosition(6, 2)));
        CurrentFrame.AddEntity(EntityTemplate.Water(new GridPosition(5, 3)));
        CurrentFrame.AddEntity(EntityTemplate.Water(new GridPosition(6, 3)));

        SaveCheckpoint();
        ResolveFrame();
    }

    public Frame CurrentFrame
    {
        get => _currentFrame;

        private set
        {
            _currentFrame = value;
            FrameChanged?.Invoke();
        }
    }

    public event Action? FrameChanged;

    public void Undo()
    {
        if (_previousFrames.TryPop(out var previousFrame))
        {
            CurrentFrame = previousFrame;
        }
    }

    private void ResolveFrame()
    {
        SkipTweenIfPlaying();
        
        _previousFrames.Push(CurrentFrame);
        var resolveTransform = CurrentFrame.GetResolveTransform();
        _pendingNextFrame = CurrentFrame.CloneWithTransform(resolveTransform);
        
        LocalClient.Print("-- MOVE --");
        foreach (var animatedTransform in resolveTransform.AllAnimated())
        {
            LocalClient.Print(animatedTransform);
        }

        LocalClient.Print("-- /MOVE --");

        var currentMultiplex = new MultiplexTween();
        
        foreach (var animatedTransform in resolveTransform.AllAnimated())
        {
            animatedTransform.AppendTween(_tween, ref currentMultiplex);
            animatedTransform.BuildBeforeAnimation(currentMultiplex, _viewTable);
        }

        _tween.Add(currentMultiplex);
        
        _tween.Add(new CallbackTween(() =>
        {
            CurrentFrame = _pendingNextFrame;
            _pendingNextFrame = null;
        }));

        currentMultiplex = new MultiplexTween();
        
        foreach (var animatedTransform in resolveTransform.AllAnimated())
        {
            animatedTransform.AppendTween(_tween, ref currentMultiplex);
            animatedTransform.BuildAfterAnimation(currentMultiplex, _viewTable);
        }
        
        _tween.Add(currentMultiplex);
    }

    public void HandleDirectionalInput(CardinalDirection cardinalDirection)
    {
        SkipTweenIfPlaying();
        
        foreach (var entityWithId in _currentFrame.AllActiveEntitiesWithIds())
        {
            if (entityWithId.Entity.IsPlayerControlled)
            {
                _currentFrame.SetEntity(entityWithId.Id, entityWithId.Entity with { MoveIntent = cardinalDirection });
            }
        }

        ResolveFrame();
    }

    private void SkipTweenIfPlaying()
    {
        if (!_tween.IsDone())
        {
            _tween.SkipToEnd();
        }
    }

    public void SoftReset()
    {
        SkipTweenIfPlaying();
        
        if (_checkpointFrame != null)
        {
            CurrentFrame = _checkpointFrame.Clone();
        }

        ResolveFrame();
    }

    public void PrimaryAction()
    {
    }

    public void SecondaryAction()
    {
    }

    public void SaveCheckpoint()
    {
        _checkpointFrame = _currentFrame.Clone();
    }

    public void UpdateAnimationStates(float dt)
    {
        _tween.UpdateAndClearIfDone(dt);

        foreach (var animationState in _viewTable.Values())
        {
            animationState.UpdateAnimation(dt);
        }
    }

    public EntityAnimationState GetAnimationState(EntityId id)
    {
        return _viewTable.GetEntity(id);
    }
}