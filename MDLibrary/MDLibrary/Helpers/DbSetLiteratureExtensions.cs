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
            var results = set.Include(x => x.Authors)
                             .Include(x => x.Keywords)
                             .Select(x => x);

            if (searchModel.Authors != null)
            {
                var authors = searchModel.Authors.ToLower().Split(", ");
                results = results.Where(x =>
                    x.Authors.Select(a => a.ToString().ToLower())
                             .Intersect(authors).Any()
                );
            }
            
            if (searchModel.PublishYearLower != null)
            {
                results = results.Where(x => x.PublishYear >= searchModel.PublishYearLower);
            }

            if (searchModel.PublishYearUpper != null)
            {
                results = results.Where(x => x.PublishYear <= searchModel.PublishYearUpper);
            }

            if (searchModel.SearchQuery != null)
            {
                results = results.Where(x =>
                    EF.Functions.ILike(x.Caption, $"%{searchModel.SearchQuery}%") ||
                    x.Abstract != null &&
                    EF.Functions.ILike(x.Abstract, $"%{searchModel.SearchQuery}%")
                );
            }

            return results;
        }
    }
}
