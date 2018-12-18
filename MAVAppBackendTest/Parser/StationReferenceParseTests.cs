using MAVAppBackend.Parser;
using Xunit;

namespace MAVAppBackendTest.Parser
{
    public class StationReferenceParseTests
    {
        [Fact]
        public void Simple()
        {
            var station = StationReference.FromScript("map.getData('STATION', { i: '423', a: 'Sásd', d: '18.12.16', language: '1' } );return false;");
            Assert.NotNull(station);
            Assert.Equal(423, station?.ID);
            Assert.Equal("Sásd", station?.Name);
        }

        [Fact]
        public void NoId()
        {
            var station = StationReference.FromScript("map.getData('STATION', { a: 'Sásd', d: '18.12.16', language: '1' } );return false;");
            Assert.NotNull(station);
            Assert.Null(station?.ID);
            Assert.Equal("Sásd", station?.Name);
        }

        [Fact]
        public void CompletelyMessedUp()
        {
            var station = StationReference.FromScript("map.ge( { asd )tDase;");
            Assert.Null(station);
        }
    }
}
