namespace Minerva.External.RateMyProfessor.Queries;

public static class SearchProfessorQuery
{
    public const string Query = """
      query TeacherSearchResultsPageQuery(
        $query: TeacherSearchQuery!
        $schoolID: ID
      ) {
        search: newSearch {
          ...TeacherSearchPagination_search_1ZLmLD
        }
        school: node(id: $schoolID) {
          __typename
          ... on School {
            name
          }
          id
        }
      }
      
      fragment TeacherSearchPagination_search_1ZLmLD on newSearch {
        teachers(query: $query, first: 8, after: "") {
          didFallback
          edges {
            cursor
            node {
              ...TeacherCard_teacher
              id
              __typename
            }
          }
          pageInfo {
            hasNextPage
            endCursor
          }
          resultCount
          filters {
            field
            options {
              value
              id
            }
          }
        }
      }
      
      fragment TeacherCard_teacher on Teacher {
        id
        legacyId
        avgRating
        numRatings
        ...CardFeedback_teacher
        ...CardSchool_teacher
        ...CardName_teacher
        ...TeacherBookmark_teacher
      }
      
      fragment CardFeedback_teacher on Teacher {
        wouldTakeAgainPercent
        avgDifficulty
      }
      
      fragment CardSchool_teacher on Teacher {
        department
        school {
          name
          id
        }
      }
      
      fragment CardName_teacher on Teacher {
        firstName
        lastName
      }
      
      fragment TeacherBookmark_teacher on Teacher {
        id
        isSaved
      }
      """;
}