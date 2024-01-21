using AutoMapper;
using HackerNews.Api.Domain.Items;
using HackerNews.Api.Features.BestStories;

namespace HackerNews.Api.Mapping;

/// <summary>
/// Hacker News Profile
/// </summary>
public class HackerNewsProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public HackerNewsProfile()
    {
        CreateMap<Item, GetBestStories.BestStoriesResponse>()
            .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.By))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Descendants))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => FormatDate(src.Time)))
            .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Url == null ? null : new Uri(src.Url)));
    }

    private static string FormatDate(long unixDate)
    {
        var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixDate).UtcDateTime;

        return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}