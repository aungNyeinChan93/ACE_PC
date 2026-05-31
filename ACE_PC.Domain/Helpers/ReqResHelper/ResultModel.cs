using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace ACE_PC.Domain.Helpers.ReqResHelper
{
    public class ResultModel<T>
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public EnumResponseType ResponseType { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsError { get { return !IsSuccess; } }
        public T? Data { get; set; }
        public static ResultModel<T> Success(int resCode, string resMess, T? data = default)
        {
            return new ResultModel<T>
            {
                ResponseCode = resCode,
                IsSuccess = true,
                ResponseMessage = resMess,
                ResponseType = EnumResponseType.Success,
                Data = data
            };
        }
        public static ResultModel<T> Pending(int resCode, string resMess, T? data = default)
        {
            return new ResultModel<T>
            {
                ResponseCode = resCode,
                IsSuccess = false,
                ResponseMessage = resMess,
                ResponseType = EnumResponseType.Pending,
                Data = data
            };
        }
        public static ResultModel<T> ValidationError(int resCode, string resMess, T? data = default)
        {
            return new ResultModel<T>
            {
                ResponseCode = resCode,
                IsSuccess = false,
                ResponseMessage = resMess,
                ResponseType = EnumResponseType.ValidationError,
                Data = data
            };
        }

        public static ResultModel<T> SystemError(int resCode, string resMess, T? data = default)
        {
            return new ResultModel<T>
            {
                ResponseCode = resCode,
                IsSuccess = false,
                ResponseMessage = resMess,
                ResponseType = EnumResponseType.SystemError,
                Data = data
            };
        }
    }

    public enum EnumResponseType
    {
        None,
        Success,
        Pending,
        ValidationError,
        SystemError
    }
}
