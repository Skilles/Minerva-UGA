using Minerva.Features.Athena.Documents;
using Minerva.Features.Athena.Services;
using Minerva.Features.CoursePlanner.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Infrastructure.Database;
using MongoDB.Driver;

namespace Minerva.Features.CoursePlanner.Assemblers;

public class PlannerDataAssembler
{
    private readonly IRepository<CourseDocument> CourseRepository;

    private readonly IRepository<SectionDocument> SectionRepository;
    
    private readonly CourseoffCapacityService CapacityService;
    
    public PlannerDataAssembler(CourseoffCapacityService capacityService, IRepository<SectionDocument> sectionRepository, IRepository<CourseDocument> courseRepository)
    {
        SectionRepository = sectionRepository;
        CourseRepository = courseRepository;
        CapacityService = capacityService;
    }

    public async Task<PlannerDataRecord> ToDataRecordAsync(PlannerDocument plannerDocument)
    {
        var courseDataRecords = new List<CourseDataRecord>();
        foreach (var courseId in plannerDocument.CourseIds)
        {
            var courseDocument = await (await CourseRepository.Collection.FindAsync(c => c.Id == courseId)).FirstAsync();
            var sectionsDataRecords = new List<CourseSectionDataRecord>();
            foreach (var sectionId in courseDocument.SectionIds)
            {
                var sectionDocument = await (await SectionRepository.Collection.FindAsync(s => s.CourseReferenceNumber == sectionId)).FirstAsync();
                var capacity = CapacityService.GetSectionCapacity(sectionDocument.CourseReferenceNumber);
                var sectionDataRecord = new CourseSectionDataRecord(sectionDocument, capacity);
                sectionsDataRecords.Add(sectionDataRecord);
            }
            var courseDataRecord = new CourseDataRecord($"{courseDocument.CourseId} - {courseDocument.Name}", sectionsDataRecords);
            courseDataRecords.Add(courseDataRecord);
        }
        
        return new(plannerDocument.Name, courseDataRecords);
    }
}