




using System.Collections;

namespace Mythonia.Sturctures
{
    public class NamedList<T> : List<T> where T:INamed
    {
        public T this[string name] => Find(v => v.Name == name);
    }
}
