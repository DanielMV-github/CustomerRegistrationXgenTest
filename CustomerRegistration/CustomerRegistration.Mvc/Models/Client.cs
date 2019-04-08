using System;
using System.Collections.Generic;

namespace CustomerRegistration.Mvc.Models
{
    public class Client
    {
        public Guid IdClient { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string DDD { get; set; }

        public string Telephone { get; set; }

        public DateTime BirthDate { get; set; }

        public int SexClient { get; set; }

        public int MaritalStatusClient { get; set; }

        public ICollection<Address> AddressList { get; set; }
    }
}
