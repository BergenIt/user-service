namespace DatabaseExtension.Configure
{
    public abstract class PageConfigurator
    {
        internal DatabaseExtensionConfig _databaseExtensionConfig;
        protected IDatabaseExtensionConfig Config => _databaseExtensionConfig;
        protected PageConfigurator()
        {
            _databaseExtensionConfig = new DatabaseExtensionConfig();
        }
    }
}
