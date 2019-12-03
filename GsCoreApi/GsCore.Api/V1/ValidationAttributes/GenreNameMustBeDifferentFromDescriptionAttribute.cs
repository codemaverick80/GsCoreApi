using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GsCore.Api.V1.Contracts.Requests;

namespace GsCore.Api.V1.ValidationAttributes
{
    public class GenreNameMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, //object is the object to validate (i.e. GenreCreateRequest dto)
            ValidationContext validationContext)
        {

            var genre = (GenreBaseRequest)validationContext.ObjectInstance;
            
            if (genre.Name == genre.Description)
            {
                return new ValidationResult(
                    "Genre description should be different from the name",
                    new[] {nameof(GenreBaseRequest) }
               );
            }

            return ValidationResult.Success;
        }
    }


    #region "Custom Attribute - Class-level input validation"

    
    //public class GenreNameMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, //object is the object to validate (i.e. GenreCreateRequest dto)
    //        ValidationContext validationContext)
    //    {

    //        var genre = (GenreCreateRequest)validationContext.ObjectInstance;

    //        if (genre.Name == genre.Description)
    //        {
    //            return new ValidationResult(
    //                "Genre description should be different from the name",
    //                new[] { nameof(GenreCreateRequest) }
    //            );
    //        }

    //        return ValidationResult.Success;
    //    }
    //}

    #endregion
}
