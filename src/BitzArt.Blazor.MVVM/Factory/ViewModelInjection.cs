﻿using System.Reflection;

namespace BitzArt.Blazor.MVVM;

public class ViewModelInjection
{
    public PropertyInfo Property { get; set; }

    public Type ViewModelType { get; set; }

    public ViewModelInjection(PropertyInfo property, Type viewModelType)
    {
        Property = property;
        ViewModelType = viewModelType;
    }
}