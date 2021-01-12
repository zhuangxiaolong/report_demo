using Domain.Core.APIClient;
using UI.Dtos.Report;

namespace Service.IApis
{
    public interface IReportService
    {
        ResultMessage<long> BuildReport(BuildReportRequestDto request);
        ResultMessage<bool> CheckReport(long event_no);
    }
}