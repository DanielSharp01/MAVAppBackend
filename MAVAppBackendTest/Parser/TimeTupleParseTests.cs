using HtmlAgilityPack;
using MAVAppBackend.Parser;
using System;
using Xunit;

namespace MAVAppBackendTest.Parser
{
    public class TimeTupleParseTests
    {
        [Fact]
        public void Simple()
        {
            var document = new HtmlDocument();
            document.LoadHtml("20:05<br><span style=\"color: red\">20:06</span>");
            var parsed = TimeTuple.Parse(document.DocumentNode);

            Assert.NotNull(parsed);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Scheduled);
            Assert.Equal(new TimeSpan(20, 06, 0), parsed?.Actual);
        }

        [Fact]
        public void NoActual()
        {
            var document = new HtmlDocument();
            document.LoadHtml("20:05<br><span style=\"color: red\"></span>");
            var parsed = TimeTuple.Parse(document.DocumentNode);

            Assert.NotNull(parsed);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Scheduled);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Actual);
        }

        [Fact]
        public void MissingBr()
        {
            var document = new HtmlDocument();
            document.LoadHtml("20:05<span style=\"color: red\">20:06</span>");
            var parsed = TimeTuple.Parse(document.DocumentNode);

            Assert.NotNull(parsed);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Scheduled);
            Assert.Equal(new TimeSpan(20, 06, 0), parsed?.Actual);
        }

        [Fact]
        public void ReallyNoActual()
        {
            var document = new HtmlDocument();
            document.LoadHtml("20:05<br>");
            var parsed = TimeTuple.Parse(document.DocumentNode);

            Assert.NotNull(parsed);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Scheduled);
            Assert.Equal(new TimeSpan(20, 05, 0), parsed?.Actual);
        }

        [Fact]
        public void CompletelyEmpty()
        {
            var document = new HtmlDocument();
            document.LoadHtml("");
            var parsed = TimeTuple.Parse(document.DocumentNode);

            Assert.Null(parsed);
        }
    }
}
