using iQmetrix.PokerHandShowdown.Models;
using System;

namespace iQmetrix.PokerHandShowdown.Implementations
{
    // making an assumption that a poker hand is a poker hand, so don't allow this class to be inherited
    public sealed class PokerHand : CardHand
    {
        public PokerHand(string[] cardsInput)
        {
            CardsInHand = 5;

            Cards = BuildHand(cardsInput);

            Sort();

            // going on the basis that the game is played with a single deck of cards and no cheating is allowed...
            if (GetGroupByRankCount(CardsInHand) != 0)
            {
                throw new Exception("Can't have five cards with the same rank");
            }

            // see above
            if (HandHasDuplicateCards())
            {
                throw new Exception("There are duplicate cards in the hand");
            }
        }

        private void Sort()
        {
            InitialSort();

            // need to put the Ace first when the other cards are 2-5
            if (Cards[4].Rank == CardRank.Ace && Cards[0].Rank == CardRank.Two && ((int)Cards[3].Rank - (int)Cards[0].Rank == 3))
            {
                Cards = new Card[] { Cards[4], Cards[0], Cards[1], Cards[2], Cards[3] };
            }
        }
    }
}
