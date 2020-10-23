using System.Runtime;
using System.Windows;

namespace SlightPenLighter
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            base.OnStartup(e);
        }
    }
}