using GCS_LAB411.ViewModels.SubViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideConfirm : ContentView
    {
        public SlideConfirm()
        {
            InitializeComponent();
            this.BindingContext = new SlideConfirmViewModel();
        }
    }
}