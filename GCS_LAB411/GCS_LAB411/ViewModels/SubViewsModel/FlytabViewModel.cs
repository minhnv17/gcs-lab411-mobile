using GCS_LAB411.Commands;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCS_LAB411.ViewModels.SubViewsModel
{
    public class FlytabViewModel : BaseViewModel
    {
        public PilotCommand FlytoCommand { get; set; }
        private SlideConfirmViewModel _scViewModel;
        public FlytabViewModel(SlideConfirmViewModel scViewModel)
        {
            _scViewModel = scViewModel;
            FlytoCommand = new PilotCommand(this, scViewModel);
        }

        public async Task<Tuple<bool, string>> FlytoAction()
        {
            return await Task.FromResult(Tuple.Create(false, ""));
        }
    }
}
