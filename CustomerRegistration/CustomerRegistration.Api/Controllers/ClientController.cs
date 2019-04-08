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
    [RoutePrefix("api/client")]
    public class ClientController : ApiController
    {
        private ClientDao dao;
        private HttpStatusCode httpStatus;
        private string replyMessage;

        public ClientController()
        {
            this.dao = new ClientDao();
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(Client client)
        {
            Guid IdClientReply = Guid.Empty;
            try
            {
                if (ValidationClientFields(client))
                {
                    if (dao.Create(client))
                    {
                        httpStatus = HttpStatusCode.OK;
                        IdClientReply = client.IdClient;
                        replyMessage = "Cadastro de cliente realizado com sucesso";
                    }
                    else
                        throw new System.InvalidOperationException("Erro ao cadastrar cliente, por favor contate o suporte");
                }
            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }

            return Request.CreateResponse(httpStatus, IdClientReply);
        }

        [HttpGet]
        [Route("read")]
        public HttpResponseMessage Read(string cpf)
        {
            Client replyClient = null;
            try
            {
                replyClient = dao.Read(cpf);
                if (replyClient != null)
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Consulta de cliente realizada com sucesso";
                }
                else
                    throw new System.InvalidOperationException("Erro na consulta, cliente não encontrado");

            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }

            return Request.CreateResponse(httpStatus, replyClient);
        }

        [HttpGet]
        [Route("readList")]
        public HttpResponseMessage ReadList()
        {
            ICollection<Client> replyClientList = null;
            try
            {
                replyClientList = dao.ReadList();
                if (replyClientList.Count > 0)
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Consulta de clientes realizada com sucesso";
                }
                else
                    throw new System.InvalidOperationException("Erro na consulta, não existe cliente cadastrado");

            }
            catch (Exception ex)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = ex.Message;
            }
            return Request.CreateResponse(httpStatus, replyClientList);
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(Client client)
        {
            bool boolValueReply = false;
            try
            {
                if (ValidationClientFields(client))
                {
                    if (dao.Update(client))
                    {
                        httpStatus = HttpStatusCode.OK;
                        boolValueReply = true;
                        replyMessage = "Cliente atualizado com sucesso";
                    }
                    else
                        throw new System.InvalidOperationException("Erro ao atualizar cliente, por favor contate o suporte");
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
        public HttpResponseMessage Delete(string cpf)
        {
            bool boolValueReply = true;
            try
            {
                if (dao.Delete(cpf))
                {
                    httpStatus = HttpStatusCode.OK;
                    replyMessage = "Cliente deletado com sucesso";
                }
                else
                {
                    boolValueReply = false;
                    throw new System.InvalidOperationException("Erro ao deletar cliente, por favor contate o suporte");
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

        private bool ValidationClientFields(Client client)
        {
            TreatmentClientFields(client);

            // Variaveis utilizadas para validação 
            IDictionary<string, string> stringFields = new Dictionary<string, string>();
            IDictionary<string, string> stringFieldsOnlyNumbers = new Dictionary<string, string>();

            stringFields.Add("Name", client.Name);
            stringFields.Add("LastName", client.LastName);
            stringFields.Add("Email", client.Email);
            stringFieldsOnlyNumbers.Add("Cpf", client.Cpf);
            stringFieldsOnlyNumbers.Add("DDD", client.DDD);
            stringFieldsOnlyNumbers.Add("Telephone", client.Telephone);

            if (!UseFul.ValidStringFill(stringFields, ref replyMessage, ref httpStatus))
                return false;
            if (!ValidadeClientStringLength(client))
                return false;
            if (!UseFul.ValidStringFillOnlyNumbers(stringFieldsOnlyNumbers, ref replyMessage, ref httpStatus))
                return false;
            if (!UseFul.DateIsValid(client.BirthDate, "BirthDate", ref replyMessage, ref httpStatus))
                return false;
            if (!VerifyExistenceCpf(client.Cpf, client.IdClient))
                return false;

            return true;
        }

        private void TreatmentClientFields(Client client)
        {
            // Remoção de espaço vazio
            client.Name = UseFul.StringRemoveSpace(client.Name, true);
            client.LastName = UseFul.StringRemoveSpace(client.LastName, true);
            client.Email = UseFul.StringRemoveSpace(client.Email, false);
            client.Cpf = UseFul.StringRemoveSpace(client.Cpf, false);
            client.DDD = UseFul.StringRemoveSpace(client.DDD, false);
            client.Telephone = UseFul.StringRemoveSpace(client.Telephone, false);
        }

        private bool ValidadeClientStringLength(Client client)
        {
            bool returnResponse = true;
            if (client.Cpf.Length != 11)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O campo Cpf possui quantidade de caracteres inválido";
                return false;
            }
            if (client.DDD.Length != 2)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O campo DDD possui quantidade de caracteres inválido";
                returnResponse = false;
            }
            if (client.Telephone.Length == 0 || client.Telephone.Length > 9)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O campo Telephone possui quantidade de caracteres inválido";
                returnResponse = false;
            }
            return returnResponse;
        }

        private bool VerifyExistenceCpf(string cpf, Guid idClient)
        {
            bool returnResponse = true;
            Guid idClientDao = ClientDao.ClientExistsInTheContext(cpf);
            if (idClientDao != Guid.Empty && idClient.ToString().ToLower() != idClientDao.ToString().ToLower())
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = "O Cpf informado já está cadastrado para outro usuário";
                returnResponse = false;
            }
            return returnResponse;
        }

        #endregion

    }
}
