using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Videos
{
    public class VideoSpecification : BaseSpecification<Video>
    {
        public VideoSpecification(VideoSpecificationParams videoParams)
            : base(x =>
                (string.IsNullOrEmpty(videoParams.Search) || x.Nombre!.Contains(videoParams.Search)) &&
                (!videoParams.DirectorId.HasValue || x.DirectorId == videoParams.DirectorId) &&
                (!videoParams.StreamerId.HasValue || x.StreamerId == videoParams.StreamerId)
            )
        {
            AddInclude(x => x.Director!);
            AddInclude(x => x.Streamer!);
            AddInclude(x => x.Actores!);

            ApplyPaging(videoParams.PageSize * (videoParams.PageIndex - 1), videoParams.PageSize);

            if (!string.IsNullOrEmpty(videoParams.Sort))
            {
                switch (videoParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(x => x.Nombre!);
                        break;
                    case "nombreDesc":
                        AddOrderByDescending(x => x.Nombre!);
                        break;

                    default:
                        AddOrderBy(x => x.CreatedDate!);
                        break;
                }
            }
        }
    }
}
