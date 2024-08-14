using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Abstract.Constants;
using System.Text;

namespace RestWithASPNETUdemy.Hypermedia.Enricher
{
    public sealed class PersonEnricher : ContentResponseEnricher<PersonVO>
    {
        protected override Task EnrichModel(PersonVO content, IUrlHelper urlHelper)
        {
            var path = "api/person";

            string link = GetLink(content.Id, urlHelper, path);

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerbs.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet,
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerbs.POST,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPost,
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerbs.PUT,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPut,
            });

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerbs.DELETE,
                Href = link,
                Rel = RelationType.self,
                Type = "int"
            }); ;

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerbs.PATH,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultPath,
            }); ;

            return Task.CompletedTask;
        }

        private string GetLink(int id, IUrlHelper urlHelper, string path)
        {
            lock (this)
            {
                var url = new { controller = path, id };

                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2f", "/").ToString();
            }
        }
    }
}
