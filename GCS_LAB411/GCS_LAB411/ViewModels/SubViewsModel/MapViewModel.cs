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
        public Command tabtab { get; set; }
        public PilotCommand FlytoCommand { get; set; }
        public MapViewModel(SlideConfirmViewModel scViewModel)
        {
            tabtab = new Command(HandleTabtab);
            _scViewModel = scViewModel;
            FlytoCommand = new PilotCommand(this, scViewModel);
        }

        private void HandleTabtab(object obj)
        {
            Console.WriteLine("aaaaaa");
        }

        private bool _isShow = true;
        public bool IsShow
        {
            get => _isShow;
            set => SetProperty(ref _isShow, value);
        }

        public async Task<Tuple<bool, string>> FlytoAction()
        {
            return await Task.FromResult(Tuple.Create(false, ""));
        }
    }
}
