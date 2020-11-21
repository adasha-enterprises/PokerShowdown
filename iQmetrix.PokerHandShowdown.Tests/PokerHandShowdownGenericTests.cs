using iQmetrix.PokerHandShowdown.Helpers;
using System;
using Xunit;

namespace iQmetrix.PokerHandShowdown.Tests
{
    public class PokerHandShowdownGenericTests
    {
        [Theory]
        [InlineData("Invalid suit")]
        public void InvalidSuitThrowsException(string exMessage)
        {
            // arrange

            // act
            var ex = Assert.Throws<Exception>(() => MappingHelper.MapSuit('A'));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        [Theory]
        [InlineData("Invalid rank")]
        public void InvalidRankThrowsException(string exMessage)
        {
            // arrange

            // act
            var ex = Assert.Throws<Exception>(() => MappingHelper.MapPictureCard('G'));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }
    }
}
