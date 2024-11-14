namespace ConsoleApp
{
    public static class ReflectionService
    {
        public static void SetProperty(object @object, string propertyPath, object value)
        {
            ArgumentNullException.ThrowIfNull(@object, nameof(@object));
            ArgumentNullException.ThrowIfNull(propertyPath, nameof(propertyPath));
            var properties = propertyPath.Split('.');
            if (!properties.Any())
            {
                throw new ArgumentException(nameof(propertyPath));
            }
            for (var i = 0; i < properties.Length - 1; ++i)
            {
                var getPropertyInfo = @object.GetType().GetProperty(properties[i]);
                if (getPropertyInfo == null)
                {
                    throw new ArgumentException(nameof(propertyPath));
                }
                @object = getPropertyInfo.GetValue(@object);
                if (@object == null)
                {
                    throw new ArgumentException(nameof(propertyPath));
                }
            }
            var setProperty = @object.GetType().GetProperty(properties[^1]);
            if (setProperty == null)
            {
                throw new ArgumentException(nameof(propertyPath));
            }
            var propertyType = setProperty.PropertyType;
            if (!propertyType.IsAssignableFrom(value.GetType()))
            {
                value = Convert.ChangeType(value, propertyType);
            }
            setProperty.SetValue(@object, value);
        }
    }
}
