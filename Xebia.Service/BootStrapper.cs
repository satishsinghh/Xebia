using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xebia.Service.Interface;

namespace Xebia.Service
{
    
    public class BootStrapper
    {
        //Service layer DI registration
        public static void BootStrapServices(IServiceCollection services)
        {
            services.AddTransient<IAssetService, AssetService>();
            services.AddTransient<IBookingDetailService, BookingDetailService>();
            services.AddTransient<IBookingRoomService, BookingRoomService>();
        }
    }
}