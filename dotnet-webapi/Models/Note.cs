using System.ComponentModel.DataAnnotations;

namespace DotnetWebAPI.Models {
    public class Note {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }

        public Note(string text) {
            Text = text;
        }
    }
}
