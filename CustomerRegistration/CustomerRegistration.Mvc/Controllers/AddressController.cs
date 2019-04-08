using CustomerRegistration.Mvc.Models;
using CustomerRegistration.Mvc.ViewModel;
using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomerRegistration.Mvc.Controllers
{
    public class AddressController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> Create(AddressViewModel viewModel)
        {
            ReplyObjectApi replyObjectApi = null;
            try
            {
                Address addressModel = AddressViewModel.ConvertViewModelToModelAddress(viewModel);
                string jsonContent = new JavaScriptSerializer().Serialize(addressModel);
                replyObjectApi = await base.RequestWebApiAsync("api/address/create", "POST", jsonContent);
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
            return Json(replyObjectApi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Read(Guid idAddress)
        {
            ReplyObjectApi replyObjectApi = null;
            AddressViewModel viewModel = null;

            try
            {
                replyObjectApi = await base.RequestWebApiAsync(string.Format("api/address/read?id={0}", idAddress), "GET");
                if (replyObjectApi.NameHttpStatusCode == "OK")
                {
                    Address addressModel = JsonConvert.DeserializeObject<Address>(replyObjectApi.Value);
                    viewModel = AddressViewModel.ConvertModelToViewModelClient(addressModel);
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
                        Message = "Erro: Não foi possível resgatar os dados do endereço, entre em contato com o suporte",
                        Value = null,
                    };
                }
                viewModel = new AddressViewModel();
            }

            replyObjectApi.Value = JsonConvert.SerializeObject(viewModel);
            return Json(replyObjectApi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(AddressViewModel viewModel)
        {
            ReplyObjectApi replyObjectApi = null;
            try
            {
                Address addressModel = AddressViewModel.ConvertViewModelToModelAddress(viewModel);
                string jsonContent = new JavaScriptSerializer().Serialize(addressModel);
                replyObjectApi = await base.RequestWebApiAsync("api/address/update", "POST", jsonContent);
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
            return Json(replyObjectApi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid idAddress)
        {
            ReplyObjectApi replyObjectApi = null;
            AddressViewModel viewModel = null;
            try
            {
                replyObjectApi = await base.RequestWebApiAsync(string.Format("api/address/delete?id={0}", idAddress), "DELETE");
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
                        Message = "Erro: Não foi possível deletar o endereço, entre em contato com o suporte",
                        Value = null,
                    };
                }
                viewModel = new AddressViewModel();
            }
            return Json(replyObjectApi, JsonRequestBehavior.AllowGet);
        }
    }
}