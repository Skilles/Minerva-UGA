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
        var sections = await GetSectionsAsync(plannerDocument);
        var sectionsDictionary = sections.ToDictionary(s => s.CourseReferenceNumber);
        return new(plannerDocument.Name, courseDataRecords, sectionsDictionary);
    }

    private async Task<IDictionary<string, CourseDataRecord>> ToCoursesDataAsync(PlannerDocument plannerDocument, CancellationToken ct)
    {
        var courseDataRecords = await CourseRepository.Client.DoTransactionAsync(async (_, _) =>
        {
            var courseDataRecords = new Dictionary<string, CourseDataRecord>();
            var courseDocuments = await GetCoursesAsync(plannerDocument);
            
            foreach (var courseDocument in courseDocuments)
            {
                var subjectDocument = await GetSubjectAsync(courseDocument, ct);

                var courseDataRecord = new CourseDataRecord($"{subjectDocument.SubjectId} {courseDocument.CourseId} - {courseDocument.Name}", courseDocument.SectionIds);
            
                courseDataRecords.Add(courseDocument.CourseId, courseDataRecord);
            }

            return courseDataRecords;
        }, cancellationToken: ct);

        return courseDataRecords;
    }
    
    public async Task<IEnumerable<SectionDocument>> GetSectionsAsync(PlannerDocument plannerDocument)
    {
        var filter = Builders<SectionDocument>.Filter.In(s => s.CourseReferenceNumber, plannerDocument.SectionIds);
        var sections = await SectionRepository.Collection.FindAsync(filter);
        return await sections.ToListAsync();
    }

    private async Task<IEnumerable<SectionDocument>> GetSectionsAsync(CourseDocument courseDocument, CancellationToken ct)
    {
        var sectionFilter = Builders<SectionDocument>.Filter.In(s => s.CourseReferenceNumber, courseDocument.SectionIds);
        var sectionsCursor = await SectionRepository.Collection.FindAsync(sectionFilter, cancellationToken: ct);
        return await sectionsCursor.ToListAsync(ct);
    }
    
    private async Task<IEnumerable<CourseDocument>> GetCoursesAsync(PlannerDocument plannerDocument)
    {
        var filter = Builders<CourseDocument>.Filter.In(c => c.CourseId, plannerDocument.CourseIds);
        var courses = await CourseRepository.Collection.FindAsync(filter);
        return await courses.ToListAsync();
    }

    private async Task<SubjectDocument> GetSubjectAsync(CourseDocument courseDocument, CancellationToken ct)
    {
        var subjectFilter = Builders<SubjectDocument>.Filter.ElemMatch(s => s.CourseIds, courseDocument.CourseId);
        var subjectProjection = Builders<SubjectDocument>.Projection.Include(s => s.SubjectId);
        return await SubjectRepository.FindOneAsync(subjectFilter, subjectProjection, cancellationToken: ct);
    }
}