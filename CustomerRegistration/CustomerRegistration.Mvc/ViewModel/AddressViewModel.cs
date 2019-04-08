using CustomerRegistration.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRegistration.Mvc.ViewModel
{
    public class AddressViewModel
    {
        public Guid IdAddress { get; set; }

        public string PublicPlace { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public State StateClient { get; set; }

        public string StateClientText { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public Guid IdClient { get; set; }

        public string CpfClient { get; set; }

        public static Address ConvertViewModelToModelAddress(AddressViewModel viewModel)
        {
            string deformedZipCode;
            int stateClientValue;

            // Desformatando campos
            deformedZipCode = viewModel.ZipCode.Replace("-", "");

            // Convertendo os valores dos Enums
            stateClientValue = (int)viewModel.StateClient;

            Address addressReply = new Address()
            {
                IdAddress = viewModel.IdAddress,
                PublicPlace = viewModel.PublicPlace,
                Number = viewModel.Number,
                Neighborhood = viewModel.Neighborhood,
                StateClient = stateClientValue,
                City = viewModel.City,
                ZipCode = deformedZipCode,
                IdClient = viewModel.IdClient
            };

            return addressReply;
        }

        public static AddressViewModel ConvertModelToViewModelClient(Address addressModel)
        {
            AddressViewModel viewModel = new AddressViewModel()
            {
                IdAddress = addressModel.IdAddress,
                PublicPlace = addressModel.PublicPlace,
                Number = addressModel.Number,
                Neighborhood = addressModel.Neighborhood,
                StateClient = (State)addressModel.StateClient,
                StateClientText = Enum.GetName(typeof(State), addressModel.StateClient),
                City = addressModel.City,
                ZipCode = addressModel.ZipCode,
                IdClient = addressModel.IdClient
            };

            return viewModel;
        }
    }

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