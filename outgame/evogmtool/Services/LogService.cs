using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface ILogService
    {
        Task<(IEnumerable<AuthLog>, int)> GetAuthLogList(
            DateTime from,
            DateTime to,
            string account,
            int? result,
            string ipAddress);

        Task<AuthLog> GetAuthLog(int logId);

        Task<(IEnumerable<OperationLog>, int)> GetOperationLogList(
            DateTime from,
            DateTime to,
            int? userId,
            short? statusCode,
            string method,
            string url,
            string queryString,
            string requestBody,
            string responseBody,
            string exception,
            string ipAddress);

        Task<OperationLog> GetOperationLog(int logId);
    }

    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<AuthLog>, int)> GetAuthLogList(
            DateTime from,
            DateTime to,
            string account,
            int? result,
            string ipAddress)
        {
            return await _unitOfWork.LogRepository.GetAuthLogList(
                from,
                to,
                account,
                result,
                ipAddress);
        }

        public async Task<AuthLog> GetAuthLog(int logId)
        {
            return await _unitOfWork.LogRepository.GetAuthLog(logId);
        }

        public async Task<(IEnumerable<OperationLog>, int)> GetOperationLogList(
            DateTime from,
            DateTime to,
            int? userId,
            short? statusCode,
            string method,
            string url,
            string queryString,
            string requestBody,
            string responseBody,
            string exception,
            string ipAddress)
        {
            return await _unitOfWork.LogRepository.GetOperationLogList(
                from,
                to,
                userId,
                statusCode,
                method,
                url,
                queryString,
                requestBody,
                responseBody,
                exception,
                ipAddress);
        }

        public async Task<OperationLog> GetOperationLog(int logId)
        {
            return await _unitOfWork.LogRepository.GetOperationLog(logId);
        }
    }
}
