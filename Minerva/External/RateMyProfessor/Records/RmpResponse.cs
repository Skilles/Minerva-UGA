namespace Minerva.External.RateMyProfessor.Records;

public record RmpResponse(SearchType Search);

public record SearchType(TeacherType Teachers);

public record TeacherType(IEnumerable<EdgesType> Edges);

public record EdgesType(NodeType Node);

public record NodeType(float AvgRating, float AvgDifficulty, string Id, int LegacyId);