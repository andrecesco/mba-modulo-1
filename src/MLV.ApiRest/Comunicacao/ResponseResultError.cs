namespace MLV.ApiRest.Comunication;

public class ResponseResultError
{
    public ResponseResultError()
    {
        Errors = new ResponseErrorMessages();
    }

    public string Title { get; set; }
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
}

public class ResponseErrorMessages
{
    public ResponseErrorMessages()
    {
        Mensagens = [];
    }

    public List<string> Mensagens { get; set; }
}

