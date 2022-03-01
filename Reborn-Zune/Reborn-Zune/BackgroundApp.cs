using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;

namespace Reborn_Zune
{
    sealed partial class App : Application
    {
        bool isInBackgroundMode;

        partial void Construct()
        {
            // During the transition from foreground to background the
            // memory limit allowed for the application changes. The application
            // has a short time to respond by bringing its memory usage
            // under the new limit.
            MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;

            // After an application is backgrounded it is expected to stay
            // under a memory target to maintain priority to keep running.
            // Subscribe to the event that informs the app of this change.
            MemoryManager.AppMemoryUsageIncreased += MemoryManager_AppMemoryUsageIncreased;

            // Subscribe to key lifecyle events to know when the app
            // transitions to and from foreground and background.
            // Leaving the background is an important transition
            // because the app may need to restore UI.
            EnteredBackground += App_EnteredBackground;
            LeavingBackground += App_LeavingBackground;

            // Subscribe to regular lifecycle events to display a toast notification
            Suspending += App_Suspending;
            Resuming += App_Resuming;
        }

        private void App_Resuming(object sender, object e)
        {
            
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            isInBackgroundMode = false;
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            isInBackgroundMode = true;
            GC.Collect();
        }

        private void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            // Obtain the current usage level
            var level = MemoryManager.AppMemoryUsageLevel;

            // Check the usage level to determine whether reducing memory is necessary.
            // Memory usage may have been fine when initially entering the background but
            // a short time later the app might be using more memory and need to trim back.
            if (level == AppMemoryUsageLevel.OverLimit || level == AppMemoryUsageLevel.High)
            {
                ReduceMemoryUsage(MemoryManager.AppMemoryUsageLimit);
            }
        }

        private void MemoryManager_AppMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs e)
        {
            if (MemoryManager.AppMemoryUsage >= e.NewLimit)
            {
                ReduceMemoryUsage(e.NewLimit);
            }
        }

        public void ReduceMemoryUsage(ulong limit)
        {
            // If the app has caches or other memory it can free, now is the time.
            // << App can release memory here >>

            // Additionally, if the application is currently
            // in background mode and still has a view with content
            // then the view can be released to save memory and 
            // can be recreated again later when leaving the background.
            if (isInBackgroundMode && Window.Current.Content != null)
            {

                // Clear the view content. Note that views should rely on
                // events like Page.Unloaded to further release resources. Be careful
                // to also release event handlers in views since references can
                // prevent objects from being collected. C++ developers should take
                // special care to use weak references for event handlers where appropriate.
                Window.Current.Content = null;

                // Finally, clearing the content above and calling GC.Collect() below 
                // is what will trigger each Page.Unloaded handler to be called.
                // In order for the resources each page has allocated to be released,
                // it is necessary that each Page also call GC.Collect() from its
                // Page.Unloaded handler.
            }

            // Run the GC to collect released resources, including triggering
            // each Page.Unloaded handler to run.
            GC.Collect();
            
        }
    }
}
