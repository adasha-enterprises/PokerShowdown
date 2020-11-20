using System.Collections.Generic;
using System.Threading.Tasks;

namespace iQmetrix.PokerHandShowdown.Interfaces
{
    public interface ICardGameService
    {
        Task<IList<string>> Evaluate<T>(IDictionary<string, T> hands) where T : ICardGameHand;
    }
}
