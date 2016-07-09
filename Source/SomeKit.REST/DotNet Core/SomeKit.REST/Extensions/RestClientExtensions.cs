using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.REST.Extensions
{
    public static class RestClientExtensions
    {
        public static IRestClient WithBasicAuthentication(this IRestClient client,
            string user,
            string password)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            client.RequestHeaders.Add(new HttpRequestHeader
            {
                Name = "Authorization",
                Value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}"))
            });

            return client;
        }
    }
}
