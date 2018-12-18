using MAVAppBackend.Parser;
using MAVAppBackendTest.TestData;
using System;
using Xunit;

namespace MAVAppBackendTest.Parser
{
    public class TrainHeaderParseTest
    {
        [Fact]
        public void SimpleHeader()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_2008.json", new DateTime(2020, 06, 08), 2008, null));

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
        public void SimpleHeaderWithName()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_16303.json", new DateTime(2020, 06, 08), 16303, null));

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
        public void ForeignHeader()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_347.json", new DateTime(2020, 06, 08), 347, null));

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
        public void SimpleHeaderElviraIdRequest()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_2008.json", new DateTime(2020, 06, 08), null, "205484-181218"));

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
        public void MultiRelationHeader()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_811.json", new DateTime(2020, 06, 08), 811, null));

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
        public void ExpiryDate()
        {
            var trainInfo = TrainParser.Parse(TrainTestData.GetAPIResponseForTestFile("train_test_568.json", new DateTime(2020, 06, 08), 568, null));

            Assert.NotNull(trainInfo);
            Assert.Equal(2019, trainInfo?.EstimatedExpiry?.Year);
            Assert.Equal(2, trainInfo?.EstimatedExpiry?.Month);
            Assert.Equal(3, trainInfo?.EstimatedExpiry?.Day);
        }
    }
}
