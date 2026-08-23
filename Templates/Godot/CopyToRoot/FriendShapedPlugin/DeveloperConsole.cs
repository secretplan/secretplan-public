using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BirdGame.Core;
using BirdGame.UI;
using FriendShapedDistributable;
using Godot;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SecretPlanGodot.Serialization;
using SecretPlanGodot.Testing;

// ReSharper disable StringLiteralTypo

namespace FriendShapedPlugin;

public partial class DeveloperConsole : Node, INavigationOwnerExtended
{
    private readonly List<string> _commandBuffer = new();
    private readonly CachedNode<Control> _controlRoot = new("Layer/Root");
    private readonly ParentCore _core = new();
    private readonly CachedNode<LineEdit> _input = new("Layer/Root/PanelContainer/Stack/Input");
    private readonly ConcurrentQueue<string> _messageBuffer = new();
    private readonly CachedNode<RichTextLabel> _messageHistory = new("Layer/Root/PanelContainer/Stack/MessageHistory");
    private int _commandBufferPosition;
    private ConsoleDebug _consoleDebug = null!;
    private bool _hasLoggedAMessage;
    private bool _isShowing;
    private CoreState CoreState => _core.State(this);
    private RichTextLabel MessageHistory => _messageHistory.Get(this);
    private LineEdit LineEdit => _input.Get(this);
    private Control Root => _controlRoot.Get(this);

    public Control? GetDefaultFocusNode()
    {
        return LineEdit;
    }

    public bool IsValidNavigationOwner()
    {
        return _isShowing;
    }

    public override void _Ready()
    {
        _consoleDebug = new ConsoleDebug(this.ClimbAncestorsUntilFindType<GameCore>()!);
        MessageHistory.Text = string.Empty;
        LocalClient.OnMessage += OnLogMessage;

        LineEdit.KeepEditingOnTextSubmit = true;
        LineEdit.TextSubmitted += OnSubmit;
        ShowConsoleOverlay(false, false);

        CoreState.ConsoleLogHistory.SetProvider(() =>
        {
            if (!this.IsValidAndNotQueuedForDeletion())
            {
                return string.Empty;
            }

            return MessageHistory.GetParsedText();
        });
    }

    private void OnLogMessage(string message)
    {
        _messageBuffer.Enqueue(message);
    }

    private void ShowConsoleOverlay(bool shouldBeVisible, bool shouldAffectMouse = true)
    {
        _isShowing = shouldBeVisible;

        LineEdit.Clear();
        Root.Visible = shouldBeVisible;

        if (shouldBeVisible)
        {
            CoreState.NavigationSystem.FocusState.SetNavigationOwner(this);
            GrabFocusDeferred();
        }

        if (shouldAffectMouse)
        {
            CoreState.MouseLock.RequestMouseLock(!shouldBeVisible);
        }

        CoreState.Debug.IsConsoleOpen = shouldBeVisible;
        CoreState.NavigationSystem.SetGamepadSupport(!shouldBeVisible);

        MessageHistory.ScrollToLine(MessageHistory.GetLineCount());
    }

    private void GrabFocusDeferred()
    {
        LineEdit.CallDeferred(Control.MethodName.GrabFocus);
    }

    private void OnSubmit(string commandText)
    {
        if (commandText.Length == 0)
        {
            return;
        }

        _commandBuffer.Add(commandText);
        _commandBufferPosition = 0;

        ParseAndRunCommand(commandText);

        LineEdit.Clear();
        MessageHistory.ScrollToLine(MessageHistory.GetLineCount());
    }

    private void ParseAndRunCommand(string commandText)
    {
        LocalClient.Print(">>> " + commandText);

        var tokens = commandText.SplitTokens();

        if (tokens.Length == 0)
        {
            return;
        }

        var currentError = ConsoleCommand.Validity.NoMatch;

        foreach (var command in Commands(false))
        {
            if (command.GetValidity(tokens) == ConsoleCommand.Validity.CorrectMatch)
            {
                var errorMessage = command.Run(tokens);
                if (errorMessage != null)
                {
                    LocalClient.Print($"[color=ffcccc]{errorMessage}[/color]");
                }

                return;
            }
        }

        switch (currentError)
        {
            case ConsoleCommand.Validity.NoMatch:
                LocalClient.Print($"Unrecognized command {tokens.First()}");
                break;
        }
    }

