using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Todos.Metabase.Client;

namespace Todos.MetabaseClient.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private SessionClient _sessionClient;

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Session { get; private set; }

        public bool IsAuthenticated() => !string.IsNullOrEmpty(Session);

        public IndexModel(ILogger<IndexModel> logger, SessionClient sessionClient)
        {
            _logger = logger;
            _sessionClient = sessionClient;
        }

        public void OnGet()
        { }

        public async Task OnPostAsync()
        {
            var result = await _sessionClient.GetSession(UserName, Password);
            Session = result.Data;
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
