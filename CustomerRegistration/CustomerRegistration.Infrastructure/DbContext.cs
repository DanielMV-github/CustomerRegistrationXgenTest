using CustomerRegistration.Domain;
using System.Collections.Generic;

namespace CustomerRegistration.Infrastructure
{
    public class DbContext
    {
        // Atributos que simulam tabelas no Banco de Dados.
        private static ICollection<Client> ClientDb;
        private static ICollection<Address> AddressDb;

        public DbContext()
        {
            if (this.Clients == null)
                this.Clients = new List<Client>();

            if (this.Address == null)
                this.Address = new List<Address>();
        }

        public ICollection<Client> Clients
        {
            get { return ClientDb; }
            set { ClientDb = value; }
        }

        public ICollection<Address> Address
        {
            get { return AddressDb; }
            set { AddressDb = value; }
        }
    }
}
