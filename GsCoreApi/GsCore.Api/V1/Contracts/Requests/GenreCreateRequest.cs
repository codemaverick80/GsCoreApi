using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GsCore.Api.V1.ValidationAttributes;

namespace GsCore.Api.V1.Contracts.Requests
{
 
    [GenreNameMustBeDifferentFromDescription]
    public class GenreCreateRequest //: IValidatableObject
    {

        /// <summary>
        /// Genre Name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Genre Description
        /// </summary>
        public string Description { get; set; }


        /* Custom rule "Name and Description should not be same" */
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Name == Description)
        //    {
        //        yield return new ValidationResult(
        //            "Genre description should be different from the name",
        //            new[] {"GenreCreateRequest"}
        //            );
        //    }
        //}

    }
}
