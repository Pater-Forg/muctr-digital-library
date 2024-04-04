using MDLibrary.Domain;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MDLibrary.Models
{
    public class SeedData
    {
        private class JsonLiteratureModel
        {
            public string[] Authors { get; set; } = [];
            public string? Bbc { get; set; }
            public string Caption { get; set; }
            public string? Isbn { get; set; }
            public string? PublishLocation { get; set; }
            public short? PublishYear { get; set; }
            public string? Publisher { get; set; }
            public string? Udc { get; set; }
            public short? Volume { get; set; }
            public string? Abstract { get; set; }
            public string[] Keywords { get; set; } = [];
        }

        public static void PopulateFromDataFile(IApplicationBuilder app, IConfiguration configuration)
        {
            MDLibraryDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<MDLibraryDbContext>();

            if (context.Literature.Any() ||
                context.Authors.Any() ||
                context.Keywords.Any())
            {
                return;
            }

            string dataFilePath = configuration["SeedDataFilePath"] ??
                throw new InvalidOperationException("Path to data file not found");

            using (StreamReader reader = System.IO.File.OpenText(dataFilePath))
            {
                string? input = null;
                while (!(input = reader.ReadLine()).IsNullOrEmpty())
                {
                    var model = JsonSerializer.Deserialize<JsonLiteratureModel>(input)!;
                    _proccessModel(model, context);
                }
            }

            context.SaveChanges();
        }

        private static void _proccessModel(JsonLiteratureModel model, MDLibraryDbContext context)
        {
            var literatureEntity = new Literature
            {
                PublishYear = model.PublishYear,
                PageCount = model.Volume,
                Caption = model.Caption,
                PublishLocation = model.PublishLocation,
                Publisher = model.Publisher,
                Isbn = model.Isbn,
                Bbc = model.Bbc,
                Udc = model.Udc,
                Abstract = model.Abstract
            };

            if (literatureEntity.Caption == "[no caption]")
                return;

            foreach (var name in model.Authors)
            {
                var authorEntity = context.Authors
                    .Include(a => a.Literature)
                    .FirstOrDefault(a => a.Name == name);

                if (authorEntity is null)
                {
                    authorEntity = new Author { Name = name };
                    context.Authors.Add(authorEntity);
                }

                authorEntity.Literature.Add(literatureEntity);
                literatureEntity.Authors.Add(authorEntity);
            }

            foreach (var keyword in model.Keywords)
            {
                var keywordEntity = context.Keywords
                    .Include(k => k.Literature)
                    .FirstOrDefault(k => k.Value == keyword);

                if (keywordEntity is null)
                {
                    keywordEntity = new Keyword { Value = keyword };
                    context.Keywords.Add(keywordEntity);
                }

                keywordEntity.Literature.Add(literatureEntity);
                literatureEntity.Keywords.Add(keywordEntity);
            }
        }
    }
}
