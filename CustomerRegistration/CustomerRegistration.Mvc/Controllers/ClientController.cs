using CustomerRegistration.Mvc.Models;
using CustomerRegistration.Mvc.ViewModel;
using System.Web.Script.Serialization;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CustomerRegistration.Mvc.Controllers
{
    public class ClientController : BaseController
    {
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Cadastrar Cliente";
            ClientViewModel viewModel = new ClientViewModel()
            {
                ActionName = "Create",
                ControllerName = "Client",
            };

            ModelState.Clear();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClientViewModel viewModel)
        {
            ReplyObjectApi replyObjectApi = null;
            try
            {
                ViewBag.Title = "Cadastrar Cliente";
                Client clientModel = ClientViewModel.ConvertViewModelToModelClient(viewModel);
                string jsonContent = new JavaScriptSerializer().Serialize(clientModel);
                replyObjectApi = await base.RequestWebApiAsync("api/client/create", "POST", jsonContent);
                if (replyObjectApi.NameHttpStatusCode == "OK")
                {
                    ViewBag.Title = "Editar Cliente";
                    viewModel.IdClient = Guid.Parse(replyObjectApi.Value.Replace("\"", ""));
                    viewModel.ActionName = "Update";
                    viewModel.ShowAddressAccess = true;
                }
                else
                    throw new System.InvalidOperationException();
            }
            catch (Exception)
            {
                if (replyObjectApi == null)
                {
                    replyObjectApi = new ReplyObjectApi()
                    {
                        NameHttpStatusCode = "InternalServerError",
                        Message = "Erro: verifique o preenchimento dos campos, caso esteja correto contate o suporte",
                        Value = null,
                    };
                }

            }
            ModelState.Clear();
            ResponsetToRequisition(replyObjectApi.Message);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Read(string cpf)
        {
            ReplyObjectApi replyObjectApi = null;
            ClientViewModel viewModel = null;
            try
            {
                cpf = cpf.Replace(".", "").Replace("-", "");
                replyObjectApi = await base.RequestWebApiAsync(string.Format("api/client/read?cpf={0}", cpf), "GET");
                if (replyObjectApi.NameHttpStatusCode == "OK")
                {
                    ViewBag.Title = "Editar Cliente";
                    Client clientModel = JsonConvert.DeserializeObject<Client>(replyObjectApi.Value);
                    viewModel = ClientViewModel.ConvertModelToViewModelClient(clientModel);
                    viewModel.ActionName = "Update";
                    viewModel.ControllerName = "Client";
                    viewModel.ShowAddressAccess = true;
                }
                else
                    throw new System.InvalidOperationException();
            }
            catch (Exception)
            {
                if (replyObjectApi == null)
                {
                    replyObjectApi = new ReplyObjectApi()
                    {
                        NameHttpStatusCode = "InternalServerError",
                        Message = "Erro: Não foi possível resgatar os dados do cliente, entre em contato com o suporte",
                        Value = null,
                    };
                }
                viewModel = new ClientViewModel()
                {
                    ActionName = "Create",
                    ControllerName = "Client",
                    ShowAddressAccess = false
                };
            }

            ModelState.Clear();
            ResponsetToRequisition(replyObjectApi.Message);
            return View("Create", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ReadList()
        {
            ReplyObjectApi replyObjectApi = null;
            ClientViewModel viewModel = null;
            try
            {
                replyObjectApi = await base.RequestWebApiAsync("api/client/readList", "GET");
                if (replyObjectApi.NameHttpStatusCode == "OK")
                {
                    viewModel = new ClientViewModel();
                    viewModel.ClientViewModelList = new List<ClientViewModel>();
                    ICollection<Client> clientModelList = JsonConvert.DeserializeObject<ICollection<Client>>(replyObjectApi.Value);
                    foreach (var item in clientModelList)
                    {
                        viewModel.ClientViewModelList.Add(ClientViewModel.ConvertModelToViewModelClient(item));
                    }
                }
                else
                    throw new System.InvalidOperationException();
            }
            catch (Exception)
            {
                ModelState.Clear();
                ResponsetToRequisition("Não existe cliente cadastrado");
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ClientViewModel viewModel)
        {
            ReplyObjectApi replyObjectApi = null;
            try
            {
                ViewBag.Title = "Editar Cliente";
                Client clientModel = ClientViewModel.ConvertViewModelToModelClient(viewModel);
                string jsonContent = new JavaScriptSerializer().Serialize(clientModel);
                replyObjectApi = await base.RequestWebApiAsync("api/client/update", "POST", jsonContent);
                if (replyObjectApi.NameHttpStatusCode != "OK")
                    throw new System.InvalidOperationException();
            }
            catch (Exception)
            {
                if (replyObjectApi == null)
                {
                    replyObjectApi = new ReplyObjectApi()
                    {
                        NameHttpStatusCode = "InternalServerError",
                        Message = "Erro: verifique o preenchimento dos campos, caso esteja correto contate o suporte",
                        Value = null,
                    };
                }

            }
            viewModel.AddressList = await AddressViewModelAsync(viewModel.IdClient);
            ModelState.Clear();
            ResponsetToRequisition(replyObjectApi.Message);
            return View("Create", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string cpf)
        {
            ReplyObjectApi replyObjectApi = null;
            try
            {
                cpf = cpf.Replace(".", "").Replace("-", "");
                replyObjectApi = await base.RequestWebApiAsync(string.Format("api/client/delete?cpf={0}", cpf), "DELETE");
                if (replyObjectApi.NameHttpStatusCode != "OK")
                    throw new System.InvalidOperationException();
            }
            catch (Exception)
            {
                if (replyObjectApi == null)
                {
                    replyObjectApi = new ReplyObjectApi()
                    {
                        NameHttpStatusCode = "InternalServerError",
                        Message = "Erro: Não foi possível deletar o cliente, caso esteja correto contate o suporte",
                        Value = null,
                    };
                }
            }
            return Json(replyObjectApi, JsonRequestBehavior.AllowGet);
        }


        #region classes of help
        private async Task<ICollection<AddressViewModel>> AddressViewModelAsync(Guid idClient)
        {
            ReplyObjectApi replyObjectApi = null;
            ICollection<AddressViewModel> listAddressViewModel = null;
            ICollection<Address> listAddress = null;
            replyObjectApi = await base.RequestWebApiAsync(string.Format("api/address/readList?idClient={0}", idClient), "GET");

            if (replyObjectApi.NameHttpStatusCode == "OK")
            {
                listAddress = JsonConvert.DeserializeObject<ICollection<Address>>(replyObjectApi.Value);
                listAddressViewModel = new List<AddressViewModel>();
                foreach (var item in listAddress)
                {
                    AddressViewModel itemViewModel = new AddressViewModel()
                    {
                        IdAddress = item.IdAddress,
                        PublicPlace = item.PublicPlace,
                        Number = item.Number,
                        Neighborhood = item.Neighborhood,
                        City = item.City,
                        IdClient = item.IdClient,
                        StateClient = (State)item.StateClient,
                        ZipCode = item.ZipCode

                    };
                    listAddressViewModel.Add(itemViewModel);
                }
            }
            else
                listAddressViewModel = new List<AddressViewModel>();

            return listAddressViewModel;
        }
        #endregion

    }
}