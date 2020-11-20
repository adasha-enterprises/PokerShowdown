using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown.Interfaces
{
    public interface ICardHandCreator
    {
        Task<ICardGameHand> CreateCardGameHand(string[] cards);
    }
}
