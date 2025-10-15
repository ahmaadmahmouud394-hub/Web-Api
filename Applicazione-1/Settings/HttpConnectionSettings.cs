namespace Applicazione_1.Settings
{
    public record HttpConnectionSettings
    {
        public string BaseUrl { get; set; }
        public int PortNumber { get; set; }
        public string FirstEndPoint { get; set; }
        public string SecondEndPoint { get; set; }
    }
}
