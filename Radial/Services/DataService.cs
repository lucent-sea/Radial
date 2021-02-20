﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Radial.Data;
using Radial.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Radial.Services
{
    public interface IDataService
    {
        Task<CharacterInfo> GetCharacterInfo(string userId);
        Task ReloadEntity<T>(T entity);

        Task WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception exception, List<string> scopeStack);
        Task<RadialUser> LoadUser(string username);
    }

    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _dbContext;

        public DataService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<CharacterInfo> GetCharacterInfo(string userId)
        {
            return await _dbContext.Users
                .Include(x => x.Info)
                .Where(x => x.Id == userId)
                .Select(x => x.Info)
                .FirstOrDefaultAsync();
        }

        public async Task<RadialUser> LoadUser(string username)
        {
            return await _dbContext.Users
                .Include(x=>x.Info)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task ReloadEntity<T>(T entity)
        {
            return _dbContext.Entry(entity).ReloadAsync();
        }

        public async Task WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception exception, List<string> scopeStack)
        {
            try
            {
                // Prevent re-entrancy.
                if (eventId.Name?.Contains("EntityFrameworkCore") == true)
                {
                    return;
                }

                _dbContext.EventLogs.Add(new EventLogEntry()
                {
                    StackTrace = exception?.StackTrace,
                    LogLevel = logLevel,
                    Message = $"[{logLevel}] [{string.Join(" - ", scopeStack)} - {category}] | Message: {state} | Exception: {exception?.Message}",
                    TimeStamp = DateTimeOffset.Now
                });

                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

    }
}
