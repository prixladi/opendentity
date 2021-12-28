import chai from 'chai';
import {
  getTokensWithAssert,
  loginRequest,
  refreshRequest,
  baseUrl,
  logoutRoute,
} from '../shared/utils';

const expect = chai.expect;

export default (userName: string, email: string, password: string) => {
  describe('Authorization', () => {
    it('LOGIN | invalid password', async () => {
      const resp = await loginRequest(userName, 'dummy');
      expect(resp).to.have.status(400);
    });

    it('LOGIN | invalid username', async () => {
      const resp = await loginRequest(email, password);
      expect(resp).to.have.status(400);
    });

    it('LOGIN | success', async () => {
      await getTokensWithAssert(userName, password);
    });

    it('LOGIN WITH EMAIL | success', async () => {
      await getTokensWithAssert(email, password, true, true);
    });

    it('LOGIN no refresh | success', async () => {
      await getTokensWithAssert(userName, password, false);
    });

    it('REFRESH | invalid refresh token', async () => {
      const resp = await refreshRequest('');
      expect(resp).to.have.status(400);
    });

    it('REFRESH | success', async () => {
      const tokens = await getTokensWithAssert(userName, password);

      const resp = await refreshRequest(tokens.refresh_token);
      expect(resp).to.have.status(200);
      expect(resp).to.have.json;
    });

    it('LOGOUT | badRequest', async () => {
      const tokens = await getTokensWithAssert(userName, password);

      const resp = await chai
        .request(baseUrl)
        .post(logoutRoute)
        .type('form')
        .set('Authorization', `Bearer ${tokens.refresh_token}`)
        .send();

      expect(resp).to.have.status(400);
    });

    it('LOGOUT | success', async () => {
      const tokens = await getTokensWithAssert(userName, password);

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
