using MediatR;

using MovieStudio.Shared.Application.DTOs;

namespace MovieStudio.Shared.Application.Queries.Handlers;

public interface IGetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    
}
