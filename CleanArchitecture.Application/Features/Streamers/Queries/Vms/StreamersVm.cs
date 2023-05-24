using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;

namespace CleanArchitecture.Application.Features.Streamers.Queries.Vms
{
    public class StreamersVm
    {
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public ICollection<VideosVm>? Videos { get; set; }
    }
}
