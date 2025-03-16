using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent(); // Certifique-se que o nome da classe no XAML está correto
        lista_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing(); // Chamada base necessária

        lista.Clear(); // Evita duplicação de produtos

        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue?.Trim();
        lista.Clear();

        if (!string.IsNullOrEmpty(q))
        {
            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
        else
        {
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total é {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.BindingContext is Produto produto)
        {
            lista.Remove(produto);
        }
    }
}

