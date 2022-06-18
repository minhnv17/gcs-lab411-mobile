using MvvmHelpers;
using MvvmHelpers.Commands;
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

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public Command SelectedTabCommand { get; set; }
        public SettingViewModel()
        {
            SelectedTabCommand = new Command(HandleSelectedTabCommand);
        }

        private void HandleSelectedTabCommand(object obj)
        {
            if(obj != null)
            {
                int index = int.Parse(obj as string);
                SelectedTabIndex = index;
            }
        }
    }
}
