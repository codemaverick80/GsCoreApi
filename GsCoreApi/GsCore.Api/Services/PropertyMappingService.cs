using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Database.Entities;

namespace GsCore.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {

        private Dictionary<string, PropertyMappingValue> _artistPropertyMappring =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            { 
                { "Name", new PropertyMappingValue(new List<string>() { "FirstName","LastName"},false) },
                { "UpdatedDate", new PropertyMappingValue(new List<string>() { "UpdatedDate"},true) }

            };

        private IList<IPropertyMapping> _propertyMappings=new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<ArtistGetResponse, Artist>(_artistPropertyMappring));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            //get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance "+ 
                                $"for <{typeof(TSource)},{typeof(TDestination)}");



        }

    }
}
