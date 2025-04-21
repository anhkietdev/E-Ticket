using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class RoomRequestDto
    {
        public string RoomNumber { get; set; }

        public RoomType Type { get; set; }

        public int Capacity { get; set; }
    }
}