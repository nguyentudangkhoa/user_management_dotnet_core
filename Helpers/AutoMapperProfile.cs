using AutoMapper;
using test_dotnet_core_migration.Models;

namespace test_dotnet_core_migration.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<User, AuthenticateResponse>();
 
            // RegisterRequest -> User
            CreateMap<RegisterUserRequest, User>();
 
            // UpdateRequest -> User
            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
 
                        return true;
                    }
                ));
 
            CreateMap<RegisterRoleRequest, Role>();
 
            CreateMap<UpdateRoleRequest, Role>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
 
                        return true;
                    }
                ));

        }
    }
}