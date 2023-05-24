using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Videos
{
    public class VideoForCountingSpecifiaction : BaseSpecification<Video>
    {
        public VideoForCountingSpecifiaction(VideoSpecificationParams videoParams)
            : base(x => 
                (string.IsNullOrEmpty(videoParams.Search) || x.Nombre!.Contains(videoParams.Search)) &&
                (!videoParams.DirectorId.HasValue || x.DirectorId == videoParams.DirectorId) &&
                (!videoParams.StreamerId.HasValue || x.StreamerId == videoParams.StreamerId)
            )
        {
            
        }
    }
}
