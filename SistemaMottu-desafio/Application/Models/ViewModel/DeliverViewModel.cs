namespace Application.Models.ViewModel
{
    public class DeliverViewModel
    {
        public required string Identifier { get; set; }
        public required string Name { get; set; }
        public required string UniqueIdentifier { get; set; }
        public DateTime Birthday { get; set; }
        public required string DriverLicenseNumber { get; set; }
        public required string DriverLicenseType { get; set; }
        public required string DriverLicenseImageS3 { get; set; }
#pragma warning disable CS8618
        public string DriverLicenseImage { get; set; }
#pragma warning restore CS8618 
        public DateTime? CreatedDate { get; set; }
    }
}
