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
            TelemetryMSG.Mode = new List<string>();
            TelemetryMSG.Mode.Add("MANUAL");
            TelemetryMSG.Mode.Add("POSCTL");
            TelemetryMSG.Mode.Add("OFFB");
            TelemetryMSG.Mode.Add("LAND");
            TelemetryMSG.CurrentMode = TelemetryMSG.Mode[0];
        }

        ~VehicleViewModel()
        {
        }

        public void DoSendMission(Waypoint _wp)
        {
            if (_com != null)
            {
                _com.SendMissionMessage(_wp);
            }
            return;
        }

        public void DoChangeMode(int mode)
        {
            if (_com != null)
            {
                _com.SendCommandSetMode(mode);
            }
            return;
        }

        public async Task<Tuple<bool, string>> Takeoff(float altitude)
        {
            if(_com != null)
            {
                return await _com.SendCommandTakeoff(altitude);
            }
            return Tuple.Create(false, "NO COMLINK, CHECK CONNECTION");
        }

        public async Task<Tuple<bool, string>> Land()
        {
            if (_com != null)
            {
                return await _com.SendCommandLand();
            }
            return Tuple.Create(false, "NO COMLINK, CHECK CONNECTION");
        }

        public async Task<Tuple<bool, string>> Flyto(byte allwp, int wpid, int type)
        {
            if (_com != null)
            {
                return await _com.SendCommandFlyto(allwp, wpid, type);
            }
            return Tuple.Create(false, "NO COMLINK, CHECK CONNECTION");
        }

        public async Task<Tuple<bool, string>> ArmDisarm()
        {
            if (_com != null)
            {
                byte result;
                if (TelemetryMSG.Arm) result = 0;
                else result = 1;
                return await _com.SendCommandArmDisarm(result);
            }
            return Tuple.Create(false, "NO COMLINK, CHECK CONNECTION");
        }

        private void handleLinkedChanged(bool isLinked)
        {
            TelemetryMSG.IsLinked = isLinked;
        }

        private void handleStateChanged(Uavlink_msg_state_t message)
        {
            TelemetryMSG.Arm = message.Armed == 0 ? false : true;
            TelemetryMSG.Connected = message.Connected == 0 ? false : true;
            if(message.Mode != -1 && message.Mode < 4)
            {
                TelemetryMSG.CurrentMode = TelemetryMSG.Mode[message.Mode];
            }
            TelemetryMSG.Battery = message.Battery;
        }

        private void handleGlobalPosChanged(Uavlink_msg_global_position_t message)
        {
            TelemetryMSG.PositionStatus = true;
            TelemetryMSG.Latitude = message.Lat;
            TelemetryMSG.Longitude = message.Lon;
            TelemetryMSG.Altitude = message.Alt;
        }

        private void handleLocalPosChanged(Uavlink_msg_local_position_t message)
        {
            TelemetryMSG.PositionStatus = true;
            TelemetryMSG.PositionX = message.PosX;
            TelemetryMSG.PositionY = message.PosY;
            TelemetryMSG.PositionZ = message.PosZ;
        }

        public void Connect(GCS_Com com)
        {
            _com = com;
            _com.StateChanged += new DelegateState(handleStateChanged);
            _com.LocalPosChanged += new DelegateLocalPos(handleLocalPosChanged);
            _com.GlobalPosChanged += new DelegateGlobalPos(handleGlobalPosChanged);
            _com.LinkedChanged += new DelegateLinked(handleLinkedChanged);
        }

        public void DisConnect()
        {
            if(_com != null)
            {
                _com.Dispose();
                _com = null;
                TelemetryMSG.IsLinked = false;
            }
        }
    }
}
