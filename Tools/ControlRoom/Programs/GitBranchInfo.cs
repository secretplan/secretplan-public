namespace ControlRoom.Programs;

public readonly record struct GitBranchInfo(string HeadSha, string BranchName);

public readonly record struct GitForkedBranchInfo(GitBranchInfo ForkBranch, GitBranchInfo SourceBranch, string CommonSha);