using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Models
{
    [Table("SoldOut")]
    public class SoldOut
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string NameItem { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public DateTime DateCreate { get; set; }
    }
}