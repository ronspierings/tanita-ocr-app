using System;
using System.Collections.Generic;
using System.Text;
using TanitaTracker.Core.Entities;

namespace TanitaTracker.Core.Interfaces
{
    public interface IOcrService
    {
        /// <summary>
        /// Analyzes a stream (image) using the custom Tanita OCR model and maps it to the domain entity.
        /// </summary>
        Task<BodyCompositionScan> AnalyzeScanAsync(Stream documentStream, CancellationToken cancellationToken = default);
    }
}
