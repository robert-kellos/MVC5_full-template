﻿

namespace MVC5_Full_template.Services.BrowserConfig
{
    using System.Text;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Boilerplate.Web.Mvc;
    using MVC5_Full_template.Constants.HomeController;

    public class BrowserConfigService : IBrowserConfigService
    {
        private readonly UrlHelper _urlHelper;

        public BrowserConfigService(UrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins 
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and 
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        public string GetBrowserConfigXml()
        {
            // The URL to the 70x70 small tile image.
            var square70X70LogoUrl = this._urlHelper.Content("~/content/icons/mstile-70x70.png");
            // The URL to the 150x150 medium tile image.
            var square150X150LogoUrl = this._urlHelper.Content("~/content/icons/mstile-150x150.png");
            // The URL to the 310x310 large tile image.
            var square310X310LogoUrl = this._urlHelper.Content("~/content/icons/mstile-310x310.png");
            // The URL to the 310x150 wide tile image.
            var wide310X150LogoUrl = this._urlHelper.Content("~/content/icons/mstile-310x150.png");
            // The color of the tile. This color only shows if part of your images above are transparent.
            const string tileColor = "#1E1E1E";
            // Update the tile every 1440 minutes. Defines the frequency, in minutes, between poll requests. Must be 
            // one of the following values: 30 (1/2 Hour), 60 (1 Hour), 360 (6 Hours), 720 (12 Hours), 1440 (24 Hours).
            const int frequency = 1440;
            // Control notification cycling. Must be one of the following values:
            // 0: (default if there's only one notification) No tiles show notifications.
            // 1: (default if there are multiple notifications) Notifications cycle for all tile sizes.
            // 2: Notifications cycle for medium tiles only.
            // 3: Notifications cycle for wide tiles only.
            // 4: Notifications cycle for large tiles only.
            // 5: Notifications cycle for medium and wide tiles.
            // 6: Notifications cycle for medium and large tiles.
            // 7: Notifications cycle for large and wide tiles.
            const int cycle = 1;

            var document = new XDocument(
                new XElement("browserconfig",
                    new XElement("msapplication",
                        new XElement("tile",
                            new XElement("square70x70logo",
                                new XAttribute("src", square70X70LogoUrl)),
                            new XElement("square150x150logo",
                                new XAttribute("src", square150X150LogoUrl)),
                            new XElement("square310x310logo",
                                new XAttribute("src", square310X310LogoUrl)),
                            new XElement("wide310x150logo",
                                new XAttribute("src", wide310X150LogoUrl)),
                            new XElement("TileColor", tileColor)),
                        new XElement("notification",
                            new XElement("polling-uri", 
                                new XAttribute("src", GetTileUrl(1))),
                            new XElement("polling-uri2",
                                new XAttribute("src", GetTileUrl(2))),
                            new XElement("polling-uri3",
                                new XAttribute("src", GetTileUrl(3))),
                            new XElement("polling-uri4",
                                new XAttribute("src", GetTileUrl(4))),
                            new XElement("polling-uri5",
                                new XAttribute("src", GetTileUrl(5))),
                            new XElement("frequency", frequency),
                            new XElement("cycle", cycle)))));

            return document.ToString(Encoding.UTF8);
        }

        /// <summary>
        /// Gets the URL to the tile XML for the specified item in the Atom feed for the site.
        /// </summary>
        /// <param name="feedEntry">The number of the Atom feed entry.</param>
        /// <returns>A URL to the tile XML.</returns>
        // We are using the service provided by http://buildmypinnedsite.com which queries our Atom feed (Which we
        // pass to it, along with the feed entry number) and returns tile XML using the Atom feed entry.
        // An alternative is to generate your own custom tile XML and return a URL to it here. You can take a look
        // at the tile template catalog (https://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx) to
        // see the types of tiles you can generate and use the NotificationsExtensions.Portable NuGet package 
        // (https://github.com/RehanSaeed/NotificationsExtensions.Portable) to generate the tile XML.
        private string GetTileUrl(int feedEntry) 
            => string.Format("http://notifications.buildmypinnedsite.com/?feed={0}&amp;id={1}",
            this._urlHelper.AbsoluteRouteUrl(HomeControllerRoute.GetFeed),
            feedEntry);
    }
}
