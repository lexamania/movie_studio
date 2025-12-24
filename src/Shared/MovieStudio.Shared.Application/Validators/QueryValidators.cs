using FluentValidation;
using MovieStudio.Shared.Application.Queries;
using MovieStudio.Shared.Domain.Models;

namespace MovieStudio.Shared.Application.Validators;

public class GetAllMoviesQueryValidator : AbstractValidator<GetAllMoviesQuery>
{
    public GetAllMoviesQueryValidator()
    {
        RuleFor(x => x.Pagination)
            .NotNull()
            .WithMessage("Pagination model is required");

        RuleFor(x => x.Pagination.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.Pagination.PageSize)
            .InclusiveBetween(1, PaginationModel.MAX_PAGE_SIZE)
            .WithMessage($"Page size must be between 1 and {PaginationModel.MAX_PAGE_SIZE}");
    }
}

public class GetMoviesByCategoryQueryValidator : AbstractValidator<GetMoviesByCategoryQuery>
{
    public GetMoviesByCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.Pagination)
            .NotNull()
            .WithMessage("Pagination model is required");

        RuleFor(x => x.Pagination.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.Pagination.PageSize)
            .InclusiveBetween(1, PaginationModel.MAX_PAGE_SIZE)
            .WithMessage($"Page size must be between 1 and {PaginationModel.MAX_PAGE_SIZE}");
    }
}

public class GetMovieByIdQueryValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdQueryValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty()
            .WithMessage("Movie ID is required");
    }
}
