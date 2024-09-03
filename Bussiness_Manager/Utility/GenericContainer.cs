namespace Bussiness_Manager.Utility
{
    public class GenericContainer<T> : IGenericContainer<T>
    {
        public bool IsSuccessful { get; set; }
        public string status {  get; set; }
        public T Value { get; set; }
    }

    public interface IGenericContainer<T>
    {
        bool IsSuccessful {  get; set; }
        string status { get; set; }
        T Value { get; set; }
    }
}
