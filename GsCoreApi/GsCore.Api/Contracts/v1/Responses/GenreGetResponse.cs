using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.Contracts.v1.Responses
{
    public class GenreGetResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
