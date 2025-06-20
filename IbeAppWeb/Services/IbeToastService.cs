namespace IbeAppWeb.Services;

/// <summary>
/// Service to show toast notifications.
/// </summary>
/// <remarks>
/// This service uses an event to notify subscribers when a toast should be shown.
/// </remarks>
public class IbeToastService
{
    public event Func<string, bool, Task>? OnShow;
    public event Func<string, bool, int, Task>? OnShowWithDuration; 

    public async Task ShowToast(string message, bool isSuccess)
    {
        if (OnShow != null)
            await OnShow.Invoke(message, isSuccess);
    }

    public async Task ShowToast(string message, bool isSuccess, int duration)
    {
        if (OnShowWithDuration != null)
            await OnShowWithDuration.Invoke(message, isSuccess, duration);
    }
}