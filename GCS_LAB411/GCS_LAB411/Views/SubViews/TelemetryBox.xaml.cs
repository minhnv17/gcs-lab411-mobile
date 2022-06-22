using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Extensions.DependencyInjection;
using GCS_LAB411.ViewModels;

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TelemetryBox : ContentView
    {
        public TelemetryBox()
        {
            InitializeComponent();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}