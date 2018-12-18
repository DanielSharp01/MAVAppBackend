using MAVAppBackend.Parser;
using MAVAppBackend.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MAVAppBackendTest.Parser
{
    public class TrainTableParseTest
    {
        [Fact]
        public void TableWithPlatform()
        {
            var parsedData = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_811.json", DateTime.Now, 811, null));

            Assert.NotNull(parsedData);
            Assert.Equal(8, parsedData?.TrainStations.Count);

            Assert.Equal(0, parsedData?.TrainStations[0].Distance);
            Assert.Equal(432, parsedData?.TrainStations[0].Station.ID);
            Assert.Equal("Pécs", parsedData?.TrainStations[0].Station.Name);
            Assert.Null(parsedData?.TrainStations[0].Arrival);
            Assert.NotNull(parsedData?.TrainStations[0].Departure);
            Assert.Equal(new TimeSpan(19, 14, 00), parsedData?.TrainStations[0].Departure?.Scheduled);
            Assert.Equal(new TimeSpan(19, 14, 00), parsedData?.TrainStations[0].Departure?.Actual);
            Assert.Equal("1", parsedData?.TrainStations[0].Platform);
            Assert.False(parsedData?.TrainStations[0].Hit);

            Assert.Equal(64, parsedData?.TrainStations[3].Distance);
            Assert.Equal(363, parsedData?.TrainStations[3].Station.ID);
            Assert.NotNull(parsedData?.TrainStations[3].Arrival);
            Assert.Equal(new TimeSpan(20, 01, 00), parsedData?.TrainStations[3].Arrival?.Scheduled);
            Assert.Equal(new TimeSpan(20, 02, 00), parsedData?.TrainStations[3].Arrival?.Actual);
            Assert.NotNull(parsedData?.TrainStations[3].Departure);
            Assert.Equal(new TimeSpan(20, 05, 00), parsedData?.TrainStations[3].Departure?.Scheduled);
            Assert.Equal(new TimeSpan(20, 06, 00), parsedData?.TrainStations[3].Departure?.Actual);
            Assert.Equal("7", parsedData?.TrainStations[3].Platform);
            Assert.True(parsedData?.TrainStations[3].Hit);

            Assert.Equal(224, parsedData?.TrainStations[7].Distance);
            Assert.Equal(3, parsedData?.TrainStations[7].Station.ID);
            Assert.Equal("Kelenföld [Budapest]", parsedData?.TrainStations[7].Station.Name);
            Assert.NotNull(parsedData?.TrainStations[7].Arrival);
            Assert.Equal(new TimeSpan(22, 20, 00), parsedData?.TrainStations[7].Arrival?.Scheduled);
            Assert.Equal(new TimeSpan(22, 20, 00), parsedData?.TrainStations[7].Arrival?.Actual);
            Assert.Null(parsedData?.TrainStations[7].Departure);
            Assert.Null(parsedData?.TrainStations[7].Platform);
            Assert.False(parsedData?.TrainStations[7].Hit);
        }

        [Fact]
        public void TableWithoutPlatform()
        {
            var parsedData = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_2008.json", DateTime.Now, 2008, null));

            Assert.NotNull(parsedData);
            Assert.Equal(21, parsedData?.TrainStations.Count);

            Assert.Equal(0, parsedData?.TrainStations[0].Distance);
            Assert.Equal(486, parsedData?.TrainStations[0].Station.ID);
            Assert.Equal("Budapest-Nyugati", parsedData?.TrainStations[0].Station.Name);
            Assert.Null(parsedData?.TrainStations[0].Arrival);
            Assert.NotNull(parsedData?.TrainStations[0].Departure);
            Assert.Equal(new TimeSpan(18, 51, 00), parsedData?.TrainStations[0].Departure?.Scheduled);
            Assert.Equal(new TimeSpan(18, 51, 00), parsedData?.TrainStations[0].Departure?.Actual);
            Assert.Null(parsedData?.TrainStations[0].Platform);
            Assert.False(parsedData?.TrainStations[0].Hit);

            foreach (TrainStation station in parsedData?.TrainStations.Skip(1))
            {
                Assert.True(station.Hit);
            }
        }
    }
}
