using Domain.Data;
using Domain.Models;

namespace Domain.Repository.EF.Basic
{
    public class InventoryRepository : Repository<Inventory, long>
    {

        public InventoryRepository(IUnitOfWork unitWork) : base(unitWork) { }
    }
}