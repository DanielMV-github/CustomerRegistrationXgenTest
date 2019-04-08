using CustomerRegistration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerRegistration.Infrastructure
{
    public class AddressDao : IDao<Address>
    {
        private DbContext context;

        public AddressDao()
        {
            this.context = new DbContext();
        }

        public bool Create(Address objectDomain)
        {
            bool returnResponse = false;
            if (objectDomain != null)
            {
                objectDomain.IdAddress = Guid.NewGuid();
                this.context.Address.Add(objectDomain);
                returnResponse = true;
            }
            return returnResponse;
        }

        public Address Read(Guid id)
        {
            Address address = null;
            if (id != Guid.Empty)
                address = this.ReturnsExistingAddressInContext(id);
            return address;
        }

        public ICollection<Address> ReadList()
        {
            ICollection<Address> address;
            address = this.context.Address;
            return address;
        }

        public ICollection<Address> ReadList(Guid IdClient)
        {
            ICollection<Address> address;
            address = this.context.Address.Where(x => x.IdClient.ToString().ToLower() == IdClient.ToString().ToLower()).ToList();
            return address;
        }

        public bool Update(Address objectDomain)
        {
            bool returnResponse = false;
            if (objectDomain != null)
            {
                Address address = this.ReturnsExistingAddressInContext(objectDomain.IdAddress);
                if (address != null)
                {
                    this.context.Address.Remove(address);
                    this.context.Address.Add(objectDomain);
                    returnResponse = true;
                }
            }
            return returnResponse;
        }

        public bool Delete(Guid id)
        {
            bool returnResponse = false;
            if (id != Guid.Empty)
            {
                Address address = this.ReturnsExistingAddressInContext(id);
                if (address != null)
                {
                    this.context.Address.Remove(address);
                    returnResponse = true;
                }
            }
            return returnResponse;
        }

        #region classes of help

        private Address ReturnsExistingAddressInContext(Guid idAddress)
        {
            Address address;
            address = this.context.Address.Where(x => x.IdAddress.ToString().ToLower() == idAddress.ToString().ToLower()).FirstOrDefault();
            return address;
        }

        public static Guid CustomerAddressExists(string zipCode, string number, Guid idClientAddress)
        {
            Guid idAddress = Guid.Empty;
            Address address = new DbContext().Address.Where(x => x.ZipCode == zipCode && x.Number == number && x.IdClient.ToString().ToLower() == idClientAddress.ToString().ToLower()).FirstOrDefault();
            if (address != null)
                idAddress = address.IdAddress;
            return idAddress;
        }

        #endregion
    }
}
