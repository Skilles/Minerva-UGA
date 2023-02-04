using Minerva.Features.CoursePlanner.Assemblers;
using Minerva.Features.CoursePlanner.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Infrastructure.Database;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Minerva.Features.CoursePlanner.Services;

public class PlannerService
{
    private readonly IRepository<PlannerDocument> PlannerRepository;
    
    private readonly PlannerDataAssembler PlannerDataAssembler;
    
    public PlannerService(IRepository<PlannerDocument> plannerRepository, PlannerDataAssembler plannerDataAssembler)
    {
        PlannerRepository = plannerRepository;
        PlannerDataAssembler = plannerDataAssembler;
    }
    
    public async Task<ObjectId> CreatePlannerAsync(string userId, string termId)
    {
        var plannerDocument = new PlannerDocument
        {
            UserId = ObjectId.Parse(userId),
            TermId = ObjectId.Parse(termId),
            CourseIds = new(),
            SectionIds = new()
        };
        
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.UserId, plannerDocument.UserId) & Builders<PlannerDocument>.Filter.Eq(p => p.TermId, plannerDocument.TermId);

        plannerDocument = await PlannerRepository.UpsertAndReturnAsync(plannerDocument, filter);
        
        return plannerDocument.Id;
    }
    
    public async Task<PlannerDataRecord> GetPlannerDataAsync(string plannerId)
    {
        var cursor = await PlannerRepository.Collection.FindAsync(planner => planner.Id == ObjectId.Parse(plannerId));

        if (!await cursor.AnyAsync())
        {
            throw new ArgumentException("Planner not found");
        }
        
        var plannerDocument = await cursor.FirstAsync();
        
        return await PlannerDataAssembler.ToDataRecordAsync(plannerDocument);
    }

    public async Task AddCourseToPlannerAsync(string plannerId, string courseId)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, ObjectId.Parse(plannerId));
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.CourseIds, ObjectId.Parse(courseId));
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update);
        
        if (result.MatchedCount == 0)
        {
            throw new ArgumentException("Planner not found");
        }
    }
    
    public async Task AddSectionToPlannerAsync(string plannerId, string sectionId)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, ObjectId.Parse(plannerId));
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.SectionIds, ObjectId.Parse(sectionId));
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update);
        
        if (result.MatchedCount == 0)
        {
            throw new ArgumentException("Planner not found");
        }
    }
}