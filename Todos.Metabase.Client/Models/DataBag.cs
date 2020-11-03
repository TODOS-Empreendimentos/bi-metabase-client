using System.Collections.Generic;

namespace Todos.Metabase.Client.Models
{
     public class Column
     {
          public string DisplaName { get; set; }
          public List<string> FieldRef { get; set; }
          public string Name { get; set; }
          public string BaseType { get; set; }
     }

     public class Data
     {
          public List<List<object>> Rows { get; set; }
          public List<Column> Cols { get; set; }
     }

     public class DataBag
     {
          public Data Data { get; set; }
     }
}