using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDBMaps.Models.Mapping;

namespace IDBMaps.Import
{
    class Program
    {
        static void Main(string[] args)
        {

            DxfMapExtractor.ImportOfficeLocations();
        }

//        --update Office set TypeId=0 where OfficeNumber in 
//('NE0303', 
//'NE0313',
//'NE0231',
//'NE0241',
//'SE0280',
//'NE0222',
//'SW0229',
//'SW0327',
//'NW0381',
//'NW0309',
//'SE0367',
//'NE0314',
//'NE0427',
//'NE0418',
//'NE0413',
//'NE0427',
//'NE0469E',
//'SE0405',
//'NE0469J',
//'SE0416',
//'SE0446',
//'NW0405',
//'NW0469',
//'SW0471',
//'SW0467',
//'SW0471',
//'NW0569',
//'SE0571',
//'NE0507B',
//'SE0507',
//'NE0512',
//'NW0607',
//'NW0616',
//'SE0661',
//'SE0605',
//'NE0620',
//'NE0625',
//'NE0753',
//'SE0763',
//'NE0719',
//'NE0857',
//'SE0859',
//'SE0822',
//'NE0820',
//'NE0922',
//'NE0919',
//'NE0942',
//'SE0933',
//'SW0945',
//'NW0935',
//'NE1069',
//'SE1057',
//'SE1035',
//'SE1039',
//'NW1035',
//'NW1028',
//'NW1007',
//'SW1055',
//'NW0935',
//'NE1069',
//'SW1055',
//'SW1000',
//'SE1020',
//'NW1035',
//'NE1101A',
//'NE1137F',
//'NE1131A',
//'NE1125A',
//'SE1119E',
//'SE1131E',
//'SE1101A',
//'NW1101A',
//'SW1125G',
//'SW1119',
//'SW1102',
//'SE1107G',
//'NE1204B',
//'NW1202D',
//'NW1200C',
//'NW1235',
//'NW1251',
//'SW1233',
//'NE1222',
//'NE1231',
//'NE1285',
//'NE1200J',
//'NE1250A')

    }
}
