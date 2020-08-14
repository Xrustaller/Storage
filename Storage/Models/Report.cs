using System;

namespace Storage.Models
{
    public class ReportItem
    {
        public string NameItem { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public DateTime DateCreate { get; set; }
        public string Status { get; set; }
    }
}
