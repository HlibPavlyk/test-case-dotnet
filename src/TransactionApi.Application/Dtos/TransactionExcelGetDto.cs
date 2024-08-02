namespace TransactionApi.Application.Dtos;

public class TransactionExcelGetDto
{
    public string TransactionId { get; set; }
    public string Email { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}