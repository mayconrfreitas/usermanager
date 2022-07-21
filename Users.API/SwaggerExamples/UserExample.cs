using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;

namespace Users.API.SwaggerExamples
{
    public class UserExample : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(User))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Name"] = new OpenApiString("Albert Einstein"),
                    ["BirthDate"] = new OpenApiString("1879-03-14"),
                };
            }
        }
    }
}
