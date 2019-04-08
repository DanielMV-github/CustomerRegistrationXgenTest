using CustomerRegistration.Domain;
using CustomerRegistration.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CustomerRegistration.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/address")]
    public class AddressController : ApiController
    {
        private AddressDao dao;
        private HttpStatusCode httpStatus;
        private string replyMessage;

        public AddressController()
        {
            this.dao = new AddressDao();
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(Address address)
        {
            Guid idAddressReply = Guid.Empty;
            try
            {
                if (ValidationAddressFields(address, true))
                {
                    if (dao.Create(address))
                    {
                        httpStatus = HttpStatusCode.OK;
                        idAddressReply = address.IdAddress;
                        replyMessage = "Cadastro de endereço realizado com sucesso";
                    }
                    else
                        throw new System.InvalidOperationException("Erro ao cadastrar endereço, por favor contate o suporte");
                }
            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }

            return Request.CreateResponse(httpStatus, idAddressReply);

        }

        [HttpGet]
        [Route("read")]
        public HttpResponseMessage Read(Guid id)
        {
            Address replyAddress = null;
            try
            {
                replyAddress = dao.Read(id);
                if (replyAddress != null)
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Consulta de endereço realizada com sucesso";
                }
                else
                    throw new System.InvalidOperationException("Erro na consulta, endereço não encontrado");

            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }
            return Request.CreateResponse(httpStatus, replyAddress);
        }

        [HttpGet]
        [Route("readList")]
        public HttpResponseMessage ReadList(Guid idClient)
        {
            ICollection<Address> replyAddressList = null;
            try
            {
                replyAddressList = dao.ReadList(idClient);
                if (replyAddressList.Count > 0)
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Consulta de endereços realizada com sucesso";
                }
                else
                    throw new System.InvalidOperationException("Erro na consulta, não existe endereço cadastrado para o cliente informado");
            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }
            return Request.CreateResponse(httpStatus, replyAddressList);
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(Address address)
        {
            bool boolValueReply = false;
            try
            {
                if (ValidationAddressFields(address, false))
                {
                    if (dao.Update(address))
                    {
                        httpStatus = HttpStatusCode.OK;
                        boolValueReply = true;
                        replyMessage = "Endereço atualizado com sucesso";
                    }
                    else
                        throw new System.InvalidOperationException("Erro ao atualizar endereço, por favor contate o suporte");
                }
            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }
            return Request.CreateResponse(httpStatus, boolValueReply);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(Guid id)
        {
            bool boolValueReply = true;
            try
            {
                if (dao.Delete(id))
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Endereço deletado com sucesso";
                }
                else
                {
                    boolValueReply = false;
                    throw new System.InvalidOperationException("Erro ao deletar endereço, por favor contate o suporte");
                }
            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }

            return Request.CreateResponse(httpStatus, boolValueReply);
        }

        #region classes of help

        private bool ValidationAddressFields(Address address, bool isCreation)
        {
            TreatmentAddressFields(address);

            // Variaveis utilizadas para validação 
            IDictionary<string, string> stringFields = new Dictionary<string, string>();
            IDictionary<string, string> stringFieldsOnlyNumbers = new Dictionary<string, string>();

            stringFields.Add("PublicPlace", address.PublicPlace);
            stringFields.Add("Neighborhood", address.Neighborhood);
            stringFields.Add("City", address.City);
            stringFieldsOnlyNumbers.Add("Number", address.Number);
            stringFieldsOnlyNumbers.Add("ZipCode", address.ZipCode);

            if (!UseFul.ValidStringFill(stringFields, ref replyMessage, ref httpStatus))
                return false;
            if (!ValidadeAddressStringLength(address))
                return false;
            if (!UseFul.ValidStringFillOnlyNumbers(stringFieldsOnlyNumbers, ref replyMessage, ref httpStatus))
                return false;
            if (new ClientDao().Read(address.IdClient) == null)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O endereço não está associação a um cliente válido";
                return false;
            }

            if (CustomerAddressExists(address.ZipCode, address.Number, address.IdClient, address.IdAddress, isCreation))
                return false;

            return true;
        }

        private void TreatmentAddressFields(Address address)
        {
            // Remoção de espaço vazio
            address.PublicPlace = UseFul.StringRemoveSpace(address.PublicPlace, true);
            address.Neighborhood = UseFul.StringRemoveSpace(address.Neighborhood, true);
            address.City = UseFul.StringRemoveSpace(address.City, true);
            address.Number = UseFul.StringRemoveSpace(address.Number, false);
            address.ZipCode = UseFul.StringRemoveSpace(address.ZipCode, false);
        }

        private bool ValidadeAddressStringLength(Address address)
        {
            bool returnResponse = true;
            if (address.ZipCode.Length != 8)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O campo ZipCode possui quantidade de caracteres inválido";
                returnResponse = false;
            }
            return returnResponse;
        }

        private bool CustomerAddressExists(string zipCode, string number, Guid idClientAddress, Guid idAddress, bool isCreation)
        {
            string messageError = "O endereço já está cadastrado para este usuário (zipCode e number já existem)";
            Guid idAddressDao = AddressDao.CustomerAddressExists(zipCode, number, idClientAddress);

            if (idAddressDao == Guid.Empty)
                return false;

            if (isCreation)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = messageError;
                return true;
            }

            if (!isCreation && idAddressDao.ToString().ToLower() != idAddress.ToString().ToLower())
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = messageError;
                return true;
            }

            return false;
        }

        #endregion
    }
}
