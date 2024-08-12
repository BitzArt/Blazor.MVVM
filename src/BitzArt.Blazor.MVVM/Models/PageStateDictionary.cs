namespace BitzArt.Blazor.MVVM;

internal class PageStateDictionaryContainer : IDisposable
{
    public PageStateDictionary? PageStateDictionary { get; set; }

    private bool Configured { get; set; } = false;

    private readonly List<CancellationTokenSource> waitingUntilConfigured = [];

    public void Dispose()
    {
        PageStateDictionary = null;
        Configured = false;
    }

    public void MarkConfigured()
    {
        Configured = true;
        while (waitingUntilConfigured.Count > 0)
        {
            var cts = waitingUntilConfigured.First();
            waitingUntilConfigured.Remove(cts);
            cts.Cancel();
        }
    }

    public async Task WaitUntilConfiguredAsync()
    {
        if (Configured) return;

        using var cts = new CancellationTokenSource();
        waitingUntilConfigured.Add(cts);

        await Task.Delay(-1, cts.Token);
    }
}

internal class PageStateDictionary
{
    private Dictionary<ComponentSignature, ComponentState?> ComponentStates { get; }

    public PageStateDictionary()
    {
        ComponentStates = [];
    }

    public void Add(ComponentSignature signature, ComponentState? state)
    {
        ComponentStates.Add(signature, state);
    }

    public ComponentState? Get(ComponentSignature signature)
        => ComponentStates.GetValueOrDefault(signature);
}
