using System;

namespace CustomerRegistration.Mvc.Models
{
    public class Address
    {
        public Guid IdAddress { get; set; }

        public string PublicPlace { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public int StateClient { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public Guid IdClient { get; set; }

        public Client Client { get; set; }

    }
}