using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.ValidationAttributes;

namespace GsCore.Api.V1.Contracts.Requests
{
    [GenreNameMustBeDifferentFromDescription]
    public abstract class GenreBaseRequest
    {
        [Required(ErrorMessage = "Genre name must not be empty.")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
