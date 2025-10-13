using Domain;

namespace Application.Interfaces
{
    public interface IReportService<T> where T : BaseEntity
    {
        Task<MemoryStream> GetCSVReport(List<T> records);
    }
}