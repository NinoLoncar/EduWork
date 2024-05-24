using AutoMapper;
using EduWork.Common.DTOs;
using EduWork.Domain.Entitites;

namespace EduWork.WebApi
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<WorkTimeDTO, WorkTime>();
            CreateMap<WorkTime, WorkTimeDTO>();
        }
    }
}
