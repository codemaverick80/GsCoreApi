using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Helpers
{
    /*
     * Author: Harish Chand
     * Purpose: Data Shaping allows the consumer of the API to choose the resource fields
     * (i.e. https://host/api/v1/artists?fields=id,name)
     */
    public static class IEnumerableExtension
    {
        /// <summary>
        /// ShapeData Extension method on IEnumerable of type TSource (Dtos or entity)
        /// </summary>
        /// <typeparam name="TSource"> IEnumerable of type Dto or entity. </typeparam>
        /// <param name="source">IEnumerable of type Dto or entity. </param>
        /// <param name="fields">Name of fields that API client has requested through query parameter. </param>
        /// <returns>IEnumerable of type ExpandoObject. </returns>
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // create a list to hold our ExpandoObjects
            var expandoObjectList=new List<ExpandoObject>();

            // create a list with propertyInfo objects on TSource. Reflection is expensive,
            // so rather doing it for each object in the list, we do it once and reuse the results.
            // after all, part of the reflection is on the type of the object (TSource), not on the instance
            
            var propertyInfoList = new List<PropertyInfo>();

            //check if fields has been pass through query string by client
            if (string.IsNullOrWhiteSpace(fields))
            {
                // all the properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfos);
            }
            else 
            {
                // the fields are separated by "," so we split it.
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    // trim each field, as it might contain leading or trailing spaces. can't trim the vat in foreach
                    // so use another var
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object.
                    // we need to include public and instance, b/c specifying a binding
                    // flag overwrites the already-existing binding flags.
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} was not found on {typeof(TSource)}.");
                    }

                    //add propertyInfo to list
                    propertyInfoList.Add(propertyInfo);
                }
            }

            // run through the source objects (i.e IEnumerable<TSource> => TSource=ArtistGetResponse)
            foreach (TSource sourceObject in source)
            {
                // create an expandoObject that will hold the selected properties & values
                var dataShapedObject=new ExpandoObject();

                // Get the value of each property we have to return. For that we run through the list
                foreach (var propertyInfo in propertyInfoList)
                {
                    // GetValue returns the value of the property on the object
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                // add the ExpandoOnject to the list
                expandoObjectList.Add(dataShapedObject);
                
            }

            return expandoObjectList;

        }
    }

}

////    [HttpGet(Name = "GetArtists")]
////    public async Task<ActionResult<IEnumerable<ArtistGetResponse>>> GetArtists([FromQuery] ArtistResourceParameters artistResourceParameters)
////    {
////      .....
////      return Ok(_mapper.Map<ArtistGetResponse[]>(artistFromRepo));
////    }


////    [HttpGet(Name = "GetArtists")]
////    public async Task<ActionResult> GetArtists([FromQuery] ArtistResourceParameters artistResourceParameters)
////    {
////      .....
////      return Ok(_mapper.Map<IEnumerable<ArtistGetResponse>>(artistFromRepo).ShapedData(artistResourceParameters.Fields));
////    }

