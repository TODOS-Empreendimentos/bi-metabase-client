using System.Collections.Generic;

namespace Todos.Metabase.Client.Parameters
{
     public class ParameterBag
     {
          public bool IgnoreCache { get; set; } = true;
          public List<Parameter> parameters { get; set; } = new List<Parameter>();
     }
}