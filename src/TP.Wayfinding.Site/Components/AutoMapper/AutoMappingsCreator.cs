using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AutoMapper;
using Fasterflect;
using TP.Wayfinding.Site.Components.Extensions;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Domain;
using TP.Wayfinding.Site.Models.Building;
using TP.Wayfinding.Site.Models.Floor;
using TP.Wayfinding.Site.Components.Settings;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TP.Wayfinding.Site.Components.AutoMapper
{
    public static class AutoMappingsCreator
    {
        public static void CreateModelMappings()
        {
            Mapper.CreateMap<bool, string>().ConvertUsing<BoolToYesNoConverter>();
            Mapper.CreateMap<int?, string>().ConvertUsing<NullableTypeToStringConverter<int>>();
            Mapper.CreateMap<decimal?, string>().ConvertUsing(new NullableTypeToStringConverter<decimal>("n"));
            Mapper.CreateMap<DateTime?, string>().ConvertUsing(new NullableTypeToStringConverter<DateTime>(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern));

            Mapper.CreateMap<Building, BuildingListModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BuildingId));

            Mapper.CreateMap<Building, BuildingModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BuildingId));
            Mapper.CreateMap<BuildingModel, Building>()
                .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FloorMaps, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore());

            Mapper.CreateMap<FloorMap, FloorListModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FloorMapId));
             Mapper.CreateMap<FloorMap, FloorModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FloorMapId))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, GlobalSettings.MapsFolder.Replace("~", "") + src.ImagePath)))
                .ForMember(dest => dest.Image, opt => opt.ResolveUsing((src) =>
                {
                    if (string.IsNullOrEmpty(src.ImagePath)) 
                    {
                        return null;
                    }

                    string filename = HttpContext.Current.Server.MapPath(Path.Combine(GlobalSettings.MapsFolder, src.ImagePath));
                    if (!File.Exists(filename))
                    {
                        return null;
                    }

                    var image = Image.FromFile(filename);

                    return image.ImageToBase64(ImageFormat.Png);
                })); 

            Mapper.CreateMap<FloorModel, FloorMap>()
                .ForMember(dest => dest.FloorMapId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Coordinates, opt => opt.Ignore())
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForMember(dest => dest.ImageFolder, opt => opt.Ignore());
                
            Mapper.AssertConfigurationIsValid();        
        }
    }

    public class EnumToStringConverter<TSource> : TypeConverter<TSource, string>
        where TSource : struct
    {
        protected override string ConvertCore(TSource source)
        {
            return ((Enum)(object)source).ToDescription();
        }
    }

    public class NullableEnumToStringConverter<TSource> : TypeConverter<TSource?, string>
        where TSource : struct
    {
        protected override string ConvertCore(TSource? source)
        {
            if (!source.HasValue)
                return string.Empty;
            return ((Enum)(object)source.Value).ToDescription();
        }
    }

    public class NullableTypeToStringConverter<TSource> : TypeConverter<TSource?, string>
        where TSource : struct, IFormattable
    {
        private readonly string _formatString;

        public NullableTypeToStringConverter()
        {
            _formatString = string.Empty;
        }

        public NullableTypeToStringConverter(string formatString)
        {
            _formatString = formatString;
        }

        protected override string ConvertCore(TSource? source)
        {
            return source.HasValue ? source.Value.ToString(_formatString, CultureInfo.CurrentUICulture) : string.Empty;
        }
    }

    public class EnumerableEnumToStringConverter<TSource> : TypeConverter<IEnumerable<TSource>, string>
        where TSource : struct
    {
        private readonly string _separator;

        public EnumerableEnumToStringConverter()
        {
            _separator = ", ";
        }

        public EnumerableEnumToStringConverter(string separator)
        {
            _separator = separator;
        }

        protected override string ConvertCore(IEnumerable<TSource> source)
        {
            return string.Join(_separator, source.Select(s => ((Enum)(object)s).ToDescription()));
        }
    }

    public class EnumerableTypeToStringConverter<TSource> : TypeConverter<IEnumerable<TSource>, string>
        where TSource : class
    {
        private readonly string _separator;

        public EnumerableTypeToStringConverter()
        {
            _separator = ", ";
        }

        public EnumerableTypeToStringConverter(string separator)
        {
            _separator = separator;
        }

        protected override string ConvertCore(IEnumerable<TSource> source)
        {
            return string.Join(_separator, source.Select(s => s.ToString()));
        }
    }

    public class BoolToYesNoConverter : TypeConverter<bool, string>
    {
        protected override string ConvertCore(bool source)
        {
            return source ? Contents.Yes : Contents.No;
        }
    }
}