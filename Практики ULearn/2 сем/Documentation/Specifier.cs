using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
	public string GetApiDescription() =>
		(typeof(T).GetCustomAttributes(typeof(ApiDescriptionAttribute), true)
			.FirstOrDefault() as ApiDescriptionAttribute)?.Description;
	
	public string[] GetApiMethodNames() => 
		typeof(T)
			.GetMethods()
			.Where(method => IsApiMethod(method.Name))
			.Select(method => method.Name).ToArray();
	
	public string GetApiMethodDescription(string methodName) => 
		typeof(T)
			.GetMethod(methodName)
			?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
	
	public string[] GetApiMethodParamNames(string methodName) => 
		typeof(T)
			.GetMethod(methodName)
			?.GetParameters()
			.Select(parameter => parameter.Name).ToArray();
	
	public string GetApiMethodParamDescription(string methodName, string paramName) => 
		typeof(T)
			.GetMethod(methodName)
			?.GetParameters()
			.Where(parameter => parameter.Name == paramName)
			.Select(parameter => parameter
				.GetCustomAttribute<ApiDescriptionAttribute>()?.Description)
			.ToArray().FirstOrDefault();
	
	public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
	{
		var parameterInfo = GetParameterInfo(methodName, paramName);
		
		if (parameterInfo == null)
			return new ApiParamDescription { ParamDescription = new CommonDescription(paramName) };
		
		var apiRequired = parameterInfo
			.Select(parameter => parameter
				.GetCustomAttribute<ApiRequiredAttribute>()?.Required).FirstOrDefault();
		
		var values = GetValueFromApiIntValidationAttribute(parameterInfo);
		
		var apiMethodParamFullDescription = new ApiParamDescription
		{
			ParamDescription = new CommonDescription(paramName, GetApiMethodParamDescription(methodName, paramName)),
			MinValue = values[0],
			MaxValue = values[1],
			Required = apiRequired ?? false
		};
		
		return apiMethodParamFullDescription;
	}

	public ApiMethodDescription GetApiMethodFullDescription(string methodName)
	{
		if (!IsApiMethod(methodName))
			return null;
		
		var apiParamDescriptions = GetApiMethodParamNames(methodName)
			.Select(parameter => GetApiMethodParamFullDescription(methodName, parameter))
			.ToArray();
		
		var apiMethodFullDescription = new ApiMethodDescription
		{
			MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
			ParamDescriptions = apiParamDescriptions
		};
		
		if (CheckIfReturnDescriptionDefined(methodName))
			apiMethodFullDescription.ReturnDescription = GetApiMethodParamFullReturnDescription(methodName);

		return apiMethodFullDescription;
	}

	private static IEnumerable<ParameterInfo> GetParameterInfo(string methodName, string paramName) => 
		typeof(T)
			.GetMethod(methodName)
			?.GetParameters()
			.Where(parameter => parameter.Name == paramName);

	private static int?[] GetValueFromApiIntValidationAttribute(IEnumerable<ParameterInfo> paramInfo)
	{
		var apiIntValidationAttribute = paramInfo
			.Select(parameter => parameter
				.GetCustomAttribute<ApiIntValidationAttribute>());
		
		var min = apiIntValidationAttribute
			.Select(parameter => parameter?.MinValue).FirstOrDefault();
		
		var max = apiIntValidationAttribute
			.Select(parameter => parameter?.MaxValue).FirstOrDefault();

		return new []{ min, max};
	}

	private static bool IsApiMethod(string methodName) => 
		typeof(T)
			.GetMethod(methodName)
			?.GetCustomAttribute<ApiMethodAttribute>() != null;

	private static bool CheckIfReturnDescriptionDefined(string methodName)
	{
		var returnParameterAttrs = typeof(T)
			.GetMethod(methodName)!
			.ReturnParameter!.GetCustomAttributes();
		
		var isReturnDescriptionDefined = false;
		
		foreach (var attr in returnParameterAttrs)
			isReturnDescriptionDefined = typeof(T)
				.GetMethod(methodName)!
				.ReturnParameter!.IsDefined(attr.GetType());

		return isReturnDescriptionDefined;
	}
	
	private static ApiParamDescription GetApiMethodParamFullReturnDescription(string methodName)
	{
		var returnParameterInfo = typeof(T)
			.GetMethod(methodName)
			?.ReturnParameter;
		
		var apiMethodParamFullReturnDescription = new ApiParamDescription
		{
			ParamDescription = new CommonDescription(null, returnParameterInfo
				.GetCustomAttribute<ApiDescriptionAttribute>()?.Description),
			MinValue = returnParameterInfo.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue,
			MaxValue = returnParameterInfo.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue
		};
		
		var apiRequired = returnParameterInfo.GetCustomAttribute<ApiRequiredAttribute>()?.Required;
		
		if (apiRequired != null)
			apiMethodParamFullReturnDescription.Required = apiRequired.Value;
		
		return apiMethodParamFullReturnDescription;
	}
}