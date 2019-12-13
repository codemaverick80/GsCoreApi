using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Requests;

namespace GsCore.Api.V1.ValidationAttributes
{
    public class ArtistMustHaveUniqueFirstNameAndLastNameAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, //object is the object to validate (i.e. ArtistCreateRequest dto)
            ValidationContext validationContext)
        {
            var artist = (ArtistBaseRequest)validationContext.ObjectInstance;

            if (artist.FirstName == artist.LastName)
            {
                return new ValidationResult("Artist First Name and Last Name must be different",new[] { nameof(ArtistBaseRequest) });
            }

            return ValidationResult.Success;
        }
    }
}
