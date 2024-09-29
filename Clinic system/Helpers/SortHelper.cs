namespace Clinic_system.Helpers
{
    public static class SortHelper
    {
        public static IEnumerable<T1> Sort<T1, T2>(IEnumerable<T1> list, Func<T1, T2> func)
        {
            return list.OrderBy(func);
        }
    }
}
