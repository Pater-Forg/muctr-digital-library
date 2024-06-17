using MDLibrary.Domain.Entities;
using MDLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MDLibrary.Helpers
{
    public static class DbSetLiteratureExtensions
    {
        public static IQueryable<Literature> Search(this DbSet<Literature> set, SearchModel searchModel)
        {
            var results = set.Include(x => x.Authors).Include(x => x.Keywords).AsQueryable();

            if (searchModel.Authors != null)
            {
                var authors = searchModel.Authors.Trim().ToLower().Split(", ");
                //results = results.Include(x => x.Authors).Where(x =>
                //    x.Authors.Select(a => a.ToString().ToLower())
                //             .Intersect(authors).Any()
                //);
                //foreach (var author in authors)
                //{
                //    results = results.Concat(results
                //        .Where(x => x.Authors.Select(x => x.ToString()).ToList().Contains(author)));
                //}
                //results = results.Distinct();
                results = results
                    .Where(x => x.Authors.Any(a => authors.Contains(a.Name.ToLower())));
            }
            
            if (searchModel.PublishYearLower != null)
            {
                results = results.Where(x => x.PublishYear >= searchModel.PublishYearLower);
            }

            if (searchModel.PublishYearUpper != null)
            {
                results = results.Where(x => x.PublishYear <= searchModel.PublishYearUpper);
            }

            if (searchModel.SearchInKeywords)
            {
				var keywords = searchModel.SearchQuery.Trim().ToLower().Split(' ');
                results = results
                    .Where(x => x.Keywords.Any(k => keywords.Contains(k.Value.ToLower())));
			}

            if (searchModel.SearchInCaption)
            {
				results = results.Where(x =>
					EF.Functions.ILike(x.Caption, $"%{searchModel.SearchQuery}%")
				);
			}

            if (searchModel.SearchInAbstract)
            {
				results = results.Where(x =>
					x.Abstract == null ||
					EF.Functions.ILike(x.Abstract, $"%{searchModel.SearchQuery}%")
				);
			}

            return searchModel.Order == "asc"
                    ? results.OrderBy(x => x.PublishYear)
                    : results.OrderByDescending(x => x.PublishYear);
        }
    }
}
