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
        public NavBarViewModel()
        {
            ChangedViewCommand = new Command(HandleChangedViewCommand);
        }
        public Command AddWayPointCommand { get; set; }
        public Command ChangedViewCommand { get; set; }

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
