import chai from 'chai';

const expect = chai.expect;

const loginRoute = '/api/v1/connect/token';
const logoutRoute = '/api/v1/logout';

const currentUserRoute = '/api/v1/users/current';
const currentUserChangePasswordRoute = '/api/v1/users/current/change-password';

const usersRoute = '/api/v1/users';
const usersCountRoute = '/api/v1/users/count';

const userRoute = (id: string) => `/api/v1/users/${id}`;
const userConfirmEmailRoute = (id: string) => `/api/v1/users/${id}/emailConfirmed`;

const baseUrl = process.env.BASE_URL || 'http://localhost:8080';
const defaultUser = process.env.DEFAULT_USER || 'Admin';
const defaultUserEmail = process.env.DEFAULT_USER_EMAIL || 'elix78963@gmail.com';
const defaultUserPassword = process.env.DEFAULT_USER_PASSWORD || 'Pass@word1';

const newUser = process.env.NEW_USER || 'User1';
const newUserEmail = process.env.NEW_USER_EMAIL || 'contact@ladislavprix.cz';
const newUserPassword = process.env.NEW_USER_PASSWORD || 'Pass@word1';

type TokenModel = {
  access_token: string;
  refresh_token: string;
  token_type: string;
};

const loginRequest = (
  username: string,
  password: string,
  offlineAccess: boolean = true,
  loginByEmail: boolean = false,
) => {
  const access = !!offlineAccess ? ' offline_access' : '';
  return chai
    .request(baseUrl)
    .post(loginRoute)
    .type('form')
    .send({
      grant_type: 'password',
      scope: 'openid profile email phone roles' + access,
      client_id: 'default',
      username,
      password,
      loginByEmail,
    });
};

const refreshRequest = (refreshToken: string) => {
  return chai.request(baseUrl).post(loginRoute).type('form').send({
    grant_type: 'refresh_token',
    scope: 'openid profile email phone roles offline_access',
    client_id: 'default',
    refresh_token: refreshToken,
  });
};

const getTokensWithAssert = async (
  username: string,
  password: string,
  offlineAccess: boolean = true,
  loginByEmail: boolean = false,
): Promise<TokenModel> => {
  const resp = await loginRequest(username, password, offlineAccess, loginByEmail);

  expect(resp).to.have.status(200);
  expect(resp).to.be.json;

  var body = resp.body as TokenModel;

  expect(body.access_token).to.not.be.null;
  expect(body.access_token).to.not.be.undefined;
  expect(body.token_type).to.be.equal('Bearer');

  if (offlineAccess) {
    expect(body.refresh_token).to.not.be.null;
    expect(body.refresh_token).to.not.be.undefined;
  } else {
    expect(body.refresh_token).to.be.undefined;
  }

  return body;
};

export type { TokenModel };

export {
  logoutRoute,
  loginRoute,
  userRoute,
  userConfirmEmailRoute,
  currentUserRoute,
  currentUserChangePasswordRoute,
  usersRoute,
  usersCountRoute,
};

export {
  baseUrl,
  defaultUser,
  defaultUserEmail,
  defaultUserPassword,
  newUser,
  newUserEmail,
  newUserPassword,
};

export { loginRequest, refreshRequest, getTokensWithAssert };
