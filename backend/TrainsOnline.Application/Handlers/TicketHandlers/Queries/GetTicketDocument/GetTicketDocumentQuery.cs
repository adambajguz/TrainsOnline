namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDocument
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CSharpVitamins;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Extensions;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.Documents;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetTicketDocumentQuery : IRequest<GetTicketDocumentResponse>
    {
        public IdRequest Data { get; }

        public GetTicketDocumentQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetTicketDocumentQuery, GetTicketDocumentResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;
            private readonly IDataRightsService _drs;
            private readonly IDocumentsService _documents;
            private readonly IQRCodeService _qr;
            private readonly IHttpContextAccessor _context;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper, IDataRightsService drs, IDocumentsService documents, IQRCodeService qr, IHttpContextAccessor context)
            {
                _uow = uow;
                _mapper = mapper;
                _drs = drs;
                _documents = documents;
                _qr = qr;
                _context = context;
            }

            public async Task<GetTicketDocumentResponse> Handle(GetTicketDocumentQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                Ticket entity = await _uow.TicketsRepository.GetByIdWithRelatedAsync(data.Id, x => x.Route, x => x.Route.From, x => x.Route.To);

                EntityRequestByIdValidator<Ticket>.Model validationModel = new EntityRequestByIdValidator<Ticket>.Model(data, entity);
                await new EntityRequestByIdValidator<Ticket>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);
                await _drs.ValidateUserId(entity, x => x.UserId);

                byte[] ticketCalendarCode = BuildCalendarQRCode(entity);
                byte[] ticketVerificationCode = BuildVerificationQRCode(entity);
                byte[] document = BuildDocument(entity, ticketCalendarCode, ticketVerificationCode);

                GetTicketDocumentResponse response = _mapper.Map<GetTicketDocumentResponse>(entity);
                response.Document = document;

                return response;
            }

            #region Handler Helpers
            private byte[] BuildVerificationQRCode(Ticket entity)
            {
                Uri baseUri = _context.HttpContext.GetAbsoluteUri();
                Uri validationUri = new Uri(baseUri, $"/api/ticket/validate-document?tid={(ShortGuid)entity.Id}&uid={(ShortGuid)entity.UserId}&rid={(ShortGuid)entity.RouteId}");

                return _qr.CreateWebCode(validationUri);
            }

            private byte[] BuildCalendarQRCode(Ticket entity)
            {
                return _qr.CreateCalendarCode($"{entity.Route.From.Name} → {entity.Route.To.Name}",
                                              $"PKP Ticket {{{entity.Id}}}",
                                              entity.Route.From.Latitude,
                                              entity.Route.To.Longitude,
                                              entity.Route.DepartureTime,
                                              entity.Route.Duration);
            }

            private byte[] BuildDocument(Ticket entity, byte[] ticketCalendarCode, byte[] ticketVerificationCode)
            {
                using (MemoryStream qrCalendarCodeMemoryStream = new MemoryStream(ticketCalendarCode))
                using (MemoryStream qrCodeMemoryStream = new MemoryStream(ticketVerificationCode))
                using (MemoryStream headerImageMemoryStream = new MemoryStream(Convert.FromBase64String(PdfHeaderImage.ImageJpeg)))
                {
                    DateTime arrival = entity.Route.DepartureTime.Add(entity.Route.Duration);
                    Color color = Color.FromArgb(57, 89, 158);

                    return _documents.NewDocument()
                                     .AddSection()

                                     .AddComplexParagraph()
                                     //.AddImage(headerImageMemoryStream, DocumentImageFormats.Jpeg, 160, 30)
                                     .AddNewLine(2)

                                     .AddRunLine("────────────────────────────────┤ TICKET ├─────────────────────────────────", bold: true, fontColor: color)
                                     .AddRun("         TID  ", bold: true, fontColor: color).AddRunLine($"PKP Ticket {{{(ShortGuid)entity.Id}}}")
                                     .AddRun("   TIMESTAMP  ", bold: true, fontColor: color).AddRunLine(entity.LastSavedOn.ToString())
                                     .AddRun("       ROUTE  ", bold: true, fontColor: color).AddRunLine($"{entity.Route.From.Name} → {entity.Route.To.Name}")
                                     .AddRun("   DEPARTURE  ", bold: true, fontColor: color).AddRunLine(entity.Route.DepartureTime.ToString())
                                     .AddNewLine()
                                     .AddRunLine("───────────────────────────────┤ PASSENGER ├───────────────────────────────", bold: true, fontColor: color)
                                     .AddRun("         UID  ", bold: true, fontColor: color).AddRunLine($"{{{(ShortGuid)entity.UserId}}}")
                                     .AddRun("        NAME  ", bold: true, fontColor: color).AddRunLine($"{entity.User.Name} {entity.User.Surname}")
                                     .AddRun("      E-MAIL  ", bold: true, fontColor: color).AddRunLine(entity.User.Email)
                                     .AddRun("     ADDRESS  ", bold: true, fontColor: color).AddRunLine(entity.User.Address)
                                     .AddRun("       PHONE  ", bold: true, fontColor: color).AddRunLine(entity.User.PhoneNumber)
                                     .AddNewLine()
                                     .AddRunLine("─────────────────────────────┤ ROUTE DETAILS ├─────────────────────────────", bold: true, fontColor: color)
                                     .AddRun("         RID  ", bold: true, fontColor: color).AddRunLine($"{{{(ShortGuid)entity.RouteId}}}")
                                     .AddRun("  CREATED ON  ", bold: true, fontColor: color).AddRunLine(entity.CreatedOn.ToString())
                                     .FinishParagraph()

                                     .AddSimpleTable(new object[,]
                                     {
                                         { "Departure",                                               "Arrival",                            "Travel time",         "Distance",                    "Ticket price"                 },
                                         { $"{entity.Route.From.Name}\n{entity.Route.DepartureTime}", $"{entity.Route.To.Name}\n{arrival}", entity.Route.Duration, $"{entity.Route.Distance} km", $"${entity.Route.TicketPrice}" },
                                     })

                                     .AddComplexParagraph()
                                     .AddNewLine()
                                     .AddRunLine("─────────────────────────────┤ MISCELLANEOUS ├─────────────────────────────", bold: true, fontColor: color)
                                     .FinishParagraph()
                                     .AddMultiColumn(2, paddingHorizontal: 4,
                                                     (x) => x.AddRunLine("Calendar event QR Code:")
                                                             .AddImage(qrCalendarCodeMemoryStream, DocumentImageFormats.Jpeg, 55, 55)
                                                             .AddNewLine()
                                                             .AddRunLine("This code allows you to add an event to calendar app on your device. The event will contain all crucial data from this ticket.", size: 8, italic: true),

                                                     (x) => x.AddRunLine("Verification QR Code:")
                                                             .AddImage(qrCodeMemoryStream, DocumentImageFormats.Jpeg, 55, 55)
                                                             .AddNewLine()
                                                             .AddRunLine("The verification QR Code allows you to verify authenticity of the ticket using a dedicated online service.", size: 8, italic: true))

                                     .FinishSection()
                                     .BuildPdf();
                }
            }
            #endregion
        }
    }
}
