using JPProject.Admin.Domain.Commands.Clients;

namespace JPProject.Admin.Domain.Validations.Client
{
    public class RemoveClientSecretCommandValidation : ClientSecretValidation<ClientSecretCommand>
    {
        public RemoveClientSecretCommandValidation()
        {
            ValidateClientId();
            ValidateType();
            ValidateValue();
        }
    }
}