using Api.Domain.Items;
using Api.Features.BestStories;
using AutoMapper;

namespace Api.Mapping;

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
        CreateMap<Item, GetBestStories.BestStoriesResponse>();
    }
}