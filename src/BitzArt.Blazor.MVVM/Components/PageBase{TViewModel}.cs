using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Blazor page base class with view model support.
/// </summary>
/// <typeparam name="TViewModel">Type of this component's ViewModel</typeparam>
public abstract class PageBase<TViewModel> : ComponentBase<TViewModel>, IStateComponent, IDisposable
    where TViewModel : ViewModel
{
    [Inject]
    public new TViewModel ViewModel
    {
        get => base.ViewModel;
        set => base.ViewModel = value;
    }

    private const string StateKey = "state";

    public delegate Task PageExceptionHandler(object sender, Exception ex);

    /// <summary>
    /// Called when an exception is thrown within the scope of the page and needs to be handled.
    /// </summary>
    public PageExceptionHandler? ExceptionHandler { get; set; }

    override protected void OnInitialized()
    {
        base.OnInitialized();
        ViewModel.ExceptionHandler += (sender, ex) =>
        {
            if (ExceptionHandler is null)
                return Task.CompletedTask;
            
            return ExceptionHandler.Invoke(sender, ex);
        };
    }

    protected override async Task RestoreStateAsync()
    {
        var state = await Js.InvokeAsync<string?>("getInnerText", [StateKey]);
        if (state is null)
        {
            PageStateDictionaryContainer.MarkConfigured();
            return;
        }
        var buffer = Convert.FromBase64String(state!);
        var json = Encoding.UTF8.GetString(buffer);

        var pageState = await ViewModelFactory.RestorePageComponentsStateAsync(ViewModel, json);
        PageStateDictionaryContainer.PageStateDictionary = pageState;
        PageStateDictionaryContainer.MarkConfigured();

        await base.RestoreStateAsync();
    }

    /// <summary>
    /// Set the parameters from the query string.
    /// </summary>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        ViewModel.SetParametersFromQueryString(NavigationManager);

        return base.SetParametersAsync(parameters);
    }

    public void Dispose()
    {
        PageStateDictionaryContainer.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal static class StateJsonOptionsProvider
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
