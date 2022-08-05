namespace WalletService.API.Requests
{
    public class BaseModel
    {
        public int Id { get; set; }


        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
