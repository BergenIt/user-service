using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DatabaseExtension;

using UserService.Core.DataInterfaces;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsIDataGetter : IDataGetter
    {
        Task<IEnumerable<TEntity>> IDataGetter.GetEntitiesAsync<TEntity>(IEnumerable<Guid> guids)
        {
            return Task.FromResult(guids.Select(i => new TEntity { Id = i }));
        }

        Task<IEnumerable<TEntity>> IDataGetter.GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(new List<TEntity> { new TEntity { Id = Guid.NewGuid() } }.AsEnumerable());
        }

        Task<TEntity> IDataGetter.GetFirstEntityAsync<TEntity>(Guid Id)
        {
            return Task.FromResult(new TEntity { Id = Id });
        }

        Task<IPageItems<TEntity>> IDataGetter.GetPage<TEntity>(FilterContract filterContract)
        {
            return Task.FromResult((IPageItems<TEntity>)new PageItems<TEntity>(
                new List<TEntity> { new TEntity { Id = Guid.NewGuid() } }.AsEnumerable(),
                3
            ));
        }

        Task<IEnumerable<TEntity>> IDataGetter.GetRecoursiveEntities<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Guid>> IDataGetter.GetRecoursiveEntityIds<TEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TEntity>>> path)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IDataGetter.GetSingleEntityAsync<TEntity>(Guid Id)
        {
            return Task.FromResult(new TEntity { Id = Id });
        }

        Task<TEntity> IDataGetter.GetSingleEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(new TEntity { Id = Guid.NewGuid() });
        }
    }
}
