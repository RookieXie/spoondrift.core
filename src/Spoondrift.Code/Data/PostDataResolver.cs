using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    public interface IPostDataResolver: IDisposable
    {
        DataSet PostDataSet { get; set; }
        void InsertForeach(ObjectData data, DataRow row, string key);
        void UpdateForeach(ObjectData data, DataRow row, string key);
        void DeleteForeach(string key, string data);
        
        void Merge(bool isBat);
    }
}
