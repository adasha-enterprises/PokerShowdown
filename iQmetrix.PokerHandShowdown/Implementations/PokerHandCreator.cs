using iQmetrix.PokerHandShowdown.Interfaces;
using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown.Implementations
{
    public class PokerHandCreator : CardHandCreator
    {
        public override async Task<ICardGameHand> CreateCardGameHand(string[] cards)
        {
            return await Task.FromResult(new PokerHand(cards));
        }
    }
}