using CustomerRegistration.Domain;
using CustomerRegistration.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerRegistration.TestInfrastructure.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string telefone = "(11) 96364-8940";

            string ddd = telefone.Substring(0, 4);
            string telefonealterado = telefone.Replace(ddd, "").Replace("-", "");
            ddd = ddd.Substring(1, 2);


            //string ddd = telefone.Substring(0, 4).Substring(1, 2);
            Console.WriteLine(ddd + "--" + telefonealterado);
        }
    }
}
