using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Types of parser errors
    /// </summary>
    public enum ErrorTypes
    {
        RequestUnsuccessful,
        NoHtml,
        NoTable,
        NoTableRows,
        HeaderEmpty,
        NoTrainIdentification,
        StationLinkUnparsable,
        TrainReferenceUnparsable,
        TrainRelationUnparsable
    }

    /// <summary>
    /// Indicates that the parser found an error (should not be processed, only aids debugging)
    /// </summary>
    public class ErrorStatement : ParserStatement
    {
        /// <summary>
        /// Type of the error
        /// </summary>
        public ErrorTypes ErrorType { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="errorType">Type of the error</param>
        public ErrorStatement(APIResponse origin, ErrorTypes errorType)
            : base(origin)
        {
            ErrorType = errorType;
        }
    }
}
