using System;
using System.Threading.Tasks;

namespace IbeAppWeb.Services;

    /// <summary>
    /// Service to show toast notifications.
    /// </summary>
    /// <remarks>
    /// This service uses an event to notify subscribers when a toast should be shown.
    /// </remarks>
    public class IbeToastService
{
        public event Func<string, bool, Task> OnShow;

        public async Task ShowToast(string message, bool isSuccess)
        {
            if (OnShow != null)
                await OnShow.Invoke(message, isSuccess);
        }
}