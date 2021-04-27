using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface ILogRepository
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

    public class LogRepository : ILogRepository
    {
        // todo: 要検討
        private static readonly int TakeCount = 1000;

        private readonly GmToolDbContext _dbContext;

        public LogRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(IEnumerable<AuthLog>, int)> GetAuthLogList(
            DateTime from,
            DateTime to,
            string account,
            int? result,
            string ipAddress)
        {
            var query = _dbContext.AuthLogs.Where(x => from <= x.CreatedAt && x.CreatedAt <= to);

            if (!string.IsNullOrWhiteSpace(account)) query = query.Where(x => x.Account == account);

            if (result.HasValue) query = query.Where(x => x.Result == result);

            if (!string.IsNullOrWhiteSpace(ipAddress)) query = query.Where(x => x.IpAddress == ipAddress);

            var list = await query.OrderByDescending(x => x.CreatedAt)
                                  .Take(TakeCount)
                                  .ToListAsync();

            var count = await query.CountAsync();

            return (list, count);
        }

        public async Task<AuthLog> GetAuthLog(int logId)
        {
            return await _dbContext.AuthLogs.FindAsync(logId);
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
            var query = _dbContext.OperationLogs
                .Where(x => from <= x.CreatedAt && x.CreatedAt <= to);

            if (userId.HasValue) query = query.Where(x => x.UserId == userId.Value);

            if (statusCode.HasValue) query = query.Where(x => x.StatusCode == statusCode.Value);

            if (!string.IsNullOrWhiteSpace(method)) query = query.Where(x => x.Method == method);

            if (!string.IsNullOrWhiteSpace(url)) query = query.Where(x => x.Url == url);

            if (!string.IsNullOrWhiteSpace(queryString)) query = query.Where(x => x.QueryString.Contains(queryString));

            if (!string.IsNullOrWhiteSpace(requestBody)) query = query.Where(x => x.RequestBody.Contains(requestBody));

            if (!string.IsNullOrWhiteSpace(responseBody)) query = query.Where(x => x.ResponseBody.Contains(responseBody));

            if (!string.IsNullOrWhiteSpace(exception)) query = query.Where(x => x.Exception.Contains(exception));

            if (!string.IsNullOrWhiteSpace(ipAddress)) query = query.Where(x => x.IpAddress == ipAddress);

            var list = await query
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Take(TakeCount)
                .ToListAsync();

            var count = await query.CountAsync();

            return (list, count);
        }

        public async Task<OperationLog> GetOperationLog(int logId)
        {
            return await _dbContext.OperationLogs
                .Include(x => x.User)
                .FirstAsync(x => x.Id == logId);
        }
    }
}
