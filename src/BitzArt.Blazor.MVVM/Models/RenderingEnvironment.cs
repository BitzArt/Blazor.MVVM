namespace BitzArt.Blazor.MVVM;

public class RenderingEnvironment
{
    public required bool IsPrerender { get; init; }
    public required bool IsServer { get; init; }
    public required bool IsClient { get; init; }
}
