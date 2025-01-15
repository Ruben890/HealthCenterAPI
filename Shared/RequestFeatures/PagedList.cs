using Microsoft.EntityFrameworkCore;

namespace HealthCenterAPI.Shared.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public Pagination Pagination { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Pagination = new Pagination
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                PreviousPage = pageNumber > 1 ? pageNumber - 1 : null,
                NextPage = pageNumber < (int)Math.Ceiling(count / (double)pageSize) ? pageNumber + 1 : null
            };
            AddRange(items);
        }

        /// <summary>
        /// Optimizado para IQueryable, permite que EF Core aplique paginación a nivel de base de datos
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // Obtener el total de elementos
            var items = await source.Skip((pageNumber - 1) * pageSize)  // Saltar páginas previas
                                    .Take(pageSize)                      // Tomar los elementos de la página actual
                                    .ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
