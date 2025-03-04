using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if(_db == null)
                {
                    _db = new SQLiteDatabaseHelper("....... db3");
                }
                return _db;
            }
                    }
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListarProduto());
        }
    }
}
