using iQmetrix.PokerHandShowdown.Configuration;
using iQmetrix.PokerHandShowdown.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace iQmetrix.PokerHandShowdown.App
{
    public class PokerHandShowdown : IHostedService
    {
        private readonly ICardGameService _cardGameService;
        private readonly ICardHandCreator _cardHandCreator;
        private readonly ConfigSettings _configSettings;
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _lifeTime;

        public PokerHandShowdown(
            ICardGameService cardGameService,
            ICardHandCreator cardHandCreator,
            IOptions<ConfigSettings> options,
            ILogger<PokerHandShowdown> logger,
            IHostApplicationLifetime lifeTime
        )
        {
            _cardGameService = cardGameService;
            _cardHandCreator = cardHandCreator;
            _configSettings = options.Value;
            _logger = logger;
            _lifeTime = lifeTime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var hands = new Dictionary<string, ICardGameHand>();

            try
            {
                //TODO: add some validation to the property to ensure it's formatted correctly, but for now the value in appsettings.json is valid
                var playersAndCards = _configSettings.PLAYERS_AND_CARDS.Split('|');

                playersAndCards.ToList().ForEach(async p =>
                    {
                        var _ = p.Split(',');
                        hands.Add(_[0], await _cardHandCreator.CreateCardGameHand(_));
                    }
                );

                var pokerHands = await _cardGameService.Evaluate(hands) as List<string>;

                pokerHands.ForEach(p =>
                    {
                        _logger.LogInformation($"{p} had a winning hand");
                    }
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred in StartAsync");
            }

            // needed to prevent host from running indefinitely
            _lifeTime.StopApplication();

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Game over man, game over!");

            return Task.CompletedTask;
        }
    }
}
