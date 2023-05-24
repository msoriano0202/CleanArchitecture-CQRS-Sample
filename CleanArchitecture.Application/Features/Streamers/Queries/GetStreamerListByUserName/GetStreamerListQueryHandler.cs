using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Commands;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUserName
{
    public class GetStreamerListQueryHandler : IRequestHandler<GetStreamerListQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetStreamerListQueryHandler> _logger;

        public GetStreamerListQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<GetStreamerListQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<StreamersVm>> Handle(GetStreamerListQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Streamer, object>>>();
            includes.Add(p => p.Videos!);

            var streamerList = await _unitOfWork.Repository<Streamer>().GetAsync(
                b => b.CreatedBy == request.UserName,
                b => b.OrderBy(x => x.CreatedBy),
                includes,
                true
                );

            return _mapper.Map<List<StreamersVm>>(streamerList);
        }
    }
}
