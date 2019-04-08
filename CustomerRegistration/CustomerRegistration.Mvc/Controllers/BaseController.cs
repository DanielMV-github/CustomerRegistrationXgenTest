using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CustomerRegistration.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected string apiBaseUrl = "http://localhost:49239/"; // endereço base da Api

        protected async Task<ReplyObjectApi> RequestWebApiAsync(string methodPath, string methodType, string jsonContent = null)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonStringResponse = string.Empty;
                HttpResponseMessage response = null;
                ReplyObjectApi replyObjectApi = new ReplyObjectApi();

                if (methodType.ToUpper() == "POST")
                {
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(apiBaseUrl + methodPath, content);
                }
                else if (methodType.ToUpper() == "DELETE")
                {
                    response = await client.DeleteAsync(apiBaseUrl + methodPath);
                }
                else
                {
                    response = await client.GetAsync(apiBaseUrl + methodPath);
                }

                jsonStringResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    replyObjectApi.NameHttpStatusCode = "OK";
                    replyObjectApi.Message = "Ação realizada com sucesso";
                }
                else
                {
                    replyObjectApi.NameHttpStatusCode = "InternalServerError";
                    replyObjectApi.Message = "Erro ao realizar ação";
                }

                replyObjectApi.Value = jsonStringResponse;

                return replyObjectApi;
            }
        }

        protected void ResponsetToRequisition(string message)
        {
            ViewBag.objResponsetToRequisition = Newtonsoft.Json.JsonConvert.SerializeObject(new { messageResponse = message });
        }
    }

    public class ReplyObjectApi
    {
        public string NameHttpStatusCode { get; set; }

        public string Message { get; set; }

        public string Value { get; set; }
    }
}