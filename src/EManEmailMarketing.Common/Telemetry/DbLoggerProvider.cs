using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EManEmailMarketing.Common.Telemetry
{
    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;

        public DbLoggerProvider(IOptions<DbLoggerOptions> _options)
        {
            Options = _options.Value; // Stores all the options.
        }

        /// <summary>
        /// Creates a new instance of the db logger.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
