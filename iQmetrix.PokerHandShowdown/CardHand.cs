using iQmetrix.PokerHandShowdown.Interfaces;
using iQmetrix.PokerHandShowdown.Helpers;
using iQmetrix.PokerHandShowdown.Models;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace iQmetrix.PokerHandShowdown
{
    public abstract class CardHand : ICardGameHand, IComparable<ICardGameHand>
    {
        public Card[] Cards { get; set; }
        public int CardsInHand { get; set; }

        protected void InitialSort()
        {
            Cards = Cards.OrderBy(c => c.Rank).OrderBy(c => Cards.Where(c1 => c1.Rank == c.Rank).Count()).ToArray();
        }

        public int GetGroupByRankCount(int n) => Cards.GroupBy(c => c.Rank).Count(g => g.Count() == n);

        public int GetGroupBySuitCount(int n) => Cards.GroupBy(c => c.Suit).Count(g => g.Count() == n);

        public int CompareTo(ICardGameHand other)
        {
            for (var i = CardsInHand - 1; i >= 0; i--)
            {
                CardRank rank1 = Cards[i].Rank, rank2 = other.Cards[i].Rank;

                if (rank1 > rank2)
                {
                    return 1;
                }
                else if (rank1 < rank2)
                {
                    return -1;
                }
            }

            return 0;
        }

        public bool Contains(Card card) => Cards.Where(c => c.Rank == card.Rank && c.Suit == card.Suit).Any();

        protected bool HandHasDuplicateCards() => Cards.GroupBy(c => new { c.Rank, c.Suit }).Where(c => c.Skip(1).Any()).Any();

        protected Card[] BuildHand(string[] cardsInput)
        {
            var cards = new Card[CardsInHand];

            // index 0 is the player, the remainder are the cards in the hand
            for (int i = 1; i < CardsInHand + 1; i++)
            {
                var card = new Card();

                try
                {
                    // when the card input contains a numeric, split out the number to determine the rank enum and also the suit
                    if (cardsInput[i].Any(char.IsDigit))
                    {
                        var numAlpha = new Regex("(?<Numeric>[0-9]*)(?<Alpha>[a-zA-Z]*)");
                        var match = numAlpha.Match(cardsInput[i]);
                        var alpha = match.Groups["Alpha"].Value;
                        var num = match.Groups["Numeric"].Value;

                        CardRank rank = (CardRank)Enum.ToObject(typeof(CardRank), Convert.ToInt16(num));
                        CardSuit suit = MappingHelper.MapSuit(alpha[0]);
                        card = new Card(rank, suit);
                    }
                    else // get the rank for the picture card and the suit
                    {
                        CardRank rank = MappingHelper.MapPictureCard(cardsInput[i][0]);
                        CardSuit suit = MappingHelper.MapSuit(cardsInput[i][1]);
                        card = new Card(rank, suit);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                cards[i - 1] = card;
            }

            return cards;
        }
    }
}
