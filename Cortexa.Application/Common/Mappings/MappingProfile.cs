using AutoMapper;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Dtos.Actors;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Clinical;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Dtos.Beds;
using Cortexa.Application.Dtos.Rooms;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.ValueObjects;
using Cortexa.Domain.Entities.Infrastructure;
namespace Cortexa.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Actors
            CreateMap<Patient, PatientDto>();
            CreateMap<Doctor, DoctorDto>();
            CreateMap<Nurse, NurseDto>();
            CreateMap<Patient, PatientSummaryDto>();
            CreateMap<Bed, BedDto>()
             .ForCtorParam("BedId", opt => opt.MapFrom(src => src.Id));

            CreateMap<Room, RoomDto>()
                .ForCtorParam("RoomId", opt => opt.MapFrom(src => src.Id));


            // Core
            CreateMap<Admission, AdmissionDto>();
            CreateMap<Address, AddressDto>();

            // Clinical
            CreateMap<VitalSigns, VitalSignsDto>();
            CreateMap<VitalSignsDto, VitalSigns>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Let DB handle Id if it's new
                .ForMember(dest => dest.Admission, opt => opt.Ignore()) // Navigation properties should be ignored
                .ForMember(dest => dest.Nurse, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());
            CreateMap<Medications, MedicationDto>();
            CreateMap<LabResult, LabResultDto>(); // Technically Diagnostic but referenced in Nurse
            CreateMap<NursingNotes, NursingNotesDto>();
            CreateMap<FluidBalance, FluidBalanceDto>()
                .ForMember(dest => dest.AmountMl,
                    opt => opt.MapFrom(src => src.Amount_ML));
            CreateMap<CaseHistory, CaseHistoryDto>();
            CreateMap<PhysicalExamination, PhysicalExaminationDto>();
            CreateMap<InterventionProcedure, InterventionProcedureDto>();

            // Diagnostics
            CreateMap<LabOrder, LabOrderDto>();
            CreateMap<Imaging, ImagingDto>();
            CreateMap<Culture, CultureDto>();

            // AI
            CreateMap<Alert, AlertDto>();
            CreateMap<RAGQuery, RagQueryDto>();
            CreateMap<KnowledgeSource, KnowledgeSourceDto>();

            // Legacy / Specific Mappings
            //CreateMap<Patient, PatientAdmissionDto>();
            //    //.ForMember(d => d.PatientId, o => o.MapFrom(s => s.Id))
            //    //.ForMember(d => d.NationalId, o => o.MapFrom(s => s.NationalId))
            //    //.ForMember(d => d.AdmissionId, o => o.Ignore())
            //    //.ForMember(d => d.AdmissionDate, o => o.Ignore())
            //    //.ForMember(d => d.InitialDiagnosis, o => o.Ignore())
            //    //.ForMember(d => d.Status, o => o.Ignore())
            //    //.ForMember(d => d.BedId, o => o.Ignore())
            //    //.ForMember(d => d.RoomId, o => o.Ignore())
            //    //.ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            //    //.ForMember(d => d.Phone, o => o.MapFrom(s => s.PhoneNumber));

            //CreateMap<Admission, PatientAdmissionDto>();
            //    //.ForMember(d => d.AdmissionId, o => o.MapFrom(s => s.Id))
            //    //.ForMember(d => d.NationalId, o => o.Ignore())
            //    //.ForMember(d => d.PatientId, o => o.Ignore())
            //    //.ForMember(d => d.FileNumber, o => o.Ignore())
            //    //.ForMember(d => d.Name, o => o.Ignore())
            //    //.ForMember(d => d.DateOfBirth, o => o.Ignore())
            //    //.ForMember(d => d.Gender, o => o.Ignore())
            //    //.ForMember(d => d.Email, o => o.Ignore())
            //    //.ForMember(d => d.Phone, o => o.Ignore())
            //    //.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<Admission, PatientAdmissionDto>()
    .ForMember(d => d.AdmissionId, o => o.MapFrom(s => s.Id))
    .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
    .ForMember(d => d.PatientId, o => o.MapFrom(s => s.Patient.Id))
    .ForMember(d => d.NationalId, o => o.MapFrom(s => s.Patient.NationalId))
    .ForMember(d => d.Name, o => o.MapFrom(s => s.Patient.Name))
    .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.Patient.DateOfBirth))
    .ForMember(d => d.Gender, o => o.MapFrom(s => s.Patient.Gender.ToString()))
    .ForMember(d => d.Email, o => o.MapFrom(s => s.Patient.Email))
    .ForMember(d => d.Phone, o => o.MapFrom(s => s.Patient.PhoneNumber));

        }
    }
}
