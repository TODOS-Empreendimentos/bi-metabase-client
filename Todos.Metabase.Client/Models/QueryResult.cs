using System;
using System.Collections.Generic;
using System.Text;

namespace Todos.Metabase.Client.Models
{
    public class QueryResult<TDataResult>
    {
        public bool Success { get; private set; }

        public TDataResult Data { get; private set; }

        public static QueryResult<TDataResult> Succeded(TDataResult data)
            => new QueryResult<TDataResult> { Success = true, Data = data };
        public static QueryResult<TDataResult> Fail()
            => new QueryResult<TDataResult> { Success = false };

    }
}
