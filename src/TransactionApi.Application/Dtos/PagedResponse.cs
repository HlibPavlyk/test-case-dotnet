namespace TransactionApi.Application.Dtos;

public class PagedResponse<T> where T: class
{
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; }

}