using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Extensions;
using iQmetrix.PokerHandShowdown.Models;
using System;
using Xunit;

namespace iQmetrix.PokerHandShowdown.Tests
{
    public class PokerHandCreatorTests
    {
        [Theory]
        [InlineData("Joe,AS,6H,8H,JH,KH")]
        public async void CanCreateHand(string playerAndCards)
        {
            // arrange

            // act
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));

            // assert
            Assert.IsType<PokerHand>(hand);
        }

        [Theory]
        [InlineData("Joe,AS,AS,AC,AH,AD", "Can't have five cards with the same rank")]
        public async void HandHasFiveCardsWithSameRankThrowsException(string playerAndCardinput, string exMessage)
        {
            // arrange
            var playerAndCards = playerAndCardinput.Split(',');

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await new PokerHandCreator().CreateCardGameHand(playerAndCards));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        [Theory]
        [InlineData("Joe,AS,AS,8H,JH,KH", "There are duplicate cards in the hand")]
        public async void HandHasDuplicateCardsThrowsException(string playerAndCardinput, string exMessage)
        {
            // arrange
            var playerAndCards = playerAndCardinput.Split(',');

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await new PokerHandCreator().CreateCardGameHand(playerAndCards));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        [Theory]
        [InlineData("Joe,AS,6H,8H,JH,KH", CardRank.Ace, CardSuit.Spades, true)]
        [InlineData("Joe,3H,6H,8H,JH,KS", CardRank.Ace, CardSuit.Spades, false)]
        public async void CanCheckIfHandContainsCard(string playerAndCards, CardRank cardRank, CardSuit cardSuit, bool expected)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));
            var card = new Card(cardRank, cardSuit);

            // act
            var result = hand.Contains(card);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", PokerHandType.Flush, true)]
        [InlineData("Joe,3H,6H,8H,JH,KS", PokerHandType.Flush, false)]
        public async void CanCheckIfValidHand(string playerAndCards, PokerHandType handType, bool expected)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));

            // act
            var result = hand.IsValidHand(handType);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "T is not enum")]
        public async void CheckIfValidHandWhenNotEnumThrowsException(string playerAndCards, string exMessage)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));

            // act
            var ex = Assert.Throws<ArgumentException>(() => hand.IsValidHand(new Dummy()));

            // assert
            Assert.NotNull(ex);
            Assert.Equal(exMessage, ex.Message);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3C,3D,3S,8C,10D", 1)]
        [InlineData("Joe,3H,4D,9C,9D,QH", "Jen,5C,7D,9H,9S,QS", -1)]
        [InlineData("Joe,3H,6H,8H,JH,KH", "Jen,3D,6D,8D,JD,KD", 0)]
        public async void CanCompareHands(string firstPlayerAndCards, string secondPlayerAndCards, int expected)
        {
            // arrange
            var firstHand = await new PokerHandCreator().CreateCardGameHand(firstPlayerAndCards.Split(','));
            var secondHand = await new PokerHandCreator().CreateCardGameHand(secondPlayerAndCards.Split(','));

            // act
            var result = firstHand.CompareTo(secondHand);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Joe,3H,6H,8H,JH,KH", 5, 1)]
        public async void CanGetGroupBySuitCount(string playerAndCards, int cardCount, int expected)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));

            // act
            var result = hand.GetGroupBySuitCount(cardCount);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Joe,3H,3D,3S,JD,KS", 3, 1)]
        [InlineData("Joe,3H,3D,4C,9D,QH", 2, 1)]
        [InlineData("Joe,3H,4H,5H,6H,KD", 1, 5)]
        public async void CanGetGroupByRankCount(string playerAndCards, int cardCount, int expected)
        {
            // arrange
            var hand = await new PokerHandCreator().CreateCardGameHand(playerAndCards.Split(','));

            // act
            var result = hand.GetGroupByRankCount(cardCount);

            // assert
            Assert.Equal(expected, result);
        }

        // dummy struct to test IsValidHand extension method
        public struct Dummy { }
    }
}
