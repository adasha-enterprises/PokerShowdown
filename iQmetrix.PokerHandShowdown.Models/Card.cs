namespace iQmetrix.PokerHandShowdown.Models
{
    public struct Card
    {
        public Card(CardRank rank, CardSuit suit) : this()
        {
            Rank = rank;
            Suit = suit;
        }

        public CardRank Rank { get; private set; }

        public CardSuit Suit { get; private set; }
    }
}
