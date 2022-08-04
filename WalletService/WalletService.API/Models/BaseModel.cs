namespace WalletService.API.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
