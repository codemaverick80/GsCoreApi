using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GsCore.Api.V1.ValidationAttributes;

namespace GsCore.Api.V1.Contracts.Requests
{


    public class GenreCreateRequest : GenreBaseDto 
    {

       

    }


    #region "Custom Attribute - Class-level input validation"

    //[GenreNameMustBeDifferentFromDescription]
    //public class GenreCreateRequest 
    //{
    //    [Required(ErrorMessage = "Genre name must not be empty.")]
    //    [MaxLength(50)]
    //    public string Name { get; set; }

    //    public string Description { get; set; }
    //}

    #endregion



    #region "IValidatableObject - Class-level input validation"


    //public class GenreCreateRequest : IValidatableObject
    //{
    //    [Required(ErrorMessage = "Genre name must not be empty.")]
    //    [MaxLength(50)]
    //    public string Name { get; set; }

    //    public string Description { get; set; }


    //    /* Custom rule "Name and Description should not be same" */
    //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //    {
    //        if (Name == Description)
    //        {
    //            yield return new ValidationResult(
    //                "Genre description should be different from the name",
    //                new[] { "GenreCreateRequest" }
    //            );
    //        }
    //    }
    //}

    #endregion

}
