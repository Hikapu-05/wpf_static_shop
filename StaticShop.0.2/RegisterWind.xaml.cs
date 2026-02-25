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
    /// <summary>
    /// Логика взаимодействия для RegisterWind.xaml
    /// </summary>
    public partial class RegisterWind : Window
    {
        public RegisterWind()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new LoginWind();
            win.Show();
            this.Close();
            Application.Current.MainWindow = win;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                if (exist == null)
                {
                    if ( RegisterPass.Text == RerRegPass.Text && !string.IsNullOrWhiteSpace(RerRegPass.Text) && !string.IsNullOrWhiteSpace(RerRegPass.Text))
                    {
                        db.Users.Add(new User { Login = username, Password = RegisterPass.Text });
                        db.SaveChanges();
                        MessageBox.Show($"Hello,{username}!");
                    }
                    else
                    {
                        MessageBox.Show("Passwords don`t match");
                    }
                    

                }
                else
                {
                    MessageBox.Show("This user is exist");
                    var win = new LoginWind();
                    win.Show();
                    this.Close();
                    Application.Current.MainWindow = win;
                }

            }
        }
    }
}
