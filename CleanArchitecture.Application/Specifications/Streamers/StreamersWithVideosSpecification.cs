using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Streamers
{
    public class StreamersWithVideosSpecification : BaseSpecification<Streamer>
    {
        public StreamersWithVideosSpecification()
        {
            Includes.Add(p => p.Videos!);
        }

        public StreamersWithVideosSpecification(string url): base(p => p.Url!.Contains(url))
        {
            Includes.Add(p => p.Videos!);
        }
    }
}
