﻿using System.ComponentModel;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Minerva.Swagger;

public class ObjectIdOperationFilter : IOperationFilter
    {
        private readonly IEnumerable<string> objectIdIgnoreParameters = new[]
        {
            "Timestamp",
            "Machine",
            "Pid",
            "Increment",
            "CreationTime"
        };
        private readonly IEnumerable<XPathNavigator> xmlNavigators;
        public ObjectIdOperationFilter(IEnumerable<string> filePaths)
        {
            xmlNavigators = filePaths != null
                ? filePaths.Select(x => new XPathDocument(x).CreateNavigator())
                : Array.Empty<XPathNavigator>();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var p in operation.Parameters.ToList())
                if (objectIdIgnoreParameters.Any(x => p.Name.EndsWith(x)))
                {
                    var parameterIndex = operation.Parameters.IndexOf(p);
                    operation.Parameters.Remove(p);
                    var dotIndex = p.Name.LastIndexOf(".");
                    if (dotIndex > -1)
                    {
                        var idName = p.Name.Substring(0, dotIndex);
                        if (!operation.Parameters.Any(x => x.Name == idName))
                        {
                            operation.Parameters.Insert(parameterIndex, new()
                            {
                                Name = idName,
                                Schema = new()
                                {
                                    Type = "string",
                                    Format = "24-digit hex string"
                                },
                                Description = GetFieldDescription(idName, context),
                                Example = new OpenApiString(ObjectId.Empty.ToString()),
                                In = p.In,
                            });
                        }
                    }
                }
        }

        private string GetFieldDescription(string idName, OperationFilterContext context)
        {
            var name = char.ToUpperInvariant(idName[0]) + idName.Substring(1);
            var classProp = context.MethodInfo.GetParameters().FirstOrDefault()?.ParameterType?.GetProperties().FirstOrDefault(x => x.Name == name);
            var typeAttr = classProp != null
                ? (DescriptionAttribute)classProp.GetCustomAttribute<DescriptionAttribute>()
                : null;
            if (typeAttr != null)
                return typeAttr?.Description;

            if (classProp != null)
                foreach (var xmlNavigator in xmlNavigators)
                {
                    var propertyMemberName = XmlCommentsNodeNameHelper.GetMemberNameForFieldOrProperty(classProp);
                    var propertySummaryNode = xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{propertyMemberName}']/summary");
                    if (propertySummaryNode != null)
                        return XmlCommentsTextHelper.Humanize(propertySummaryNode.InnerXml);
                }

            return null;
        }
    }