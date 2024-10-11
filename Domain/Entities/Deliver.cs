using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("entregador")]
    public class Deliver : StringIdentifierEntity
    {
        [Required]
        [Column("nome")]
        public string Name { get; set; }
        [Required]
        [Column("cnpj")]
        public string UniqueIdentifier { get; set; }
        [Required]
        [Column("data_nascimento")]
        public DateTime Birthday { get; set; }
        [Required]
        [Column("numero_cnh")]
        public string DriverLicenseNumber { get; set; }
        [Required]
        [Column("tipo_cnh")]
        public string DriverLicenseType { get; set; }
        [Required]
        [Column("imagem_cnh_s3")]
        public string DriverLicenseImageS3 { get; set; }
        [NotMapped]
        [Column("imagem_cnh")]
        public string DriverLicenseImage { get; set; }
        [Column("data_criada")]
        public DateTime? CreatedDate { get; set; }

#pragma warning disable CS8618 
        public Deliver() { }


        public Deliver(
#pragma warning restore CS8618 
            string identifier,
            string name, 
            string uniqueIdentifier, 
            DateTime birthday, 
            string driverLicenseNumber, 
            string driverLicenseType, 
            string driverLicenseImage)
        {
            this.Identifier = identifier;
            this.Name = name;
            this.Birthday = birthday;
            this.DriverLicenseNumber = driverLicenseNumber;            
            this.DriverLicenseType = driverLicenseType;
            this.UniqueIdentifier = uniqueIdentifier;
            this.DriverLicenseImage = driverLicenseImage ?? string.Empty;
            this.CreatedDate = DateTime.UtcNow;
        }
        
        public Deliver Update(string driverLicenseImage, string driverLicenseS3)
        {
            this.DriverLicenseImage = driverLicenseImage;
            this.DriverLicenseImageS3 = driverLicenseS3 ?? string.Empty;
            return this;
        }
    }
}
