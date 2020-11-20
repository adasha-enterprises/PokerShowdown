using iQmetrix.PokerHandShowdown.Interfaces;
using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown
{
    public abstract class CardHandCreator : ICardHandCreator
    {
        public abstract Task<ICardGameHand> CreateCardGameHand(string[] cards);
    }
}
