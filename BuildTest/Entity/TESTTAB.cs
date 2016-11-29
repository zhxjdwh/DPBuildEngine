using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Attribute;

namespace BuildTest.Entity
{
    /// <summary>
    /// TESTTAB
    /// </summary>
    [Table("TESTTAB"),Instance("testdb")]
    public class TESTTAB //: Dragonpass.Model.CommonModel.BaseEntity
    {
        #region auto generate property

        private string _fd_agreementno = default(string);
        /// <summary>
        /// 协议号
        /// </summary>
        public string FD_AGREEMENTNO
        {
            set
            { 
                _fd_agreementno = value; 
                //_modifyList.Add("FD_AGREEMENTNO");
            }
            get{ return _fd_agreementno; }
        }
        
        private string _fd_appchannelid = default(string);
        /// <summary>
        /// 推送设备号
        /// </summary>
        public string FD_APPCHANNELID
        {
            set
            { 
                _fd_appchannelid = value; 
                //_modifyList.Add("FD_APPCHANNELID");
            }
            get{ return _fd_appchannelid; }
        }
        
        private string _fd_appuserid = default(string);
        /// <summary>
        /// 手机设备号
        /// </summary>
        public string FD_APPUSERID
        {
            set
            { 
                _fd_appuserid = value; 
                //_modifyList.Add("FD_APPUSERID");
            }
            get{ return _fd_appuserid; }
        }
        
        private string _fd_company = default(string);
        /// <summary>
        /// 所属企业(当FD_TYPE为2，3，4，5该字段代表对应的供应商/销售商ID）
        /// </summary>
        public string FD_COMPANY
        {
            set
            { 
                _fd_company = value; 
                //_modifyList.Add("FD_COMPANY");
            }
            get{ return _fd_company; }
        }
        
        private DateTime _fd_createtime = default(DateTime);
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime FD_CREATETIME
        {
            set
            { 
                _fd_createtime = value; 
                //_modifyList.Add("FD_CREATETIME");
            }
            get{ return _fd_createtime; }
        }
        
        private decimal _fd_createuserid = default(decimal);
        /// <summary>
        /// 创建人
        /// </summary>
        public decimal FD_CREATEUSERID
        {
            set
            { 
                _fd_createuserid = value; 
                //_modifyList.Add("FD_CREATEUSERID");
            }
            get{ return _fd_createuserid; }
        }
        
        private string _fd_department = default(string);
        /// <summary>
        /// 所属部门
        /// </summary>
        public string FD_DEPARTMENT
        {
            set
            { 
                _fd_department = value; 
                //_modifyList.Add("FD_DEPARTMENT");
            }
            get{ return _fd_department; }
        }
        
        private decimal _fd_disabledpay = default(decimal);
        /// <summary>
        /// 是否禁用支付（1为禁用,0为不禁用,票点中使用）
        /// </summary>
        public decimal FD_DISABLEDPAY
        {
            set
            { 
                _fd_disabledpay = value; 
                //_modifyList.Add("FD_DISABLEDPAY");
            }
            get{ return _fd_disabledpay; }
        }
        
        private string _fd_email = default(string);
        /// <summary>
        /// 邮箱
        /// </summary>
        public string FD_EMAIL
        {
            set
            { 
                _fd_email = value; 
                //_modifyList.Add("FD_EMAIL");
            }
            get{ return _fd_email; }
        }
        
        private string _fd_employeecode = default(string);
        /// <summary>
        /// 员工编号
        /// </summary>
        public string FD_EMPLOYEECODE
        {
            set
            { 
                _fd_employeecode = value; 
                //_modifyList.Add("FD_EMPLOYEECODE");
            }
            get{ return _fd_employeecode; }
        }
        
        private decimal _fd_id = default(decimal);
        /// <summary>
        /// 主键
        /// </summary>
        [Key,GenerateBy("SEQ_TD_USER",GeneratorType.Sequence)]
        public decimal FD_ID
        {
            set
            { 
                _fd_id = value; 
                //_modifyList.Add("FD_ID");
            }
            get{ return _fd_id; }
        }
        
        private decimal _fd_level = default(decimal);
        /// <summary>
        /// 用户级别(1管理员，2普通帐号)
        /// </summary>
        public decimal FD_LEVEL
        {
            set
            { 
                _fd_level = value; 
                //_modifyList.Add("FD_LEVEL");
            }
            get{ return _fd_level; }
        }
        
        private DateTime _fd_loaddate = default(DateTime);
        /// <summary>
        /// 最新登录时间
        /// </summary>
        public DateTime FD_LOADDATE
        {
            set
            { 
                _fd_loaddate = value; 
                //_modifyList.Add("FD_LOADDATE");
            }
            get{ return _fd_loaddate; }
        }
        
        private string _fd_loadip = default(string);
        /// <summary>
        /// 最新登录IP
        /// </summary>
        public string FD_LOADIP
        {
            set
            { 
                _fd_loadip = value; 
                //_modifyList.Add("FD_LOADIP");
            }
            get{ return _fd_loadip; }
        }
        
