using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todos.Metabase.Client.Models;

namespace Todos.Metabase.Client.Web.Pages
{
    public class RankingProspeccaoModel : PageModel
    {
        [BindProperty]
        public string Session { get; set; }

        [BindProperty]
        public DateTime Inicio { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime Fim { get; set; } = DateTime.Now;

        public RankingProspeccao[] Ranking { get; set; } = Array.Empty<RankingProspeccao>();

        private readonly QueryClient _client;

        public RankingProspeccaoModel(QueryClient client)
        {
            _client = client;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            var result = await _client.ConsultarRankingProspeccao(Inicio, Fim, Session);
            if (result.Success)
            {
                Ranking = result.Data.ToArray();
            }
        }
    }
}
