using MAVAppBackend.MAV;
using MAVAppBackend.Parser;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace MAVAppBackendTest
{
    public class TrainParseTest
    {
        [Fact]
        public void SimpleHeaderTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_2008.json", new DateTime(2020, 06, 08), 2008, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(2008, trainInfo?.Number);
            Assert.Null(trainInfo?.Name);
            Assert.Equal("S72", trainInfo?.ViszNumber);
            Assert.Equal(1, trainInfo?.TrainRelations.Count);
            Assert.Equal("Budapest-Nyugati", trainInfo?.TrainRelations[0].From);
            Assert.Equal("Esztergom", trainInfo?.TrainRelations[0].To);
            Assert.Equal(TrainType.Local, trainInfo?.TrainRelations[0].Type);
        }

        [Fact]
        public void SimpleHeaderWithNameTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_16303.json", new DateTime(2020, 06, 08), 16303, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(16303, trainInfo?.Number);
            Assert.Equal("HÉTMÉRFÖLDES", trainInfo?.Name);
            Assert.Null(trainInfo?.ViszNumber);
            Assert.Equal(1, trainInfo?.TrainRelations.Count);
            Assert.Equal("Zajta", trainInfo?.TrainRelations[0].From);
            Assert.Equal("Budapest-Nyugati", trainInfo?.TrainRelations[0].To);
            Assert.Equal(TrainType.Fast, trainInfo?.TrainRelations[0].Type);
        }

        [Fact]
        public void ForeignHeaderTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_347.json", new DateTime(2020, 06, 08), 347, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(347, trainInfo?.Number);
            Assert.Equal("DACIA", trainInfo?.Name);
            Assert.Null(trainInfo?.ViszNumber);
            Assert.Equal(1, trainInfo?.TrainRelations.Count);
            Assert.Equal("Wien Hbf", trainInfo?.TrainRelations[0].From);
            Assert.Equal("Bucureşti Nord", trainInfo?.TrainRelations[0].To);
            Assert.Equal(TrainType.Fast, trainInfo?.TrainRelations[0].Type);
        }

        [Fact]
        public void SimpleHeaderElviraIdRequestTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_2008.json", new DateTime(2020, 06, 08), null, "205484-181218"));

            Assert.NotNull(trainInfo);
            Assert.Equal(2008, trainInfo?.Number);
            Assert.Equal("205484-181218", trainInfo?.ElviraID);
            Assert.Null(trainInfo?.Name);
            Assert.Equal("S72", trainInfo?.ViszNumber);
            Assert.Equal(1, trainInfo?.TrainRelations.Count);
            Assert.Equal("Budapest-Nyugati", trainInfo?.TrainRelations[0].From);
            Assert.Equal("Esztergom", trainInfo?.TrainRelations[0].To);
            Assert.Equal(TrainType.Local, trainInfo?.TrainRelations[0].Type);
        }

        [Fact]
        public void MultiRelationHeaderTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_811.json", new DateTime(2020, 06, 08), 811, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(811, trainInfo?.Number);
            Assert.Equal("SOPIANAE", trainInfo?.Name);
            Assert.Null(trainInfo?.ViszNumber);
            Assert.Equal(2, trainInfo?.TrainRelations.Count);
            Assert.Equal("Pécs", trainInfo?.TrainRelations[0].From);
            Assert.Equal("Százhalombatta", trainInfo?.TrainRelations[0].To);
            Assert.Equal(TrainType.InterCity, trainInfo?.TrainRelations[0].Type);
            Assert.Equal("Százhalombatta", trainInfo?.TrainRelations[1].From);
            Assert.Equal("Kelenföld [Budapest]", trainInfo?.TrainRelations[1].To);
            Assert.Equal(TrainType.SubstitutionBus, trainInfo?.TrainRelations[1].Type);
        }

        [Fact]
        public void ExpiryDateTest()
        {
            var trainInfo = TrainParser.Parse(GetAPIResponseForTestFile("train_test_568.json", new DateTime(2020, 06, 08), 568, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(2019, trainInfo?.EstimatedExpiry?.Year);
            Assert.Equal(2, trainInfo?.EstimatedExpiry?.Month);
            Assert.Equal(3, trainInfo?.EstimatedExpiry?.Day);
        }

        private APIResponse GetAPIResponseForTestFile(string testFile, DateTime requestDate, int? trainId, string? elviraId)
        {
            var request = new JObject
            {
                ["a"] = "TRAIN",
                ["jo"] = new JObject(),
                ["request-date"] = requestDate
            };
            if (trainId != null) request["jo"]["vsz"] = "55" + trainId;
            if (elviraId != null) request["jo"]["v"] = elviraId;

            using (StreamReader reader = new StreamReader(testFile, Encoding.UTF8))
            {
                var responseObject = JObject.Parse(reader.ReadToEnd());
                if (trainId != null) responseObject["d"]["param"]["vsz"] = "55" + trainId;
                if (elviraId != null) responseObject["d"]["param"]["v"] = elviraId;
                
                return new APIResponse(HttpStatusCode.OK, request, responseObject);
            }
        }
    }
}
