using System.ComponentModel.DataAnnotations;

namespace Jazyshaly.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public virtual User? Sender { get; set; }
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Введите отправителя")]
        public string? SenderName { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        public string? Content { get; set; }

        [Required]
        public DateTime When { get; set; } = DateTime.UtcNow;
    }
}
