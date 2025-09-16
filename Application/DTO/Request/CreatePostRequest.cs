using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class CreatePostRequest
    {
        //public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public PostType Type { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
    }
}
