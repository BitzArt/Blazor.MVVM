namespace BitzArt.Blazor.MVVM;

public interface IStateManager
{
    /// <summary>
    /// Encodes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states into a base64 string.
    /// </summary>
    public string? EncodeState(ViewModel viewModel);
}
