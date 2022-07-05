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

        float previ_p, cur_p, posX;
        public MapView()
        {
            InitializeComponent();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MapViewModel>();
            posX = 0;
            // Load in all the available bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = assembly.GetManifestResourceNames();
            SKPoint position = new SKPoint();

            foreach (string resourceID in resourceIDs)
            {
                if (resourceID.EndsWith(".png") ||
                    resourceID.EndsWith(".jpg"))
                {
                    if(resourceID != "GCS_LAB411.Media.mymap.png")
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                        {
                            SKBitmap bitmap = SKBitmap.Decode(stream);
                            bitmapCollection.Add(new TouchManipulationBitmap(bitmap)
                            {
                                Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
                            });
                            position.X += 0;
                            position.Y += 0;
                        }
                    }
                }
            }
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
                    previ_p = point.X;
                    for (int i = bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
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
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        cur_p = bitmap.current_object.NewPoint.X;
                        bitmapDictionary.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    posX += cur_p - previ_p;
                    Console.WriteLine(posX);
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            string resourceID = "GCS_LAB411.Media.mymap.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                SKBitmap mapBitmap = SKBitmap.Decode(stream);
                float x2 = (info.Width - mapBitmap.Width) / 2;
                float y2 = (info.Height - mapBitmap.Height) / 2;
                canvas.DrawBitmap(mapBitmap, x2, y2);
            }

            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
        }
    }
}