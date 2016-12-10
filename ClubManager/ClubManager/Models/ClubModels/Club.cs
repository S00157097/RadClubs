using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClubManager.Models.ClubModels
{
    [Table("Club")]
    public class Club
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Club ID")]
        public int ClubId { get; set; }

        [Display(Name = "Club Name")]
        public string ClubName { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Admininstrator ID")]
        public int AdminId { get; set; }

        public virtual ICollection<Member> clubMembers { get; set; }
        public virtual ICollection<ClubEvent> clubEvents { get; set; }
        
        
    }
}
