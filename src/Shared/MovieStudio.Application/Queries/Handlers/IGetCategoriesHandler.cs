using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries.Handlers;

public interface IGetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    
}