    private IEnumerable<ConsoleCommand> Commands(bool skipHidden)
    {
        yield return new ConsoleCommand("help", _ =>
        {
            var answer = new StringBuilder();
            answer.Append("Valid commands: ");
            foreach (var command in Commands(true))
            {
                answer.AppendLine(command.InvokeWord);
            }

            LocalClient.Print(answer);
        });

        foreach (var command in _consoleDebug.GenerateConsoleCommands(skipHidden))
        {
            yield return command;
        }

        foreach (var debuggers in CoreState.AllDebuggers())
        {
            foreach (var command in debuggers.GenerateConsoleCommands(skipHidden))
            {
                yield return command;
            }
        }

        foreach (var command in CoreState.SerializedState.Settings.Debug.GenerateConsoleCommands(skipHidden))
        {
            yield return command;
        }

        yield return new ConsoleCommand("clear", _ =>
        {
            MessageHistory.Text = string.Empty;
            _hasLoggedAMessage = false;
        });

        yield return new ConsoleCommand("trailermode", _ =>
        {
            CoreState.SerializedState.Settings.ShowFramerateCounter = false;
            CoreState.SerializedState.Settings.WindowMode = FullscreenMode.Windowed;

            CoreState.SerializedState.Settings.InvokeChangedForAllSettings();

            DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
        });

        yield return new ConsoleCommand("renderingdriver",
            args => { LocalClient.Print(RenderingServer.GetCurrentRenderingDriverName()); });

        yield return new ConsoleCommand("getnodecount", args =>
        {
            LocalClient.Print("node count: ", Performance.GetMonitor(Performance.Monitor.ObjectNodeCount));
        });

        yield return new ConsoleCommand("version", args => { LocalClient.Print(VersionManager.GetVersion()); });

        yield return new ConsoleCommand("savesettings", _ =>
        {
            CoreState.SerializedState.SaveSettings();
        });
        
#if TOOLS

        yield return new ConsoleCommand("throwexception", args => throw new Exception("Invoked by console command!"));

        yield return new ConsoleCommand("runtests", _ =>
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                TestHelpers.RunAllTestsInAssembly(assembly);
            }
        });

        yield return new ConsoleCommand("editconfigs",
            _ =>
            {
                _core.State(this).LoadScene(
                    new CachedPackedScene<Node>("res://FriendShapedPlugin/ConfigEditor/Scenes/ConfigEditor.tscn"),
                    scene => { });
            });
#endif
    }

    public override void _Process(double delta)
    {
        // clear queue
        while (_messageBuffer.TryDequeue(out var message))
        {
            if (_hasLoggedAMessage)
            {
                MessageHistory.AppendText("\n");
            }

            MessageHistory.AppendText(message);
            _hasLoggedAMessage = true;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(StringNameCache.Console) &&
            CoreState.SerializedState.Settings.AllowConsole)
        {
            ShowConsoleOverlay(!_isShowing);
            GetViewport().SetInputAsHandled();
        }

        if (CoreState.Debug.IsConsoleOpen && LineEdit.HasFocus())
        {
            if (inputEvent.IsActionPressed(StringNameCache.UiCancel))
            {
                ShowConsoleOverlay(false);
                GetViewport().SetInputAsHandled();
            }

            if (inputEvent.IsActionPressed(StringNameCache.UiUp))
            {
                GoToPreviousCommand();
                GetViewport().SetInputAsHandled();
            }

            if (inputEvent.IsActionPressed(StringNameCache.UiDown))
            {
                GoToNextCommand();
                GetViewport().SetInputAsHandled();
            }
        }
    }

    private void GoToNextCommand()
    {
        _commandBufferPosition--;
        if (_commandBufferPosition <= 0)
        {
            _commandBufferPosition = 0;
            LineEdit.Clear();
        }
        else
        {
            UpdateTextBufferForCommandBuffer();
        }
    }

    private void GoToPreviousCommand()
    {
        _commandBufferPosition++;
        var bufferMax = _commandBuffer.Count;
        if (_commandBufferPosition > bufferMax)
        {
            _commandBufferPosition = bufferMax;
        }

        UpdateTextBufferForCommandBuffer();
    }

    private void UpdateTextBufferForCommandBuffer()
    {
        var index = _commandBuffer.Count - _commandBufferPosition;
        if (_commandBuffer.IsValidIndex(index))
        {
            LineEdit.Text = _commandBuffer[index];
            LineEdit.SetDeferred(LineEdit.PropertyName.CaretColumn, LineEdit.Text.Length);
        }
    }

    private enum VoiceArgument
    {
        Reset,
        Disable,
        Enable
    }
}