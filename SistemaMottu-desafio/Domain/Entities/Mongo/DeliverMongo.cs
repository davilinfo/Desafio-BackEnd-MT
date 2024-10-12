using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Mongo
{
    [ExcludeFromCodeCoverage]
    [Table("entregador")]
    public class DeliverMongo : StringIdentifierEntity
    {
#pragma warning disable CS8618
        public string _id { get; set; }
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
#pragma warning restore CS8618 

    }
}
