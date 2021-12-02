﻿using MediatR;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record LogoutCommand: IRequest, ITransactionRequest;
