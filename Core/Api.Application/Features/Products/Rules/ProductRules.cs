using Api.Application.Bases;
using Api.Application.Features.Products.Exceptions;
using Api.Domain.Entities;

namespace Api.Application.Features.Products.Rules
{
    public class ProductRules : BaseRules
    {
        public Task ProductTitleMustBeUnique(string requestTitle, IList<Product> productTitle)
        {
            if(productTitle.Any(x => x.Title == requestTitle)) throw new ProductTitleMustBeUniqueException();
            return Task.CompletedTask;
        }
    }
}
