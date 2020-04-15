using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.Data
{
    public abstract class ListDataTable : IListDataTable, IDisposable
    {
        public const string PAGE_SYS = "PAGE_SYS";
        public const string KEYVALUE = "KeyValue";
        public string CodePlugName { get; set; }
        private string fRegName;
        // private string fDataXmlPath;
        //private DataBaseConfig fDataBase;

        public List<string> UniqueList
        {
            get;
            set;
        }

        public List<ColumnConfig> FullColumns
        {
            get;
            set;
        }

        public string Order
        {
            get;
            set;
        }

        public bool IsFillEmpty
        {
            get;
            set;
        }

        public abstract Type ObjectType
        {
            get;
            set;
        }

        public virtual string ForeignKey
        {
            get;
            set;
        }

        public DataFormConfig DataFormConfig
        {
            get;
            set;
        }

        public HashSet<string> ColumnLegalHashTable
        {
            get;
            set;
        }

        public PageStyle PageStyle
        {
            get;
            set;
        }

        public FormConfig ModuleFormConfig { get; set; }

        public virtual ColumnKind GetColumnKind(string feildName)
        {
            return DataFormConfig.Columns.Where(a => a.Name == feildName).FirstOrDefault().Kind;
        }

        public virtual void setGroupShow(DataTable dt, List<string> groupColumnList)
        {
            Dictionary<string, DataRow> _dict = new Dictionary<string, DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                foreach (string col in groupColumnList)
                {
                    if (dt.Columns.Contains(col))
                    {
                        this.setColGroup(dt, "_" + col + "_group_");

                        if (i == 0)
                        {

                            row["_" + col + "_group_"] = 1;
                            _dict[col] = row;
                        }
                        else
                        {
                            var _lastVal = dt.Rows[i - 1][col];
                            if (_lastVal.Equals(row[col]))
                            {
                                row["_" + col + "_group_"] = dt.Rows[i - 1]["_" + col + "_group_"].Value<int>() + 1;
                                if (i == dt.Rows.Count - 1)
                                {
                                    this.setColGroup(dt, "_" + col + "_group_max_");
                                    row["_" + col + "_group_max_"] = row["_" + col + "_group_"];
                                }
                                string _groupKey = "_" + col + "_group_key_";
                                this.setColGroup(dt, _groupKey);

                                row[_groupKey] = _dict[col][_dict[col].Table.PrimaryKey[0].ColumnName];
                            }
                            else
                            {

                                if (_dict.ContainsKey(col))
                                {
                                    this.setColGroup(dt, "_" + col + "_group_max_");
                                    _dict[col]["_" + col + "_group_max_"] = dt.Rows[i - 1]["_" + col + "_group_"];

                                }
                                _dict[col] = row;

                                row["_" + col + "_group_"] = 1;
                            }

                        }
                    }
                }
            }

        }

        private void setColGroup(DataTable dt, string colName)
        {
            if (!dt.Columns.Contains(colName))
            {
                dt.Columns.Add(colName);
            }
        }


        public virtual IEnumerable<ObjectData> List
        {
            get;
            set;
        }

        public virtual List<string> GroupColList
        {
            get;
            set;
        }

        public virtual void AppendTo(DataSet ds)
        {
            var list = List.ToList();
            if (list != null && list.Count > 0)
            {
               
                DataTable table = list.ToDataTable<ObjectData>();
                table.PrimaryKey = new DataColumn[] { table.Columns[PrimaryKey] };
                if (!string.IsNullOrEmpty(RegName))
                    table.TableName = RegName;
                ds.Tables.Add(table);

                if (this.GroupColList != null && this.GroupColList.Count > 0)
                {
                    this.setGroupShow(table, this.GroupColList);
                }
            }
        }


        public virtual string RegName
        {
            get { return fRegName; } 
        }


        public abstract string PrimaryKey
        {
            get;
            set;
        }

        public DataSet PostDataSet
        {
            get;
            set;
        }

        public virtual void InsertForeach(ObjectData data, DataRow row, string key)
        {

        }

        public virtual void UpdateForeach(ObjectData data, DataRow row, string key)
        {

        }
        //Foreach

        public virtual void DeleteForeach(string key, string data)
        {

        }

        

        protected HashSet<string> XmlColumns
        {
            get;
            set;
        }
        //


        public virtual void MergeTable(bool isBat, bool isInsert)
        {
            //this.SubmitFilterEvent += ListDataTable_SubmitFilterEvent;
            //this.SubmitFilterEvent(null);
        }

        //SubmitData ListDataTable_SubmitFilterEvent(ResponseResult arg)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual void Merge(bool isBat)
        {
            DataTable dt = this.PostDataSet.Tables[RegName];
            var _editorColumns = DataFormConfig.Columns.FindAll(a => a.ControlType == ControlType.Editor);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _editorColumns.ForEach(
                         a =>
                         {
                             if (dt.Columns.Contains(a.Name))
                             {
                                 dt.Rows[i][a.Name] = StringUtil.HexToString(dt.Rows[i][a.Name].ToString());
                             }
                         }
                         );
                }
            }
            var sysDt = this.PostDataSet.Tables[PAGE_SYS]; //该表存放主从表的主表主键值
            if (sysDt != null)
                ForeignKeyValue = sysDt.Rows[0][KEYVALUE].ToString();



            List<ObjectDataView> InsertDataViewList = new List<ObjectDataView>();
            List<ObjectDataView> UpdateDataViewList = new List<ObjectDataView>();
            List<string> DeleteStringList = new List<string>();

            if (dt != null)
            {
                this.ObjectType = this.ObjectType ?? typeof(ObjectData);
                List<ObjectData> list = DataSetUtil.FillModel(dt, this.ObjectType, XmlColumns);
                foreach (ObjectData objectData in list)
                {
                    DataRow row = objectData.Row;
                    // DataFormConfig.Columns.Find(a=>a.Name == 
                    row.BeginEdit();

                    bool isInsert = !row.Table.Columns.Contains(PrimaryKey) || (row.Table.Columns.Contains(PrimaryKey) && row[PrimaryKey].Value<string>().IsEmpty());
                    MergeTable(isBat, isInsert);
                    if (isInsert)
                    {
                        string _insertKey = GetInsertKey(objectData);
                        //if (string.IsNullOrEmpty(ForeignKey))
                        //{
                        //    var insertKeys = AtawAppContext.Current.PageFlyweight.PageItems["InsertKeys"] as List<string>;
                        //    if (insertKeys == null)
                        //    {
                        //        AtawAppContext.Current.PageFlyweight.PageItems.Add("InsertKeys", new List<string> { _insertKey });
                        //    }
                        //    else
                        //    {
                        //        insertKeys.Add(_insertKey);
                        //    }
                        //}
                        SetPostDataRow(objectData, DataAction.Insert, _insertKey);
                        InsertForeach(objectData, row, _insertKey);
                        InsertDataViewList.Add(new ObjectDataView()
                        {
                            KeyId = _insertKey,
                            objectData = objectData
                        });
                    }
                    else
                    {
                        string key = row[PrimaryKey].Value<string>();
                        SetPostDataRow(objectData, DataAction.Update, key);
                        UpdateForeach(objectData, row, key);

                        UpdateDataViewList.Add(
                            new ObjectDataView()
                            {
                                KeyId = key,
                                objectData = objectData
                            }
                            );
                    }
                    row.EndEdit();
                }
            }
            DataTable opdt = this.PostDataSet.Tables[RegName + "_OPERATION"];
            if (opdt != null && opdt.Rows.Count > 0)
            {
                foreach (DataRow row in opdt.Rows)
                {
                    string _key = row["KeyValue"].ToString();
                    if (row["OperationName"].ToString() == "Delete")
                    {
                        DeleteForeach(_key, "");
                        DeleteStringList.Add(_key);
                    }

                }
            }
            bool isCheck = CheckPostData(InsertDataViewList, UpdateDataViewList, DeleteStringList);
            //AtawDebug.Assert(isCheck, "数据源插件{0}，的提交数据验证不通过".AkFormat(RegName), this);
        }

        public virtual bool CheckPostData(List<ObjectDataView> insertDataViewList, List<ObjectDataView> updateDataViewList, List<string> deleteStringList)
        {
            return true;
        }

        public virtual void Initialize(ModuleFormInfo info)
        {
            Order = info.Order;
           // fDataBase = info.DataBase;
            ModuleFormConfig = info.ModuleFormConfig;
            FullColumns = info.FullColumns;
            InternalInitialize(info.DataSet, info.PageSize, info.KeyValue, info.ForeignKeyValue, info.TableName, info.PrimaryKey, info.ForeignKey, info.IsFillEmpty, info.DataFormConfig);
        }

        private void InternalInitialize(DataSet dataSet, int pageSize, string keyValue, string foreignKeyValue, string tableName, string primaryKey, string foreignKey, bool isFillEmpty, DataFormConfig dataFormConfig)
        {
            // ColumnLegalHashTable = new HashSet<string>();
            this.DataFormConfig = dataFormConfig;

            UniqueList = this.DataFormConfig.Columns.Where(a => a.IsUniqueKey).Select(a => a.Name).ToList();
            //SingleUploadColumns = this.DataFormConfig.Columns.FindAll(
            //    a => (a.ControlType == ControlType.SingleImageUpload || a.ControlType == ControlType.SingleFileUpload) && (a.Upload != null && a.Upload.HasKey)).ToList();
            //MultiUploadColumns = this.DataFormConfig.Columns.FindAll(
            //    a => (a.ControlType == ControlType.MultiImageUpload || a.ControlType == ControlType.MultiFileUpload) && (a.Upload != null && a.Upload.HasKey)).ToList();
            MomeryColumns = this.DataFormConfig.Columns.FindAll(
                a => (a.ControlType == ControlType.Momery && !a.RegName.IsEmpty())
               ).ToList();
            this.KeyValues = new List<string>();
            this.IsFillEmpty = isFillEmpty;
            this.PostDataSet = dataSet;
            this.KeyValue = keyValue;
            if (dataSet != null)
            {
                var _dtPageSys = dataSet.Tables["PAGE_SYS"];

                if (_dtPageSys != null && _dtPageSys.Rows.Count > 0)
                {
                    if (_dtPageSys.Columns.Contains("PageStyle"))
                    {
                        this.PageStyle = _dtPageSys.Rows[0]["PageStyle"].Value<PageStyle>();
                    }
                }

                var dt = dataSet.Tables["_KEY"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> keyValueList = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string _key = row["KeyValue"].ToString();
                        keyValueList.Add(_key);
                    }
                    KeyValues = keyValueList;
                }
            }

            // if (dataSet.Tables["PAGE"])
            if (RegName.IsEmpty())
            {
                fRegName = tableName;
            }
            if (PrimaryKey.IsEmpty())
            {
                PrimaryKey = primaryKey;
            }
            if (ForeignKey.IsEmpty())
                ForeignKey = foreignKey;
            if (!ForeignKey.IsEmpty())
            {
                if (foreignKeyValue.IsEmpty())
                {
                    this.ForeignKeyValue = KeyValues.Count() == 1 ? KeyValues.First() : foreignKeyValue;
                }
                else
                    this.ForeignKeyValue = foreignKeyValue;
            }
            if (dataSet != null)
            {
                this.Pagination = new Pagination().FormDataTable(dataSet.Tables["PAGER"]);

            }
            else
                this.Pagination = new Pagination()
                {
                    //TableName = RegName,
                    DataTime = DateTime.Now,
                    PageSize = pageSize
                };
            this.Pagination.TableName = RegName;
            if (this.Pagination.PageSize == 0)
            {
                this.Pagination.PageSize = pageSize;
            }
            if (this.Pagination.PageSize == 0)
            {
                this.Pagination.PageSize = 20;
            }
        }



        public Pagination Pagination
        {
            get;
            set;
        }


        public string KeyValue
        {
            get;
            set;
        }

        public IEnumerable<string> KeyValues
        {
            get;
            set;
        }

        public string ForeignKeyValue
        {
            get;
            set;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (PostDataSet != null)
                    PostDataSet.Dispose();
            }
            //base.Dispose(disposing);
        }





        public void AppendToEmptyRow(DataSet ds)
        {
            var list = new List<ObjectData>();
            list.Add(new ObjectData());
            if (list != null && list.Count > 0)
            {
                var _type = list[0].GetType();
                DataTable table =list.ToDataTable<ObjectData>();
                table.PrimaryKey = new DataColumn[] { table.Columns[PrimaryKey] };
                if (!string.IsNullOrEmpty(RegName))
                    table.TableName = RegName;
                ds.Tables.Add(table);
            }
        }


        //public virtual DataBaseConfig DataBase
        //{
        //    get
        //    {
        //        return fDataBase;
        //    }
        //}

        // private void FileS
        private List<ColumnConfig> SingleUploadColumns
        {
            get;
            set;
        }

        private List<ColumnConfig> MultiUploadColumns
        {
            get;
            set;
        }

        private List<ColumnConfig> MomeryColumns
        {
            get;
            set;
        }

        ///// <summary>
        ///// 若一条记录里有多个单上传控件字段，则每个上传控件的文件名和主键需保持一致
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="fileStorageName"></param>
        ///// <returns></returns>
        //protected string GetSingleResourceInfo(string path, string fileStorageName)
        //{
        //    if (!path.IsEmpty())
        //    {
        //        var pathInfo = path.Split('&');
        //        string fileName = pathInfo[0].Substring(pathInfo[0].LastIndexOf('/') + 1);
        //        string fileID = fileName.Split('.')[0];
        //        string pathID = fileName.Substring(0, 8);
        //        string fileExt = Path.GetExtension(fileName);
        //        if (FileID.IsEmpty())
        //        {
        //            FileID = fileID;
        //        }
        //        else
        //        {
        //            //重命名
        //            string fullPath = new Uri(Path.Combine(FileManagementUtil.GetRootPath(fileStorageName, FilePathScheme.Physical), path)).LocalPath;
        //            string dir = Path.GetDirectoryName(fullPath);
        //            string newPath = new Uri(Path.Combine(dir, "\\" + FileID + fileExt)).LocalPath;
        //            File.Move(fullPath, newPath);
        //            File.Delete(fullPath);
        //            fileID = FileID;
        //        }
        //        var resourceInfo = new ResourceInfo();
        //        resourceInfo.InfoType = ResourceInfoType.Config;
        //        resourceInfo.FileId = fileID;
        //        resourceInfo.PathID = pathID.Value<int>();
        //        resourceInfo.ExtName = fileExt;
        //        resourceInfo.FileNameTitle = pathInfo[1];
        //        resourceInfo.FileSizeK = pathInfo[2].Value<int>();
        //        resourceInfo.StorageConfigName = fileStorageName;
        //        return AtawAppContext.Current.FastJson.ToJSON(resourceInfo);

        //    }
        //    return "";
        //}

        //protected string GetMultiResourceInfo(string pathList, string fileStorageName)
        //{

        //    //string fileExtension = Path.GetExtension(fileName);
        //    //string fileID = AtawAppContext.Current.UnitOfData.GetUniId();
        //    //int pathID = fileID.Substring(0, 8).Value<int>();
        //    //string relativePath = Path.Combine(FileManagementUtil.GetRelativePath(fileStorageName, pathID),
        //    //    FileManagementUtil.GetFileName(fileStorageName, fileID, fileExtension));
        //    //string fullPath = new Uri(Path.Combine(FileManagementUtil.GetRootPath(fileStorageName, FilePathScheme.Physical), relativePath)).LocalPath;
        //    //FileManagementUtil.ForeDirectories(FileManagementUtil.GetParentDirectory(fullPath));
        //    //string tempfile = Path.Combine(AtawAppContext.Current.MapPath, Callback.Src(tempPath));

        //    //if (File.Exists(tempfile))
        //    //{
        //    //    File.Copy(tempfile, fullPath, false);
        //    //    File.Delete(tempfile);
        //    //}
        //    if (pathList.IsEmpty())
        //        return "";
        //    var pathArr = pathList.Split(',');
        //    string fileName = "";
        //    List<ResourceInfo> infoList = new List<ResourceInfo>();
        //    pathArr.ToList().ForEach(a =>
        //    {
        //        if (!a.IsEmpty())
        //        {
        //            var path = a.Split('&');
        //            fileName = path[0].Substring(path[0].LastIndexOf('\\') + 1);
        //            string fileID = fileName.Split('.')[0];
        //            string pathID = fileName.Substring(0, 8);
        //            string fileExt = Path.GetExtension(fileName);
        //            var resourceInfo = new ResourceInfo();
        //            resourceInfo.InfoType = ResourceInfoType.Config;
        //            resourceInfo.FileId = fileID;
        //            resourceInfo.PathID = pathID.Value<int>();
        //            resourceInfo.ExtName = fileExt;
        //            resourceInfo.FileNameTitle = path[1];
        //            resourceInfo.FileSizeK = path[2].Value<int>();
        //            resourceInfo.StorageConfigName = fileStorageName;
        //            infoList.Add(resourceInfo);
        //        }
        //    });
        //    //var pathInfo = new FilePathInfo { PathID = pathID.ToString(), FileID = fileID, FileExtension = fileExtension };
        //    return AtawAppContext.Current.FastJson.ToJSON(infoList);
        //}

        public virtual void SetPostDataRow(ObjectData data, DataAction dataAction, string key)
        {
            // throw new NotImplementedException();
            // if(data.)
            if (SingleUploadColumns != null && SingleUploadColumns.Count > 0)
            {
                SingleUploadColumns.ForEach(a =>
                {
                    //if (data.MODEFY_COLUMNS.Contains(a.Name))
                    //{
                    //    string storange = a.Upload.StorageName;
                    //    string fpath = data.Row[a.Name].ToString();
                    //    if (!fpath.IsEmpty())
                    //    {
                    //        ResourceArrange arrange = AtawAppContext.Current.FastJson.ToObject<ResourceArrange>(fpath);
                    //        if (arrange != null)
                    //        {
                    //            arrange.MoveKeyPath(key, storange);
                    //            data.Row[a.Name] = AtawAppContext.Current.FastJson.ToJSON(arrange);
                    //        }
                    //    }
                    //    //data.Row[a.Name] = GetSingleResourceInfo(fpath, a.Upload.StorageName);
                    //}
                });
            }

            if (MultiUploadColumns != null && MultiUploadColumns.Count > 0)
            {
                MultiUploadColumns.ForEach(a =>
                {
                    //if (data.MODEFY_COLUMNS.Contains(a.Name))
                    //{
                    //    string storange = a.Upload.StorageName;
                    //    string fpath = data.Row[a.Name].ToString();
                    //    if (!fpath.IsEmpty())
                    //    {
                    //        ResourceArrange arrange = AtawAppContext.Current.FastJson.ToObject<ResourceArrange>(fpath);
                    //        arrange.MoveKeyPath(key, storange);
                    //        if (arrange != null)
                    //        {
                    //            data.Row[a.Name] = AtawAppContext.Current.FastJson.ToJSON(arrange);
                    //        }
                    //    }
                    //    // data.Row[a.Name] = GetMultiResourceInfo(fpath, a.Upload.StorageName);
                    //}
                });
            }
            if (MomeryColumns != null && MomeryColumns.Count > 0)
            {
                MomeryColumns.ForEach(a =>
                {
                    if (data.MODEFY_COLUMNS.Contains(a.Name))
                    {
                        //string _regname = a.RegName;
                        //IMomery rr = AtawIocContext.Current.FetchInstance<IMomery>(_regname);
                        //rr.AddText(data.Row[a.Name].ToString(), AtawAppContext.Current.UnitOfData);

                    }

                });

            }
        }

        ///// <summary>
        ///// 当前的新增行中，若存在字段控件类型为单文件/图片上传控件时，该值为当前行的主键值和上传文件的文件名，该行新增后，需要清空
        ///// </summary>
        //protected string FileID { get; set; }






        public abstract string GetInsertKey(ObjectData data);

       

        //private Func<SubmitData, SubmitData> fSubmitFilterFun = (a) => a;

        //public virtual Func<SubmitData, SubmitData> SubmitFilterFun
        //{
        //    get
        //    {
        //        return fSubmitFilterFun;
        //    }
        //}
       
    }
}
