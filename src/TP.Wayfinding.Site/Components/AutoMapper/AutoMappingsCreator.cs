using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using TP.Wayfinding.Domain;
using TP.Wayfinding.Resources;
using TP.Wayfinding.Site.Components.Extensions;
using TP.Wayfinding.Site.Components.Settings;
using TP.Wayfinding.Site.Models.Building;
using TP.Wayfinding.Site.Models.Connection;
using TP.Wayfinding.Site.Models.Device;
using TP.Wayfinding.Site.Models.Floor;
using TP.Wayfinding.Site.Models.Node;
using TP.Wayfinding.Site.Models.Office;
using TP.Wayfinding.Site.Models.OfficeType;
using TP.Wayfinding.Site.Models.Person;

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

            Mapper.CreateMap<Building, BuildingModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BuildingId));
            Mapper.CreateMap<BuildingModel, Building>()
                .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FloorMaps, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore());

            Mapper.CreateMap<Office, OfficeModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OfficeId));
            Mapper.CreateMap<OfficeModel, Office>()
                .ForMember(dest => dest.OfficeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Detail, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.FloorMap, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            Mapper.CreateMap<OfficeType, OfficeTypeModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OfficeTypeId));
            Mapper.CreateMap<OfficeTypeModel, OfficeType>()
                .ForMember(dest => dest.OfficeTypeId, opt => opt.MapFrom(src => src.Id));

            Mapper.CreateMap<FloorMap, FloorListModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FloorMapId))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, GlobalSettings.MapsFolder.Replace("~", "") + src.ImagePath)));
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

            Mapper.CreateMap<Node, NodeModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NodeId));
            Mapper.CreateMap<NodeModel, Node>()
                .ForMember(dest => dest.NodeId, opt => opt.MapFrom(src => src.Id));

            Mapper.CreateMap<Connection, ConnectionModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ConnectionId));
            Mapper.CreateMap<ConnectionModel, Connection>()
                .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.Id));

            Mapper.CreateMap<Device, DeviceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeviceId));
            Mapper.CreateMap<DeviceModel, Device>()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LastBatteryStatus, opt => opt.Ignore())
                .ForMember(dest => dest.LastTick, opt => opt.Ignore());

            Mapper.CreateMap<Person, PersonModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.ImagePath,
                    opt => opt.MapFrom(src => string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority,
                                    GlobalSettings.PeopleFolder.Replace("~", "") + src.ImagePath)));
            Mapper.CreateMap<PersonModel, Person>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OfficeNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ImagePath,
                    opt => opt.MapFrom(src => string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Authority,
                                    GlobalSettings.PeopleFolder.Replace("~", "") + src.ImagePath)));

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