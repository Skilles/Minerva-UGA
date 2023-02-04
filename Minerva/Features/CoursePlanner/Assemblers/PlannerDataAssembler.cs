using Minerva.Features.Athena.Documents;
using Minerva.Features.Athena.Services;
using Minerva.Features.CoursePlanner.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Infrastructure.Database;
using Minerva.Utility;
using MongoDB.Driver;

namespace Minerva.Features.CoursePlanner.Assemblers;

public class PlannerDataAssembler
{
    private readonly IRepository<CourseDocument> CourseRepository;

    private readonly IRepository<SectionDocument> SectionRepository;
    
    private readonly IRepository<SubjectDocument> SubjectRepository;

    private readonly CourseoffCapacityService CapacityService;
    
    public PlannerDataAssembler(CourseoffCapacityService capacityService, IRepository<SubjectDocument> subjectRepository, IRepository<SectionDocument> sectionRepository, IRepository<CourseDocument> courseRepository)
    {
        SubjectRepository = subjectRepository;
        SectionRepository = sectionRepository;
        CourseRepository = courseRepository;
        CapacityService = capacityService;
    }

    public async Task<PlannerDataRecord> ToPlannerDataAsync(PlannerDocument plannerDocument, CancellationToken ct)
    {
        var courseDataRecords = await ToCoursesDataAsync(plannerDocument, ct);
        return new(plannerDocument.Name, courseDataRecords);
    }
    
    public async Task<IEnumerable<SectionDocument>> ToSectionsAsync(PlannerDocument plannerDocument)
    {
        var filter = Builders<SectionDocument>.Filter.In(s => s.CourseReferenceNumber, plannerDocument.SectionIds);
        var sections = await SectionRepository.Collection.FindAsync(filter);
        return await sections.ToListAsync();
    }

    private async Task<IEnumerable<CourseDocument>> ToCoursesAsync(PlannerDocument plannerDocument)
    {
        var filter = Builders<CourseDocument>.Filter.In(c => c.CourseId, plannerDocument.CourseIds);
        var courses = await CourseRepository.Collection.FindAsync(filter);
        return await courses.ToListAsync();
    }

    private async Task<IEnumerable<CourseDataRecord>> ToCoursesDataAsync(PlannerDocument plannerDocument, CancellationToken ct)
    {
        var courseDataRecords = new List<CourseDataRecord>();
        var courseDocuments = await ToCoursesAsync(plannerDocument);
        
        foreach (var courseDocument in courseDocuments)
        {
            var subjectDocument = await GetSubjectAsync(ct, courseDocument);
            
            var sectionDocuments = await GetSectionsAsync(ct, courseDocument);
            
            var courseDataRecord = new CourseDataRecord($"{subjectDocument.SubjectId} {courseDocument.CourseId} - {courseDocument.Name}", sectionDocuments);
            
            courseDataRecords.Add(courseDataRecord);
        }
        
        return courseDataRecords;
    }

    private async Task<List<SectionDocument>> GetSectionsAsync(CancellationToken ct, CourseDocument courseDocument)
    {
        var sectionFilter = Builders<SectionDocument>.Filter.In(s => s.CourseReferenceNumber, courseDocument.SectionIds);
        var sectionsCursor = await SectionRepository.Collection.FindAsync(sectionFilter, cancellationToken: ct);
        var sectionDocuments = await sectionsCursor.ToListAsync(ct);
        return sectionDocuments;
    }

    private async Task<SubjectDocument> GetSubjectAsync(CancellationToken ct, CourseDocument courseDocument)
    {
        var subjectFilter = Builders<SubjectDocument>.Filter.ElemMatch(s => s.CourseIds, courseDocument.CourseId);
        var subjectProjection = Builders<SubjectDocument>.Projection.Include(s => s.SubjectId);
        var subjectDocument = await SubjectRepository.FindOneAsync(subjectFilter, subjectProjection, cancellationToken: ct);
        return subjectDocument;
    }
}