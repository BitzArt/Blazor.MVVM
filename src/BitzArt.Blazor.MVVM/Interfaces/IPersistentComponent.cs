using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

internal interface IPersistentComponent : IComponent
{
    internal void StateHasChanged();
}
