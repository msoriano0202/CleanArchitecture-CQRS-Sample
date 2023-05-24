using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUserName
{
    public class GetStreamerListQuery : IRequest<List<StreamersVm>>
    {
        public string? UserName { get; set; }

        public GetStreamerListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
