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

    private readonly ILogger<PlannerService> Logger;

    public PlannerService(ILogger<PlannerService> logger, IRepository<PlannerDocument> plannerRepository, IRepository<UserDocument> userRepository, IRepository<TermDocument> termRepository, PlannerDataAssembler plannerDataAssembler)
    {
        Logger = logger;
        PlannerRepository = plannerRepository;
        UserRepository = userRepository;
        TermRepository = termRepository;
        PlannerDataAssembler = plannerDataAssembler;
    }
    
    public async Task<string> CreatePlannerAsync(string userEmail, int termId, CancellationToken ct)
    {
        
        var userDocument = await UserRepository.FindOneAsync(user => user.Email.Address == userEmail, cancellationToken: ct);
    
        if (userDocument == null)
        {
            throw new MinervaValidationException("User not found");
        }
    
        var termDocument = await TermRepository.FindOneAsync(term => term.TermId == termId, cancellationToken: ct);
        
        if (termDocument == null)
        {
            throw new MinervaValidationException("Term not found");
        }

        var plannerDocument = new PlannerDocument
        {
            UserId = userEmail,
            TermId = termId,
            Name = "My Planner",
            CourseIds = new(),
            SectionIds = new()
        };

        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.UserId, plannerDocument.UserId) & Builders<PlannerDocument>.Filter.Eq(p => p.TermId, plannerDocument.TermId);

        plannerDocument = await PlannerRepository.UpsertAndReturnAsync(plannerDocument, filter, ct);
        
        userDocument.Data.Planners[termDocument.TermId.ToString()] = plannerDocument.Id;
        
        var userEmailFilter = Builders<UserDocument>.Filter.Eq(u => u.Email.Address, userEmail);
        await UserRepository.UpsertAsync(userDocument, userEmailFilter, cancellationToken: ct);

        return plannerDocument.Id;
        
    }
    
    public async Task<PlannerDataRecord> GetPlannerDataAsync(string plannerId, CancellationToken ct)
    {
        var plannerDocument = await PlannerRepository.FindOneAsync(planner => planner.Id == plannerId, cancellationToken: ct);
        
        if (plannerDocument == null)
        {
            throw new MinervaValidationException("Planner not found");
        }
        
        return await PlannerDataAssembler.ToPlannerDataAsync(plannerDocument, ct);
    }
    
    public async Task<IEnumerable<SectionDocument>> GetPlannerSectionsAsync(string plannerId, CancellationToken ct)
    {
        var cursor = await PlannerRepository.Collection.FindAsync(planner => planner.Id == plannerId, cancellationToken: ct);

        if (!await cursor.AnyAsync(cancellationToken: ct))
        {
            throw new MinervaValidationException("Planner not found");
        }
        
        var plannerDocument = await PlannerRepository.FindOneAsync(planner => planner.Id == plannerId, cancellationToken: ct);
        
        return await PlannerDataAssembler.GetSectionsAsync(plannerDocument);
    }

    public async Task AddCourseToPlannerAsync(string plannerId, string courseId, CancellationToken ct)
    {
        var filter = Builders<PlannerDocument>.Filter.Eq(p => p.Id, plannerId);
        
        var update = Builders<PlannerDocument>.Update.AddToSet(p => p.CourseIds, courseId);
        
        var result = await PlannerRepository.Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
        {
            throw new MinervaValidationException("Planner not found");
        }
    }
    
    public async Task AddSectionToPlannerAsync(string plannerId, int courseReferenceNumber, CancellationToken ct)
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