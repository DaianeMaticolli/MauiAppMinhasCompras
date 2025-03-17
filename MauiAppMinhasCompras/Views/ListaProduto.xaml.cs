using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();  // Lista original de produtos
    ObservableCollection<Produto> listaFiltrada = new ObservableCollection<Produto>(); // Coleção filtrada para a busca dinâmica
    CancellationTokenSource cts; // Usado para cancelar buscas anteriores

    public ListaProduto()
    {
        InitializeComponent();
        lista_produtos.ItemsSource = listaFiltrada; // Define a fonte de dados para o ListView (que exibirá a lista filtrada)
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        lista.Clear();  // Limpa a lista antes de carregar os dados do banco

        // Recupera todos os produtos do banco de dados e os adiciona na lista original
        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));

        // Exibe todos os produtos na lista filtrada inicialmente
        listaFiltrada = new ObservableCollection<Produto>(lista);
        lista_produtos.ItemsSource = listaFiltrada;
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new NovoProduto()); // Navega para a página de cadastro de novos produtos
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK"); // Exibe erro caso ocorra uma exceção
        }
    }

    // Método chamado sempre que o texto no SearchBar é alterado
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue?.Trim(); // Pega o texto digitado no SearchBar e remove espaços extras

        cts?.Cancel(); // Cancela a busca anterior, caso haja alguma em andamento
        cts = new CancellationTokenSource(); // Cria uma nova instância de CancellationTokenSource para a nova busca

        // Aguarda 300ms antes de realizar a nova busca para evitar chamadas excessivas ao banco de dados
        await Task.Delay(300, cts.Token);

        // Executa a busca de maneira assíncrona
        await Task.Run(async () =>
        {
            if (!string.IsNullOrEmpty(q)) // Se o texto da busca não for vazio
            {
                // Realiza a busca no banco de dados com base no texto fornecido
                List<Produto> tmp = await App.Db.Search(q);
                UpdateListView(tmp); // Atualiza a lista filtrada com os resultados da busca
            }
            else
            {
                // Se o texto da busca estiver vazio, exibe todos os produtos
                List<Produto> tmp = await App.Db.GetAll();
                UpdateListView(tmp); // Atualiza a lista com todos os produtos
            }
        });
    }

    // Atualiza a lista exibida no ListView com os novos produtos
    private void UpdateListView(List<Produto> produtos)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            listaFiltrada.Clear(); // Limpa a lista filtrada
            produtos.ForEach(i => listaFiltrada.Add(i)); // Adiciona os produtos encontrados à lista filtrada
        });
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Calcula a soma total dos valores dos produtos
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total é {soma:C}"; // Exibe o total formatado como moeda
        DisplayAlert("Total dos Produtos", msg, "OK"); // Exibe o alerta com o total
    }

    // Método chamado quando um produto é removido da lista
    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.BindingContext is Produto produto)
        {
            // Remove o produto da lista original e da lista filtrada
            lista.Remove(produto);
            listaFiltrada.Remove(produto);
        }
    }
}

