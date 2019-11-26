using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class GenreUpdateRequest: GenreBaseDto
    {
        [Required(ErrorMessage = "Genre description must be provided while updating.")]
        public override string Description
        {
            get=>base.Description; 
            set=>base.Description=value;
        }
    }
}
