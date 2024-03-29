﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Minerva.Swagger;

public class AuthenticationRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var isAuthorized = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
        var allowAnonymous = actionMetadata.Any(metadataItem => metadataItem is AllowAnonymousAttribute);

        if (!isAuthorized || allowAnonymous)
        {
            return;
        }
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Security = new List<OpenApiSecurityRequirement>();

        //Add JWT bearer type
        operation.Security.Add(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            // Definition name. 
                            // Should exactly match the one given in the service configuration
                            Id = "Bearer"
                        }
                    }, Array.Empty<string>()
                }
            }
        );

        operation.Responses.Add("401", new() { Description = "Unauthorized" });
        operation.Responses.Add("403", new() { Description = "Forbidden" });
    }
}