using System;

namespace iQmetrix.PokerHandShowdown.Implementations
{
    public class PokerHand : CardHand
    {
        public PokerHand(string[] cardsInput)
        {
            Cards = BuildHand(cardsInput);

            Sort();

            // going on the basis that the game is played with a single deck of cards and no cheating is allowed...
            if (GetGroupByRankCount(5) != 0)
            {
                throw new Exception("Can't have five cards with the same rank");
            }

            // see above
            if (HandHasDuplicateCards())
            {
                throw new Exception("There are duplicate cards in the hand");
            }
        }
    }
}
