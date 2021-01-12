using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core.APIClient;
using Microsoft.AspNetCore.Mvc;
using UI.Dtos.Report;
using UI.Dtos.User;
using UI.IAssemblers;

namespace UI.Apis.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportAssembler _assembler;
        public ReportController(IReportAssembler assembler)
        {
            _assembler = assembler;
        }
        [HttpPost]
        [Route("BuildReport")]
        public  ResultMessage<long> BuildReport([FromBody]BuildReportRequestDto request)
        {
            return _assembler.BuildReport(request);
        }
        /// <summary>
        /// 该接口如需轮询
        /// </summary>
        /// <param name="event_no"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckReport")]
        public  ResultMessage<bool> CheckReport(long event_no)
        {
            return _assembler.CheckReport(event_no);
        }

    }
}
