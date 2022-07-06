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
            int initValue = 0;

            if (parameter is string)
            {
                string actionType = parameter as string;
                bool resConfirm = false;
                int outValue = 0;
                Tuple<bool, string> answer;

                // Initial value before show slide confirm command
                switch(actionType)
                {
                    case "DoTakeOff":
                        initValue = 1; // Default take off
                        break;
                    default:
                        break;
                }

                // Show slider confirm command
                var res = await _scViewModel.ShowSlideConfirm(actionType, initValue);
                resConfirm = res.Item1;
                outValue = res.Item2;

                // Handle confirm or not
                if (resConfirm)
                {
                    switch (actionType)
                    {
                        case "DoFlyTo":
                            Console.WriteLine("DoFlyTo");
                            answer = await (_parent as MapViewModel).FlytoAction(1, 0);
                            break;

                        case "DoTakeOff":
                            answer = await (_parent as MapViewModel).Takeoff((float)outValue);
                            break;
                        case "DoArmDisarm":
                            answer = await (_parent as MapViewModel).ArmDisarm();
                            break;

                        case "DoRTL":
                            Console.WriteLine("RTL");
                            answer = await (_parent as MapViewModel).Land();
                            break;

                        case "DoLand":
                            Console.WriteLine("do land");
                            break;
                        default: 
                            break;
                    }
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
