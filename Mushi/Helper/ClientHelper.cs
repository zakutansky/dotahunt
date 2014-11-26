using Newtonsoft.Json;
using MushiDataTypes;

namespace Mushi.Helper
{
    public class ClientHelper
    {
        /// <summary>
        /// Sets the result for client.
        /// </summary>
        /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public static string SetResultForClient(bool isSuccess, string param)
        {
            return JsonConvert.SerializeObject( new ClientResponse
                {
                    ResultType = isSuccess ? "success" : "invalid",
                    Parameter = param
                }
            );
        }
    }
}