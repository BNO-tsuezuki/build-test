using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace evogmtool.Utils
{
    // todo: ref) https://stackoverflow.com/questions/59156514/asp-net-core-api-iso8601-not-parsed-in-local-time-when-suffix-is-z

    public class UtcAwareDateTimeModelBinder : IModelBinder
    {
        private readonly DateTimeStyles _supportedStyles;
        private readonly ILogger _logger;

        public UtcAwareDateTimeModelBinder(DateTimeStyles supportedStyles, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _supportedStyles = supportedStyles;
            _logger = loggerFactory.CreateLogger<UtcAwareDateTimeModelBinder>();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                // no entry
                return Task.CompletedTask;
            }

            var modelState = bindingContext.ModelState;
            modelState.SetModelValue(modelName, valueProviderResult);

            var metadata = bindingContext.ModelMetadata;
            var type = metadata.UnderlyingOrModelType;

            try
            {
                var value = valueProviderResult.FirstValue;
                var culture = valueProviderResult.Culture;

                object model;
                if (string.IsNullOrWhiteSpace(value))
                {
                    model = null;
                }
                else if (type == typeof(DateTime))
                {
                    // You could put custom logic here to sniff the raw value and call other DateTime.Parse overloads, e.g. forcing UTC
                    model = DateTime.Parse(value, culture, _supportedStyles);
                }
                else
                {
                    // unreachable
                    throw new NotSupportedException();
                }

                // When converting value, a null model may indicate a failed conversion for an otherwise required
                // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
                // current bindingContext. If not, an error is logged.
                if (model == null && !metadata.IsReferenceOrNullableType)
                {
                    var message = metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(valueProviderResult.ToString());
                    modelState.TryAddModelError(modelName, message);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            catch (Exception exception)
            {
                var isFormatException = exception is FormatException;
                if (!isFormatException && exception.InnerException != null)
                {
                    // Unlike TypeConverters, floating point types do not seem to wrap FormatExceptions. Preserve
                    // this code in case a cursory review of the CoreFx code missed something.
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }

                modelState.TryAddModelError(modelName, exception, metadata);

                // Conversion failed.
            }

            return Task.CompletedTask;
        }
    }
}
