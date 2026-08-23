using System;
using System.Collections.Generic;
using FriendShapedDistributable;
using Godot;
using Godot.Collections;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class SceneRoot : Node
{
    private readonly Queue<Func<LoadingHandle>> _enqueuedLoadSteps = new();
    private readonly ParentCore _parentCore = new();
    private LoadingHandle? _inProgressStep;
    public CoreState GameState => _parentCore.State(this);

    public void LoadScene<T>(CachedPackedScene<T> packedScene, Action<T>? callback = null) where T : Node
    {
        LocalClient.Print("LOAD: Requested scene transition, enqueueing work");

        _enqueuedLoadSteps.Enqueue(() =>
            {
                GameState.LoadingStatus.IncrementLoadingScrim();
                return LoadingHandle.Instant("Increment Loading Scrim");
            }
        );

        _enqueuedLoadSteps.Enqueue(() =>
        {
            TearDownScenes(GetChildren());
            return LoadingHandle.Instant("Tear Down Scenes");
        });

        _enqueuedLoadSteps.Enqueue(() =>
        {
            return packedScene.LoadAndInstantiateAsync(sceneInstance =>
            {
                var callbackAsCallable = Callable.From((Node node) =>
                {
                    if (callback != null && node is T t)
                    {
                        callback(t);
                    }
                });

                CallDeferred(nameof(FinishLoadScene), sceneInstance, callbackAsCallable);
            });
        });
    }

    private void TearDownScenes(Array<Node> oldScenes)
    {
        LocalClient.Print("LOAD: Removing old scene(s) from scene tree");
        foreach (var oldScene in oldScenes)
        {
            RemoveChild(oldScene);
        }
        LocalClient.Print("LOAD: Done removing old scene(s) from scene tree");

        LocalClient.Print("LOAD: QueueFreeing old scene(s)");
        foreach (var oldScene in oldScenes)
        {
            oldScene.QueueFree();
        }
        LocalClient.Print("LOAD: Done QueueFreeing old scene(s)");
    }

    private void FinishLoadScene(Node sceneInstance, Callable callback)
    {
        LocalClient.Print("LOAD: Adding new scene");
        AddChild(sceneInstance, true);
        LocalClient.Print("LOAD: Done adding new scene");

        LocalClient.Print("LOAD: Running callback on new scene");
        callback.Call(sceneInstance);
        LocalClient.Print("LOAD: Done running callback on new scene");

        LocalClient.Print("LOAD: Forcing GC collect");
        GC.Collect();
        LocalClient.Print("LOAD: Done forcing GC collect");

        LocalClient.Print("LOAD: Decrementing Loading Scrim");
        GameState.LoadingStatus.DecrementLoadingScrim();
    }

    public override void _Process(double delta)
    {
        if (_inProgressStep != null)
        {
            var status = _inProgressStep.Poll();
            if (status != ResourceLoader.ThreadLoadStatus.InProgress)
            {
                _inProgressStep = null;
            }
        }
        else
        {
            if (_enqueuedLoadSteps.TryDequeue(out var front))
            {
                _inProgressStep = front();
                LocalClient.Print($"LOAD: Starting next load step: {_inProgressStep}");
            }
        }
    }
}
