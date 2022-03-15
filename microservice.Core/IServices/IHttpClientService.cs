using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Core.IServices
{
    public interface IHttpClientService
    {
        /// <summary>
        /// Throws an exception if the user does not exist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CheckIfUserIsActive(Guid userId);
        /// <summary>
        /// Gets the booking day limit
        /// </summary>
        /// <param name="barberShopId"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        Task<int> GetBookingDayLimit(int barberShopId, DayOfWeek dayOfWeek);
    }
}
