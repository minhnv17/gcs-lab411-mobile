using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels
{
    class NavBarViewModel : BaseViewModel
    {
        public NavBarViewModel()
        {
            AddWayPointCommand = new Command(handleAddWaypoint);
        }
        public Command AddWayPointCommand { get; set; }
        public Command FlyViewCommand { get; set; }
        public Command SettingCommand { get; set; }

        private void handleAddWaypoint()
        {
            Console.WriteLine("test");
        }
    }
}
