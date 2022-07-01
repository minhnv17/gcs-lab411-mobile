using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Extensions.DependencyInjection;
using GCS_LAB411.ViewModels.SubViewsModel;

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentView
    {
        private double x_wp, y_wp;
        private int max_width, max_height;

        private int _selectedWP;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _selectedWP = 1;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            _selectedWP = 0;
        }

        public MapView()
        {
            InitializeComponent();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MapViewModel>();
            waypoint.TranslationX = 50;
            x_wp = 50;
            waypoint.TranslationY = 50;
            y_wp = 50;
            max_width = 640;
            max_height = 360;
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Console.WriteLine("started");
                    break;
                case GestureStatus.Running:
                    switch (_selectedWP)
                    {
                        case 0:
                            Console.WriteLine("Map select");
                            break;
                        case 1:
                            Console.WriteLine("WP select");
                            waypoint.TranslationX = x_wp + e.TotalX;
                            waypoint.TranslationY = y_wp + e.TotalY;
                            if (waypoint.TranslationX >= max_width) waypoint.TranslationX = max_width - 30;
                            else if (waypoint.TranslationX <= 0) waypoint.TranslationX = 0;
                            if (waypoint.TranslationY >= max_height) waypoint.TranslationY = max_height - 30;
                            else if (waypoint.TranslationY <= 0) waypoint.TranslationY = 0;
                            break;
                    }
                    break;
                case GestureStatus.Completed:
                    switch(_selectedWP)
                    {
                        case 0:
                            break;
                        case 1:
                            Console.WriteLine("Completed");
                            x_wp = waypoint.TranslationX;
                            y_wp = waypoint.TranslationY;
                            break;
                    }
                    break;
            }
        }
    }
}