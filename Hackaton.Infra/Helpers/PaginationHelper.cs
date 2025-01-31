namespace Hackaton.Infra.Helpers
{
    public static class PaginationHelper<TEntity> where TEntity : class
    {
        public static IEnumerable<TEntity> Set(int page, int count, IEnumerable<TEntity> listData)
        {
            return listData.Skip((page - 1) * count)
                    .Take(count);
        }
    }
}
