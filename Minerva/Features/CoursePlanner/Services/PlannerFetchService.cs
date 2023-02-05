using Minerva.Features.Athena.Documents;
using Minerva.Infrastructure.Database;
using Minerva.Utility;
using MongoDB.Driver;

namespace Minerva.Features.CoursePlanner.Services;

public class PlannerFetchService
{
    private readonly IRepository<TermDocument> TermRepository;

    private readonly IRepository<SubjectDocument> SubjectRepository;
    
    private readonly IRepository<CourseDocument> CourseRepository;
    
    private readonly IRepository<SectionDocument> SectionRepository;
    
    public PlannerFetchService(IRepository<TermDocument> termRepository, IRepository<SubjectDocument> subjectRepository, IRepository<CourseDocument> courseRepository, IRepository<SectionDocument> sectionRepository)
    {
        SubjectRepository = subjectRepository;
        CourseRepository = courseRepository;
        SectionRepository = sectionRepository;
    }
    
    public async Task<IEnumerable<SubjectDocument>> GetAllSubjectsAsync(int termId, CancellationToken ct)
    {
        var termFilter = Builders<TermDocument>.Filter.Eq(s => s.TermId, termId);
        var termProjection = Builders<TermDocument>.Projection.Include(s => s.SubjectIds);
        return await TermRepository.Client.DoTransactionAsync(async (_, _) =>
        {
            var term = await TermRepository.FindOneAsync(termFilter, termProjection, ct);
            var subjectFilter = Builders<SubjectDocument>.Filter.In(s => s.SubjectId, term.SubjectIds);
            var subjects = await SubjectRepository.Collection.FindAsync(subjectFilter, cancellationToken: ct);
            return await subjects.ToListAsync(ct);
        }, cancellationToken: ct);
    }

    public async Task<IEnumerable<CourseDocument>> GetCoursesBySubjectIdAsync(string subjectId, CancellationToken ct)
    {
        var subjectFilter = Builders<SubjectDocument>.Filter.Eq(s => s.SubjectId, subjectId);
        var subjectProjection = Builders<SubjectDocument>.Projection.Include(s => s.CourseIds);
        return await CourseRepository.Client.DoTransactionAsync(async (_, _) =>
        {
            var subject = await SubjectRepository.FindOneAsync(subjectFilter, subjectProjection, ct);
            var courseFilter = Builders<CourseDocument>.Filter.In(c => c.CourseId, subject.CourseIds);
            var courses = await CourseRepository.Collection.FindAsync(courseFilter, cancellationToken: ct);
            return await courses.ToListAsync(ct);
        }, cancellationToken: ct);
    }
    
    public async Task<IEnumerable<SectionDocument>> GetSectionsByCourseIdAsync(string courseId, CancellationToken ct)
    {
        var courseFilter = Builders<CourseDocument>.Filter.Eq(c => c.CourseId, courseId);
        var courseProjection = Builders<CourseDocument>.Projection.Include(c => c.SectionIds);
        return await SectionRepository.Client.DoTransactionAsync(async (_, _) =>
        {
            var course = await CourseRepository.FindOneAsync(courseFilter, courseProjection, ct);
            var sectionFilter = Builders<SectionDocument>.Filter.In(s => s.CourseReferenceNumber, course.SectionIds);
            var sections = await SectionRepository.Collection.FindAsync(sectionFilter, cancellationToken: ct);
            return await sections.ToListAsync(ct);
        }, cancellationToken: ct);
    }
    
    public async Task<IEnumerable<TermDocument>> GetAllTermsAsync(CancellationToken ct)
    {
        var terms = await TermRepository.Collection.FindAsync(FilterDefinition<TermDocument>.Empty, cancellationToken: ct);
        return await terms.ToListAsync(ct);
    }
}