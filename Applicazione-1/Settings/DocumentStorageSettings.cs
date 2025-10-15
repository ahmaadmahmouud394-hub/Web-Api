namespace Applicazione_1.Settings
{
    public record DocumentStorageSettings
    {
        public StorageType StorageType { get; set; }
        public IEnumerable<string> AllowedExtensions { get; set; }
        public int MaxContentLengthInBytes { get; set; }
    }

    public enum StorageType
    {
        File,
        AzureBlobStorage,
        Database
    }
}
