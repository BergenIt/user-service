using System;

using AutoMapper;

using Google.Protobuf.WellKnownTypes;

namespace UserService.Main.Automapper
{
    internal class TimeProfile : Profile
    {
        public TimeProfile()
        {
            CreateMap<DateTime, Timestamp>().ConvertUsing(x => Timestamp.FromDateTimeOffset(x));
            CreateMap<DateTimeOffset, Timestamp>().ConvertUsing(x => Timestamp.FromDateTimeOffset(x));
            CreateMap<Timestamp, DateTime>().ConvertUsing(x => x.ToDateTime());
            CreateMap<Timestamp, DateTimeOffset>().ConvertUsing(x => x.ToDateTimeOffset());

            CreateMap<TimeSpan, Duration>().ConvertUsing(x => Duration.FromTimeSpan(x));
            CreateMap<Duration, TimeSpan>().ConvertUsing(x => x.ToTimeSpan());
        }
    }
}
