using System.ComponentModel.DataAnnotations;

namespace ServiceAPI
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }
    }
}
