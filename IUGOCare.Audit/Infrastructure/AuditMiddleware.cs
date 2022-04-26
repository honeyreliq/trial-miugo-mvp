using System;
using IUGOCare.Audit.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using Microsoft.IO;
using System.Linq;

namespace IUGOCare.Audit.Infrastructure
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private IAuditService _auditService;

        public AuditMiddleware(RequestDelegate next, IAuditService auditService)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _auditService = auditService;
        }

        public async Task Invoke(HttpContext context)
        {
            // do custom stuff here with service   
            Guid requestId = Guid.NewGuid();
            //TODO Must be change as the userId parameter needs to be the one that sent the request 
            Guid userId = Guid.NewGuid(); 
            
            await LogRequest(context, requestId, userId);
            await LogResponse(context, requestId, userId);
        }

        private async Task LogRequest(HttpContext context, Guid requestId,  Guid userId)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value);

            _auditService.AddRequestAudit(
                requestId,
                GetAbsoluteUri(context.Request),
                context.Request.Method.ToString(),
                JsonSerializer.Serialize(headers),
                ReadStreamInChunks(requestStream),
                userId,
                "",
                ""
            );
            context.Request.Body.Position = 0;
        }
        private async Task LogResponse(HttpContext context, Guid requestId, Guid userId)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var headers = context.Response.Headers.ToDictionary(k => k.Key, v => v.Value);
            
            _auditService.AddResponseAudit(
                requestId,
                GetAbsoluteUri(context.Request),
                context.Request.Method.ToString(),
                context.Response.StatusCode.ToString(),
                context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase,
                JsonSerializer.Serialize(headers),
                text,
                userId,
                "",
                ""
            );
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private Uri GetAbsoluteUri(HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }
    }
}
