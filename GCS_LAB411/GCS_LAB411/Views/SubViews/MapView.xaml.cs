using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Extensions.DependencyInjection;
using GCS_LAB411.ViewModels.SubViewsModel;
using SkiaSharp;
using System.Reflection;
using System.IO;
using SkiaSharp.Views.Forms;
using TouchTracking;
using GCS_LAB411.TouchTracking;

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentView
    {
        List<TouchManipulationBitmap> bitmapCollection =
            new List<TouchManipulationBitmap>();

        Dictionary<long, TouchManipulationBitmap> bitmapDictionary =
            new Dictionary<long, TouchManipulationBitmap>();

        MapViewModel _mapViewModel;
        SKBitmap mapBitmap, droneBitmap;
        SKPoint previ_p, cur_p, waypoint, drone_pos;
        public MapView()
        {
            InitializeComponent();
            _mapViewModel = App.ServiceProvider.GetRequiredService<MapViewModel>();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MapViewModel>();
            // Load in all the available bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = assembly.GetManifestResourceNames();
             
            foreach (string resourceID in resourceIDs)
            {
                if (resourceID.EndsWith(".png") ||
                    resourceID.EndsWith(".jpg"))
                {
                    if(resourceID != "GCS_LAB411.Media.map.png"
                        && resourceID != "GCS_LAB411.Media.drone.png")
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                        {
                            SKBitmap bitmap = SKBitmap.Decode(stream);
                            bitmapCollection.Add(new TouchManipulationBitmap(bitmap));
                        }
                    }
                }
            }

            Device.StartTimer(TimeSpan.FromSeconds(0.1), () =>
            {
                if(mapBitmap!=null)
                {
                    drone_pos.X = _mapViewModel.VehicleManagerViewModel.Vehicle.TelemetryMSG.PositionX * mapBitmap.Width / 4;
                    drone_pos.Y = _mapViewModel.VehicleManagerViewModel.Vehicle.TelemetryMSG.PositionY * mapBitmap.Height / 4;
                }
                canvasView.InvalidateSurface();
                return true;
            });
        }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    for (int i = bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
                            previ_p = point;
                            // Move bitmap to end of collection
                            bitmapCollection.Remove(bitmap);
                            bitmapCollection.Add(bitmap);
                            // Do the touch processing
                            bitmapDictionary.Add(args.Id, bitmap);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            canvasView.InvalidateSurface();
                            break;
                        }
                    }
                    break;

                case TouchActionType.Moved:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        cur_p = bitmap.current_object.NewPoint;
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        waypoint += cur_p - previ_p;
                        _mapViewModel._curentWP.PosX = TransformArucoMapX(waypoint.X + 32f);
                        _mapViewModel._curentWP.PosY = TransformArucoMapY(waypoint.Y + 64f);
                        bitmapDictionary.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            mapBitmap?.Dispose();
            droneBitmap?.Dispose();

            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream("GCS_LAB411.Media.map.png"))
            {
                mapBitmap = SKBitmap.Decode(stream);
                canvas.DrawBitmap(mapBitmap, 0, 0);
            }

            using (Stream stream = assembly.GetManifestResourceStream("GCS_LAB411.Media.drone.png"))
            {
                droneBitmap = SKBitmap.Decode(stream);
                canvas.DrawBitmap(droneBitmap, drone_pos.X, drone_pos.Y);
            }

            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
        }

        float TransformArucoMapX(float value)
        {
            float result;
            result = value / mapBitmap.Width * 4;
            if (result >= 4) return 4f;
            if (result <= 0) return 0f;
            return result;
        }

        float TransformArucoMapY(float value)
        {
            float result;
            result = value / mapBitmap.Height * 4;
            if (result >= 4f) return 4f;
            if (result <= 0f) return 0f;
            return result;
        }
    }
}