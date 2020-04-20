using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class EnumCodeTable : HashCodeTable
    {
        public EnumCodeTable(Type type)
        {
            DataList = new Dictionary<string, HashCodeDataModel>();
            //string key = "SELECTOR_ENUM";
            // HasCache = true;
           
            var arrs = Enum.GetValues(type);
            int count = arrs.Length;
            foreach (var en in arrs)
            {
                if (en.Equals(arrs.GetValue(count - 1)))
                {
                    var cdm = new HashCodeDataModel()
                    {
                        CodeValue = en.Value<int>().ToString(),
                        CodeText = en.GetDescription(),
                        CodeName = en.ToString(),
                    };
                    DataList.Add(cdm.CodeValue, cdm);
                }
                else
                {
                    if (en.Equals(arrs.GetValue(0)))
                    {
                        var cdm2 = new HashCodeDataModel()
                        {
                            CodeValue = en.Value<int>().ToString(),
                            CodeText = en.GetDescription(),
                            CodeName = en.ToString(),
                        };
                        DataList.Add(cdm2.CodeValue, cdm2);
                    }
                    else
                    {
                        var cdm = new HashCodeDataModel()
                        {
                            CodeValue = en.Value<int>().ToString(),
                            CodeText = en.GetDescription(),
                            CodeName = en.ToString(),
                        };
                        DataList.Add(cdm.CodeValue, cdm);
                    }
                }
                // AppContext.Current.Cache.Set<EnumCodeDataModel>(KEY_NAME + en.Value<int>().ToString(), cdm);
                //  AppContext.Current.Cache.Set<CodeDataModel>(key + en, new CodeDataModel() {  CODE_TEXT});

            }
            //AppContext.Current.Cache.Set<CodeDataModel>(, DataList);
        }

        public override IEnumerable<CodeDataModel> FillData(DataSet postDataSet)
        {
            foreach (var item in DataList)
            {
                if (item.Key != "0" || item.Value.CodeName.ToUpper() != "NONE")
                {
                    yield return item.Value;
                }
                else
                {
                    continue;
                }
            }
        }

        public override IEnumerable<CodeDataModel> FillAllData(DataSet postDataSet)
        {
            foreach (var item in DataList)
            {
                if (item.Key != "0" || item.Value.CodeName.ToUpper() != "NONE")
                {
                    yield return item.Value;
                }
                else
                {
                    continue;
                }
            }
        }

    }
}
