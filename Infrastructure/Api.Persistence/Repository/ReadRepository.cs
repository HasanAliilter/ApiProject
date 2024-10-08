﻿using Api.Application.Interface.Repositories;
using Api.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Api.Persistence.Repository
{
    public class ReadRepository<T> : IReadRepository<T> where T : class, IEntityBase, new()
    {
        private readonly DbContext dbContext;

        public ReadRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private DbSet<T> Table { get => dbContext.Set<T>(); } //dbContext nesnesini kullanmak için her seferinde dbContext.Set<T>() yazmamız gerek bunun kısa yolu
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;
            if(!enableTracking) queryable = queryable.AsNoTracking(); //tracking mekanizması vt üzerinde işlem yapılıp yapılmadığını kontrol eden mekanizmadır. Biz bu fonksiyonda sadece okuma işlemi yapacağımız ve bunu üzerine performansı iyleştirmek için bu mekanizmayı devre dışı bırakmalıyız
            if(include != null) queryable = include(queryable);
            if(predicate != null) queryable = queryable.Where(predicate);
            if(orderBy != null) return await orderBy(queryable).ToListAsync();

            return await queryable.ToListAsync();
        }
        //Örnek kullanım
        //var products = await repository.GetAllAsync(
        //    predicate: p => p.Price > 100 && p.CategoryId == 10, // Fiyatı 100'den fazla ve CategoryId'si 10 olan ürünler
        //    include: q => q.Include(p => p.Category), // Ürünler ve kategorileri
        //    orderBy: q => q.OrderBy(p => p.Name), // Ürünleri ada göre sırala
        //    enableTracking: false // İzleme kapalı
        //);

        public async Task<IList<T>> GetAllByPagingAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false, int currentPage = 1, int pageSize = 3)
        {
            IQueryable<T> queryable = Table;
            if (!enableTracking) queryable = queryable.AsNoTracking(); //tracking mekanizması vt üzerinde işlem yapılıp yapılmadığını kontrol eden mekanizmadır. Biz bu fonksiyonda sadece okuma işlemi yapacağımız ve bunu üzerine performansı iyleştirmek için bu mekanizmayı devre dışı bırakmalıyız
            if (include != null) queryable = include(queryable);
            if (predicate != null) queryable = queryable.Where(predicate);
            if (orderBy != null) return await orderBy(queryable).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return await queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(); // skip in içindeki işlem kadar veri atlayıp pagesize kadar veri alıyoruz
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = Table;
            if (!enableTracking) queryable = queryable.AsNoTracking(); //tracking mekanizması vt üzerinde işlem yapılıp yapılmadığını kontrol eden mekanizmadır. Biz bu fonksiyonda sadece okuma işlemi yapacağımız ve bunu üzerine performansı iyleştirmek için bu mekanizmayı devre dışı bırakmalıyız
            if (include != null) queryable = include(queryable);

            return await queryable.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false)
        {
            if (!enableTracking) Table.AsNoTracking();
            return Table.Where(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            Table.AsNoTracking();
            if (predicate != null) Table.Where(predicate);
            return await Table.CountAsync();
        }

    }
}
