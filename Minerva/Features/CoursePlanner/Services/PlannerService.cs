using Minerva.Features.Athena.Documents;
using Minerva.Features.Authentication.Documents;
using Minerva.Features.CoursePlanner.Assemblers;
using Minerva.Features.CoursePlanner.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Infrastructure.Database;
using Minerva.Infrastructure.Database.Repositories;
using Minerva.Utility;
using Minerva.Validation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Minerva.Features.CoursePlanner.Services;

public class PlannerService
{
    private readonly IRepository<PlannerDocument> PlannerRepository;
    
    private readonly IRepository<UserDocument> UserRepository;
    
    private readonly IRepository<TermDocument> TermRepository;

    private readonly PlannerDataAssembler PlannerDataAssembler;
    
    public PlannerService(IRepository<PlannerDocument> plannerRepository, IRepository<UserDocument> userRepository, IRepository<TermDocument> termRepository, PlannerDataAssembler plannerDataAssembler)
    {
        PlannerRepository = plannerRepository;
        UserRepository = userRepository;
        TermRepository = termRepository;
        PlannerDataAssembler = plannerDataAssembler;
    }
    
    public async Task<ObjectId> CreatePlannerAsync(ObjectId userId, ObjectId termId, CancellationToken ct)
    {
        var result = await UserRepository.Client.DoTransactionAsync(async (_, _) =>
        {
            var userDocument = await UserRepository.FindOneAsync(user => user.Id == userId, cancellationToken: ct);
        
            if (userDocument == null)
            {
                throw new MinervaValidationException("User not found");
            }
        
            var termDocument = await TermRepository.FindOneAsync(term => term.Id == termId, cancellationToken: ct);
            
            if (termDocument == null)
            {
                throw new MinervaValidationException("Term not found");
            }

            var plannerDocument = new PlannerDocument
            {
                UserId = userId,
                TermId = termId,
                CourseIds = new(),
                SectionIds = new()
            };

            var filter = Builders<PlannerDocument>.Filter.Eq(p => p.UserId, plannerDocument.UserId) & Builders<PlannerDocument>.Filter.Eq(p => p.TermId, plannerDocument.TermId);

            plannerDocument = await PlannerRepository.UpsertAndReturnAsync(plannerDocument, filter, ct);

            userDocument.Data.Planners[termDocument.TermId] = plannerDocument.Id;
            
            await UserRepository.UpsertAsync(userDocument, u => u.Id == userId, cancellationToken: ct);

            return plannerDocument.Id;
        }, cancellationToken: ct);

        return result;
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
            throw new MinervaValidationException("Planner not found");
        }
        
        var plannerDocument = await PlannerRepository.FindOneAsync(planner => planner.Id == plannerId, cancellationToken: ct);
        
        return await PlannerDataAssembler.GetSectionsAsync(plannerDocument);
    }

    public async Task AddCourseToPlannerAsync(ObjectId plannerId, string courseId, CancellationToken ct)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, plannerId);
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.CourseIds, courseId);
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
        {
            throw new MinervaValidationException("Planner not found");
        }
    }
    
    public async Task AddSectionToPlannerAsync(ObjectId plannerId, int courseReferenceNumber, CancellationToken ct)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, plannerId);
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.SectionIds, courseReferenceNumber);
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
        {
            throw new MinervaValidationException("Planner not found");
        }
    }
}