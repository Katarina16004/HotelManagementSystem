using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool vidljivostLozinke = false;
        public MainWindow()
        {
            InitializeComponent();
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            EyeIcon.Visibility = Visibility.Collapsed;
            UsernameTextBox.Focus();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
                EyeIcon.Visibility = Visibility.Collapsed;
            else
                EyeIcon.Visibility = Visibility.Visible;
        }

        private void EyeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (vidljivostLozinke)
            {
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Password = PasswordTextBox.Text;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/otvoreno_oko.png", System.UriKind.Relative));
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordBox.Password;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/zatvoreno_oko.png", System.UriKind.Relative));
            }

            vidljivostLozinke = !vidljivostLozinke;
        }
    }
}