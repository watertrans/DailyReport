using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Net.Mime;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Web.Api.Resources;
using WaterTrans.DailyReport.Web.Api.ResponseObjects;

namespace WaterTrans.DailyReport.Web.Api.ObjectResults
{
    /// <summary>
    /// エラー応答のインスタンス作成ファクトリー
    /// </summary>
    public static class ErrorObjectResultFactory
    {
        /// <summary>
        /// ValidationErrorエラー応答を生成します。
        /// </summary>
        /// <param name="modelState"><see cref="ModelStateDictionary"/></param>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult ValidationError(ModelStateDictionary modelState)
        {
            var errors = new List<Error>();

            foreach (var keyValue in modelState)
            {
                foreach (var error in keyValue.Value.Errors)
                {
                    errors.Add(new Error
                    {
                        Code = ErrorCodes.ValidationErrorDetail,
                        Message = error.ErrorMessage,
                        Target = keyValue.Key.ToCamelCase(),
                    });
                }
            }

            return ValidationError(errors);
        }

        /// <summary>
        /// ValidationErrorエラー応答を生成します。
        /// </summary>
        /// <param name="error"><see cref="Error"/></param>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult ValidationError(Error error)
        {
            return ValidationError(new List<Error>() { error });
        }

        /// <summary>
        /// ValidationErrorエラー応答を生成します。
        /// </summary>
        /// <param name="errors"><see cref="List&lt;Error&gt;"/></param>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult ValidationError(List<Error> errors)
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.ValidationError,
                Message = ErrorMessages.ErrorResultValidationError,
                Details = errors,
            });
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            return result;
        }

        /// <summary>
        /// 単一のValidationErrorエラー応答を生成します。
        /// </summary>
        /// <param name="message">エラーメッセージ。</param>
        /// <param name="target">エラーターゲット。</param>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult ValidationErrorDetail(string message, string target)
        {
            return ValidationError(new Error
            {
                Code = ErrorCodes.ValidationErrorDetail,
                Message = message,
                Target = target,
            });
        }

        /// <summary>
        /// BadRequestエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult BadRequest()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.BadRequest,
                Message = ErrorMessages.ErrorResultBadRequest,
            });
            result.StatusCode = StatusCodes.Status400BadRequest;
            return result;
        }

        /// <summary>
        /// NoAuthorizationHeaderエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult NoAuthorizationHeader()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.NoAuthorizationHeader,
                Message = ErrorMessages.ErrorResultNoAuthorizationHeader,
            });
            result.StatusCode = StatusCodes.Status401Unauthorized;
            return result;
        }

        /// <summary>
        /// InvalidClientエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InvalidClient()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InvalidClient,
                Message = ErrorMessages.ErrorResultInvalidClient,
            });
            result.StatusCode = StatusCodes.Status401Unauthorized;
            return result;
        }

        /// <summary>
        /// InvalidGrantTypeエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InvalidGrantType()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InvalidGrantType,
                Message = ErrorMessages.ErrorResultInvalidGrantType,
            });
            result.StatusCode = StatusCodes.Status400BadRequest;
            return result;
        }

        /// <summary>
        /// InvalidScopeエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InvalidScope()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InvalidScope,
                Message = ErrorMessages.ErrorResultInvalidScope,
            });
            result.StatusCode = StatusCodes.Status400BadRequest;
            return result;
        }

        /// <summary>
        /// InvalidAuthorizationSchemeエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InvalidAuthorizationScheme()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InvalidAuthorizationScheme,
                Message = ErrorMessages.ErrorResultInvalidAuthorizationScheme,
            });
            result.StatusCode = StatusCodes.Status400BadRequest;
            return result;
        }

        /// <summary>
        /// InvalidAuthorizationTokenエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InvalidAuthorizationToken()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InvalidAuthorizationToken,
                Message = ErrorMessages.ErrorResultInvalidAuthorizationToken,
            });
            result.StatusCode = StatusCodes.Status401Unauthorized;
            return result;
        }

        /// <summary>
        /// AuthorizationTokenExpiredエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult AuthorizationTokenExpired()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.AuthorizationTokenExpired,
                Message = ErrorMessages.ErrorResultAuthorizationTokenExpired,
            });
            result.StatusCode = StatusCodes.Status401Unauthorized;
            return result;
        }

        /// <summary>
        /// Unauthorizedエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult Unauthorized()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.Unauthorized,
                Message = ErrorMessages.ErrorResultUnauthorized,
            });
            result.StatusCode = StatusCodes.Status401Unauthorized;
            return result;
        }

        /// <summary>
        /// Forbiddenエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult Forbidden()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.Forbidden,
                Message = ErrorMessages.ErrorResultForbidden,
            });
            result.StatusCode = StatusCodes.Status403Forbidden;
            return result;
        }

        /// <summary>
        /// NotFoundエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult NotFound()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.NotFound,
                Message = ErrorMessages.ErrorResultNotFound,
            });
            result.StatusCode = StatusCodes.Status404NotFound;
            return result;
        }

        /// <summary>
        /// InternalServerErrorエラー応答を生成します。
        /// </summary>
        /// <returns>エラー応答</returns>
        public static ErrorObjectResult InternalServerError()
        {
            var result = new ErrorObjectResult(new Error()
            {
                Code = ErrorCodes.InternalServerError,
                Message = ErrorMessages.ErrorResultInternalServerError,
            });
            result.StatusCode = StatusCodes.Status500InternalServerError;
            return result;
        }
    }
}
