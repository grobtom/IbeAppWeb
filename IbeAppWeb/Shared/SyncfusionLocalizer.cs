using Syncfusion.Blazor;
using IbeAppWeb.Shared;

namespace IbeAppWeb.Shared
{
    public static class SfResources
    {
        public static System.Resources.ResourceManager ResourceManager { get; } = new System.Resources.ResourceManager("IbeAppWeb.Resources.SfResources", typeof(SfResources).Assembly);
    }
}

public class SyncfusionLocalizer : ISyncfusionStringLocalizer
{
    public string GetText(string key)
    {
        return ResourceManager.GetString(key);
    }

    public System.Resources.ResourceManager ResourceManager
    {
        get
        {
            return SfResources.ResourceManager;
        }
    }
}