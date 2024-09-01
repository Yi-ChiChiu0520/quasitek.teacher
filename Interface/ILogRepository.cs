using quasitekWeb.Models;
using System.Threading.Tasks;

namespace quasitekWeb.Interface
{
    public interface ILogRepository
    {
        Task<string> UploadLogsFromJson(LogWrapper logWrapper);
    }
}
