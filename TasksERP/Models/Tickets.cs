//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TasksERP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tickets
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string CreationDate { get; set; }
        public string AssignmentDate { get; set; }
        public string ClosureDate { get; set; }
        public string DeadlineDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string Team { get; set; }
        public string Comment { get; set; }
        public string ManagersComment { get; set; }
        public string Notes1 { get; set; }
    }
}