
namespace BlogApi.Dtos.Common;

public class PaginationList<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }

}