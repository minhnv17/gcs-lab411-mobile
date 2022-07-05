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

namespace GCS_LAB411.Views.SubViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentView
    {
        // Bitmap and matrix for display
        SKBitmap bitmap;
        SKMatrix matrix = SKMatrix.MakeIdentity();
        // Touch information
        long touchId = -1;
        SKPoint previousPoint;
        public MapView()
        {
            InitializeComponent();
            this.BindingContext = App.ServiceProvider.GetRequiredService<MapViewModel>();

            string resourceID = "GCS_LAB411.Media.waypoint.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                bitmap = SKBitmap.Decode(stream);
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Display the bitmap
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(bitmap, new SKPoint());

            using (var paint = new SKPaint())
            {
                paint.TextSize = 13.0f;
                paint.IsAntialias = true;
                paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
                paint.TextAlign = SKTextAlign.Center;

                canvas.DrawText("ID : 1", bitmap.Width, bitmap.Height, paint);
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
                    // Find transformed bitmap rectangle
                    SKRect rect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                    rect = matrix.MapRect(rect);

                    // Determine if the touch was within that rectangle
                    if (rect.Contains(point))
                    {
                        touchId = args.Id;
                        previousPoint = point;
                    }
                    break;

                case TouchActionType.Moved:
                    if (touchId == args.Id)
                    {
                        // Adjust the matrix for the new position
                        matrix.TransX += point.X - previousPoint.X;
                        matrix.TransY += point.Y - previousPoint.Y;
                        previousPoint = point;
                        Console.WriteLine(matrix.TransX);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    touchId = -1;
                    break;
            }
        }
    }
}