namespace WalletService.API
{
    public class Response
    {
        public bool isSuccess { get; set; }
        public MessageCodes? MessageCodes { get; set; }  
    }

    public class Response<DataType>
    {
        public bool isSuccess { get; set; }
        public MessageCodes? MessageCodes { get; set; }
        public DataType data { get; set; }  

    }
    public enum MessageCodes
    {

    }
}
