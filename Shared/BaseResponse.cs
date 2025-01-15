using HealthCenterAPI.Shared.RequestFeatures;

namespace HealthCenterAPI.Shared
{
    public class BaseResponse
    {
        public string? Message { get; set; } = null!;

        public object? Details { get; set; } = null!;

        public Pagination? Pagination { get; set; } = null!;

        public void SetPagination(Pagination pagination)
        {
            Pagination!.CurrentPage = pagination.CurrentPage;
            Pagination!.TotalPages = pagination.TotalPages;
            Pagination!.PreviousPage = pagination.PreviousPage;
            Pagination!.NextPage = pagination.NextPage;
            Pagination!.TotalCount = pagination.TotalCount;
            Pagination!.PageSize = pagination.PageSize;
        }

    }
}
