using Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessService.DataAccess
{
    public interface IAppConfigRepository
    {
        Task<IEnumerable<AppConfig>> GetAll();
        Task<string> GetRecordingPathConfigAsync();
        Task<string> SetRecordingPathConfigAsync(string path);
        Task<string> SetApplicationNameAsync(string name);
        Task<string> GetApplicationNameAsync();
        Task<string> SetCompanyNameAsync(string name);
        Task<string> GetCompanyNameAsync();
        Task<string> SetMediaFileNamingConventionAsync(string name);
        Task<string> GetMediaFileNamingConventionAsync();

    }
}
