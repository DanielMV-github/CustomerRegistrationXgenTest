using System;

namespace CustomerRegistration.Domain
{
    public class Address
    {
        public Guid IdAddress { get; set; }

        public string PublicPlace { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public State StateClient { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public Guid IdClient { get; set; }

        public Client Client { get; set; }
        
        public enum State
        {
            AC,
            AL,
            AP,
            AM,
            BA,
            CE,
            DF,
            ES,
            GO,
            MA,
            MT,
            MS,
            MG,
            PA,
            PB,
            PR,
            PE,
            PI,
            RJ,
            RN,
            RS,
            RO,
            RR,
            SC,
            SP,
            SE,
            TO
        }
    }
}
