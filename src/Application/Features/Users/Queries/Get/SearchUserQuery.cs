using Kondongpu.Application.Common.Interfaces;
using Kondongpu.Application.Common.Mappings;
using Kondongpu.Application.Common.Models;
using Kondongpu.Application.Features.Users.ViewModel;

namespace Kondongpu.Application.Features.Users.Queries.Get;

public record SearchUserQuery : IRequest<PaginatedList<UserViewModel>>
{
    /// <summary>
    /// คำค้น
    /// </summary>
    public required string KeyWord { get; set; }

    /// <summary>
    /// หน้าของข้อมูลเป็นเลขจำนวนเต็ม
    /// </summary>
    public required int Page { get; set; } = 1;

    /// <summary>
    /// จำนวนของข้อมูลที่ต้องการเป็นเลขจำนวนเต็ม
    /// </summary>
    public required int Length { get; set; } = 10;

}

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, PaginatedList<UserViewModel>>
{
    private readonly IMapper _mapper;
    private readonly KondongpuDatabaseContext _context;

    public SearchUserQueryHandler(KondongpuDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserViewModel>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
                .AsNoTracking()
                .Where(e => e.VisitNumber.Contains(request.KeyWord))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.Page, request.Length, cancellationToken: cancellationToken);
    }
}
