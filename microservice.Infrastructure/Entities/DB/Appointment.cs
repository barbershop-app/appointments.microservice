using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Infrastructure.Entities.DB
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public int BarberShopId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public bool HasBeenHandeled { get; set; }
    }
}
