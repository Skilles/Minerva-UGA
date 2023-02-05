using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Minerva.Swagger;

public static class SwaggerExtensions
{
    public static void Configure(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new()
        {
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.OperationFilter<AuthenticationRequirementsOperationFilter>();

        //This action return true/false after selected SwaggerDoc section for add/decline Request
        options.DocInclusionPredicate((docName, apiDescription) => docName == apiDescription.GroupName);

        var swaggerFiles = new string[] { "SwaggerAPI.xml", "SwaggerApplicationAPI.xml" }
                           .Select(fileName => Path.Combine(AppContext.BaseDirectory, fileName))
                           .Where(File.Exists);
        foreach (var filePath in swaggerFiles)
            options.IncludeXmlComments(filePath);


        options.OperationFilter<ObjectIdOperationFilter>(swaggerFiles);
        options.SchemaFilter<ObjectIdSchemaFilter>();
    }
}