namespace Bussiness_Manager.Utility
{
    public class GenericContainer<T> : IGenericContainer<T>
    {
        public string status {  get; set; }
        public T Value { get; set; }
    }

    public interface IGenericContainer<T>
    {
        string status { get; set; }
        T Value { get; set; }
    }
}
