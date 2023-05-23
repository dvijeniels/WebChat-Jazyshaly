using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Jazyshaly.Models
{
    public class User:IdentityUser
    {
        [Required]
        [StringLength(40)]
        [DisplayName("ФИО")]
        public string FirstAndLastName { get; set; }
        public virtual List<Message>? Messages { get; set; }
    }
}
