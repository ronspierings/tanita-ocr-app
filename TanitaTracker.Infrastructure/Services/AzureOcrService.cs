using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Invocations of the Azure Document Intelligence service start here
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AzureOcrService(IConfiguration config)
        {
            // Retrieve endpoints and keys from config
            var endpoint = config["AzureAi:Endpoint"] ?? throw new ArgumentNullException("AzureAi:Endpoint");
            
            // Retrieve a Client. Note: AzureCredentials are received via Entra ID 
            _client = new DocumentIntelligenceClient(new Uri(endpoint), new DefaultAzureCredential());
        }

        /// <summary>
        /// Invoke the OCR-service and (try to) map the values into a BodyCompositionScan
        /// Note: The OCR returns per value a confidence score which we abide a 80% score minimum
        /// </summary>
        /// <param name="documentStream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Data and other general errors generate a generic Exception</exception>
        public async Task<BodyCompositionScan> AnalyzeScanAsync(Stream documentStream, CancellationToken cancellationToken = default)
        {
            // Convert the filestream directly to BinaryData (instead of Base64 a
            BinaryData bytesSource = BinaryData.FromStream(documentStream);

            // Invoke the Document Intelligence call
            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, _modelId, bytesSource, cancellationToken);
            AnalyzeResult result = operation.Value;

            // Extracting the fields (nullable)
            var document = result.Documents.FirstOrDefault();
            if (document == null) 
                throw new Exception("No document data found.");

            var fields = document.Fields;

            // Map to the Domain Entity using helpers because of the OCR confidence scores
            return new BodyCompositionScan
            {
                ScanDate = ParseDateField(fields, "datetime") ?? DateTime.UtcNow,
                WeightKg = GetDoubleValue(fields, "weight"),
                FatPercentage = GetDoubleValue(fields, "fat_percentage"),
                FatMassKg = GetDoubleValue(fields, "fat_mass"),
                FfmKg = GetDoubleValue(fields, "FFM"),
                MuscleMassKg = GetDoubleValue(fields, "muscle_mass"),
                MusclePercentage = GetDoubleValue(fields, "muscle_percentage"),
                TbwKg = GetDoubleValue(fields, "TBW"),
                TbwPercentage = GetDoubleValue(fields, "TBW_percentage"),
                BoneMassKg = GetDoubleValue(fields, "bone_mass"),
                BmrKcal = GetIntValue(fields, "BMR_kcal"),
                MetabolicAge = GetIntValue(fields, "metabolic_age"),
                VisceralFatRating = GetIntValue(fields, "visceral_fat_rating"),
                Bmi = GetDoubleValue(fields, "BMI"),
                ObesityPercentage = GetDoubleValue(fields, "obesity_percentage"),
                PhysiqueRating = GetStringValue(fields, "physique_rating")
            };
        }

        // Extraction helpers
        private double GetDoubleValue(IReadOnlyDictionary<string, DocumentField> fields, string key)
        {
            if (fields.TryGetValue(key, out var field) && field.FieldType == DocumentFieldType.Double)
                return field.ValueDouble ?? 0;

            // Fallback: Sometimes Azure reads decimals as strings depending on locale (e.g., "88,3" vs "88.3")
            if (fields.TryGetValue(key, out var stringField) && stringField.FieldType == DocumentFieldType.String)
                if (double.TryParse(stringField.ValueString.Replace(",", "."), out var parsed)) return parsed;

            return 0;
        }

        private static int GetIntValue(IReadOnlyDictionary<string, DocumentField> fields, string key)
        {
            if (fields.TryGetValue(key, out var field) && field?.ValueInt64 is not null)
                return (int) field.ValueInt64;

            return 0;
        }

        private static string GetStringValue(IReadOnlyDictionary<string, DocumentField> fields, string key)
        {
            if (fields.TryGetValue(key, out var field) && field?.ValueString is not null)
                return field.ValueString;

            return string.Empty;
        }

        private static DateTime? ParseDateField(IReadOnlyDictionary<string, DocumentField> fields, string key)
        {
            if (fields.TryGetValue(key, out var field) && field?.ValueDate is not null)
                return field.ValueDate.Value.DateTime;

            return null;
        }
    }
}
