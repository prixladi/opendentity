import chai from 'chai';
import chaiHttp from 'chai-http';
import initializedUser from './initializedUser';
import newUser from './newUser';
import { baseUrl } from './shared/utils';

chai.use(chaiHttp);

const expect = chai.expect;

describe('Sanity test', () => {
  it('GET root | not found', async () => {
    const resp = await chai.request(baseUrl).get('/');
    expect(resp).to.have.status(404);
  });
});

initializedUser();
newUser();
