import chai from 'chai';
import {
  getTokensWithAssert,
  TokenModel,
  baseUrl,
  defaultUser,
  defaultUserPassword,
  usersCountRoute,
  usersRoute,
} from '../shared/utils';

const expect = chai.expect;

export default () => {
  describe('Users', () => {
    let tokens: TokenModel;

    before(async () => {
      tokens = await getTokensWithAssert(defaultUser, defaultUserPassword);
    });

    it('GET COUNT | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .get(usersCountRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(resp).to.have.status(200);
      expect(resp.body.count).to.be.equal(1);
    });

    it('GET LIST | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .get(usersRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .query({ offset: 0, limit: 10 })
        .send();

      expect(resp).to.have.status(200);
      expect(resp).to.have.json;
      expect(resp.body.total).to.be.equal(1);
      expect(resp.body.users).to.have.length(1);
      expect(resp.body.users[0].userName).to.be.equal(defaultUser);
      expect(resp.body.users[0].emailConfirmed).to.be.equal(true);
    });
  });
};
