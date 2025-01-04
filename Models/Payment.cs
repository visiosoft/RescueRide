namespace RescueRide.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int ServiceId { get; set; }
        public string PaymentMethod { get; set; } // Credit Card, Wallet, Cash
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } // Success, Failed
    }

}
