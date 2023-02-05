using System.Security.Claims;
using FastEndpoints;

namespace Minerva.Features.CoursePlanner.Records;

public class CreatePlannerRequest
{
    [FromClaim(ClaimTypes.Email)]
    public string Email { get; set; }
    
    public int TermId { get; set; }
}