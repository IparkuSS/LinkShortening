namespace LinkShortening.Settings.Contracts
{
    public interface ILinkShorteningDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string LinksCollectionNameUsers { get; set; }
        public string LinksCollectionNameUrl { get; set; }
    }
}
