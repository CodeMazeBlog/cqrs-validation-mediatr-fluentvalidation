using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAsync(CancellationToken cancellationToken = default);

        Task<User> GetByIdAsync(int userId, CancellationToken cancellationToken = default);

        void Insert(User user);
    }
}
