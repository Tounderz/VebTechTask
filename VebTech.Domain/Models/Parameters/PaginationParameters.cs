namespace VebTech.Domain.Models.Parameters;

public class PaginationParameters
{
    const int maxPageSize = 20;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber <= 0 ? 1 : _pageNumber;
        set => _pageNumber = value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }
}