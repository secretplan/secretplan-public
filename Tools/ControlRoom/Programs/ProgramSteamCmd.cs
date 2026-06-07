using System.Text;
using ControlRoom.Core;
using ControlRoom.Missions;

namespace ControlRoom.Programs;

public class ProgramSteamCmd : ExternalProgramFromShortcut
{
    public ProgramSteamCmd() : base("steamcmd", null,
        "- Install Steamworks SDK: https://partner.steamgames.com/downloads/list" +
        $"\n- Add SteamCmd shortcut (hint: {Constants.GenerateMissionCommand<Shortcut>("add steamcmd C:/Path/To/SteamCmd.exe")})")
    {
    }

    public async Task Login()
    {
        await Run($"+login {Platform.Shortcut("steam_username")} +quit");
    }

    public async Task RunAppBuild(string pathToVdf)
    {
        await Run($"+login {Platform.Shortcut("steam_username")} +run_app_build {pathToVdf} +quit");
    }

    public static string GenerateBuildVdf(SteamUploadSku uploadSku, string contentRoot, string description)
    {
        var table = new VdfRoot("AppBuild")
        {
            Content =
            {
                { "AppID", uploadSku.AppId.ToString() },
                { "Desc", description },
                { "ContentRoot", contentRoot }, // content, relative to the location of the VDF
                { "SetLive", Constants.TestInternalSteamBranch },
                { "BuildOutput", "../steamcmd_output" },
                {
                    "Depots", new VdfValueTable
                    {
                        Content =
                        {
                            {
                                uploadSku.DepotId.ToString(), new VdfValueTable
                                {
                                    Content =
                                    {
                                        {
                                            "FileMapping",
                                            new VdfValueTable
                                            {
                                                Content =
                                                {
                                                    { "LocalPath", "*" },
                                                    { "DepotPath", "." },
                                                    { "recursive", "1" }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
        return table.Generate();
    }

    private static string WrapInQuotes(string text)
    {
        return $"\"{text}\"";
    }

    public abstract class VdfValue
    {
        public static implicit operator VdfValue(string str)
        {
            return new VdfValueString(str);
        }
    }

    public class VdfValueString : VdfValue
    {
        public VdfValueString(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }

    /// <summary>
    ///     Functionally the same as VdfTable, but it already knows its root name so you can call Generate parameterless.
    /// </summary>
    public class VdfRoot : VdfValueTable
    {
        private readonly string _rootName;

        public VdfRoot(string rootName)
        {
            _rootName = rootName;
        }

        public string Generate()
        {
            return Generate(_rootName, 0);
        }
    }

    public class VdfValueTable : VdfValue
    {
        public Dictionary<string, VdfValue> Content { get; } = new();

        protected string Generate(string rootName, int currentIndent)
        {
            var stringBuilder = new StringBuilder();
            IndentThenNewLine(WrapInQuotes(rootName));
            IndentThenNewLine("{");
            foreach (var (key, value) in Content)
            {
                if (value is VdfValueString vdfString)
                {
                    AppendIndent(currentIndent + 1);
                    stringBuilder.Append(WrapInQuotes(key));
                    stringBuilder.Append("   ");
                    stringBuilder.Append(WrapInQuotes(vdfString.Data));
                    stringBuilder.AppendLine();
                }

                if (value is VdfValueTable table)
                {
                    stringBuilder.Append(table.Generate(key, currentIndent + 1));
                }
            }

            IndentThenNewLine("}");

            return stringBuilder.ToString();

            void AppendIndent(int givenIndent)
            {
                for (var i = 0; i < givenIndent * 2; i++)
                {
                    stringBuilder.Append(' ');
                }
            }
            
            void IndentThenNewLine(string text)
            {
                AppendIndent(currentIndent);
                stringBuilder.AppendLine(text);
            }
        }

        public void AddKeyValuePair(string key, VdfValue value)
        {
            Content.Add(key, value);
        }
    }
}