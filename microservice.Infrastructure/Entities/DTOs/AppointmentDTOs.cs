using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Infrastructure.Entities.DTOs
{
    public class AppointmentDTOs
    {
        public class Create
        {
            public int BarberShopId { get; set; }
            public Guid UserId { get; set; }
            public DateTime Date { get; set; }

        }
        public class Update
        {
            public Guid Id { get; set; }
            public bool HasBeenHandeled { get; set; }
        }

        public class AvailableSlots
        {
            public int BarberShopId { get; set; }
            public DateTime Date { get; set; }
        }

    }
}
