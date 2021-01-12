using Domain.Data;
using Domain.Models;

namespace Domain.Repository.EF.Basic
{
    public class UserRepository : Repository<UserInfo, int>
    {

        public UserRepository(IUnitOfWork unitWork) : base(unitWork) { }
    }
}