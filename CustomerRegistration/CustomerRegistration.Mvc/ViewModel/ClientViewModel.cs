using CustomerRegistration.Mvc.Models;
using System;
using System.Collections.Generic;

namespace CustomerRegistration.Mvc.ViewModel
{
    public class ClientViewModel
    {
        public Guid IdClient { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string BirthDate { get; set; }

        public Sex SexClient { get; set; }

        public MaritalStatus MaritalStatusClient { get; set; }

        public ICollection<AddressViewModel> AddressList { get; set; }

        public ICollection<ClientViewModel> ClientViewModelList { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public bool ShowAddressAccess { get; set; }

        public static Client ConvertViewModelToModelClient(ClientViewModel viewModel)
        {
            string deformedCpf;
            string deformedDDD;
            string deformedPhone;
            int SexClientValue;
            int MaritalStatusClientValue;

            // Desformatando campos
            deformedCpf = viewModel.Cpf.Replace(".", "").Replace("-", "");
            deformedDDD = viewModel.Telephone.Substring(0, 4);
            deformedPhone = viewModel.Telephone.Replace(deformedDDD, "").Replace("-", "");
            deformedDDD = deformedDDD.Substring(1, 2);

            // Convertendo os valores dos Enums
            SexClientValue = (int)viewModel.SexClient;
            MaritalStatusClientValue = (int)viewModel.MaritalStatusClient;

            Client clientReply = new Client()
            {
                IdClient = viewModel.IdClient,
                Name = viewModel.Name,
                LastName = viewModel.LastName,
                Cpf = deformedCpf,
                Email = viewModel.Email,
                DDD = deformedDDD,
                Telephone = deformedPhone,
                BirthDate = DateTime.Parse(viewModel.BirthDate),
                SexClient = SexClientValue,
                MaritalStatusClient = MaritalStatusClientValue

            };

            return clientReply;
        }

        public static ClientViewModel ConvertModelToViewModelClient(Client clientModel)
        {
            ClientViewModel viewModel = new ClientViewModel()
            {
                IdClient = clientModel.IdClient,
                Name = clientModel.Name,
                LastName = clientModel.LastName,
                Cpf = clientModel.Cpf,
                Email = clientModel.Email,
                Telephone = clientModel.DDD + clientModel.Telephone,
                BirthDate = clientModel.BirthDate.ToString().Replace("/", "").Substring(0, 9),
                SexClient = (Sex)clientModel.SexClient,
                MaritalStatusClient = (MaritalStatus)clientModel.MaritalStatusClient
            };

            if (clientModel.AddressList != null)
            {
                if (clientModel.AddressList.Count > 0)
                {
                    viewModel.AddressList = new List<AddressViewModel>();
                    foreach (var item in clientModel.AddressList)
                    {
                        AddressViewModel itemViewModel = new AddressViewModel()
                        {
                            IdAddress = item.IdAddress,
                            PublicPlace = item.PublicPlace,
                            Number = item.Number,
                            Neighborhood = item.Neighborhood,
                            City = item.City,
                            IdClient = item.IdClient,
                            StateClient = (State)item.StateClient
                        };
                        viewModel.AddressList.Add(itemViewModel);
                    }
                }
            }
            return viewModel;
        }
    }

    public enum Sex
    {
        Masculino,
        Feminino
    }

    public enum MaritalStatus
    {
        Casado,
        Solteiro,
        Divorciado,
        Viúvo
    }
}