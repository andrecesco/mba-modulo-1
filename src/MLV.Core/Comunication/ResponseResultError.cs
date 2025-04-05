using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLV.Core.Comunication;

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

