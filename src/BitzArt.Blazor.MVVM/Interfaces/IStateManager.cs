using System.Text.Json.Nodes;

namespace BitzArt.Blazor.MVVM;

public interface IStateManager
{
    /// <summary>
    /// Serializes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states to JSON.
    /// </summary>
    public byte[]? SerializeState(ViewModel viewModel);

    /// <summary>
    /// Restores <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states from JSON.
    /// </summary>
    public Task RestoreStateAsync(ViewModel viewModel, string json);

    /// <summary>
    /// Initializes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states.
    /// </summary>
    public Task InitializeStateAsync(ViewModel viewModel);
}
