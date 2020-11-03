using System;
using System.Collections.Generic;

namespace Todos.Metabase.Client.Parameters
{
     public class Parameter
     {
          public string Type { get; set; }
          public List<object> Target { get; set; }
          public string Value { get; set; }

          internal static Parameter Date(string label, DateTime date) =>
            new Parameter
            {
                Type = ParameterType.Date,
                Target = new List<object>(){
                            "variable",
                            new List<string>(){
                                "template-tag",
                                label
                            }
                        },
                Value = date.ToString("yyyy-MM-dd")
            };
     }
}