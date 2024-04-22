﻿using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

internal interface IPersistentComponent : IComponent
{
    internal PersistentComponentState ComponentState { get; }

    internal void StateHasChanged();
}
