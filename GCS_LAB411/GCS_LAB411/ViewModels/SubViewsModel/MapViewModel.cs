using GCS_LAB411.Commands;
using GCS_LAB411.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class MapViewModel : BaseViewModel
    {
        private SlideConfirmViewModel _scViewModel;
        public Waypoint _curentWP { get; set; }
        private VehicleManagerViewModel _vhManagerViewModel;
        public VehicleManagerViewModel VehicleManagerViewModel
        {
            get => _vhManagerViewModel;
        }
        public PilotCommand AutoPilotCommand { get; set; }
        public Command SendMissionCommand { get; set; }
        public MapViewModel(SlideConfirmViewModel scViewModel, VehicleManagerViewModel vhManagerViewModel)
        {
            _scViewModel = scViewModel;
            _vhManagerViewModel = vhManagerViewModel;
            _curentWP = new Waypoint();
            _curentWP.WaypointID = 1;
            _curentWP.PosX = 0;
            _curentWP.PosY = 0;
            _curentWP.IsComplete = false;
            AutoPilotCommand = new PilotCommand(this, scViewModel);
            SendMissionCommand = new Command(HandleSendMission);
        }

        private bool _isFlytabShow = true;
        public bool IsFlytabShow
        {
            get => _isFlytabShow;
            set => SetProperty(ref _isFlytabShow, value);
        }

        private bool _isMissionShow = false;
        public bool IsMissionShow
        {
            get => _isMissionShow;
            set => SetProperty(ref _isMissionShow, value);
        }

        private bool _isMapShow = true;
        public bool IsMapShow
        {
            get => _isMapShow;
            set => SetProperty(ref _isMapShow, value);
        }

        public void HandleChangeMode(int mode)
        {
            _vhManagerViewModel.Vehicle.DoChangeMode(mode);
        }

        public void HandleSendMission(object obj)
        {
            _vhManagerViewModel.Vehicle.DoSendMission(_curentWP);
        }

        public async Task<Tuple<bool, string>> FlytoAction(byte allwp, int wpid, int type)
        {
            
            return await _vhManagerViewModel.Vehicle.Flyto(allwp, wpid, type);
        }

        public async Task<Tuple<bool, string>> Takeoff(float altitude)
        {
            return await _vhManagerViewModel.Vehicle.Takeoff(altitude);
        }

        public async Task<Tuple<bool, string>> Land()
        {
            return await _vhManagerViewModel.Vehicle.Land();
        }

        public async Task<Tuple<bool, string>> ArmDisarm()
        {
            return await _vhManagerViewModel.Vehicle.ArmDisarm();
        }
    }
}
