using Rushea.Identity.API.Features.Organisations.Create;
using Rushea.Identity.API.Features.Organisations.Delete;
using Rushea.Identity.API.Features.Organisations.GetAll;
using Rushea.Identity.API.Features.Organisations.GetById;
using Rushea.Identity.API.Features.Organisations.Update;

namespace Rushea.Identity.API.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterOrganisationServices(IServiceCollection services)
    {
        services.AddScoped<IGetAllOrganisationsService, GetAllOrganisationsService>();
        services.AddScoped<IGetOrganisationByIdService, GetOrganisationByIdService>();
        services.AddScoped<ICreateOrganisationService, CreateOrganisationService>();
        services.AddScoped<IUpdateOrganisationService, UpdateOrganisationService>();
        services.AddScoped<IDeleteOrganisationService, DeleteOrganisationService>();
    }
}
