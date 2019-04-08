using System;
using System.Collections.Generic;

namespace CustomerRegistration.Domain
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

        public Sex SexClient{ get; set; }

        public MaritalStatus MaritalStatusClient { get; set; }

        public ICollection<Address> AddressList { get; set; }

        public enum Sex
        {
            Male,
            Female
        }

        public enum MaritalStatus
        {
            Married,
            NotMarried,
            Divorced,
            Widower
        }
    }    
}
