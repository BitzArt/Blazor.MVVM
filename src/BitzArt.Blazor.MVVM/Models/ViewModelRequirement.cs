namespace BitzArt.Blazor.MVVM;

internal record ViewModelRequirement(Func<bool> Condition, CancellationToken? CancellationToken = null)
{
    public bool IsAlreadyMet = false;
}
