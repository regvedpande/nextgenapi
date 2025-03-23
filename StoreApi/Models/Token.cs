namespace StoreApi.Models
{
    public class Token
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string TokenValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
