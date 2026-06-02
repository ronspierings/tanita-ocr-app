using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using TanitaTracker.Core.Entities;
using TanitaTracker.Core.Interfaces;

namespace TanitaTracker.Infrastructure.Services
{
    /// <summary>
    /// Specific Azure Document Intelligence resource that runs a Custom Exteraction Model to understand the Tanita Body Composition paper print.
    /// Read more: https://contentunderstanding.ai.azure.com/documentintelligence/studio/custommodel
    /// </summary>
    public class AzureOcrService : IOcrService
    {
        private readonly DocumentIntelligenceClient _client;
        private readonly string _modelId = "tantita-model1"; // Your custom model name

        public AzureOcrService(IConfiguration config)
        {
            // Retrieve endpoints and keys from config
            var endpoint = config["AzureAi:Endpoint"] ?? throw new ArgumentNullException("AzureAi:Endpoint");
            
            _client = new DocumentIntelligenceClient(new Uri(endpoint), new DefaultAzureCredential());
        }

        public Task<BodyCompositionScan> AnalyzeScanAsync(Stream documentStream, CancellationToken cancellationToken = default)
        {
            // TODO: Write the implementation
            throw new NotImplementedException("Work in progress (as in: Azure Document Intelligence Setup... ");
        }
    }
}
