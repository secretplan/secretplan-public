using ControlRoom;
using ControlRoom.Core;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;


// -------------------------------------------------------------- //
Platform.Startup();

if (await Constants.CheckLocalIsLatest())
{
    var remainingArgs = new List<string>();
    remainingArgs.AddRange(args);

    if (!args.IsValidIndex(0))
    {
        await Repl();
    }
    else
    {
        await AttemptToRunMissionWithArgs(remainingArgs);
    }
}

Platform.Shutdown();
return;

// -------------------------------------------------------------- //

async Task Repl()
{
    await OutPipe.AgentLogMessage("Entered REPL mode");

    while (true)
    {
        var input = await OutPipe.ReplPrompt();

        if (input == null)
        {
            continue;
        }

        if (input is "quit" or "exit")
        {
            break;
        }
        
        await AttemptToRunMissionWithArgs(input.Trim().SplitTokens().ToList());
    }
}

async Task AttemptToRunMissionWithArgs(List<string> givenArgs)
{
    var desiredMissionName = givenArgs[0];

    // remove mission name
    givenArgs.RemoveAt(0);

    var missionTypes = Reflection.GetAllTypesThatDeriveFrom<Mission>();
    var missionDictionary = missionTypes.ToDictionary(a => a.Name.ToLower(), a => a);

    var queryResult = missionDictionary.Keys.Where(a => a.StartsWith(desiredMissionName.ToLower())).ToList();

    switch (queryResult.Count)
    {
        case 1:
        {
            var type = missionDictionary[queryResult.First()];
            var instance = await MissionDispatch.CreateMission(type, givenArgs);

            if (instance != null)
            {
                await MissionDispatch.Execute(instance);
            }

            break;
        }
        case 0:
            await OutPipe.AgentLogError($"No missions found matching {desiredMissionName}");
            break;
        default:
            await OutPipe.AgentLogError(
                $"Found {queryResult.Count} matching missions:\n{string.Join("\n", queryResult.Select(a => "- " + missionDictionary[a].Name))}");
            await OutPipe.AgentLogError("Please run again with more specific mission name");
            break;
    }
}