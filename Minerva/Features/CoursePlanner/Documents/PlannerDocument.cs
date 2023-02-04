﻿using Minerva.Infrastructure.Database;
using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Documents;

public class PlannerDocument : MongoDocument
{
    public string Name { get; set; }
    
    public ObjectId UserId { get; set; }
    
    public ObjectId TermId { get; set; }
    
    public List<ObjectId> CourseIds { get; set; }
    
    public List<ObjectId> SectionIds { get; set; }
}