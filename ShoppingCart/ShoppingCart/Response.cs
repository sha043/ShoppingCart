namespace ShoppingCart
{
    public class Response
    {
        public bool IsSuccessful { get; private set; }
        public string ResponseMessage { get; private set; }

        public Response( bool isSuccessful, string response)
        {
            IsSuccessful = isSuccessful;
            ResponseMessage = response;
        }
    }
}
