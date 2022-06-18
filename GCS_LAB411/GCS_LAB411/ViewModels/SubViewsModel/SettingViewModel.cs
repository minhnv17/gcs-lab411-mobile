using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class SettingViewModel : BaseViewModel
    {
        private bool _isShow = false;

        public bool IsShow
        {
            get => _isShow;
            set => SetProperty(ref _isShow, value);
        }

        public SettingViewModel()
        {

        }
    }
}
