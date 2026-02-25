using StaticShop._0._2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StaticShop._0._2
{
    public static class Session
    {
        public static User CurrentUser { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для LoginWind.xaml
    /// </summary>
    public partial class LoginWind : Window
    {

        
        bool _isPassVis = false;
        public LoginWind()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new RegisterWind();
            win.Show();
            this.Close();
            Application.Current.MainWindow = win;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_isPassVis)
            {
                PasswordScript.Password = PasswordText.Text;
                PasswordScript.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
                _isPassVis = false;
            }
            else
            {
              
                PasswordText.Text = PasswordScript.Password;
                PasswordText.Visibility = Visibility.Visible;
                PasswordScript.Visibility = Visibility.Collapsed;
                _isPassVis = true;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var win = new ForgotPassword();
            win.Show();
            this.Close();
            Application.Current.MainWindow = win;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();
            string? username = Username.Text;
      
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("You didn`t fill in the username field");
            }
            else
            {
                var exist = db.Users.FirstOrDefault(u => u.Login == username);
                if (exist != null)
                {
                    if (( _isPassVis && exist.Password == PasswordText.Text) || (!_isPassVis && exist.Password == PasswordScript.Password))
                    {
                        Session.CurrentUser = exist;
                        var win = new MainWindow();
                        win.Show();
                        this.Close();
                        Application.Current.MainWindow = win;
                    }
                    else
                    {
                        MessageBox.Show("Username or paswworsd is incorect");
                    }
              
                }
                else
                {
                    MessageBox.Show("This user doesn`t exist");
                }
            }
            

        }


    }
}
