using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Todos.Metabase.Client.Models;
using Todos.Metabase.Client.Parameters;

namespace Todos.Metabase.Client
{
    public class QueryClient
    {
        private readonly ILogger _logger;
        private readonly UriBuilder _uriBuilder;
        private readonly string _queryPath = "/api/card/{0}/query";

        public QueryClient(IConfiguration configuration, ILogger<QueryClient> logger)
        {
            _logger = logger;
            _uriBuilder = new UriBuilder(configuration["ApiBaseUrl"]);
        }

        public async Task<QueryResult<IEnumerable<RankingProspeccao>>> ConsultarRankingProspeccao(DateTime inicio, DateTime fim, string session)
        {
            var parameters = new ParameterBag();
            parameters.parameters.Add(Parameter.Date("DataInicio", inicio));
            parameters.parameters.Add(Parameter.Date("DataFim", fim));

            _uriBuilder.Path = string.Format(_queryPath, (int)QueryType.RankingProspecao);
            var result = await ExecuteAsync<DataBag>(session, parameters);

            if (result.Success)
            {
                var parsedData = MapFromDataSource<RankingProspeccao>(result.Data);
                return QueryResult<IEnumerable<RankingProspeccao>>.Succeded(parsedData);
            }

            return QueryResult<IEnumerable<RankingProspeccao>>.Fail();

        }

        private IEnumerable<TModel> MapFromDataSource<TModel>(DataBag data) where TModel : new()
        {
            var props = typeof(RankingProspeccao).GetProperties();
            var propIndexes = new Dictionary<int, PropertyInfo>();
            foreach (var prop in props)
            {
                var displayNameAttribute = prop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (displayNameAttribute != null)
                {
                    var index = data.Data.Cols.FindIndex(x => x.Name == displayNameAttribute.Name);
                    if (index > -1)
                    {
                        propIndexes.Add(index, prop);
                    }
                }
            }

            return data.Data.Rows.Select(x =>
            {
                var tuple = new TModel();
                foreach (var propIndex in propIndexes)
                {
                    propIndex.Value.SetValue(tuple, x[propIndex.Key]);
                }
                return tuple;
            });
        }

        public async Task<QueryResult<TResponse>> ExecuteAsync<TResponse>(string session, ParameterBag parameters) where TResponse : new()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaton/json"));
            client.DefaultRequestHeaders.Add("X-Metabase-Session", session);

            try
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                };
                var jsonContent = JsonConvert.SerializeObject(parameters, jsonSettings);
                var response = await client.PostAsync(_uriBuilder.Uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<TResponse>(
                        await response.Content.ReadAsStringAsync(),
                        jsonSettings);

                    return QueryResult<TResponse>.Succeded(result);
                }

                return QueryResult<TResponse>.Fail();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"Falha de rede ao executar a consulta: {httpEx.Message}");
                _logger.LogError(httpEx.StackTrace);
                return QueryResult<TResponse>.Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha desconhecida ao executar a consulta: {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return QueryResult<TResponse>.Fail();
            }
        }
    }
}