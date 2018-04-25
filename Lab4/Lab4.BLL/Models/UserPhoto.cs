using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab4.BLL.Models
{
    public class UserPhoto
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(260)")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime LoadTime { get; set; }
    }
}
