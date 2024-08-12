namespace BitzArt.Blazor.MVVM;

internal class ViewModelRequirementCollection
{
    private readonly List<ViewModelRequirement> _requirements = [];

    /// <summary>
    /// Waits for all requirements to be met.
    /// </summary>
    public async Task WaitAsync()
    {
        foreach (var requirement in _requirements)
        {
            await WaitAsync(requirement);
        }
    }

    private async Task WaitAsync(ViewModelRequirement requirement)
    {
        if (requirement.IsAlreadyMet)
            return;

        if (requirement.CancellationToken is not null)
            await WaitAsync(requirement.Condition, requirement.CancellationToken!.Value);

        else
            await WaitAsync(requirement.Condition);

        requirement.IsAlreadyMet = true;
    }

    private static async Task WaitAsync(Func<bool> condition, CancellationToken cancellationToken)
    {
        if (condition())
            return;

        try
        {
            await Task.Delay(-1, cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }

        if (!condition())
            throw new InvalidOperationException("Condition was not met.");
    }

    // Waits for the specified condition to be met by periodically checking it.
    private static async Task WaitAsync(Func<bool> condition)
    {
        while(!condition())
            await Task.Delay(10);
    }

    internal void Add(ViewModelRequirement requirement)
    {
        _requirements.Add(requirement);
    }

    internal void Add(Func<bool> requirement, CancellationToken? cancellationToken = null)
        => Add(new ViewModelRequirement(requirement, cancellationToken));
}
