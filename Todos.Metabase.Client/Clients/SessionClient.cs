using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Todos.Metabase.Client.Models;
using Todos.Metabase.Client.Models.Security;

namespace Todos.Metabase.Client
{
    public class SessionClient
    {
        private readonly ILogger _logger;
        private readonly UriBuilder _uriBuilder;

        public SessionClient(IConfiguration configuration, ILogger<SessionClient> logger)
        {
            _logger = logger;
            _uriBuilder = new UriBuilder(configuration["ApiBaseUrl"])
            {
                Path = $"/api/session"
            };
        }

        public async Task<QueryResult<string>> GetSession(string userName, string password)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaton/json"));

            try
            {
                var credentials = new Credentials { Password = password, Username = userName };
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };
                var jsonContent = JsonConvert.SerializeObject(credentials, jsonSettings);
                var response = await client.PostAsync(_uriBuilder.Uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Session>(
                        await response.Content.ReadAsStringAsync());

                    return QueryResult<string>.Succeded(result.Id);
                }

                return QueryResult<string>.Fail();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"Falha de rede ao obter a sessao: {httpEx.Message}");
                _logger.LogError(httpEx.StackTrace);
                return QueryResult<string>.Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha desconhecida ao obter a sessao: {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return QueryResult<string>.Fail();
            }
        }

    }
}
