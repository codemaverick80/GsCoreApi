using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests.Put
{
    public class AlbumPutRequest:AlbumBaseRequest
    {
        [Required(ErrorMessage = "Album rating must not be empty while updating.")]
        public override int? Rating
        {
            get=>base.Rating; 
            set=>base.Rating=value;
        }

        [Required(ErrorMessage = "Album release year is empty.")]
        public override int? Year { get=>base.Year; set=>base.Year=value; }

        [Required(ErrorMessage = "Album label is empty.")]
        public override string Label { get=>base.Label; set=>base.Label=value; }

        [Required(ErrorMessage = "Thumbnail tag must be provided while updating.")]
        public override string ThumbnailTag { get=>base.ThumbnailTag; set=>base.ThumbnailTag=value; }

        [Required(ErrorMessage = "Album small thumbnail must be provided while updating.")]
        public override string SmallThumbnail { get=>base.SmallThumbnail; set=>base.SmallThumbnail=value; }

        [Required(ErrorMessage = "Album medium thumbnail must be provided while updating.")]
        public override string MediumThumbnail { get=>base.MediumThumbnail; set=>base.MediumThumbnail=value; }

        [Required(ErrorMessage = "Album large thumbnail must be provided while updating.")]
        public override string LargeThumbnail { get=>base.LargeThumbnail; set=>base.LargeThumbnail=value; }

        
       //public virtual string AlbumUrl { get; set; }

    }
}
