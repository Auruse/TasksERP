using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksERP.Models
{
    public class TicketsViewModel : Tickets
    {
        public int _Id { get; set; }
        public  string _Owner { get; set; }
        public  string _CreationDate { get; set; }
        public  string _AssignmentDate { get; set; }
        public  string _ClosureDate { get; set; }
        public  string _DeadlineDate { get; set; }
        public  string _Subject { get; set; }
        public  string _Description { get; set; }
        public  string _Status { get; set; }
        public  string _AssignedTo { get; set; }
        public  string _Team { get; set; }
        public  string _Comment { get; set; }
        public  string _ManagersComment { get; set; }
        public  string _Notes1 { get; set; }
    }
}