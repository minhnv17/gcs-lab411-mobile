using GCS_LAB411.ViewModels.SubViewsModel;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _selectedTabIndex = 0;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private NavBarViewModel _nbViewModel;
        private SettingViewModel _stViewModel;
        private FlytabViewModel _flytabViewModel;
        public MainViewModel(NavBarViewModel nbViewModel, SettingViewModel stViewModel, FlytabViewModel flViewModel)
        {
            _nbViewModel = nbViewModel;
            _stViewModel = stViewModel;
            _flytabViewModel = flViewModel;
            _nbViewModel.SelectTabEvent += HandleSelectTab;
            _flytabViewModel.IsShow = true;
        }

        private void HandleSelectTab(int index)
        {
            SelectedTabIndex = index;
            if(index == 1) // Fly tab 
            {
                _flytabViewModel.IsShow = true;
                _stViewModel.IsShow = false;
            }
            if(index == 0) // Setting tab
            {
                _flytabViewModel.IsShow = false;
                _stViewModel.IsShow = true;
            }    
        }
    }
}
