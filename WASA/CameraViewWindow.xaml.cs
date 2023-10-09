using AForge.Video.DirectShow;
using System;
using System.Windows;
using ZXing.SkiaSharp;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;
using static WASA.SellWindow;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для CameraViewWindow.xaml
    /// </summary>
    public partial class CameraViewWindow : Window
    {

        FilterInfoCollection? filterInfoCollection;
        VideoCaptureDevice? videoCaptureDevice;

        public CameraViewWindow()
        {
            InitializeComponent();
            Task.Run(async () => await Dispatcher.InvokeAsync(() =>
            {
                filterInfoCollection = new(FilterCategory.VideoInputDevice);
                foreach (FilterInfo device in filterInfoCollection)
                {
                    cboCamera.Items.Add(device.Name);
                    cboCamera.SelectedIndex = 0;
                }
                videoCaptureDevice = new(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();
            }));
           
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            videoCaptureDevice = new(filterInfoCollection![cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap cameraFrame = (Bitmap)eventArgs.Frame.Clone();
            SKBitmap bitmap = cameraFrame.ToSKBitmap();
            pictureBox1.Image = bitmap.ToBitmap();
            Task.Run(async () => await Dispatcher.InvokeAsync(() =>
            {
                var reader = new BarcodeReader
                {
                };
                var result = reader.Decode(bitmap);
                if (reader != null && result != null)
                {
                    txtBarcode.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        txtBarcode.Text = result.ToString();
                        if (BarcodeClass.BarcodeSended == false && result.Text.Length == 13)
                        {
                            BarcodeClass.Barcode = result.Text;
                            BarcodeClass.BarcodeSended = true;
                            txtBarcode.Text = "";
                        }
                    }));
                }
            }));
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.SignalToStop();
                }
            }
        }


        private void btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void txtBarcode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
        }

        private void cboCamera_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.SignalToStop();
                }
                else
                {
                    Task.Run(async () => await Dispatcher.InvokeAsync(() =>
                    {
                        videoCaptureDevice = new(filterInfoCollection![cboCamera.SelectedIndex].MonikerString);
                        videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                        videoCaptureDevice.Start();
                    }));
                }
            }
        }
        
    }
}