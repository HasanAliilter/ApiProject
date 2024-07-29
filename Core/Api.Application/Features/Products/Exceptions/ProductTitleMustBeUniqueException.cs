using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Features.Products.Exceptions
{
    public class ProductTitleMustBeUniqueException : BaseExceptions
    {
        public ProductTitleMustBeUniqueException() : base("Ürün başlığı aynı olmamalı") { }
    }
}
