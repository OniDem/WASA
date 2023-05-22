using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new SellWindow();
            sellWindow.Show();
            Close();
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.Show();
            Close();
        }
    }
}
