using System.Text.Json.Serialization;

namespace BitzArt.Blazor.MVVM;

public abstract class ComponentState
{
    // ========================    PROPERTIES    ========================

    [JsonIgnore]
    public bool IsInitialized { get; internal set; }

    // ============================  EVENTS  ============================

    /// <summary>
    /// Called when the state is initialized.
    /// </summary>
    [JsonIgnore]
    public StateInitializedHandler? OnInitialized { get; set; }

    /// <summary>
    /// Called when the state is initialized.
    /// </summary>
    [JsonIgnore]
    public StateInitializedAsyncHandler? OnInitializedAsync { get; set; }
}
