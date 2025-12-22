using MediatR;

using MovieStudio.Application.DTOs;

namespace MovieStudio.Application.Queries;

public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;
