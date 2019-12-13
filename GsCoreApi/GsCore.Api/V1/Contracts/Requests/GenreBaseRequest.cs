using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.ValidationAttributes;

namespace GsCore.Api.V1.Contracts.Requests
{

    #region "IValidatableObject - Class-level input validation"


    //public class GenreBaseRequest : IValidatableObject
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
    //                new[] { "GenreBaseRequest" }
    //            );
    //        }
    //    }
    //}

    #endregion

    #region "Custom Attribute - Class-level input validation"

    //[GenreNameMustBeDifferentFromDescription]
    //public class GenreBaseRequest 
    //{
    //    [Required(ErrorMessage = "Genre name must not be empty.")]
    //    [MaxLength(50)]
    //    public string Name { get; set; }

    //    public string Description { get; set; }
    //}

    #endregion


    [GenreNameMustBeDifferentFromDescription]
    public abstract class GenreBaseRequest
    {
        [Required(ErrorMessage = "Genre name must not be empty.")]
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual string Description { get; set; }
    }






}
