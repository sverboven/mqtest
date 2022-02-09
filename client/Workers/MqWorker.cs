using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack.Messaging;

namespace client.Workers
{
    public class MqWorker : BackgroundService
    {
        private const int MqStatsDescriptionDurationMs = 10000;

        private readonly ILogger<MqWorker> _logger;

        private readonly IMessageService _mqServer;

        public MqWorker(ILogger<MqWorker> logger, IMessageService mqServer)
        {
            this._logger = logger;
            this._mqServer = mqServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._mqServer.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("MQ Worker running at: {stats}", this._mqServer.GetStatsDescription());
                await Task.Delay(MqStatsDescriptionDurationMs, stoppingToken);
            }

            this._mqServer.Stop();
        }
    }
}