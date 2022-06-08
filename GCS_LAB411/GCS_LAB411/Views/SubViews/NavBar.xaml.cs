﻿using GCS_LAB411.ViewModels;
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
    public partial class NavBar : ContentView
    {
        public NavBar()
        {
            InitializeComponent();
            this.BindingContext = new NavBarViewModel();
        }
    }
}