using System.Security.Cryptography;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entity
{
    public class Media
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [MaxLength(55)]
        public string ContentType { get; set; }

        public double SizeInMb 
        {
            get => Data.Length / (double)(1024 * 1024);
        }
        
        [Required]
        [MaxLength(3 * 1024 * 1024)]
        public byte[] Data { get; set; }

        [Obsolete("Used only for Entities binding.", true)]
        public Media() { }

        public Media(string contentType, byte[] data)
        {
            Id = Guid.NewGuid();
            ContentType = contentType;
            Data = data;
        }
    }
}