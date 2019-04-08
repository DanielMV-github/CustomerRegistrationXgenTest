using System;
using System.Collections.Generic;
using System.Net;

namespace CustomerRegistration.Api
{
    public static class UseFul
    {
        public static bool ValidStringFill(IDictionary<string, string> fields, ref string replyMessage, ref HttpStatusCode httpStatus)
        {
            bool returnResponse = true;
            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field.Value))
                {
                    httpStatus = HttpStatusCode.InternalServerError;
                    replyMessage = string.Format("O preenchimento do campo {0} é obrigatório", field.Key);
                    returnResponse = false;
                    break;
                }
            }
            return returnResponse;
        }

        public static bool ValidStringFillOnlyNumbers(IDictionary<string, string> fields, ref string replyMessage, ref HttpStatusCode httpStatus)
        {
            bool returnResponse = true;
            Int64 convertedNumber;
            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field.Value))
                {
                    httpStatus = HttpStatusCode.InternalServerError;
                    replyMessage = string.Format("O preenchimento do campo {0} é obrigatório", field.Key);
                    returnResponse = false;
                    break;
                }
                else if (!Int64.TryParse(field.Value, out convertedNumber))
                {
                    httpStatus = HttpStatusCode.InternalServerError;
                    replyMessage = string.Format("O campo {0} aceita apenas números", field.Key);
                    returnResponse = false;
                    break;
                }
            }
            return returnResponse;
        }

        public static bool DateIsValid(DateTime date, string atributeDateName, ref string replyMessage, ref HttpStatusCode httpStatus)
        {
            bool returnResponse = true;
            if (date == DateTime.MinValue)
            {
                httpStatus = HttpStatusCode.InternalServerError;
                replyMessage = string.Format("O campo {0} é obrigatório ou deve ter um formato válido", atributeDateName);
                returnResponse = false;
            }
            return returnResponse;
        }

        public static string StringRemoveSpace(string value, bool onlyLaterals)
        {
            string returnResponse = string.Empty;
            if (onlyLaterals)
                returnResponse = value.Trim();
            else
                returnResponse = value.Replace(" ", "");
            return returnResponse;
        }
    }
}