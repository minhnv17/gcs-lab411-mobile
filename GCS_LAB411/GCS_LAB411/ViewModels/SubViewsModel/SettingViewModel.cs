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

        private Udp_Connect _myUdp;
        private GCS_Com _myCom;

        public Command SelectedTabCommand { get; set; }
        public Command ConnectCommand { get; set; }
        public Command DisConnectCommand { get; set; }
        
        public SettingViewModel()
        {
            SelectedTabCommand = new Command(HandleSelectedTabCommand);
            ConnectCommand = new Command(HandleConnectCommand);
            DisConnectCommand = new Command(HandleDisConnectCommand);

            // Default
            HostName = "192.168.11.1";
            Port = 12345;
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
            }
        }

        private void HandleDisConnectCommand(object obj)
        {
            _myUdp.DoDisconnect();
        }
    }
}
