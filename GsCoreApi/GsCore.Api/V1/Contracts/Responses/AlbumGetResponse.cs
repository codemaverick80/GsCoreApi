namespace GsCore.Api.V1.Contracts.Responses
{
    public class AlbumGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ArtistId { get; set; }
        public int? Rating { get; set; }
        public int? Year { get; set; }
        public string Label { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string MediumThumbnail { get; set; }
        public string LargeThumbnail { get; set; }
        public string AlbumUrl { get; set; }
    }
}
