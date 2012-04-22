using System;
using Caliburn.Micro;
using NSync.Client;

namespace MarkPad.Updater
{
    public class UpdaterViewModel : PropertyChangedBase
    {
        private readonly IWindowManager windowManager;
        private readonly Func<UpdaterChangesViewModel> changesCreator;
        private readonly IUpdateManager _manager;

        public int Progress { get; private set; }
        public UpdateState UpdateState { get; set; }
        public bool Background { get; set; }

        public UpdaterViewModel(
            IWindowManager windowManager, 
            Func<UpdaterChangesViewModel> changesCreator, 
            IUpdateManager manager)
        {
            this.windowManager = windowManager;
            this.changesCreator = changesCreator;

            _manager = manager;
            _manager.CheckForUpdate()
                    .Subscribe(CheckRelease, CheckException);

            UpdateState = UpdateState.Downloading;
            Background = true;
        }

        private void CheckRelease(UpdateInfo updateInfo)
        {
            if (updateInfo == null)
            {
                UpdateState = UpdateState.UpToDate;
                Background = false;
                return;
            }

            UpdateState = UpdateState.UpdatePending;

            var vm = changesCreator();
            vm.Version = updateInfo.Version;
            vm.Message = "You should really install this update now!";
            windowManager.ShowDialog(vm);
            if (!vm.WasCancelled)
            {
                // TODO: copy files into new location
            }

            Background = false;
        }

        private void CheckException(Exception obj)
        {
            UpdateState = UpdateState.Error;
        }

        public void CheckForUpdate()
        {
            _manager.CheckForUpdate()
                    .Subscribe(CheckRelease, CheckException);
        }
    }
}
