using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentQuizSystem.Data.Entities
{
    public class StudentAttempt
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty; // link to Identity user

    public int QuizId { get; set; }
        public DateTime TakenAt { get; set; } = DateTime.UtcNow;
        public int Score { get; set; }
        // store selected option ids as JSON serialized list
        public string SelectedOptionIdsJson { get; set; } = "[]";
        [NotMapped]
        public List<int> SelectedOptionIds
        {
            get =>
            System.Text.Json.JsonSerializer.Deserialize<List<int>>(SelectedOptionIdsJson) ??
            new List<int>();
            set => SelectedOptionIdsJson =
            System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}
