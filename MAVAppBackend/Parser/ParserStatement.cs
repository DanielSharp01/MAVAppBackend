using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    /// <summary>
    /// Something that the Parser states after processing an API response
    /// </summary>
    public abstract class ParserStatement
    {
        /// <summary>
        /// API response that was processed to make this statement
        /// </summary>
        public APIResponse Origin { get; }

        /// <summary>
        /// Tells whether this statement was processed
        /// </summary>
        public bool Processed { get; private set; } = false;

        /// <param name="origin">API response that was processed to make this statement</param>
        public ParserStatement(APIResponse origin)
        {
            Origin = origin;
        }

        /// <summary>
        /// Process the statement into the database
        /// </summary>
        /// <param name="appContext">DbContext of the app</param>
        public void Process(AppContext appContext)
        {
            InternalProcess(appContext);
            Processed = true;
        }

        /// <summary>
        /// Process the statement into the database (actual implementation)
        /// </summary>
        /// <param name="appContext">DbContext of the app</param>
        protected abstract void InternalProcess(AppContext appContext);
    }
}
