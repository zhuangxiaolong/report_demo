using Domain.Core.APIClient;
using Service.IApis;
using UI.Core;
using UI.Dtos.Report;
using UI.Dtos.User;
using UI.IAssemblers;

namespace UI.Assemblers
{
    public class ReportAssembler:AssemblerBase,IReportAssembler
    {
        private IReportService _service;

        public ReportAssembler(IReportService service)
        {
            _service = service;
        }
        public ResultMessage<long> BuildReport(BuildReportRequestDto request)
        {
            var response = new ResultMessage<long>();

            var result = _service.BuildReport(request);

            response.body = result.body;
            response.err_code = result.err_code;
            response.err_msg = result.err_msg;

            return response;
        }
        public ResultMessage<bool> CheckReport(long event_no)
        {
            var response = new ResultMessage<bool>();

            var result = _service.CheckReport(event_no);

            response.body = result.body;
            response.err_code = result.err_code;
            response.err_msg = result.err_msg;

            return response;
        }
    }
}