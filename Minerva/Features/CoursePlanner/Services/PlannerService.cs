using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Assemblers;
using Minerva.Features.CoursePlanner.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Infrastructure.Database;
using Minerva.Utility;
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
    
    public async Task<ObjectId> CreatePlannerAsync(ObjectId userId, ObjectId termId, CancellationToken ct)
    {
        var plannerDocument = new PlannerDocument
        {
            UserId = userId,
            TermId = termId,
            CourseIds = new(),
            SectionIds = new()
        };
        
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.UserId, plannerDocument.UserId) & Builders<PlannerDocument>.Filter.Eq(p => p.TermId, plannerDocument.TermId);

        plannerDocument = await PlannerRepository.UpsertAndReturnAsync(plannerDocument, filter, ct);
        
        return plannerDocument.Id;
    }
    
    public async Task<PlannerDataRecord> GetPlannerDataAsync(ObjectId plannerId, CancellationToken ct)
    {
        var plannerDocument = await PlannerRepository.FindOneAsync(planner => planner.Id == plannerId, cancellationToken: ct);
        
        return await PlannerDataAssembler.ToPlannerDataAsync(plannerDocument, ct);
    }
    
    public async Task<IEnumerable<SectionDocument>> GetPlannerSectionsAsync(ObjectId plannerId, CancellationToken ct)
    {
        var cursor = await PlannerRepository.Collection.FindAsync(planner => planner.Id == plannerId, cancellationToken: ct);

        if (!await cursor.AnyAsync(cancellationToken: ct))
        {
            throw new ArgumentException("Planner not found");
        }
        
        var plannerDocument = await PlannerRepository.FindOneAsync(planner => planner.Id == plannerId, cancellationToken: ct);
        
        return await PlannerDataAssembler.ToSectionsAsync(plannerDocument);
    }

    public async Task AddCourseToPlannerAsync(ObjectId plannerId, string courseId, CancellationToken ct)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, plannerId);
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.CourseIds, courseId);
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
        {
            throw new ArgumentException("Planner not found");
        }
    }
    
    public async Task AddSectionToPlannerAsync(ObjectId plannerId, int courseReferenceNumber, CancellationToken ct)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, plannerId);
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.SectionIds, courseReferenceNumber);
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
        {
            throw new ArgumentException("Planner not found");
        }
    }
}