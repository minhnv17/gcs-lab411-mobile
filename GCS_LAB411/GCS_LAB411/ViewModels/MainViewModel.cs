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

        NavBarViewModel _nbViewModel;
        public MainViewModel(NavBarViewModel nbViewModel)
        {
            _nbViewModel = nbViewModel;

            _nbViewModel.SelectTabEvent += HandleSelectTab;
        }

        private void HandleSelectTab(int index)
        {
            SelectedTabIndex = index;
        }
    }
}
