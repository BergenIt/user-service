
using Microsoft.EntityFrameworkCore;

using UserService.Core.Entity;

namespace UserService.Core.Models
{
    public record SavedEntry(EntityState State, IBaseEntity Entity);
}
