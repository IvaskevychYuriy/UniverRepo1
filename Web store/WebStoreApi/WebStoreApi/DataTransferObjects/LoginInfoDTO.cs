using WebStore.Api.Contracts;

namespace WebStore.Api.DataTransferObjects
{
    public class LoginInfoDTO : IDataTransferObject
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
