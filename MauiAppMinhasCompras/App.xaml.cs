using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Views;
using System.IO;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        // Propriedade para acessar o banco de dados SQLite
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    // Define o caminho do banco de dados SQLite no dispositivo
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_compras.db3");
                    // Instancia o SQLiteDatabaseHelper passando o caminho do banco
                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db;
            }
        }

        // Construtor da classe App
        public App()
        {
            InitializeComponent();
            // Define a página principal como uma NavigationPage com a tela ListaProduto
            MainPage = new NavigationPage(new ListaProduto());
        }
    }
}
