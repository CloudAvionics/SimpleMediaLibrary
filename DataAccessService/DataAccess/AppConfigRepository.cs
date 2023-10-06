using Microsoft.EntityFrameworkCore;
using Persistence.Model;
using SimpleLibrary.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessService.DataAccess
{
    public class AppConfigRepository : IAppConfigRepository
    {
        private readonly SimpleLibraryDbContext _context;

        public AppConfigRepository(SimpleLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppConfig>> GetAll()
        {
            return await _context.AppConfigs.ToListAsync();
        }

        public async Task<string> GetApplicationNameAsync()
        {
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "ApplicationName")
                .FirstOrDefaultAsync();
            return config?.ConfigValue ?? "";
        }

        public async Task<string> GetCompanyNameAsync()
        {
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "CompanyName")
                .FirstOrDefaultAsync();
            return config?.ConfigValue ?? "";
        }

        public async Task<string> GetMediaFileNamingConventionAsync()
        {
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "MediaFileNamingConvention")
                .FirstOrDefaultAsync();
            return config?.ConfigValue ?? "";
        }

        public async Task<string> GetRecordingPathConfigAsync()
        {
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "RecordingDir")
                .FirstOrDefaultAsync();
            return config?.ConfigValue ?? "";
        }

        public async Task<string> SetApplicationNameAsync(string name)
        {
            string returnVal = "";
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "ApplicationName")
                .FirstOrDefaultAsync();

            if (config is not null)
            {
                config.ConfigValue = name;
                await _context.SaveChangesAsync();

                returnVal = (await _context.AppConfigs
                                    .Where(item => item.Id == config.Id)
                                    .FirstOrDefaultAsync())?
                                    .ConfigValue ?? "";
            }
            return returnVal;
        }

        public async Task<string> SetCompanyNameAsync(string name)
        {
            string returnVal = "";
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "CompanyName")
                .FirstOrDefaultAsync();

            if (config is not null)
            {
                config.ConfigValue = name;
                await _context.SaveChangesAsync();

                returnVal = (await _context.AppConfigs
                                    .Where(item => item.Id == config.Id)
                                    .FirstOrDefaultAsync())?
                                    .ConfigValue ?? "";
            }
            return returnVal;
        }

        public async Task<string> SetMediaFileNamingConventionAsync(string name)
        {
            string returnVal = "";
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "MediaFileNamingConvention")
                .FirstOrDefaultAsync();

            if (config is not null)
            {
                config.ConfigValue = name;
                await _context.SaveChangesAsync();

                returnVal = (await _context.AppConfigs
                                    .Where(item => item.Id == config.Id)
                                    .FirstOrDefaultAsync())?
                                    .ConfigValue ?? "";
            }
            return returnVal;
        }

        public async Task<string> SetRecordingPathConfigAsync(string path)
        {
            string returnVal = "";
            var config = await _context.AppConfigs
                .Where(p => p.ConfigName == "RecordingDir")
                .FirstOrDefaultAsync();

            if (config is not null)
            {
                config.ConfigValue = path;
                await _context.SaveChangesAsync();

                returnVal = (await _context.AppConfigs
                                    .Where(item => item.Id == config.Id)
                                    .FirstOrDefaultAsync())?
                                    .ConfigValue ?? "";
            }
            return returnVal;
        }
    }
}
