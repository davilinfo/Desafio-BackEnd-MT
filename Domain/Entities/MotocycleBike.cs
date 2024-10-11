using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("moto")]
    public class MotocycleBike : StringIdentifierEntity
    {
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

        public MotocycleBike(string identifier, int year, string model, string plate)
        {
            this.Identifier = identifier;
            this.Year = year;
            this.Model = model;
            this.Plate = plate;
            this.CreatedDate = DateTime.UtcNow;
        }

        public MotocycleBike Update(string plate)
        {
            this.UpdatedDate = DateTime.UtcNow;            
            this.Plate= plate;

            return this;
        }
    }
}
