using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Extensions;
using iQmetrix.PokerHandShowdown.Helpers;
using iQmetrix.PokerHandShowdown.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace iQmetrix.PokerHandShowdown.Implementations
{
    public class PokerGameService : ICardGameService
    {
        public async Task<IList<string>> Evaluate<T>(IDictionary<string, T> hands) where T : ICardGameHand
        {
            // going on the basis that a single deck of cards will only allow for 10 players with 5 cards each...
            if (hands.Count > 10)
            {
                throw new Exception("Exceeded the maximum number of players");
            }

            // again, going on the basis that the game is played with a single deck of cards and no cheating is allowed...
            if (CardGameHandHelper.HasDuplicateCardsAcrossHands(hands.Values.ToList()))
            {
                throw new Exception("There are duplicate cards across the hands");
            }

            var length = Enum.GetValues(typeof(PokerHandType)).Length;
            var winners = new List<string>();
            var winningType = PokerHandType.HighCard;

            try
            {
                //TODO: may need to consider refactoring re: Big O Notation
                foreach (var name in hands.Keys)
                {
                    for (var handType = PokerHandType.Flush; (int)handType < length; handType += 1)
                    {
                        var hand = hands[name];

                        if (hand.IsValidHand(handType))
                        {
                            int compareHands = 0, compareCards = 0;
                            if (winners.Count == 0 || (compareHands = winningType.CompareTo(handType)) > 0 || compareHands == 0 && (compareCards = hand.CompareTo(hands[winners[0]])) >= 0)
                            {
                                if (compareHands > 0 || compareCards > 0)
                                {
                                    winners.Clear();
                                }
                                winners.Add(name);
                                winningType = handType;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("There are no winners in this game!");
            }

            return await Task.FromResult(winners);
        }
    }
}
