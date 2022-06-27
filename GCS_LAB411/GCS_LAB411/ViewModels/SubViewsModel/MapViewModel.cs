using GCS_LAB411.Commands;
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
        public PilotCommand AutoPilotCommand { get; set; }
        public Command tabtab { get; set; }
        public MapViewModel(SlideConfirmViewModel scViewModel)
        {
            _scViewModel = scViewModel;
            AutoPilotCommand = new PilotCommand(this, scViewModel);
            tabtab = new Command(HandleTab);
        }

        private bool _isShow = true;
        public bool IsShow
        {
            get => _isShow;
            set => SetProperty(ref _isShow, value);
        }

        private void HandleTab(object obj)
        {
            Console.WriteLine("test");
            Console.WriteLine(obj);
        }
        public async Task<Tuple<bool, string>> FlytoAction()
        {
            return await Task.FromResult(Tuple.Create(false, ""));
        }
    }
}
