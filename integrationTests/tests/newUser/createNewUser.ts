import chai from 'chai';
import {
  baseUrl,
  usersRoute,
  newUser,
  newUserEmail,
  newUserPassword,
  getTokensWithAssert,
  loginRequest,
  defaultUser,
  defaultUserPassword,
  userConfirmEmailRoute,
} from '../shared/utils';

const expect = chai.expect;

export default () => {
  describe('CreateNewUser', () => {
    it('CREATE | success', async () => {
      const resp = await chai.request(baseUrl).post(usersRoute).send({
        username: newUser,
        email: newUserEmail,
        password: newUserPassword,
      });

      expect(resp).to.have.status(201);
      expect(resp).to.have.header('location');
    });

    it('CREATE | conflict username', async () => {
      const resp = await chai.request(baseUrl).post(usersRoute).send({
        username: newUser,
        email: 'random4856242@ladisalavprix.cz',
        password: newUserPassword,
      });

      expect(resp).to.have.status(409);
      expect(resp).to.be.json;
      expect(resp.body.code).to.not.be.null;
    });

    it('CREATE | conflict email', async () => {
      const resp = await chai.request(baseUrl).post(usersRoute).send({
        username: 'random4856242',
        email: newUserEmail,
        password: newUserPassword,
      });

      expect(resp).to.have.status(409);
      expect(resp).to.be.json;
      expect(resp.body.code).to.not.be.null;
    });

    it('LOGIN | not confirmed', async () => {
      const resp = await loginRequest(newUser, newUserPassword);

      expect(resp).to.have.status(449);
      expect(resp).to.have.json;
      expect(resp.body.code).to.not.be.undefined;
      expect(resp.body.code).to.not.be.null;
      expect(resp.body.features).to.not.be.undefined;
      expect(resp.body.features).to.not.be.null;
      expect(resp.body.features).to.have.length(1);
    });

    it('LOGIN | success', async () => {
      const tokens = await getTokensWithAssert(defaultUser, defaultUserPassword);

      const usersResp = await chai
        .request(baseUrl)
        .get(usersRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .query({ offset: 0, limit: 10 })
        .send();

      expect(usersResp).to.have.status(200);
      expect(usersResp).to.have.json;
      expect(usersResp.body.total).to.be.equal(2);
      expect(usersResp.body.users).to.have.length(2);

      const newUserId = usersResp.body.users.filter((x: any) => x.email === newUserEmail)[0].id;
      expect(newUserId).to.not.be.null;
      expect(newUserId).to.not.be.undefined;

      const confirmResp = await chai
        .request(baseUrl)
        .put(userConfirmEmailRoute(newUserId))
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .query({ offset: 0, limit: 10 })
        .send({ value: true });

      expect(confirmResp).to.have.status(204);

      await getTokensWithAssert(newUser, newUserPassword);
    });
  });
};
