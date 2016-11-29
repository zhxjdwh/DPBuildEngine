using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildTest.Config;

namespace BuildTest.Uow
{
    public static class UnitOfWork
    {
        internal class UnitOfWorlImpl : IUnitOfWork
        {
            /// <summary>
            /// 线程上所有工作单元集合
            /// </summary>
            private static ThreadLocal<Stack<UnitOfWork.UnitOfWorlImpl>> UnitOfWorks { get; set; }
                = new ThreadLocal<Stack<UnitOfWork.UnitOfWorlImpl>>(() => new Stack<UnitOfWork.UnitOfWorlImpl>());

            private DbTransaction _trans;
            /// <summary>
            /// 工作单元所关联的事务
            /// </summary>
            internal DbTransaction DbTransaction
            {
                get { return _trans; }
                set { _trans = value; }
            }

            /// <summary>
            /// 父级工作单元
            /// </summary>
            public UnitOfWorlImpl Parent { get; private set; }

            /// <summary>
            /// 工作单元类型
            /// </summary>
            public UowType Type { get; private set; }

            /// <summary>
            /// 当前工作单元
            /// </summary>
            public static UnitOfWorlImpl Current
            {
                get
                {
                    UnitOfWork.UnitOfWorlImpl unitOfWork = null;
                    if (UnitOfWorks.Value.Count > 0)
                    {
                        unitOfWork = UnitOfWorks.Value.Peek();
                    }

                    return unitOfWork;
                }
            }

            public bool IsProccessed { get; private set; } = false;


            public UnitOfWorlImpl():this(UowType.RequireNew)
            {
                
            }
            public UnitOfWorlImpl(UowType type)
            {
                Type = type;
                UnitOfWorks.Value.Push(this);
            }

            /// <summary>
            /// 回滚事务,并从栈上移除uow
            /// </summary>
            public void Cancel()
            {
                try
                {
                    if (UnitOfWorks.Value.Count > 0)
                    {
                        var work = UnitOfWorks.Value.Peek();
                        if (work == this)
                        {
                            UnitOfWorks.Value.Pop();
                            IsProccessed = true;
                            if (_trans != null)
                            {
                                _trans.Rollback();
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("illeagl operation");
                        }
                    }
                    
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (_trans != null)
                    {
                        if (_trans.Connection != null && _trans.Connection.State != ConnectionState.Closed)
                        {
                            _trans.Connection.Close();
                        }
                        _trans.Dispose();
                    }
                }
            }

            /// <summary>
            /// 提交事务,并从栈上移除uow
            /// </summary>
            public void Commit()
            {
                try
                {
                    if (UnitOfWorks.Value.Count>0)
                    {
                        var work = UnitOfWorks.Value.Peek();
                        if (work == this)
                        {
                            UnitOfWorks.Value.Pop();
                            IsProccessed = true;
                            if (_trans != null)
                            {
                                _trans.Commit();
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("illeagl operation");
                        }
                    }
                    
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (_trans != null)
                    {
                        if (_trans.Connection != null && _trans.Connection.State != ConnectionState.Closed)
                        {
                            _trans.Connection.Close();
                        }
                        _trans.Dispose();
                    }
                }
            }

            /// <summary>
            /// 回滚事务,并从栈上移除一个uow
            /// </summary>
            public void Dispose()
            {
                if (!IsProccessed)
                {
                    this.Cancel();
                }
            }
        }

        /// <summary>
        /// 开启一个新的工作单元,使用UowType.RequireNew方式
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork Begin()
        {
            return UnitOfWork.Begin(UowType.RequireNew);
        }

        /// <summary>
        /// 开启一个工作单元
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IUnitOfWork Begin(UowType type)
        {
            var work = new UnitOfWorlImpl(type);
            return work;
        }

        /// <summary>
        /// 当前的工作单元
        /// </summary>
        public static IUnitOfWork Current
        {
            get { return UnitOfWorlImpl.Current; }
        }
    }
}
