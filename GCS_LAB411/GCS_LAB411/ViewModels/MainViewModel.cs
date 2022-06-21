using GCS_Comunication.Comunication;
using GCS_LAB411.ViewModels.SubViewsModel;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using static GCS_LAB411.ViewModels.SubViewsModel.SettingViewModel;

namespace GCS_LAB411.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _test = 50;

        public int Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        private int _selectedTabIndex = 0;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private NavBarViewModel _nbViewModel;
        private SettingViewModel _stViewModel;
        private FlytabViewModel _flytabViewModel;
        private VehicleViewModel _vhViewModel;

        public NavBarViewModel NavBarViewModel
        {
            get => _nbViewModel;
        }
        public VehicleViewModel VehicleViewModel
        {
            get => _vhViewModel;
        }

        public MainViewModel(NavBarViewModel nbViewModel, SettingViewModel stViewModel, 
            FlytabViewModel flViewModel)
        {
            _nbViewModel = nbViewModel;
            _stViewModel = stViewModel;
            _flytabViewModel = flViewModel;
            _nbViewModel.SelectTabEvent += HandleSelectTab;
            _stViewModel.ConnectVehicle += new DelegateConnectVehicle(HandleConnectVehicle);
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

        private void HandleConnectVehicle(GCS_Com _mycom)
        {
            _vhViewModel = new VehicleViewModel(_mycom);
        }
    }
}
