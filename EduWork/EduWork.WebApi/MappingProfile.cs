using AutoMapper;
using EduWork.Common.DTOs;
using EduWork.Data.Entitites;

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
