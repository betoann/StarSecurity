namespace StarSecurity.Models;

public class PaymentInformationModel
{
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

}