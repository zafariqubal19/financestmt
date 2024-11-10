﻿namespace RSCS.FinancingStatements.Web.Models
{
    public partial class User
    {
        //public int UserId { get; set; }

        //public string Username { get; set; } = null!;
        public int NameID { get; set; }

        public string ID { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
