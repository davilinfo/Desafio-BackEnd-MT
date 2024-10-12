using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Mongo
{
    [ExcludeFromCodeCoverage]
    [Table("moto")]
    public class MotocycleBikeMongo : StringIdentifierEntity
    {
#pragma warning disable CS8618
        public string _id { get; set; }
        [Column("ano")]
        public int Year { get; set; }
        [Column("modelo")]
        public string Model { get; set; }
        [Column("placa")]
        public string Plate { get; set; }
        [Column("data_criada")]
        public DateTime? CreatedDate { get; set; }
        [Column("data_atualizada")]
        public DateTime? UpdatedDate { get; set; }
#pragma warning restore CS8618
    }
}
