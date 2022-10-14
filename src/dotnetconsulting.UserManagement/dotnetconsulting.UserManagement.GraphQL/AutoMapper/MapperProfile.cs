using AutoMapper;
using dotnetconsulting.UserManagement.Infrastructure.DTOs;
using dotnetconsulting.UserManagement.Logic.EntityFramework;

namespace dotnetconsulting.UserManagement.GraphQL.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(m => m.Name, f => f.MapFrom(o => o.Username))
            .ForMember(m => m.Name, f => f.MapFrom(o => o.UNumber))
            .ForMember(m => m.ResourceId, f => f.MapFrom(o => o.Culture))
            .ForMember(m => m.Roles, f => f.MapFrom(o => o.Permissions));

        CreateMap<PermissionDto, Role>()
            .ForMember(m => m.Name, f => f.MapFrom(o => o.Permission));
    }
}