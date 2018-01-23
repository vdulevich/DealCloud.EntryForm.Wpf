using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Entities.Query
{
    public static class QueryExtentions
    {
        public static List<FilterTerm> Join(this IEnumerable<FilterTerm> filterTerms, FilterTermType joinWith)
        {
            var result = filterTerms.SelectMany(filterTerm => new[]
            {
                filterTerm,
                new FilterTerm {FilterTermType = joinWith}
            }).ToList();

            if (result.Count > 0) result.RemoveAt(result.Count - 1);

            return result;
        }

        public static List<FilterTerm> Join(this IEnumerable<IEnumerable<FilterTerm>> filterTerms, FilterTermType joinWith)
        {
            var result = new List<FilterTerm>();

            foreach (var filterTermsEnumerable in filterTerms)
            {
                var filterTermsList = filterTermsEnumerable.ToList();

                if (filterTermsList.Any())
                {
                    result.AddRange(filterTermsList);
                    result.Add(new FilterTerm { FilterTermType = joinWith });
                }
            }

            if (result.Count > 0) result.RemoveAt(result.Count - 1);

            return result;
        }
    }
}