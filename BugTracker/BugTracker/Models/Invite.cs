﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class Invite
    {
        public int Id { get; set; }

        [Display(Name = "Date Sent")]
        public DateTimeOffset InviteDate { get; set; }

        [Display(Name = "Join Date")]
        public DateTimeOffset JoinDate { get; set; }

        [DisplayName("Code")]
        public Guid CompanyToken { get; set; }

        [DisplayName("Company")]
        public int CompanyId { get; set; }

        [DisplayName("Project")]
        public int? ProjectId { get; set; }

        [DisplayName("Invitor")]
        public string InvitorId { get; set; }

        [DisplayName("Invitee")]
        public string InviteeId { get; set; }

        public bool IsValid { get; set; }

        ////////////////
        
        public virtual Company Company { get; set; }

        public virtual BTUser Invitor { get; set; }

        public virtual BTUser Invitee { get; set; }

        public virtual Project Project { get; set; }
    }
}
