using GCS_Comunication.Comunication;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class SettingViewModel : BaseViewModel
    {
        public delegate void DelegateConnectVehicle(GCS_Com _mycom);
        public delegate void DelegateEnableVideoStream(string videoSourde, bool enable);
        public delegate void DelegateDisableVideoStream();
        public event DelegateConnectVehicle ConnectVehicle;
        public event DelegateEnableVideoStream ConnectStreamEvent;
        public event DelegateDisableVideoStream DisConnectStreamEvent;

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

        private string _hostName;
        public string HostName
        {
            get => _hostName;
            set => SetProperty(ref _hostName, value);
        }

        private int _port;
        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private string _videoSource;
        public string VideoSource
        {
            get => _videoSource;
            set => SetProperty(ref _videoSource, value);
        }

        private bool _isLiveEnable;
        public bool IsLiveEnable
        {
            get => _isLiveEnable;
            set
            {
                SetProperty(ref _isLiveEnable, value);
                if (value == true) ConnectStreamEvent?.Invoke(_videoSource, value);
                else DisConnectStreamEvent?.Invoke();
            }
        }

        private Udp_Connect _myUdp;
        private GCS_Com _myCom;
        public GCS_Com MyCom
        {
            get => _myCom;
        }

        public Command SelectedTabCommand { get; set; }
        public Command ConnectCommand { get; set; }
        public Command DisConnectCommand { get; set; }
        public Command CompletedVideoSourceCommand { get; set; }
        public SettingViewModel()
        {
            SelectedTabCommand = new Command(HandleSelectedTabCommand);
            ConnectCommand = new Command(HandleConnectCommand);
            DisConnectCommand = new Command(HandleDisConnectCommand);
            CompletedVideoSourceCommand = new Command(HandleCompletedSourceCommand);
            // Default
            HostName = "192.168.11.1";
            Port = 12345;
            _videoSource = "http://192.168.0.122:8080/stream?topic=/main_camera/image_raw";
        }

        private void HandleSelectedTabCommand(object obj)
        {
            if(obj != null)
            {
                int index = int.Parse(obj as string);
                SelectedTabIndex = index;
            }
        }

        private void HandleConnectCommand(object obj)
        {
            _myUdp = new Udp_Connect(_hostName, _port);
            _myUdp.DoConnect();
            if (_myUdp.IsOpen)
            {
                _myCom = new GCS_Com(_myUdp);
                ConnectVehicle?.Invoke(_myCom);
            }
        }

        private void HandleDisConnectCommand(object obj)
        {
            _myUdp.DoDisconnect();
        }

        private void HandleCompletedSourceCommand(object obj)
        {
            if(_isLiveEnable) ConnectStreamEvent?.Invoke(_videoSource, _isLiveEnable);
        }
    }
}
