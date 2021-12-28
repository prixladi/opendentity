import chai from 'chai';
import {
  getTokensWithAssert,
  loginRequest,
  refreshRequest,
  baseUrl,
  defaultUser,
  defaultUserPassword,
  logoutRoute,
  defaultUserEmail,
} from '../shared/utils';

const expect = chai.expect;

export default () => {
  describe('Authorization', () => {
    it('LOGIN | invalid password', async () => {
      const resp = await loginRequest(defaultUser, 'dummy');
      expect(resp).to.have.status(400);
    });

    it('LOGIN | invalid username', async () => {
      const resp = await loginRequest(defaultUserEmail, defaultUserPassword);
      expect(resp).to.have.status(400);
    });

    it('LOGIN | success', async () => {
      await getTokensWithAssert(defaultUser, defaultUserPassword);
    });

    it('LOGIN WITH EMAIL | success', async () => {
      await getTokensWithAssert(defaultUserEmail, defaultUserPassword, true, true);
    });

    it('LOGIN no refresh | success', async () => {
      await getTokensWithAssert(defaultUser, defaultUserPassword, false);
    });

    it('REFRESH | invalid refresh token', async () => {
      const resp = await refreshRequest('');
      expect(resp).to.have.status(400);
    });

    it('REFRESH | success', async () => {
      const tokens = await getTokensWithAssert(defaultUser, defaultUserPassword);

      const resp = await refreshRequest(tokens.refresh_token);
      expect(resp).to.have.status(200);
      expect(resp).to.have.json;
    });

    it('LOGOUT | badRequest', async () => {
      const tokens = await getTokensWithAssert(defaultUser, defaultUserPassword);

      const resp = await chai
        .request(baseUrl)
        .post(logoutRoute)
        .type('form')
        .set('Authorization', `Bearer ${tokens.refresh_token}`)
        .send();

      expect(resp).to.have.status(400);
    });

    it('LOGOUT | success', async () => {
      const tokens = await getTokensWithAssert(defaultUser, defaultUserPassword);

      const resp = await chai
        .request(baseUrl)
        .post(logoutRoute)
        .timeout(20000)
        .type('form')
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(resp).to.have.status(204);

      const respRefresh = await refreshRequest(tokens.refresh_token);
      expect(respRefresh).to.have.status(400);
    });
  });
};
