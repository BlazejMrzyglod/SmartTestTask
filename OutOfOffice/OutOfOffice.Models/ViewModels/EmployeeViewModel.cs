using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string PeoplePartner { get; set; } = null!;

        public int OutOfOfficeBalance { get; set; }

        public byte[]? Photo { get; set; }
    }
}
