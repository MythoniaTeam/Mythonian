
#nullable enable


namespace Mythonia.Game
{
    public static class MainExtension
    {


        public static MVec2 Size(this GraphicsDevice v) => new(v.Viewport.Width, v.Viewport.Height);

        public static void Swap<T>(ref T v1, ref T v2) => (v1, v2) = (v2, v1);

        public static bool IsEmpty<T>(this ICollection<T> v) => v.Count == 0;

        /// <summary>
        /// 如果列表为空返回 <see langword="null"/>, 否则返回自身
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <returns>
        /// <list type="table">
        /// <item><term><see langword="this"/></term> <description><see cref="ICollection{T}"/> 不为空列表</description></item>
        /// <item><term><see langword="null"/></term> <description><see cref="ICollection{T}"/> 为空列表 (<see cref="ICollection{T}.Count"/> == 0)</description></item>
        /// </list>
        /// </returns>
        public static ICollection<T>? NullIfEmpty<T>(this ICollection<T> v) => v.IsEmpty() ? null : v;
    }
}
