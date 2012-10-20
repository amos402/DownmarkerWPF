using System;
using Caliburn.Micro;
using MarkPad.Settings;
using MarkPad.Settings.Models;
using Shimmer.Client;

namespace MarkPad.Updater
{
    public class UpdaterViewModel : PropertyChangedBase
    {
        readonly ISettingsProvider settingsProvider;

        public int Progress { get; private set; }
        public UpdateState UpdateState { get; set; }
        public bool Background { get; set; }

        public UpdaterViewModel(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;

            DoUpdate();
        }

        public void CheckForUpdate()
        {
            DoUpdate();
        }

        public async void DoUpdate()
        {
            var settings = settingsProvider.GetSettings<MarkPadSettings>();

            var updateManager = new UpdateManager(settings.ConfigUrl, settings.Channel, FrameworkVersion.Net40);

            try 
            {
                var updateInfo = await updateManager.CheckForUpdateAsync(false, x => Progress += (x/3));
                if (updateInfo == null) 
                {
                    UpdateState = UpdateState.UpToDate;
                    return;
                }

                UpdateState = UpdateState.Downloading;  
                Background = true;
                await updateManager.DownloadReleasesAsync(updateInfo.ReleasesToApply, x => Progress += (x/3));
                await updateManager.ApplyReleasesAsync(updateInfo, x => Progress += (x/3));

                UpdateState = UpdateState.UpdatePending;
            } 
            catch (Exception) 
            {
                // NB: Probably want to log this or something
                UpdateState = UpdateState.Error;
            } 
            finally 
            {
                Background = false;
                updateManager.Dispose();
            }
        }
    }
}
