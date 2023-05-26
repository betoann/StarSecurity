namespace StarSecurity.Models;

public class PaymentResponseModel
{
    public string OrderDescription { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public decimal Amount { get; set; }
    public long TransactionId { get; set; }
    public long OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public long PaymentId { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
}