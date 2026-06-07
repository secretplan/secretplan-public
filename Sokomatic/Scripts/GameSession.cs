using System;
using System.Collections.Generic;
using SecretPlanGodot.Core;
using SokoCore;
using SokoGame.World;

namespace Sokomatic;

public class GameSession
{
    private readonly Dictionary<EntityId, EntityAnimationState> _animationTable = new();
    private readonly FrameIdSource _frameIdSource = new();
    private readonly Stack<Frame> _previousFrames = new();
    private Frame? _checkpointFrame;
    private Frame _currentFrame;

    public GameSession()
    {
        _currentFrame = new Frame(_frameIdSource);

        CurrentFrame.AddEntity(EntityTemplate.Player(new GridPosition(2, 2)));
        CurrentFrame.AddEntity(EntityTemplate.Crate(new GridPosition(3, 2)));
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
        _previousFrames.Push(CurrentFrame);
        var resolveTransform = CurrentFrame.GetResolveTransform();
        LocalClient.Print(resolveTransform);
        CurrentFrame = CurrentFrame.CloneWithTransform(resolveTransform);
    }

    public void HandleDirectionalInput(CardinalDirection cardinalDirection)
    {
        foreach (var entityWithId in _currentFrame.AllActiveEntitiesWithIds())
        {
            if (entityWithId.Entity.IsPlayerControlled)
            {
                _currentFrame.SetEntity(entityWithId.Id, entityWithId.Entity with { MoveIntent = cardinalDirection });
            }
        }

        ResolveFrame();
    }

    public void SoftReset()
    {
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
        foreach (var animationState in _animationTable.Values)
        {
            animationState.UpdateAnimation(dt);
        }
    }

    public EntityAnimationState GetAnimationState(EntityId id)
    {
        if (!_animationTable.ContainsKey(id))
        {
            LocalClient.Print($"Added animation state: {id}");
            _animationTable[id] = new EntityAnimationState();
        }

        return _animationTable[id];
    }
}