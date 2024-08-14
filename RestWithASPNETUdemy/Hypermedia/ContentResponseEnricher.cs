﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestWithASPNETUdemy.Hypermedia.Abstract;
using RestWithASPNETUdemy.Hypermedia.Utils;
using System.Collections.Concurrent;

namespace RestWithASPNETUdemy.Hypermedia
{
    public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISsuporstHyperMedia
    {
        public ContentResponseEnricher()
        {

        }

        public bool CanEnrich(Type ContentType)
        {
            return ContentType == typeof(T) || ContentType == typeof(List<T>) || ContentType == typeof(PageSearchVO<T>);
        }

        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okObjectResult)
            {
                return CanEnrich(okObjectResult.Value.GetType());
            }

            return false;
        }

        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);

            if (response.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
                else if (okObjectResult.Value is List<T> collection)
                {
                    ConcurrentBag<T> bag = new(collection);

                    Parallel.ForEach(bag, (element) =>
                    {
                        EnrichModel(element, urlHelper);
                    });
                }
                else if (okObjectResult.Value is PageSearchVO<T> pagedSearch)
                {
                    Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
                    {
                        EnrichModel(element, urlHelper);
                    });
                }
            }

            await Task.FromResult<object>(null);
        }
    }
}
