using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Time tuple that has a Scheduled and an Actual field appearing in time referencing fields in the API
    /// </summary>
    public class TimeTuple
    {
        /// <summary>
        /// Scheduled time
        /// </summary>
        public TimeSpan Scheduled { get; }
        /// <summary>
        /// Actual time (including delays, should not neccesarily be used to determine it though)
        /// </summary>
        public TimeSpan Actual { get; }

        /// <param name="scheduled">Scheduled time</param>
        /// <param name="actual">Actual time (including delays, should not neccesarily be used to determine it though)</param>
        public TimeTuple(TimeSpan scheduled, TimeSpan actual)
        {
            Scheduled = scheduled;
            Actual = actual;
        }

        public static TimeTuple? Parse(HtmlNode htmlNode)
        {
            var nodeEnum = htmlNode.ChildNodes.ToList().GetEnumerator();
            if (!nodeEnum.MoveNext()) return null;
            if (!TimeSpan.TryParse(nodeEnum.Current.InnerText, out TimeSpan scheduled)) return null;
            TimeSpan actual = (!nodeEnum.MoveNext() || !TimeSpan.TryParse(nodeEnum.Current.InnerText, out actual)) ? scheduled : actual;
            return new TimeTuple(scheduled, actual);
        }
    }
}
