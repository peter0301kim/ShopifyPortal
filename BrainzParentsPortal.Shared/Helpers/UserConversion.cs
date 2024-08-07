using BrainzParentsPortal.Integration.PortalDb.Models;
using Paytec.Integration.Shopify.Models;

namespace BrainzParentsPortal.Shared.Helpers;

public class UserConversion
{
    public static Member ConvertShopifyCustomerToPortalDbUser(Customer customer)
    {
        var portalUser = new Member()
        {
            Email = customer.email,
            FirstName = customer.first_name ?? "",
            LastName = customer.last_name ?? "",
            Phone = customer.phone,
            Address = customer.addresses.Count != 0 ? customer.addresses[0].address1 ?? "" : "",
            Address2 = customer.addresses.Count != 0 ? customer.addresses[0].address2 ?? "" : "",
            City = customer.addresses.Count != 0 ? customer.addresses[0].city ?? "" : "",
            Region = customer.addresses.Count != 0 ? customer.addresses[0].province ?? "" : "",
            Password = EncryptionHelper.SHA512("Brainz123!@#"),//
            Role = "User",
            Tags = customer.tags,
            CustomerID = customer.id.ToString(),
            BrainzPoint = 0,
            ImagePathFile = "",
            RegisteredDate = customer.created_at.Value,
            ModifiedDate = customer.updated_at.Value,
        };

        return portalUser;
    }

    
}
