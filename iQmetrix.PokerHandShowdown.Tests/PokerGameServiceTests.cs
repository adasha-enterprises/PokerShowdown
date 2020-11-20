using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Xunit;
using Moq;
using iQmetrix.PokerHandShowdown.Extensions;
using iQmetrix.PokerHandShowdown.Helpers;

namespace iQmetrix.PokerHandShowdown.Tests
{
    public class PokerGameServiceTests
    {
        [Theory]
        [InlineData("Exceeded the maximum number of players")]
        public void ExceedingMaximumPlayersThrowsException(string exMessage)
        {
            // arrange
            var hands = new Dictionary<string, ICardGameHand>();
            var cardGameHand = new Mock<ICardGameHand>();

            for (var i = 0; i <= 10; i++)
            {
                hands.Add(i.ToString(), cardGameHand.Object);
            }

            // act
            var ex = Assert.ThrowsAsync<Exception>(async () => await new PokerGameService().Evaluate(hands));

            // assert
            Assert.NotNull(ex.Result);
            Assert.Equal(exMessage, ex.Result.Message);
        }

        [Theory]
        [InlineData("There are duplicate cards across the hands")]
        public void HasDuplicateCardsAcrossHandsThrowsException(string exMessage)
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
            var ex = Assert.ThrowsAsync<Exception>(async () => await new PokerGameService().Evaluate(hands));

            // assert
            Assert.NotNull(ex.Result);
            Assert.Equal(exMessage, ex.Result.Message);
        }

        // more or less the same test as above, except that this is directly using the helper method that doesn't throw an exception
        [Theory]
        [InlineData(true)]
        public void HasDuplicateCardsAcrossHands(bool expected)
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
        [InlineData("Joe,AS,AS,AC,AH,AD", "Can't have five cards with the same rank")]
        public void HandHasFiveCardsWithSameRankThrowsException(string playerAndCardinput, string exMessage)
        {
            // arrange
            var playerAndCards = playerAndCardinput.Split(',');

            // act
            var ex = Assert.ThrowsAsync<Exception>(async () => await new PokerHandCreator().CreateCardGameHand(playerAndCards));

            // assert
            Assert.NotNull(ex.Result);
            Assert.Equal(exMessage, ex.Result.Message);
        }

        [Theory]
        [InlineData("Joe,AS,AS,8H,JH,KH", "There are duplicate cards in the hand")]
        public void HandHasDuplicateCardsThrowsException(string playerAndCardinput, string exMessage)
        {
            // arrange
            var playerAndCards = playerAndCardinput.Split(',');

            // act
            var ex = Assert.ThrowsAsync<Exception>(async () => await new PokerHandCreator().CreateCardGameHand(playerAndCards));

            // assert
            Assert.NotNull(ex.Result);
            Assert.Equal(exMessage, ex.Result.Message);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3C,3D,3S,8C,10D", "Bob,2H,5C,7S,10C,AC", "Joe")]
        [InlineData("Joe,3H,4D,9C,9D,QH", "Jen,5C,7D,9H,9S,QS", "Bob,2H,2C,5S,10C,AC", "Jen")]
        public void EvaluateSingleWinner(string joesHand, string jensHand, string bobsHand, string expected)
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
            var winner = cardGameService.Evaluate(hands).Result;

            // assert
            Assert.Equal(1, winner.Count);
            Assert.Equal(expected, winner[0]);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3D,6D,8D,JD,KD", "Bob,2H,5C,7S,10C,AC", "Joe,Jen")]
        public void EvaluateMultipleWinners(string joesHand, string jensHand, string bobsHand, string expected)
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
            var winners = cardGameService.Evaluate(hands).Result;

            // assert
            Assert.Equal(2, winners.Count);
            Assert.Equal(expected.Split(',')[0], winners[0]);
            Assert.Equal(expected.Split(',')[1], winners[1]);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", PokerHandType.Flush, true)]
        [InlineData("Joe,3H,6H,8H,JH,KS", PokerHandType.Flush, false)]
        public async void CheckIfValidHand(string cards, PokerHandType handType, bool expected)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(cards.Split(','));

            // act
            var result = hand.IsValidHand(handType);

            // assert
            Assert.Equal(expected, result);
        }

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
