using Firmeza.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.API.Interfaces
{
    public interface IEmailService
    {
        bool SendAccountCreated(IdentityUser user);
        bool SendPurcharseConfirmation(Sale sale);
    }
}
