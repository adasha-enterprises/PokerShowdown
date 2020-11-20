using iQmetrix.PokerHandShowdown.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace iQmetrix.PokerHandShowdown.Helpers
{
    public static class CardGameHandHelper
    {
        public static bool HasDuplicateCardsAcrossHands<T>(IList<T> hands) where T : ICardGameHand
        {
            //TODO: possibly need to refactor this re: Big O Notation
            for (var i = 0; i < hands.Count; i++)
            {
                foreach (var card in hands[i].Cards)
                {
                    if (hands.Where((h, p) => p != i).Any(h => h.Cards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
