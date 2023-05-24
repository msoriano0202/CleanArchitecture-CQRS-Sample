﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUserName;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Application.Specifications.Streamers;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUrl
{
    public class GetStreamerListByUrlQueryHandler : IRequestHandler<GetStreamerListByUrlQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly ILogger<GetStreamerListByUrlQueryHandler> _logger;

        public GetStreamerListByUrlQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper
            //ILogger<GetStreamerListByUrlQueryHandler> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_logger = logger;
        }

        public async Task<List<StreamersVm>> Handle(GetStreamerListByUrlQuery request, CancellationToken cancellationToken)
        {
            var spec = new StreamersWithVideosSpecification(request.Url);
            var streamerList = await _unitOfWork.Repository<Streamer>().GetAllWithSpec(spec);

            return _mapper.Map<List<StreamersVm>>(streamerList);
        }
    }
}
