using GCS_Comunication.Comunication;
using GCS_LAB411.Models;
using GCS_LAB411.ViewModels.SubViewsModel;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels
{
    public class VehicleManagerViewModel : BaseViewModel
    {
        public VehicleViewModel Vehicle { get; set; }
        public VehicleManagerViewModel()
        {
            Vehicle = new VehicleViewModel();
        }

        public void ConnectToVehicle(GCS_Com com)
        {
            Vehicle.Connect(com);
        }
    }
}
