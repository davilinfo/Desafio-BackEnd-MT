namespace Application.Models.ViewModel
{
    public class MessageDeliver
    {
#pragma warning disable CS8618 
        public string Action { get; set; } 
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string UniqueIdentifier { get; set; }
        public DateTime Birthday { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriverLicenseType { get; set; }
        public string DriverLicenseImageS3 { get; set; }
#pragma warning restore CS8618
    }
}
