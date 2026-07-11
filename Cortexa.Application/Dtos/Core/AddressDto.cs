namespace Cortexa.Application.Dtos.Core
{
    public record AddressDto(
        string Street,
        string City,
        string State,
        string? ZipCode
    );
}
