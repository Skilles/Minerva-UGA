using Minerva.Features.Authentication.Enums;

namespace Minerva.Features.Authentication.Records;

public record UserResponseRecord
(
    string FirstName, 
    string LastName, 
    Role role, 
    string JwtToken,
    bool Verified
);