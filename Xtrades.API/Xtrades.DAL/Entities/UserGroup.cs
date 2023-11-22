using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Xtrades.DAL.Entities
{
    public class UserGroup
    {
        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }
        public bool IsCreator { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }

    }
}
