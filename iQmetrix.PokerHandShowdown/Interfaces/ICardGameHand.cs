using iQmetrix.PokerHandShowdown.Models;

namespace iQmetrix.PokerHandShowdown.Interfaces
{
    public interface ICardGameHand
    {
        Card[] Cards { get; set; }
        int CardsInHand { get; set; }
        int CompareTo(ICardGameHand other);
        bool Contains(Card card);
        int GetGroupByRankCount(int n);
        int GetGroupBySuitCount(int n);
    }
}
