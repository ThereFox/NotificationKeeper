using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Customs
{
    public static class ResultJsonDeserializer
    {
        public static Result<T> DeserializeObject<T>(string input) where T : class
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(input);

                if (result == default)
                {
                    return Result.Failure<T>("Invalid input value");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<T>($"Error while converting: {ex.Message}");
            }
        }
    }
}
