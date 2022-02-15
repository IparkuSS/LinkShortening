using LinkShortening.Settings.Contracts;

namespace LinkShortening.Settings
{
    public class LinkShorteningDatabaseSettings : ILinkShorteningDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string LinksCollectionName { get; set; }
    }
}
