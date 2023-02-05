using Minerva.External.RateMyProfessor.Records;
using Minerva.Features.Athena.Documents;
using Minerva.Infrastructure.Database;
using Minerva.Utility;
using Minerva.Validation;
using MongoDB.Driver;

namespace Minerva.Features.Rating.Services;

public class RmpService
{
    private readonly IRepository<ProfessorDocument> ProfessorRepository;

    private readonly RmpClient RmpClient;

    public RmpService(IRepository<ProfessorDocument> professorRepository, RmpClient rmpClient)
    {
        ProfessorRepository = professorRepository;
        RmpClient = rmpClient;
    }

    public async Task<ProfessorInfo> GetProfessorInfoAsync(string professorName, CancellationToken cancellationToken)
    {
        var professorDocument =
            await ProfessorRepository.FindOneAsync(x => x.FullName == professorName,
                cancellationToken: cancellationToken);

        if (professorDocument == null)
        {
            throw new MinervaValidationException("Professor not found in Minerva");
        }

        if (professorDocument.RmpInfo != null)
        {
            return professorDocument.RmpInfo;
        }

        var professorInfo = await RmpClient.GetProfessorInfoAsync(professorName, cancellationToken);
        if (professorInfo == null)
        {
            throw new MinervaValidationException("Professor not found in RMP");
        }

        professorDocument.RmpInfo = professorInfo;
        var filter = Builders<ProfessorDocument>.Filter.Eq(x => x.FullName, professorDocument.FullName);
        await ProfessorRepository.UpsertAsync(professorDocument, filter, cancellationToken);

        return professorDocument.RmpInfo;
    }
}