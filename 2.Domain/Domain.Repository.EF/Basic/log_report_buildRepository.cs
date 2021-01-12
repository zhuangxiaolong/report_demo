using Domain.Data;
using Domain.Models;

namespace Domain.Repository.EF.Basic
{
    public class log_report_buildRepository : Repository<log_report_build, int>
    {

        public log_report_buildRepository(IUnitOfWork unitWork) : base(unitWork) { }
    }
}