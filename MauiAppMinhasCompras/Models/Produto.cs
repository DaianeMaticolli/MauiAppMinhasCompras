using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao;
        private double _quantidade;
        private double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Por favor, preencha a descrição!");
                }
                _descricao = value.Trim();
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("A quantidade deve ser maior que zero!");
                }
                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("O preço deve ser maior que zero!");
                }
                _preco = value;
            }
        }

        public double Total => Quantidade * Preco;
    }
}
