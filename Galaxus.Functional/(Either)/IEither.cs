namespace Galaxus.Functional
{
    public interface IEither
    {
        /// <summary>
        /// Returns the contained value as an <see cref="object"/>.
        /// </summary>
        object ToObject();
    }
}
