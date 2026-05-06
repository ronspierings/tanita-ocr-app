using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;

using TanitaTracker.Core.Entities;
using TanitaTracker.Core.Interfaces;

namespace TanitaTracker.Infrastructure.Services
{
    public class AzureOcrService : IOcrService
    {
        public Task<BodyCompositionScan> AnalyzeScanAsync(Stream documentStream, CancellationToken cancellationToken = default)
        {
            // TODO: Write the implementation
            throw new NotImplementedException("Work in progress (as in: Azure Document Intelligence Setup... ");
        }
    }
}
