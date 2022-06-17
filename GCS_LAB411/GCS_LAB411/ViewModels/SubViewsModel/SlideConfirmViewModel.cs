using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class SlideConfirmViewModel : BaseViewModel
    {
        private enum ButtonReturn
        {
            OK = 0,
            Cancel,
            Unknown
        }

        private ButtonReturn _res = ButtonReturn.Unknown;

        private int _slideValue = 0;

        public int SlideValue
        {
            get => _slideValue;
            set => SetProperty(ref _slideValue, value);
        }

        private string _nameOfCommand = "No command!";

        public string NameOfCommand
        {
            get => _nameOfCommand;
            set => SetProperty(ref _nameOfCommand, value);
        }

        private bool _isShow = false;
        public bool IsShow
        {
            get { return _isShow; }
            set { SetProperty(ref _isShow, value); }
        }

        public Command DragCompleteCommand { get; set; }
        public Command CancelCommand { get; set; }

        public SlideConfirmViewModel()
        {
            DragCompleteCommand = new Command(HandleDragComplete);
            CancelCommand = new Command(HandleCancelCommand);
        }

        private void HandleDragComplete(object obj)
        {
            if(_slideValue != 100) SlideValue = 0;
            else
            {

            }
        }

        private void HandleCancelCommand()
        {
            Console.WriteLine("command cancel!");
        }

        //private void HandleCommandConfirm()
        //{
        //    switch(_newCommand)
        //    {
        //        case CommandName.Takeoff:
        //            break;
        //        case CommandName.Land:
        //            break;
        //        case CommandName.Arm:
        //            break;
        //        case CommandName.Disarm:
        //            break;
        //        case CommandName.Flyto:
        //            break;
        //        case CommandName.RTL:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void HandleCommandUnConfirm()
        {

        }
    }
}
