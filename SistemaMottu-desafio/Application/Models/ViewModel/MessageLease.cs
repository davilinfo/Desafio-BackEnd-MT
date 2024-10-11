namespace Application.Models.ViewModel
{
    public class MessageLease
    {
#pragma warning disable CS8618
        public string Action { get; set; }
        public string Identifier { get; set; }        
        public string DeliverId { get; set; }        
        public string MotocycleBikeId { get; set; }        
        public DateTime InitialDate { get; set; }        
        public DateTime EndDate { get; set; }        
        public DateTime PreviewEndDate { get; set; }        
        public int Plan { get; set; }        
        public DateTime? DevolutionDate { get; set; }        
        public double? Value { get; set; }
#pragma warning restore CS8618
    }
}
