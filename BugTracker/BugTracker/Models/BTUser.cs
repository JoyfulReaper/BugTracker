using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get => $"{FirstName} {LastName}"; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile FormFile { get; set; }

        [DisplayName("Avatar")]
        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        [DisplayName("File Extenstion")]
        public string FileContentType { get; set; }

        public int? CompanyId { get; set; }

        //////////////////

        public virtual Company Company { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
