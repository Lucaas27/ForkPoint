using FluentAssertions;
using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;
using ForkPoint.Application.Validators;
using ForkPoint.Domain.Enums;

namespace ForkPoint.Application.Tests.Validators;

public class GetAllRestaurantsRequestValidatorTests
{
    private readonly GetAllRestaurantsRequestValidator _validator = new();

    [Fact]
    public void GetAllRestaurantsRequest_PassesValidation_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            "Pizza",
            1,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenPageNumberIsLessThanOne()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            "Pizza",
            0,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenPageSizeIsInvalid()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            "Pizza",
            1,
            (PageSizeOptions)999,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenSortByIsInvalid()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            "Pizza",
            1,
            PageSizeOptions.Ten,
            (SortByOptions)999,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.SortBy);
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenSearchByIsInvalid()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest((
            SearchOptions)999,
            "Pizza",
            1,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.SearchBy);
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenSortDirectionIsProvidedWithoutSortBy()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            "Pizza",
            1,
            PageSizeOptions.Ten,
            null,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("SortBy");
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenSearchTermIsProvidedWithoutSearchBy()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            null,
            "Pizza",
            1,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("SearchBy");
    }

    [Fact]
    public void GetAllRestaurantsRequest_FailsValidation_WhenSearchByIsProvidedWithoutSearchTerm()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(
            SearchOptions.Name,
            null, 1,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("SearchTerm");
    }
}
