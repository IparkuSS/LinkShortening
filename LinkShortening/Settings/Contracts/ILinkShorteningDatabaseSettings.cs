namespace LinkShortening.Settings.Contracts
{
    public interface ILinkShorteningDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string LinksCollectionName { get; set; }
    }
}
