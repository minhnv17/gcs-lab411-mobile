using GCS_LAB411.ViewModels.SubViewsModel;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCS_LAB411.Commands
{
    public class PilotCommand : AsyncCommandBase
    {
        protected BaseViewModel _parent;
        protected SlideConfirmViewModel _scViewModel;

        public PilotCommand(BaseViewModel parent, SlideConfirmViewModel scViewModel)
        {
            _parent = parent;
            _scViewModel = scViewModel;
            _scViewModel.PropertyChanged += _rbViewModel_PropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !_scViewModel.IsShow || IsExecuting;
        }

        public override async void Execute(object parameter)
        {
            if (IsExecuting) return;
            IsExecuting = true;
            await ExecuteAsync(parameter);
            IsExecuting = false;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            if (parameter is string)
            {
                bool resRightButton = false;
                Tuple<bool, string> answer;
                var res = await _scViewModel.ShowSlideConfirm("Do arm", 20);
                resRightButton = res.Item1;

                switch (parameter as string)
                {
                    case "DoFlyTo":
                        answer = await (_parent as FlytabViewModel).FlytoAction();
                        break;
                    default: break;
                }
            }
        }

        private void _rbViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SlideConfirmViewModel.IsShow))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
