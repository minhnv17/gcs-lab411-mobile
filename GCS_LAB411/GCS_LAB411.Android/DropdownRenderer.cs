using System.ComponentModel;
using Android.Content;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using GCS_LAB411.Droid;
using GCS_LAB411.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DropDown), typeof(DropdownRenderer))]
namespace GCS_LAB411.Droid
{
    public class DropdownRenderer : ViewRenderer<DropDown, AppCompatSpinner>
    {
        AppCompatSpinner spinner;
        public DropdownRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<DropDown> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                spinner = new AppCompatSpinner(Context);
                SetNativeControl(spinner);
            }

            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }
            if (e.NewElement != null)
            {
                var view = e.NewElement;

                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;

                if (view.SelectedIndex != -1)
                {
                    Control.SetSelection(view.SelectedIndex);
                }

                Control.ItemSelected += OnItemSelected;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = Element;
            Control.DropDownVerticalOffset = 55;
            if (e.PropertyName == DropDown.ItemsSourceProperty.PropertyName)
            {
                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;
            }
            if (e.PropertyName == DropDown.SelectedIndexProperty.PropertyName)
            {
                Control.SetSelection(view.SelectedIndex);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var view = Element;
            if (view != null)
            {
                view.SelectedIndex = e.Position;
                view.OnItemSelected(e.Position);
            }
        }
    }
}