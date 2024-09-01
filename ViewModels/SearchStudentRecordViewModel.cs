
using quasitekWeb.Models;
namespace quasitekWeb.ViewModels;
public class SearchStudentRecordViewModel
{
    public List<RecordLogViewModel> Records { get; set; }
    public bool SearchPerformed { get; set; }
    public string MessageIfNotFound { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
