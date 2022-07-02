using GCS_LAB411.ViewModels;
using GCS_LAB411.ViewModels.SubViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavBar : ContentView
    {
        private MainViewModel _mainViewModel;
        public NavBar()
        {
            InitializeComponent();
            _mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}