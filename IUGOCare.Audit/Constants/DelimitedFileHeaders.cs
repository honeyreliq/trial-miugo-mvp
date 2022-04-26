namespace IUGOCare.Audit.Constants
{
    public static class DelimitedFileHeaders
    {
        public static readonly string ApiAuditModelPipeDelimited = string.Join("|",
               "Id",
               "Uri",
               "RequestId",
               "Method",
               "StatusCode",
               "ReasonPhrase",
               "Headers",
               "Content",
               "RequestReliqUserId",
               "CreateDate",
               "FirstName",
               "LastName");
    }
}
