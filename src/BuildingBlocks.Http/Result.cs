using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Http
{
    public class Result
    {
        public bool Success { get; protected set; }
        public IEnumerable<string> Errors { get; protected set; }

        protected Result(bool success, IEnumerable<string> errors)
        {
            Success = success;
            Errors = errors;
        }

        public static class Factory
        {
            public static Result AsError(params string[] error)
            {
                return new Result(false, error);
            }
            
            public static Result AsSuccess()
            {
                return new Result(true, Enumerable.Empty<string>());
            }
        }
    }
    
    public class Result<TData> : Result
    {
        public TData Data { get; private set; }

        public Result(bool success, TData data, IEnumerable<string> errors) : base(success, errors)
        {
            Data = data;
        }
        
        public new static class Factory
        {
            public static Result<TData> AsError(params string[] error)
            {
                return new Result<TData>(false, default, error);
            }
            
            public static Result<TData> AsSuccess(TData data)
            {
                return new Result<TData>(true, data, Enumerable.Empty<string>());
            }
        }
    }
}