namespace Iara.DomainModel.ResponseDTOs
{
    public class CatchCompositionResponseDTO
    {
        public long CatchId { get; set; }
        public long LogEntryId { get; set; }
        public string FishSpecies { get; set; }
        public decimal? WeightKg { get; set; }
        public int? Count { get; set; }
        public string Status { get; set; }
    }
}