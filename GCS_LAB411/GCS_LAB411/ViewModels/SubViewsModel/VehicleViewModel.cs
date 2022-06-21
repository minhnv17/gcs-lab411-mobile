using GCS_Comunication.Comunication;
using GCS_Comunication.Protocol;
using GCS_Comunication.Protocol.Uavlink;
using GCS_LAB411.Models;
using GCS_LAB411.Protocol.Uavlink;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using static GCS_Comunication.Comunication.GCS_Com;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class VehicleViewModel : BaseViewModel
    {
        private GCS_Com _com;
        public Telemetry telemetry { get; set; }

        public VehicleViewModel(GCS_Com com)
        {
            _com = com;
            telemetry = new Telemetry();
            _com.StateChanged += new DelegateState(handleStateChanged);
            _com.LocalPosChanged += new DelegateLocalPos(handleLocalPosChanged);
        }

        ~VehicleViewModel()
        {
            _com = null;
        }
        private void handleStateChanged(Uavlink_msg_state_t message)
        {
            telemetry.Arm = message.Armed == 0 ? false : true;
            telemetry.Connected = message.Connected == 0 ? false : true;
            telemetry.CurMode = (Telemetry.Mode)message.Mode;
            telemetry.Battery = message.Battery;
        }

        private void handleLocalPosChanged(Uavlink_msg_local_position_t message)
        {
            telemetry.PositionStatus = true;
            telemetry.PositionX = message.PosX;
            telemetry.PositionY = message.PosY;
        }
    }
}
