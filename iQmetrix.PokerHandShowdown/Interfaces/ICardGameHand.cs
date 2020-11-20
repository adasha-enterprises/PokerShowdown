using iQmetrix.PokerHandShowdown.Models;

namespace iQmetrix.PokerHandShowdown.Interfaces
{
    public interface ICardGameHand
    {
        Card[] Cards { get; set; }
        int CompareTo(ICardGameHand other);
        bool Contains(Card card);
        bool HandHasDuplicateCards();
        int GetGroupByRankCount(int n);
        int GetGroupBySuitCount(int n);
    }
}
