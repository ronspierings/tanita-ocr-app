namespace TanitaTracker.Core.Entities;

/// <summary>
/// This Core Class represents the physical Tantita Print and matches the self trained Azure OCR-Model "tanita-ocr".
/// It is based on the JSON output of the tanita-ocr output.
/// </summary>
public class BodyCompositionScan
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty; // For row-level security

    public DateTime ScanDate { get; set; }
    public double WeightKg { get; set; }
    public double FatPercentage { get; set; }
    public double FatMassKg { get; set; }
    public double FfmKg { get; set; }
    public double MuscleMassKg { get; set; }
    public double MusclePercentage { get; set; }
    public double TbwKg { get; set; }
    public double TbwPercentage { get; set; }
    public double BoneMassKg { get; set; }
    public int BmrKj { get; set; }
    public int BmrKcal { get; set; }
    public int MetabolicAge { get; set; }
    public int VisceralFatRating { get; set; }
    public double Bmi { get; set; }
    public double ObesityPercentage { get; set; }
    public string PhysiqueRating { get; set; } = string.Empty;
}
