using iQmetrix.PokerHandShowdown.Interfaces;
using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown
{
    public abstract class CardHandCreator
    {
        public abstract Task<ICardGameHand> CreateCardGameHand(string[] cards);
    }
}
