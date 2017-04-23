using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SKBKontur.Billy.Core.Common.Extensions;
using SKBKontur.Billy.Core.Common.Quering;

namespace SKBKontur.Billy.Core.BusinessObjects.Entities
{
    public class Data
    {
        public Guid Id { get; set; }
    }

    public class EntityHandler : BaseEntityHandler<Data>
    {
        public Data[] {caret}SelectByIds(Guid[] ids)
        {
            return SelectByKeys(data => data.Id, ids);
        }
    }

    public abstract class BaseEntityHandler<T> where T : class
    {
        protected T[] SelectByKeys<TKey>(Expression<Func<T, TKey>> getKey, params TKey[] keys)
        {
            throw new NotImplementedException();
        }

        protected async Task<T[]> SelectByKeysAsync<TKey>(Expression<Func<T, TKey>> getKey, params TKey[] keys)
        {
            throw new NotImplementedException();
        }
    }
}