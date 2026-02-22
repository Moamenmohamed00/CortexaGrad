using System;

namespace Cortexa.Application.Dtos.AI
{
    public record KnowledgeSourceDto(
        string Id,
        string Title,
        string Type,
        string Url,
        string DoctorId
    );
}
