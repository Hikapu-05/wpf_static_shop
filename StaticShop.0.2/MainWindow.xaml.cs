using Microsoft.EntityFrameworkCore;
using StaticShop._0._2.Models;
using System.Collections.ObjectModel;
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

namespace StaticShop._0._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class CategoryFilter
    {
        public int id { get; set; }
        public string Name { get; set; }
        public bool isChecked { get; set; }
    }




    public partial class MainWindow : Window
    {

        public ObservableCollection<CategoryFilter> categoryFilters { get; set; }
        public ObservableCollection<Product> catalog {  get; set; }
        public ObservableCollection<CartItem> cartItems { get; set; }
        public ObservableCollection<Favorite> favoriteitems { get; set; }
        public ObservableCollection<OrderItem > orderitems { get; set; }
  
        public MainWindow()
        {
            InitializeComponent();
            loaddata();
            loadfilter();
            loadcatalog();
            DataContext = this;
        }

        void loaddata()
        {
            balance.Text = $"{Session.CurrentUser.Balance.ToString()} руб ";
            Username.Text = Session.CurrentUser.Login;
        }

        void loadfilter()
        {
            using (var db = new AppDbContext())
            {
                categoryFilters = new ObservableCollection<CategoryFilter>(db.Categories.Select(c => new CategoryFilter { id = c.Id, Name = c.Name, isChecked = false}));

            }
            CategoryList.ItemsSource = categoryFilters;
        }

        private void loadcatalog()
        {
            using (var db = new AppDbContext())
            {
                catalog = new ObservableCollection<Product>(db.Products.Include( p=> p.Category).ToList());

            }
            ProductGrid.ItemsSource = catalog;
           
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            catalog.Clear();
            var text = Searchtext.Text;
            using (var db = new AppDbContext())
            {  
              var  res = new ObservableCollection<Product>(db.Products.Include(p => p.Category).Where(p => p.Name.Contains(text)).ToList());
              foreach (var el in res)
              {
                    catalog.Add(el);
              }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            catalog.Clear();
            var ot = Ot.Text;
            var _do = Do.Text;
            using (var db = new AppDbContext())
            {
                var SelectedCategories = categoryFilters.Where(c => c.isChecked).Select(c => c.id).ToList();
          
                var res = new ObservableCollection<Product>(db.Products.Include(p => p.Category).Where(p => (string.IsNullOrWhiteSpace(ot) || p.Price >= Convert.ToInt32(ot)) && ( string.IsNullOrWhiteSpace(_do) || p.Price <= Convert.ToInt32(_do) ) && (SelectedCategories.Count == 0 || SelectedCategories.Contains(p.CategoryId) )));
                foreach (var el in res)
                {
                    catalog.Add(el);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int quantity = 1;
            var product = ProductGrid.SelectedItem as StaticShop._0._2.Models.Product;
            if (product == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }
            using (var db = new AppDbContext())
            {
                var userid =  Session.CurrentUser.Id;

                var item = db.CartItems.FirstOrDefault(c => c.UserId == userid && c.ProductId == product.Id);
                if (item == null) 
                {
                    db.CartItems.Add(new CartItem { ProductId = product.Id, UserId = userid, Quantity = quantity });
                    db.SaveChanges();
                }
                else
                {
                    item.Quantity += 1;
                    db.SaveChanges();
                }
             
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int quantity = 1;
            var product = ProductGrid.SelectedItem as StaticShop._0._2.Models.Product;
            if (product == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }
            using (var db = new AppDbContext())
            {
                var userid = Session.CurrentUser.Id;

                var item = db.Favorites.Any(c => c.UserId == userid && c.ProductId == product.Id);
                if (!item)
                {
                    db.Favorites.Add(new Favorite { ProductId = product.Id, UserId = userid });
                    db.SaveChanges();
                    MessageBox.Show("Товар добавлен в избранное");
                }
                else
                {
                    MessageBox.Show("Товар уже добавлен в избранное");
                   
                }

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 0;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
            Cartload();

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 2;
            Favoriteload();


        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 3;
            HistoryLoad();
        }

        public void Cartload()
        {
            using (var db = new AppDbContext())
            {
                var userid = Session.CurrentUser.Id;
                cartItems = new ObservableCollection<CartItem>(db.CartItems.Where(c => c.UserId == userid).Include(c => c.Product).ThenInclude(c => c.Category).ToList());
                CartGrid.ItemsSource = cartItems;
            }
    
        }
        private void Favoriteload()
        {
            using (var db = new AppDbContext())
            {
                var userid = Session.CurrentUser.Id;
                favoriteitems = new ObservableCollection<Favorite>(db.Favorites.Where(c => c.UserId == userid).Include(c => c.Product).ThenInclude(c => c.Category).ToList());
            }
            FavoriteGrid.ItemsSource = favoriteitems;
        }

        private void HistoryLoad()
        
        {
            using (var db = new AppDbContext())
            {
                var userid = Session.CurrentUser.Id;
                orderitems = new ObservableCollection<OrderItem>(db.OrderItems.Where(c => c.Order.UserId == userid).Include(c => c.Order).Include(c => c.Product).ThenInclude(c => c.Category).ToList());
            }
            HistoryGrid.ItemsSource = orderitems;
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var len = CartGrid.SelectedItem as CartItem;
            if (len == null)
            {
                MessageBox.Show("Вы не выбрали товар");
                return;
            }
            using (var db = new AppDbContext())
            {
                var item = db.CartItems.Find(len.Id);
                if (item  != null)
                {
                    db.CartItems.Remove(item);
                    db.SaveChanges();
                }
            }
            cartItems.Remove(len);
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            var select = CartGrid.SelectedItem as CartItem;
            if (select == null)
            {
                MessageBox.Show("Вы не выбрали товар");
                return;
            }
            else
            {
                var ok = MessageBox.Show("Подтвердить покупку?"," ", MessageBoxButton.YesNo);
                if (ok == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        var userid = Session.CurrentUser.Id;
                        DateTime OrderDate = DateTime.Now;

                        var item = db.CartItems.FirstOrDefault(c => c.Id == select.Id);

                        var k = Convert.ToDouble(select.Product.Price);
                        var order = new Order { UserId = userid, OrderDate = OrderDate, Status = "в процессе", Cost = (Convert.ToDouble(select.Quantity) * k) };

                        db.Orders.Add(order);
                        db.SaveChanges();
                        db.OrderItems.Add(new OrderItem { ProductId = select.ProductId, Price = select.Product.Price, Quantity = select.Quantity, OrderId = order.Id });

                        var user = db.Users.Find(Session.CurrentUser.Id);
                        var p = item.ProductId;
                        var product = db.Products.Find(p);

                       
                        if (user.Balance >= order.Cost)
                        {
                            user.Balance -= order.Cost;
                            if (product.Quantity >= item.Quantity)
                            {

                                product.Quantity -= item.Quantity;
                            }
                            else
                            {
                                MessageBox.Show("недостаточно товаров на складе");
                                return;
                            }

                        }
                        else
                        {
                            MessageBox.Show("недостаточно средств");
                            return;
                        }

                        db.CartItems.Remove(item);
                        db.SaveChanges();
                    }
                    cartItems.Remove(select);
                    loadcatalog();
                }
                else
                {
                    return;
                }
                
            }
            
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            var len = FavoriteGrid.SelectedItem as Favorite;
            if (len == null)
            {
                MessageBox.Show("Вы не выбрали товар");
                return;
            }
            using (var db = new AppDbContext())
            {
                var item = db.Favorites.Find(len.Id);
                if (item != null)
                {
                    db.Favorites.Remove(item);
                    db.SaveChanges();
                }
            }
            favoriteitems.Remove(len);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var len = FavoriteGrid.SelectedItem as Favorite;
            if (len == null)
            {
                MessageBox.Show("Вы не выбрали товар");
                return;
            }
            using(var db = new AppDbContext())
            {
                var item = db.Favorites.Find(len.Id);
                db.CartItems.Add(new CartItem { ProductId=item.ProductId, UserId=item.UserId, Quantity = 1 });
                
                db.Favorites.Remove(item) ;
                db.SaveChanges();
            }
            favoriteitems.Remove(len);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
 }
    