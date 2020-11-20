using iQmetrix.PokerHandShowdown.Models;
using System;

namespace iQmetrix.PokerHandShowdown.Helpers
{
    public static class MappingHelper
    {
        public static CardSuit MapSuit(char suit)
        {
            return suit switch
            {
                'C' => CardSuit.Clubs,
                'D' => CardSuit.Diamonds,
                'H' => CardSuit.Hearts,
                'S' => CardSuit.Spades,
                _ => throw new Exception("Invalid suit"),
            };
        }

        public static CardRank MapPictureCard(char pictureCard)
        {
            return pictureCard switch
            {
                'J' => CardRank.Jack,
                'Q' => CardRank.Queen,
                'K' => CardRank.King,
                'A' => CardRank.Ace,
                _ => throw new Exception("Invalid rank"),
            };
        }
    }
}
