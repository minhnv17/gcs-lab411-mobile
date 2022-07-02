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

        public List<String> ListMode { get; set; }

        private NavBarViewModel _nbViewModel;
        private SettingViewModel _stViewModel;
        private VehicleManagerViewModel _vhManagerViewModel;
        private MapViewModel _mapViewModel;
        private CameraLiveViewModel _cameraLiveViewModel;

        public NavBarViewModel NavBarViewModel
        {
            get => _nbViewModel;
        }
        public VehicleManagerViewModel VehicleManagerViewModel
        {
            get => _vhManagerViewModel;
        }

        public MapViewModel MapViewModel
        {
            get => _mapViewModel;
        }

        public MainViewModel(NavBarViewModel nbViewModel, SettingViewModel stViewModel, 
            MapViewModel mapViewModel, VehicleManagerViewModel vhManagerViewModel,
            CameraLiveViewModel clViewModel)
        {
            _nbViewModel = nbViewModel;
            _stViewModel = stViewModel;
            _mapViewModel = mapViewModel;
            _vhManagerViewModel = vhManagerViewModel;
            _cameraLiveViewModel = clViewModel;
            _nbViewModel.SelectTabEvent += HandleSelectTab;
            _stViewModel.ConnectVehicle += new DelegateConnectVehicle(HandleConnectVehicle);
            _stViewModel.ConnectStreamEvent += new DelegateEnableVideoStream(HandleConnectStream);
            _stViewModel.DisConnectStreamEvent += new DelegateDisableVideoStream(HandleDisConnectStream);
            ListMode = new List<string>();
            ListMode.Add("MANUAL");
            ListMode.Add("POSCTL");
            ListMode.Add("OFFB");
            ListMode.Add("HOLD");
            ListMode.Add("LAND");
            SelectedTabIndex = 1;
        }

        private void HandleSelectTab(int index)
        {
            SelectedTabIndex = index;
            if(index == 1) // Fly tab 
            {
                _mapViewModel.IsShow = true;
                _stViewModel.IsShow = false;
                _cameraLiveViewModel.IsSelectFlyTab = true;
            }
            if(index == 0) // Setting tab
            {
                _mapViewModel.IsShow = false;
                _stViewModel.IsShow = true;
                _cameraLiveViewModel.IsSelectFlyTab = false;
            }    
        }

        private void HandleConnectVehicle(GCS_Com _mycom)
        {
            _vhManagerViewModel.ConnectToVehicle(_mycom);
        }

        private void HandleConnectStream(string url, bool isEnable)
        {
            _cameraLiveViewModel.IsEnable = isEnable;
            if(isEnable) _cameraLiveViewModel.ConnectCamera(url);
        }

        private void HandleDisConnectStream()
        {
            _cameraLiveViewModel.IsEnable = false;
            _cameraLiveViewModel.DisConnectCamera();
        }
    }
}
