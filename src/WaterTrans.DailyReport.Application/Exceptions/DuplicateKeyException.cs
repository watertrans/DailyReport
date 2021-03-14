using System;
using System.Runtime.Serialization;

namespace WaterTrans.DailyReport.Application.Exceptions
{
    /// <summary>
    /// プライマリーキーが重複した場合に生成する例外
    /// </summary>
    public class DuplicateKeyException : Exception, ISerializable
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DuplicateKeyException()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">エラーメッセージ。</param>
        public DuplicateKeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">エラーメッセージ。</param>
        /// <param name="inner">内部例外オブジェクト。</param>
        public DuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected DuplicateKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
