import authorization from '../shared/authorization';
import currentUser from '../shared/currentUser';
import users from './users';
import { defaultUser, defaultUserEmail, defaultUserPassword } from '../shared/utils';

export default () => {
  describe('Initialized user', () => {
    authorization(defaultUser, defaultUserEmail, defaultUserPassword);
    currentUser(defaultUser, defaultUserEmail, defaultUserPassword);
    users();
  });
};
