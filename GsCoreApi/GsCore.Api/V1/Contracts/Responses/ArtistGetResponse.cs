using GsCore.Database.Entities;

namespace GsCore.Api.V1.Contracts.Responses
{
    public class ArtistGetResponse
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string YearActive { get; set; }
        public string Biography { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string LargeThumbnail { get; set; }

        public string Born { get; set; }
        public string Died { get; set; }
        public string AlsoKnownAs { get; set; }

       

    }
}
