using GCS_Comunication.Comunication;
using GCS_Comunication.Protocol;
using GCS_Comunication.Protocol.Uavlink;
using GCS_LAB411.Models;
using GCS_LAB411.Protocol.Uavlink;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static GCS_Comunication.Comunication.GCS_Com;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class VehicleViewModel : BaseViewModel
    {
        private GCS_Com _com;
        public Telemetry TelemetryMSG { get; set; }

        public VehicleViewModel()
        {
            TelemetryMSG = new Telemetry();
        }

        ~VehicleViewModel()
        {
            TelemetryMSG = null;
        }

        public async Task<Tuple<bool, string>> Takeoff(float altitude)
        {
            return await _com.SendCommandTakeoff(altitude);
        }

        private void handleStateChanged(Uavlink_msg_state_t message)
        {
            TelemetryMSG.Arm = message.Armed == 0 ? false : true;
            TelemetryMSG.Connected = message.Connected == 0 ? false : true;
            TelemetryMSG.CurMode = (Telemetry.Mode)message.Mode;
            TelemetryMSG.Battery = message.Battery;
        }

        private void handleLocalPosChanged(Uavlink_msg_local_position_t message)
        {
            TelemetryMSG.PositionStatus = true;
            TelemetryMSG.PositionX = message.PosX;
            TelemetryMSG.PositionY = message.PosY;
        }

        public void Connect(GCS_Com com)
        {
            _com = com;
            _com.StateChanged += new DelegateState(handleStateChanged);
            _com.LocalPosChanged += new DelegateLocalPos(handleLocalPosChanged);
        }

        public void DisConnect()
        {
            _com.Dispose();
            TelemetryMSG = null;
        }
    }
}
