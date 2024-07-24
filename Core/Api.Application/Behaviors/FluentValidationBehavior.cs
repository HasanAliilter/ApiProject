using FluentValidation;
using MediatR;

namespace Api.Application.Behaviors
{
    internal class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validator;

        public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            this.validator = validator;
        }
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request); //Bu satırda, request nesnesi için bir ValidationContext<TRequest> nesnesi oluşturulur yani doğrulama işlemlerinin hangi nesne için olduğunu belirttik ve bunun için doğrulayıcıları atadık
            var failtures = validator
                .Select(v => v.Validate(context)) // Her doğrulayıcı için validate yi çağırdık
                .SelectMany(result => result.Errors) // Bu sonuçları alıp listeledik
                .GroupBy(x => x.ErrorMessage) // Bu listeyi alıp içeriğine sıraladık
                .Select(x => x.First()) // her çeşit hatanın İlk mesajını aldık
                .Where(w => w != null) //null olmayanları al
                .ToList(); // sonuçları listeye çevir

            if (failtures.Any())
            {
                throw new ValidationException(failtures);
            }

            return next();
        }
    }
}
