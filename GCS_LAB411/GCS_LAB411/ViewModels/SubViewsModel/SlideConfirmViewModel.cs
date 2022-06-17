using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            get => _isShow;
            set => SetProperty(ref _isShow, value);
        }

        public Command ConfirmCommand { get; set; }
        public Command CancelCommand { get; set; }

        public SlideConfirmViewModel()
        {
            ConfirmCommand = new Command(HandleConfirmCommand);
            CancelCommand = new Command(HandleCancelCommand);
        }

        private void HandleConfirmCommand()
        {
            if(_slideValue != 100) SlideValue = 0;
            else
            {
                _res = ButtonReturn.OK;
                SlideValue = 0;
            }
        }

        private void HandleCancelCommand()
        {
            _res = ButtonReturn.Cancel;
            SlideValue = 0;
        }

        private int _outValue = 0;
        public Task<Tuple<bool, int>> ShowSlideConfirm(string title, int initValue)
        {
            Func<Tuple<bool, int>> waitConfirm = () =>
            {
                if (_isShow)
                    return Tuple.Create<bool, int>(false, initValue);

                NameOfCommand = title;
                _outValue = initValue;
                _res = ButtonReturn.Unknown;

                IsShow = true;

                while (_res != ButtonReturn.OK && _res != ButtonReturn.Cancel)
                {
                    Thread.Sleep(100);
                }

                IsShow = false;
                if (_res == ButtonReturn.Cancel)
                    return Tuple.Create<bool, int>(false, initValue);
                else
                    return Tuple.Create<bool, int>(true, initValue);
            };

            var task = new Task<Tuple<bool, int>>(waitConfirm);
            task.Start();
            return task;
        }

        public Task<bool> ShowSlideConfirm(string title = "", string message = "")
        {
            Func<bool> waitSlideConfirm = () =>
            {
                if (_isShow)
                    return false;

                _res = ButtonReturn.Unknown;

                NameOfCommand = title;
                IsShow = true;

                while (_res != ButtonReturn.OK && _res != ButtonReturn.Cancel)
                {
                    Thread.Sleep(100);
                }

                IsShow = false;

                if (_res == ButtonReturn.Cancel)
                    return false;
                else
                    return true;
            };
            var task = new Task<bool>(waitSlideConfirm);
            task.Start();
            return task;
        }
    }

}
