﻿using CleanArchitecture.Application.Features.Actors.Queries.Vms;

namespace CleanArchitecture.Application.Features.Videos.Queries.Vms
{
    public class VideosWithIncludesVm
    {
        public string? Nombre { get; set; }
        public int StreamerId { get; set; }
        public string? StreamerNombre { get; set; }
        public int DirectorId { get; set; }
        public string? DirectorNombreCompleto { get; set; }

        public virtual ICollection<ActorVm>? Actores { get; set; }

    }
}
