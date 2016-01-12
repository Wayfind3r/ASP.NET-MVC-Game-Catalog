namespace PotatoCatalog.Models
{
    public class Image
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}