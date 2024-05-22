using System.Text.Json.Serialization;

namespace BitzArt.Blazor.MVVM;

public abstract class ComponentState
{
    [JsonIgnore]
    public bool IsInitialized { get; internal set; }
}
