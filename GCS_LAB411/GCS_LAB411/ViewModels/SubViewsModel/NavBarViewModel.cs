using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.ViewModels
{
    public class NavBarViewModel : BaseViewModel
    {
        public delegate void SelectTabDelegate(int index);
        public event SelectTabDelegate SelectTabEvent;
        public Command AddWayPointCommand { get; set; }
        public Command ChangedViewCommand { get; set; }
        public NavBarViewModel()
        {
            ChangedViewCommand = new Command(HandleChangedViewCommand);
        }

        private void HandleChangedViewCommand(object obj)
        {
            if(obj != null)
            {
                int index = int.Parse(obj as string);
                SelectTabEvent?.Invoke(index);
            }
        }
    }
}
