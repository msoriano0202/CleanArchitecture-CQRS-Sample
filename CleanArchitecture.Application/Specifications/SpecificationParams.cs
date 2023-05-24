using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Specifications
{
    public abstract class SpecificationParams
    {
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = ConstantValues.Default_PageIndex;

        private const int MaxPageSize = ConstantValues.Default_MaxPageSize;
        private int _pageSize = ConstantValues.Default_MinPageSize;

        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
