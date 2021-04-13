using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeleGram
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GroupsTele",
                url: "tele-groups-data",
                defaults: new { controller = "Tele", action = "Groups"}
            );

            routes.MapRoute(
                name: "KhachHangTele",
                url: "tele-khach-hang-data",
                defaults: new { controller = "Tele", action = "KhachHang"}
            );

            routes.MapRoute(
                name: "ChiTieuTele",
                url: "tele-chi-tieu-data",
                defaults: new { controller = "Tele", action = "ChiTieu"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Tele", action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}
