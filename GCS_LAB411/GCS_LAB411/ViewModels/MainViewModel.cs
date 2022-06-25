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
        private int _selectedTabIndex = 0;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private NavBarViewModel _nbViewModel;
        private SettingViewModel _stViewModel;
        private VehicleManagerViewModel _vhManagerViewModel;
        private MapViewModel _mapViewModel;

        public NavBarViewModel NavBarViewModel
        {
            get => _nbViewModel;
        }
        public VehicleManagerViewModel VehicleManagerViewModel
        {
            get => _vhManagerViewModel;
        }

        public MainViewModel(NavBarViewModel nbViewModel, SettingViewModel stViewModel, 
            MapViewModel mapViewModel, VehicleManagerViewModel vhManagerViewModel)
        {
            _nbViewModel = nbViewModel;
            _stViewModel = stViewModel;
            _mapViewModel = mapViewModel;
            _vhManagerViewModel = vhManagerViewModel;
            _nbViewModel.SelectTabEvent += HandleSelectTab;
            _stViewModel.ConnectVehicle += new DelegateConnectVehicle(HandleConnectVehicle);
            _mapViewModel.IsShow = true;
        }

        private void HandleSelectTab(int index)
        {
            SelectedTabIndex = index;
            if(index == 1) // Fly tab 
            {
                _mapViewModel.IsShow = true;
                _stViewModel.IsShow = false;
            }
            if(index == 0) // Setting tab
            {
                _mapViewModel.IsShow = false;
                _stViewModel.IsShow = true;
            }    
        }

        private void HandleConnectVehicle(GCS_Com _mycom)
        {
            _vhManagerViewModel.ConnectToVehicle(_mycom);
        }
    }
}
