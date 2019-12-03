using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests.Put
{
    public class ArtistPutRequest: ArtistBaseRequest
    {
        [Required(ErrorMessage = "Biography must be provided while updating.")]
        public override string Biography
        {
            get=>base.Biography;
            set=>base.Biography=value;
        }

        [Required(ErrorMessage = "Year active must be provided while updating.")]
        public override string YearActive { get=>base.YearActive; set=>base.YearActive=value; }

        [Required(ErrorMessage = "Thumbnail tag must be provided while updating.")]
        public override string ThumbnailTag { get=> base.ThumbnailTag; set=>base.ThumbnailTag=value; }

        [Required(ErrorMessage = "Small thumbnail must be provided while updating.")]
        public override string SmallThumbnail { get=>base.SmallThumbnail; set=>base.SmallThumbnail=value; }

        [Required(ErrorMessage = "Large thumbnail must be provided while updating.")]
        public override string LargeThumbnail { get=>base.LargeThumbnail; set=>base.LargeThumbnail=value; }
    }
}
