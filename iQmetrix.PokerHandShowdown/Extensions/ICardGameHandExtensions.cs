using iQmetrix.PokerHandShowdown.Implementations;
using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Models;
using System;

namespace iQmetrix.PokerHandShowdown.Extensions
{
    public static class ICardGameHandExtensions
    {
        public static bool IsValidHand<T>(this ICardGameHand cardGameHand, T handType) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T is not enum");
            }

            //TODO: create a nested switch expression for the different types of card game hands
            if (cardGameHand is PokerHand)
            {
                return handType switch
                {
                    PokerHandType.Flush => cardGameHand.GetGroupBySuitCount(5) == 1,
                    PokerHandType.ThreeOfAKind => cardGameHand.GetGroupByRankCount(3) == 1,
                    PokerHandType.OnePair => cardGameHand.GetGroupByRankCount(2) == 1,
                    PokerHandType.HighCard => cardGameHand.GetGroupByRankCount(1) == 5,
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
