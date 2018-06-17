using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachePolicyService
{
    public interface ICachePolicyService
    {
        CacheItemPolicy GetCachePolicy();
    }

    public class CachePolicyService : ICachePolicyService
    {
        public CacheItemPolicy GetCachePolicy()
        {
            var policy = new CacheItemPolicy();
            if (ConfigurationManager.AppSettings["PolicyType"] == "ExpirationTime")
            {
                double interval;
                bool isDouble = double.TryParse(ConfigurationManager.AppSettings["TimeIntervalMilliseconds"], out interval);
                if (!isDouble)
                {
                    throw new ArgumentException("Expiration time interval should be a number");
                }
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(interval);
            }
            if (ConfigurationManager.AppSettings["PolicyType"] == "UsingSQLMonitors")
            {
                policy.ChangeMonitors.Add(new SqlChangeMonitor(new SqlDependency(new SqlCommand("Select * from Northwind.Category"))));
            }
            return policy;

        }
    }
}
