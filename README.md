# Opendentity

Identity provider apliacation, providing *password*, *resfresh_token* and *google* oauth flows. Also provides basic user management, such as *user registration*, *password reset*, *user disabling* and so on. 

## Status

[![CI](https://github.com/prixladi/opendentity/actions/workflows/main.yml/badge.svg)](https://github.com/prixladi/opendentity/actions/workflows/main.yml)

## Application settings

Json file with init data - [InitData.json](src/Shamyr.Opendentity.Service/InitData.json), this file is compiled inside docker image and can be maped using docker volumes.

|Class | Desription|
|:--- |  :---:|
|[Database](src/Shamyr.Opendentity.Database/DatabaseSettings.cs) | Settings for database connection|
|[DatabaseInit](src/Shamyr.Opendentity.Service/DatabaseInit/DatabaseInitSettings.cs) | Settings for database init eg. path to json data |
|[Redis](src/Shamyr.Opendentity.Service/Settings/RedisSettings.cs) | Settings for redis|
|[OpenId](src/Shamyr.Opendentity.OpenId/OpenIdSettings.cs) | Settings for OpenIddict engine|
|[Identity](src/Shamyr.Opendentity.OpenId/IdentitySettings.cs) | Settings for AspNet identity|
|[Email](src/Shamyr.Opendentity.Emails/EmailClientSettings.cs) | Settings for email cliemt|
|[Validation](src/Shamyr.Opendentity.Service/Settings/ValidationSettings.cs) | API validaton settings|
|[Ui](src/Shamyr.Opendentity.Service/Settings/UISettings.cs) | Settings for UI|
|[RateLimits](https://github.com/stefanprodan/AspNetCoreRateLimit/blob/master/src/AspNetCoreRateLimit/Models/IpRateLimitOptions.cs)| Settings for API rate limits|
