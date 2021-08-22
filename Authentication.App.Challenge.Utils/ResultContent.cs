namespace Authentication.App.Challenge.Utils
{
    public abstract class ResultBase {}

    public class ResultContent<T> : ResultBase where T : ResultContentBase
    {
        public T Result { get; set; }
    }

    public class ResultContentBase
    {
    }

    public class Info : ResultContentBase
    {
        public string Message { get; set; }
    }
    
    public class Error : ResultContentBase
    {
        public string Message { get; set; }
    }
}