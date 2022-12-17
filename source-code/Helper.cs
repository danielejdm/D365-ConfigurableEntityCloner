using System.Linq;

namespace ConfigurableEntityCloner
{
    public static class Helper
    {
        public static string GetRecordIdFromUrl(string url)
        {

            string[] urlParts = url.Split("?".ToArray());
            string[] urlParams = urlParts[1].Split("&".ToCharArray());
            var id = urlParams.Where(e => e.StartsWith("id=")).FirstOrDefault().Replace("id=", "");

            return id;
        }
    }
}