        private string _fd_memo = default(string);
        /// <summary>
        /// 描述
        /// </summary>
        public string FD_MEMO
        {
            set
            { 
                _fd_memo = value; 
                //_modifyList.Add("FD_MEMO");
            }
            get{ return _fd_memo; }
        }
        
        private string _fd_name = default(string);
        /// <summary>
        /// 用户名称
        /// </summary>
        public string FD_NAME
        {
            set
            { 
                _fd_name = value; 
                //_modifyList.Add("FD_NAME");
            }
            get{ return _fd_name; }
        }
        
        private string _fd_pwd = default(string);
        /// <summary>
        /// 登录密码
        /// </summary>
        public string FD_PWD
        {
            set
            { 
                _fd_pwd = value; 
                //_modifyList.Add("FD_PWD");
            }
            get{ return _fd_pwd; }
        }
        
        private string _fd_role = default(string);
        /// <summary>
        /// 角色
        /// </summary>
        public string FD_ROLE
        {
            set
            { 
                _fd_role = value; 
                //_modifyList.Add("FD_ROLE");
            }
            get{ return _fd_role; }
        }
        
        private decimal _fd_sellerid = default(decimal);
        /// <summary>
        /// 协议编号
        /// </summary>
        public decimal FD_SELLERID
        {
            set
            { 
                _fd_sellerid = value; 
                //_modifyList.Add("FD_SELLERID");
            }
            get{ return _fd_sellerid; }
        }
        
        private string _fd_sessionid = default(string);
        /// <summary>
        /// 
        /// </summary>
        public string FD_SESSIONID
        {
            set
            { 
                _fd_sessionid = value; 
                //_modifyList.Add("FD_SESSIONID");
            }
            get{ return _fd_sessionid; }
        }
        
        private decimal _fd_status = default(decimal);
        /// <summary>
        /// 启用状态(1启用,0禁用)
        /// </summary>
        public decimal FD_STATUS
        {
            set
            { 
                _fd_status = value; 
                //_modifyList.Add("FD_STATUS");
            }
            get{ return _fd_status; }
        }
        
        private string _fd_telephone = default(string);
        /// <summary>
        /// 手机号码
        /// </summary>
        public string FD_TELEPHONE
        {
            set
            { 
                _fd_telephone = value; 
                //_modifyList.Add("FD_TELEPHONE");
            }
            get{ return _fd_telephone; }
        }
        
        private decimal _fd_type = default(decimal);
        /// <summary>
        /// 用户类型（1系统后台2票点系统3企业门户  5餐厅,6休息室,7礼宾车调度系统,8零售店）
        /// </summary>
        public decimal FD_TYPE
        {
            set
            { 
                _fd_type = value; 
                //_modifyList.Add("FD_TYPE");
            }
            get{ return _fd_type; }
        }
        
        private DateTime _fd_updatetime = default(DateTime);
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime FD_UPDATETIME
        {
            set
            { 
                _fd_updatetime = value; 
                //_modifyList.Add("FD_UPDATETIME");
            }
            get{ return _fd_updatetime; }
        }
        
        private decimal _fd_updateuserid = default(decimal);
        /// <summary>
        /// 更新人
        /// </summary>
        public decimal FD_UPDATEUSERID
        {
            set
            { 
                _fd_updateuserid = value; 
                //_modifyList.Add("FD_UPDATEUSERID");
            }
            get{ return _fd_updateuserid; }
        }
        
        private string _fd_uselg = default(string);
        /// <summary>
        /// 上次登录使用语种
        /// </summary>
        public string FD_USELG
        {
            set
            { 
                _fd_uselg = value; 
                //_modifyList.Add("FD_USELG");
            }
            get{ return _fd_uselg; }
        }
        
        private string _fd_user = default(string);
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string FD_USER
        {
            set
            { 
                _fd_user = value; 
                //_modifyList.Add("FD_USER");
            }
            get{ return _fd_user; }
        }
        
        private string _fd_usernode = default(string);
        /// <summary>
        /// 用户级别（1是父账号，2是子账号）
        /// </summary>
        public string FD_USERNODE
        {
            set
            { 
                _fd_usernode = value; 
                //_modifyList.Add("FD_USERNODE");
            }
            get{ return _fd_usernode; }
        }
        
        private string _fd_workno = default(string);
        /// <summary>
        /// 工号
        /// </summary>
        public string FD_WORKNO
        {
            set
            { 
                _fd_workno = value; 
                //_modifyList.Add("FD_WORKNO");
            }
            get{ return _fd_workno; }
        }
        
        ///// <summary>
        ///// 序列
        ///// </summary>
        //public override string SEQ_NAME
        //{
        //    get{ return "SEQ_TESTTAB"; }
        //}

        ///// <summary>
        ///// 表名
        ///// </summary>
        //public override string TableName
        //{
        //    get { return "TESTTAB"; }
        //} 
        #endregion 
    }
}
