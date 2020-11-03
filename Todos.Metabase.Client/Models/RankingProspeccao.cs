using System.ComponentModel.DataAnnotations;

namespace Todos.Metabase.Client.Models
{
    public class RankingProspeccao
    {
        [Display(Name = "Regional")]
        public string Regional { get; set; }

        [Display(Name = "Franquia")]
        public string Franquia { get; set; }
        
        [Display(Name = "Vendedor")]
        public string Vendedor { get; set; }
        
        [Display(Name = "Total de Vendas")]
        public long TotalVendas { get; set; }
        
        [Display(Name = "Vendas Cartão Cred.")]
        public long VendasCartaoCredito { get; set; }
        
        [Display(Name = "Porcentagem CC")]
        public double PorcentagemCC { get; set; }
    }
}
