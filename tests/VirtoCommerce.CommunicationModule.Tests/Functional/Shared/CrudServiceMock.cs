using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.CommunicationModule.Tests.Functional;

[ExcludeFromCodeCoverage]
public class CrudServiceMock<T> : ICrudService<T>
    where T : Entity
{
    public virtual List<T> Models { get; set; } = new();

    public virtual Task<IList<T>> GetAsync(IList<string> ids, string responseGroup = null, bool clone = true)
    {
        return Task.FromResult<IList<T>>(Models.Where(x => ids.Contains(x.Id)).ToList());
    }

    public virtual Task SaveChangesAsync(IList<T> models)
    {
        Models.RemoveAll(x => models.Any(m => m.Id == x.Id));
        Models.AddRange(models);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(IList<string> ids, bool softDelete = false)
    {
        Models.RemoveAll(x => ids.Contains(x.Id));
        return Task.CompletedTask;
    }
}

