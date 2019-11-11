namespace GsCore.Database.Entities
{
    public partial class Track
    {
        public int Id { get; set; }
        public string TrackName { get; set; }
        public int? AlbumId { get; set; }
        public string Composer { get; set; }
        public string Performer { get; set; }
        public string Featuring { get; set; }
        public string Duration { get; set; }

        public virtual Album Album { get; set; }
    }
}
