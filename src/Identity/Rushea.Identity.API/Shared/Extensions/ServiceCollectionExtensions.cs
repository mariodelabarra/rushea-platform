using Rushea.Identity.API.Features.Organisations.Create;
using Rushea.Identity.API.Features.Organisations.Delete;
using Rushea.Identity.API.Features.Organisations.GetAll;
using Rushea.Identity.API.Features.Organisations.GetById;
using Rushea.Identity.API.Features.Organisations.Update;
using Rushea.Identity.API.Features.Sites.Create;
using Rushea.Identity.API.Features.Sites.Delete;
using Rushea.Identity.API.Features.Sites.GetAll;
using Rushea.Identity.API.Features.Sites.GetById;
using Rushea.Identity.API.Features.Sites.Update;

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

    public static void RegisterSiteServices(IServiceCollection services)
    {
        services.AddScoped<ICreateSitesService, CreateSitesService>();
        services.AddScoped<IUpdateSiteService, UpdateSiteService>();
        services.AddScoped<IGetSiteByIdService, GetSiteByIdService>();
        services.AddScoped<IGetAllSitesService, GetAllSitesService>();
        services.AddScoped<IDeleteSiteService, DeleteSiteService>();
    }
}
