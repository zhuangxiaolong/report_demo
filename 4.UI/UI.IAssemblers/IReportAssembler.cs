using System;
using Domain.Core.APIClient;
using UI.Dtos.Report;
using UI.Dtos.User;

namespace UI.IAssemblers
{
    public interface IReportAssembler
    {
        ResultMessage<long> BuildReport(BuildReportRequestDto request);
        ResultMessage<bool> CheckReport(long  request);
    }
}
