using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.EventSourcedNormalizers;
using JPProject.Admin.Application.ViewModels;
using JPProject.Admin.Application.ViewModels.ApiResouceViewModels;
using JPProject.Admin.Application.ViewModels.ClientsViewModels;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.ViewModels;
using System.Globalization;
using System.Security.Claims;

namespace JPProject.Admin.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ApiResource, ApiResourceListViewModel>();

            CreateMap<StoredEvent, EventHistoryData>().ConstructUsing(a => new EventHistoryData(a.Message, a.Id.ToString(), a.Details, a.Timestamp.ToString(CultureInfo.InvariantCulture), a.User, a.MessageType, a.RemoteIpAddress));
            CreateMap<Client, ClientListViewModel>(MemberList.Destination);
            CreateMap<Secret, SecretViewModel>(MemberList.Destination);
            CreateMap<IDictionary<string, string>, ClientPropertyViewModel>();
            CreateMap<Claim, ClaimViewModel>().ConstructUsing(a => new ClaimViewModel(a.Type, a.Value));
            CreateMap<IdentityResource, IdentityResourceListView>(MemberList.Destination);
            CreateMap<Scope, ScopeViewModel>();

        }
    }
}
