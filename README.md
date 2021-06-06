# Opendentity

Identity provider apliacation, providing *password*, *resfresh_token* and *google* oauth flows. Also provides basic user management, such as *user registration*, *password reset*, *user disabling* and so on. 

## Application settings

Settings using environment variables - [EnvVariables.cs](src/Shamyr.Opendentity.Service/EnvVariables.cs)

Json file with init data - [InitData.json](src/Shamyr.Opendentity.Service/InitData.json), this file is compiled inside docker image and can be maped using docker volumes.

|Name | Description|
|:--- | :---:|
|DATABASE_CONNECTION_STRING | Connection string to PostgreSQL database|
|INIT_FILE_PATH | Path to init file|
|ACCESS_TOKEN_DURATION | Duration of access token in Timespan format (eg. 00:10:00)|
|REFRESH_TOKEN_DURATION| Duration of refresh token in Timespan format (eg. 14:00:00)|
|REQUIRE_VERIFIED_ACCOUNT| Flag if account needs to be verified in order to log in|
|EMAIL_SERVER_URL | Url of email server|
|EMAIL_SENDER_ADDRESS| Address of email sender|
|PORTAL_URL| Base url of application using this identity provider. Used in Emails.|