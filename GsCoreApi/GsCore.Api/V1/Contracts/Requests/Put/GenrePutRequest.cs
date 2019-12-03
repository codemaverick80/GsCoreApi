using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests.Put
{
    public class GenrePutRequest: GenreBaseRequest
    {
        [Required(ErrorMessage = "Genre description must be provided while updating.")]
        public override string Description
        {
            get=>base.Description; 
            set=>base.Description=value;
        }
    }
}
