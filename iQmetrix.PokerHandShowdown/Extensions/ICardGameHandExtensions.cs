using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Models;

namespace iQmetrix.PokerHandShowdown.Extensions
{
    public static class ICardGameHandExtensions
    {
        public static bool IsValidHand(this ICardGameHand pokerHand, PokerHandType handType)
        {
            //TODO: create a nested switch expression for the different types of card game hands
            if (pokerHand is PokerHand)
            {
                return handType switch
                {
                    PokerHandType.Flush => pokerHand.GetGroupBySuitCount(5) == 1,
                    PokerHandType.ThreeOfAKind => pokerHand.GetGroupByRankCount(3) == 1,
                    PokerHandType.OnePair => pokerHand.GetGroupByRankCount(2) == 1,
                    PokerHandType.HighCard => pokerHand.GetGroupByRankCount(1) == 5,
                    _ => false,
                };
            }
            else
            {
                return false;
            }
        }
    }
}
