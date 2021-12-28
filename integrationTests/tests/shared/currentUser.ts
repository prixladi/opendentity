import chai from 'chai';
import {
  getTokensWithAssert,
  loginRequest,
  TokenModel,
  baseUrl,
  currentUserRoute,
  currentUserChangePasswordRoute,
} from '../shared/utils';

const expect = chai.expect;

export default (userName: string, email: string, password: string) => {
  describe('CurrentUser', () => {
    let tokens: TokenModel;

    before(async () => {
      tokens = await getTokensWithAssert(userName, password);
    });

    it('GET CURRENT | unauthorized', async () => {
      const resp = await chai.request(baseUrl).get(currentUserRoute).send();

      expect(resp).to.have.status(401);
    });

    it('GET CURRENT | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .get(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(resp).to.have.status(200);
      expect(resp).to.have.json;
      expect(resp.body.userName).to.be.equal(userName);
      expect(resp.body.email).to.be.equal(email);
      expect(resp.body.emailConfirmed).to.be.equal(true);
    });

    it('UPDATE CURRENT | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .put(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({
          userName: 'jackChange',
          firstName: 'jack1',
          lastName: 'change1',
          imageUrl: 'https://images.com/default',
        });

      expect(resp).to.have.status(204);

      const current = await chai
        .request(baseUrl)
        .get(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(current).to.have.status(200);
      expect(current).to.have.json;
      expect(current.body.userName).to.be.equal('jackChange');
      expect(current.body.email).to.be.equal(email);
      expect(current.body.firstName).to.be.equal('jack1');
      expect(current.body.lastName).to.be.equal('change1');
      expect(current.body.emailConfirmed).to.be.equal(true);

      const resp2 = await chai
        .request(baseUrl)
        .put(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({
          userName: userName,
          firstName: 'jack2',
          lastName: 'change2',
          imageUrl: 'https://images.com/default',
        });

      expect(resp2).to.have.status(204);

      const current2 = await chai
        .request(baseUrl)
        .get(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(current2).to.have.status(200);
      expect(current2).to.have.json;
      expect(current2.body.userName).to.be.equal(userName);
      expect(current2.body.email).to.be.equal(email);
      expect(current2.body.firstName).to.be.equal('jack2');
      expect(current2.body.lastName).to.be.equal('change2');
      expect(current2.body.emailConfirmed).to.be.equal(true);
    });

    it('UPDATE CURRENT SAME USERNAME | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .put(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({
          userName: userName,
        });

      expect(resp).to.have.status(204);

      const current = await chai
        .request(baseUrl)
        .get(currentUserRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send();

      expect(current).to.have.status(200);
      expect(current).to.have.json;
      expect(current.body.userName).to.be.equal(userName);
      expect(current.body.email).to.be.equal(email);
      expect(current.body.emailConfirmed).to.be.equal(true);
    });

    it('CHANGE PASSWORD | wrong password', async () => {
      const resp = await chai
        .request(baseUrl)
        .patch(currentUserChangePasswordRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({ oldPassword: 'Pass@word2', newPassword: 'Pass@word2' });

      expect(resp).to.have.status(400);
    });

    it('CHANGE PASSWORD | success', async () => {
      const resp = await chai
        .request(baseUrl)
        .patch(currentUserChangePasswordRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({
          oldPassword: password,
          newPassword: 'Pass@word2',
        });

      expect(resp).to.have.status(204);

      const badRequestRes = await loginRequest(userName, password);
      expect(badRequestRes).to.have.status(400);

      tokens = await getTokensWithAssert(userName, 'Pass@word2');

      const resp2 = await chai
        .request(baseUrl)
        .patch(currentUserChangePasswordRoute)
        .set('Authorization', `Bearer ${tokens.access_token}`)
        .send({
          oldPassword: 'Pass@word2',
          newPassword: password,
        });

      expect(resp2).to.have.status(204);

      const badRequestRes2 = await loginRequest(userName, 'Pass@word2');
      expect(badRequestRes2).to.have.status(400);

      tokens = await getTokensWithAssert(userName, password);
    });
  });
};
