namespace UserManagement.UsesCases.Utils
{
    public class Mappers
    {
        public static TModel Map<TViewModel, TModel>(TViewModel viewModel) where TModel : new()
        {
            TModel model = new();
            foreach (var viewModelProperty in typeof(TViewModel).GetProperties())
            {
                var modelProperty = typeof(TModel).GetProperty(viewModelProperty.Name);
                if (modelProperty != null && modelProperty.CanWrite)
                {
                    modelProperty.SetValue(model, viewModelProperty.GetValue(viewModel));
                }
            }

            return model;
        }

        public static TViewModel Map<TViewModel, TModel>(TModel viewModel) where TViewModel : new()
        {
            TViewModel model = new();
            foreach (var viewModelProperty in typeof(TModel).GetProperties())
            {
                var modelProperty = typeof(TViewModel).GetProperty(viewModelProperty.Name);
                if (modelProperty != null && modelProperty.CanWrite)
                {
                    modelProperty.SetValue(model, viewModelProperty.GetValue(viewModel));
                }
            }

            return model;
        }
    }
}
