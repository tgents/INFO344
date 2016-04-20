using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace GetNumsApp
{
    /// <summary>
    /// Summary description for GetNumsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class GetNumsService : System.Web.Services.WebService
    {
        public static List<int> nums;

        public GetNumsService()
        {
            nums = new List<int>();
            Random gen = new Random();
            for (int i = 0; i < 10000; i++)
            {
                nums.Add(gen.Next(int.MaxValue));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetNumbersThatStartsWith(int start)
        {
            if (start > 0)
            { 
                List<int> vals = new List<int>();
                foreach (int num in nums)
                {
                    if (num.ToString().StartsWith(start.ToString()))
                    {
                        vals.Add(num);
                    }
                    if(vals.Count == 10)
                    {
                        break;
                    }
                }
                return new JavaScriptSerializer().Serialize(vals);
            }
            return "{ \"Error\": \"please enter a positive integer\" }";
        }
    }
}
