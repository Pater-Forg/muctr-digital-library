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
            var results = set.Include(x => x.Authors).Select(x => x);
            if (searchModel.Authors != null)
            {
                var authors = searchModel.Authors.ToLower().Split(", ");
                results = results.Where(x =>
                    x.Authors.Select(a => a.ToString().ToLower())
                             .Intersect(authors).Any()
                );
            }
            if (searchModel.Keywords != null)
            {
                var keywords = searchModel.Keywords.ToLower().Split(", ");
                results = results.Where(x =>
                    x.Keywords.Select(k => k.ToString().ToLower())
                              .Intersect(keywords).Any()
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
                    EF.Functions.ILike(x.Caption.ToLower(), $"%{searchModel.SearchQuery}%") ||
                    x.Abstract != null &&
                    EF.Functions.ILike(x.Abstract.ToLower(), $"%{searchModel.SearchQuery}%")
                );
            }

            return results;
        }
    }
}
