using AutoMapper;
using AutoMapper.Internal;
using System.Net.Http.Headers;

namespace Api.Mapper.AutoMapper
{
    public class Mapper : Application.Interface.AutoMapper.IMapper
    {
        public static List<TypePair> typePairs = new(); //yapılandırılmış tür çiftlerini saklayan bir yapı
        private IMapper MapperContainer;

        public TDestination Map<TDestination, TSource>(TSource source, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return MapperContainer.Map<TSource, TDestination>(source);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return MapperContainer.Map<IList<TSource>, IList<TDestination>>(source);
        }

        public TDestination Map<TDestination>(object source, string? ignore = null)
        {
            Config<TDestination, object>(5, ignore);
            return MapperContainer.Map<TDestination>(source);
        }

        public IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null)
        {
            Config<TDestination, IList<object>>(5, ignore);
            return MapperContainer.Map<IList<TDestination>>(source);
        }

        protected void Config<TDestination, TSource>(int depth = 5, string? ignore = null)
        {
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));

            if (typePairs.Any(a => a.DestinationType == typePair.DestinationType && a.SourceType == typePair.SourceType) && ignore is null) // yapılandırma zaten mevcutsa yeni yapılandırma oluşturma
                return;
            //var products = mapper.Map<ProductDto, Product>(productDto, "Price"); mesela burada ignore ye price verildiği zaman price özelliği maplenmeyecektir
            typePairs.Add(typePair);
            var config = new MapperConfiguration(cfg =>  // AutoMapper yapılandırmalarını tanımlamak için lazım
            {
                foreach (var item in typePairs)
                {
                    if (ignore is not null)
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ForMember(ignore, x => x.Ignore()).ReverseMap(); //ignore null değilse göz ardı edeceğimiz özelliği belirtmeliyiz

                    else
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ReverseMap(); //ilk createmap ile veri türlerini mapliyoruz sonrasında depth değerini verip karşılıklı maplensin diye reversemap özelliğini kullanıyoruz
                }
            });

            MapperContainer = config.CreateMapper();
        }
    }
}
