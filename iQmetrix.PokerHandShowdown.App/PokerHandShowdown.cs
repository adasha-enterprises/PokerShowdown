﻿using iQmetrix.PokerHandShowdown.Interfaces;
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
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _lifeTime;

        public PokerHandShowdown(ICardGameService cardGameService, ICardHandCreator cardHandCreator, ILogger<PokerHandShowdown> logger, IHostApplicationLifetime lifeTime)
        {
            _cardGameService = cardGameService;
            _cardHandCreator = cardHandCreator;
            _logger = logger;
            _lifeTime = lifeTime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var hands = new Dictionary<string, ICardGameHand>();

            var playersAndCards = new string[] { "Joe,3H,6H,8H,JH,KH" , "Jen,3C,3D,3S,8C,10D", "Bob,2H,5C,7S,10C,AC" };

            try
            {
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
