using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Helpers;
using iQmetrix.PokerHandShowdown.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Xunit;
using Moq;

namespace iQmetrix.PokerHandShowdown.Tests
{
    public class PokerGameServiceTests
    {
        [Theory]
        [InlineData("Exceeded the maximum number of players")]
        public async void ExceedingMaximumPlayersThrowsException(string exMessage)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();
            var cardGameHand = new Mock<ICardGameHand>();

            for (var i = 0; i <= 10; i++)
            {
                hands.Add(i.ToString(), cardGameHand.Object);
            }

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await new PokerGameService().Evaluate(hands));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        [Theory]
        [InlineData("There are duplicate cards across the hands")]
        public async void HasDuplicateCardsAcrossHandsThrowsException(string exMessage)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();
            var cardGameHand = new Mock<ICardGameHand>();
            cardGameHand.Setup(h => h.Cards).Returns(new Card[] { new Card(CardRank.Ace, CardSuit.Spades) });

            for (var i = 0; i <= 9; i++)
            {
                hands.Add(i.ToString(), cardGameHand.Object);
            }

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await new PokerGameService().Evaluate(hands));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        // more or less the same test as above, except that this is directly using the helper method that doesn't throw an exception
        [Theory]
        [InlineData(true)]
        public void CanCheckHasDuplicateCardsAcrossHands(bool expected)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();
            var cardGameHand = new Mock<ICardGameHand>();
            cardGameHand.Setup(h => h.Cards).Returns(new Card[] { new Card(CardRank.Ace, CardSuit.Spades) });

            for (var i = 0; i <= 9; i++)
            {
                hands.Add(i.ToString(), cardGameHand.Object);
            }

            // act
            var result = CardGameHandHelper.HasDuplicateCardsAcrossHands(hands.Values.ToList());

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3C,3D,3S,8C,10D", "Bob,2H,5C,7S,10C,AC", "Joe")]
        [InlineData("Joe,3H,4D,9C,9D,QH", "Jen,5C,7D,9H,9S,QS", "Bob,2H,2C,5S,10C,AC", "Jen")]
        public async void CanEvaluateSingleWinner(string joesHand, string jensHand, string bobsHand, string expected)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();

            new string[] { joesHand, jensHand, bobsHand }.ToList().ForEach(async h =>
                {
                    var _ = h.Split(',');
                    hands.Add(_[0], await new PokerHandCreator().CreateCardGameHand(_));
                }
            );

            var cardGameService = new PokerGameService();

            // act
            var winner = await cardGameService.Evaluate(hands);

            // assert
            Assert.Equal(1, winner.Count);
            Assert.Equal(expected, winner[0]);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3D,6D,8D,JD,KD", "Bob,2H,5C,7S,10C,AC", "Joe,Jen")]
        public async void CanEvaluateMultipleWinners(string joesHand, string jensHand, string bobsHand, string expected)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();

            new string[] { joesHand, jensHand, bobsHand }.ToList().ForEach(async h =>
                {
                    var _ = h.Split(',');
                    hands.Add(_[0], await new PokerHandCreator().CreateCardGameHand(_));
                }
            );

            var cardGameService = new PokerGameService();

            // act
            var winners = await cardGameService.Evaluate(hands);

            // assert
            Assert.Equal(2, winners.Count);
            Assert.Equal(expected.Split(',')[0], winners[0]);
            Assert.Equal(expected.Split(',')[1], winners[1]);
        }
    }
}
