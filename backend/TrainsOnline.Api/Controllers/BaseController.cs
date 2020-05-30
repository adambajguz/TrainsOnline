namespace TrainsOnline.Api.Controllers
{
    using System;
    using Application.Interfaces;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly Lazy<IMediator> _mediator;
        private readonly Lazy<ICurrentUserService> _currentUser;

        protected IMediator Mediator => _mediator.Value;
        protected ICurrentUserService CurrentUser => _currentUser.Value;

        public BaseController()
        {
            _mediator = new Lazy<IMediator>(() => HttpContext.RequestServices.GetService<IMediator>());
            _currentUser = new Lazy<ICurrentUserService>(() => HttpContext.RequestServices.GetService<ICurrentUserService>());
        }
    }
}
