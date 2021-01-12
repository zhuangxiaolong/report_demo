using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using Domain.Core;
using Domain.Core.APIClient;
using Domain.Core.Authorization;
using Domain.Core.Cache;
using Domain.Core.Common;
using Domain.Core.Log;
using Domain.Data;
using Domain.Models;
using Hangfire;
using Service.IApis;
using Services.Core.Services;
using UI.Core;
using UI.Dtos.Report;
using UI.Dtos.User;

namespace Service.Apis
{
    public class ReportService: ServiceBase,IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Configuration _configuration;
        private IRepository<Inventory, long> _inventoryRep;
        private IRepository<log_report_build, int> _log_report_buildRep;
        private ICacheClient _cacheClient;
        public ReportService(
            IRepository<Inventory, long> inventoryRep
             ,IRepository<log_report_build, int> log_report_buildRep
             ,IUnitOfWork untiOfWork
             ,ICacheClient cacheClient
            ,Configuration configuration
        )
            : base("ReportService:")
        {
            _unitOfWork = untiOfWork;
            _configuration = configuration;
            _inventoryRep = inventoryRep;
            _log_report_buildRep = log_report_buildRep;
            _cacheClient = cacheClient;
        }
        /// <summary>
        /// 生成报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResultMessage<long> BuildReport(BuildReportRequestDto request)
        {
            var response = new ResultMessage<long>()
            {
                err_code = 400,
                err_msg = "错误",
            };

            return Logger("生成报表接口", () =>
            {
                var eventNo = GenerateEventNo();
                response.body = eventNo;

                var log = new log_report_build()
                {
                    is_finish = 0,
                    report_build_date = DateTime.Now,
                    report_event_no = eventNo,
                };
                _log_report_buildRep.Add(log);
                //加入队列
                BackgroundJob.Enqueue<ReportService>(r => r.Async_BuildReport(request,eventNo));

                response.err_code = 200;
                response.err_msg = "成功";
                return response;
            }, ErrorHandles.Continue, LogModes.Error, ex =>
            {
                return response;
            });

        }
        /// <summary>
        /// 生成报表ID
        /// </summary>
        /// <returns></returns>
        private long GenerateEventNo()
        {
            using (var conn=new SqlConnection(_configuration.ConnStr))
            {
                conn.Open();
                var sql = "SELECT NEXT VALUE FOR SQ_EVENTNO";
                var result=conn.Query<long>(sql).First();
                return result;

            }
        }
        /// <summary>
        /// 异步生成接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="eventNo"></param>
        [Queue("report")]
        public void Async_BuildReport(BuildReportRequestDto request,long eventNo)
        {
            {
                Logger("生成报表接口 自动后台", () =>
                {
                    //取数
                    var objInventory = _inventoryRep.Find(r => r.SapId == request.sap_id).ToList();


                    #region 生成Excel到本地
                    
                    var excel=new ExcelHelper();

                    //excel2003
                    var excel_name = string.Empty;
                    var file_name = string.Empty;
                    
                    //excel_name=eventNo + ".xls";
                    //file_name = Path.Combine("c://excel//", excel_name);
                    //excel.ExportOneSheepExcel2003(objInventory,file_name);
                    
                    //excel2007
                    excel_name = eventNo + ".xlsx";
                    file_name = Path.Combine("c://excel//", excel_name);
                    excel.ExportOneSheepExcel2007(objInventory,file_name);

                    #endregion

                    #region 上传到云的存储桶

                    //todo

                    #endregion

                    #region 更新报表记录

                    var obj = _log_report_buildRep.Find(r => r.report_event_no == eventNo).FirstOrDefault();
                    obj.is_finish = 1;
                    obj.report_build_finish_date=DateTime.Now;
                    _log_report_buildRep.Update(obj);
                    #endregion

                }, ErrorHandles.Continue, LogModes.Error, ex =>
                    {
                       // db.Rollback();
                    });
            }
        }
        /// <summary>
        /// 检查报表是否生成
        /// </summary>
        /// <param name="event_no"></param>
        /// <returns></returns>
        public ResultMessage<bool> CheckReport(long event_no)
        {
            var response = new ResultMessage<bool>()
            {
                err_code = 400,
                err_msg = "错误",
            };

            return Logger("检查报表接口", () =>
            {
                var obj = _log_report_buildRep.Find(r => r.report_event_no == event_no).FirstOrDefault();
                if (obj == null)
                {
                    response.body = false;
                    response.err_code = 400;
                    response.err_msg = "未生成成功";
                    return response;
                }

                #region 返回云的存储桶文件地址

                //todo

                #endregion

                response.body = true;
                response.err_code = 200;
                response.err_msg = "成功";
                return response;
            }, ErrorHandles.Continue, LogModes.Error, ex =>
            {
                return response;
            });

        }
    }
}