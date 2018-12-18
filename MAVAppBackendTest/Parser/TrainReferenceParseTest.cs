using HtmlAgilityPack;
using MAVAppBackend.Parser;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MAVAppBackendTest.Parser
{
    public class TrainReferenceParseTest
    {
        [Fact]
        public void SimpleFromStation()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml("<td style=\"color:#000000\"><a onclick=\"map.getData('TRAIN', { v: '1391430_181218', d: '18.12.18', language: '1' } );\"" +
                "2844</a>&nbsp;&nbsp;zónázó <br>&nbsp;13:03&nbsp;Budapest-Nyugati -- Cegléd&nbsp;14:12</td>");
            var train = TrainReference.ParseFromStation(document.DocumentNode);
            Assert.NotNull(train);
            Assert.Equal("1391430_181218", train?.ElviraID);
            Assert.Equal(2844, train?.Number);
            Assert.Equal(TrainType.Local, train?.Type);
            Assert.NotNull(train?.From);
            Assert.Equal("Budapest-Nyugati", train?.From?.Name);
            Assert.Equal(new TimeSpan(13, 03, 00), train?.FromTime);
            Assert.NotNull(train?.To);
            Assert.Equal("Cegléd", train?.To?.Name);
            Assert.Equal(new TimeSpan(14, 12, 00), train?.ToTime);
        }

        [Fact]
        public void MissingFromFromStation()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml("<td style=\"color:#000000\"><a onclick=\"map.getData('TRAIN', { v: '1391430_181218', d: '18.12.18', language: '1' } );\"" +
                "2844</a>&nbsp;&nbsp;zónázó <br>&nbsp; -- Cegléd&nbsp;14:12</td>");
            var train = TrainReference.ParseFromStation(document.DocumentNode);
            Assert.NotNull(train);
            Assert.Null(train?.From);
            Assert.Null(train?.FromTime);
            Assert.NotNull(train?.To);
            Assert.Equal("Cegléd", train?.To?.Name);
            Assert.Equal(new TimeSpan(14, 12, 00), train?.ToTime);
        }

        [Fact]
        public void MissingToFromStation()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml("<td style=\"color:#000000\"><a onclick=\"map.getData('TRAIN', { v: '1391430_181218', d: '18.12.18', language: '1' } );\"" +
                "2844</a>&nbsp;&nbsp;zónázó <br>&nbsp;13:03&nbsp;Budapest-Nyugati -- </td>");
            var train = TrainReference.ParseFromStation(document.DocumentNode);
            Assert.NotNull(train);
            Assert.NotNull(train?.From);
            Assert.Equal("Budapest-Nyugati", train?.From?.Name);
            Assert.Equal(new TimeSpan(13, 03, 00), train?.FromTime);
            Assert.Null(train?.To);
        }

        [Fact]
        public void MissingOnclick()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml("<td style=\"color:#000000\"><a>2844</a>&nbsp;&nbsp;zónázó <br>&nbsp;13:03&nbsp;Budapest-Nyugati -- Cegléd&nbsp;14:12</td>");
            var train = TrainReference.ParseFromStation(document.DocumentNode);
            Assert.NotNull(train);
            Assert.Null(train?.ElviraID);
            Assert.Equal(2844, train?.Number);
            Assert.Equal(TrainType.Local, train?.Type);
            Assert.NotNull(train?.From);
            Assert.Equal("Budapest-Nyugati", train?.From?.Name);
            Assert.Equal(new TimeSpan(13, 03, 00), train?.FromTime);
            Assert.NotNull(train?.To);
            Assert.Equal("Cegléd", train?.To?.Name);
            Assert.Equal(new TimeSpan(14, 12, 00), train?.ToTime);
        }

        [Fact]
        public void MessedUpFormatFromStation()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml("<td style=\"color:#0000pr>&nbsp;13:03&nbsp;Budaéd&nbsp;14:12</td>");
            var train = TrainReference.ParseFromStation(document.DocumentNode);
            Assert.Null(train);
        }
    }
}
