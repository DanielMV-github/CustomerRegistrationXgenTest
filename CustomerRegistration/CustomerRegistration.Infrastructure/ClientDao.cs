using CustomerRegistration.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerRegistration.Infrastructure
{
    public class ClientDao : IDao<Client>
    {
        private DbContext context;

        public ClientDao()
        {
            this.context = new DbContext();
        }

        public bool Create(Client objectDomain)
        {
            bool returnResponse = false;
            if (objectDomain != null)
            {
                objectDomain.IdClient = Guid.NewGuid();
                this.context.Clients.Add(objectDomain);
                returnResponse = true;
            }
            return returnResponse;
        }

        public Client Read(Guid id)
        {
            Client client = null;
            if (id != Guid.Empty)
            {
                client = this.ReturnsExistingClientInContext(id);

                if (client != null)
                    client.AddressList = this.context.Address.Where(x => x.IdClient.ToString().ToLower() == client.IdClient.ToString().ToLower()).ToList();
            }
            return client;
        }

        public Client Read(string cpf)
        {
            Client client = null;
            if (!string.IsNullOrEmpty(cpf))
            {
                client = this.context.Clients.Where(x => x.Cpf == cpf).FirstOrDefault();
                if (client != null)
                    client.AddressList = this.context.Address.Where(x => x.IdClient.ToString().ToLower() == client.IdClient.ToString().ToLower()).ToList();
            }
            return client;
        }

        public ICollection<Client> ReadList()
        {
            ICollection<Client> clients;
            clients = this.context.Clients;
            return clients;
        }

        public bool Update(Client objectDomain)
        {
            bool returnResponse = false;
            if (objectDomain != null)
            {
                Client client = this.ReturnsExistingClientInContext(objectDomain.IdClient);
                if (client != null)
                {
                    this.context.Clients.Remove(client);
                    this.context.Clients.Add(objectDomain);
                    returnResponse = true;
                }
            }
            return returnResponse;
        }

        public bool Delete(string cpf)
        {
            bool returnResponse = false;
            if (!string.IsNullOrEmpty(cpf))
            {
                Client client = this.context.Clients.Where(x => x.Cpf == cpf).FirstOrDefault();
                if (client != null)
                {
                    foreach (Address item in this.context.Address.Where(x => x.IdClient.ToString().ToLower() == client.IdClient.ToString().ToLower()).ToList())
                    {
                        this.context.Address.Remove(item);
                    }
                    this.context.Clients.Remove(client);
                    returnResponse = true;
                }
            }
            return returnResponse;
        }

        public bool Delete(Guid id)
        {
            bool returnResponse = false;
            if (Guid.Empty == id)
            {
                Client client = this.ReturnsExistingClientInContext(id);
                if (client != null)
                {
                    foreach (Address item in this.context.Address.Where(x => x.IdClient.ToString().ToLower() == client.IdClient.ToString().ToLower()).ToList())
                    {
                        this.context.Address.Remove(item);
                    }
                    this.context.Clients.Remove(client);
                    returnResponse = true;
                }
            }
            return returnResponse;
        }

        #region classes of help

        private Client ReturnsExistingClientInContext(Guid idClient)
        {
            Client client;
            client = this.context.Clients.Where(x => x.IdClient.ToString().ToLower() == idClient.ToString().ToLower()).FirstOrDefault();
            return client;
        }

        public static Guid ClientExistsInTheContext(string cpf)
        {
            Guid returnResponse = Guid.Empty;
            Client client = new DbContext().Clients.Where(x => x.Cpf == cpf).FirstOrDefault();
            if (client != null)
                returnResponse = client.IdClient;
            return returnResponse;
        }

        #endregion
    }
}
